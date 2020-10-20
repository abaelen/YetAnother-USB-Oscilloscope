#include "pch.h"
#include "USB.h"


_USB_Device::_USB_Device()
{
	//Status.Initialized[0] = Initialize(BufferSize,PacketSize, MicroFrameSize);
}

_USB_Device::~_USB_Device()
{
	RequestToStop[0] = FALSE;
	Status.Started[0] = FALSE;
	Status.Stopped[0] = TRUE;
	//Dispose();
}

VOID _USB_Device::GetHandle()
{
	BOOL deviceNotFound = FALSE;
	HRESULT Hresult = OpenDevice(&Handle, &deviceNotFound);
	Status.Action.DeviceFound[0] = !(deviceNotFound);
	if (Hresult < 0 || Status.Action.DeviceFound[0] == FALSE) throw "No device handle retrieved";
}

VOID _USB_Device::GetDescriptor()
{
	ULONG lengthReceived;
	Status.Action.GetDescriptor[0] = (uint8_t) WinUsb_GetDescriptor(Handle.WinusbHandle, USB_DEVICE_DESCRIPTOR_TYPE, 0, 0, (PBYTE)&Descriptor, sizeof(Descriptor), &lengthReceived);
	if (lengthReceived != sizeof(Descriptor) || Status.Action.GetDescriptor[0] == FALSE) throw "No Descriptor retrieved";
}

VOID _USB_Device::GetInterface()
{
	Status.Action.GetInterface[0] = (uint8_t)WinUsb_QueryInterfaceSettings(Handle.WinusbHandle, 0, &Interface);
}

VOID _USB_Device::SetBuffers(int16_t* pBackBuffer,  uint32_t bufferSize8,  uint32_t backBufferSize8)
{
	BackBuffer = NULL;
	BackBuffer = ((uint8_t*) pBackBuffer)+bufferSize8-backBufferSize8; 
	BackBufferSize8 = backBufferSize8;
	BackBuffer16 = (int16_t *) BackBuffer;
	BackBufferLocked[0] = FALSE;
}

VOID _USB_Device::GetIsochPipe()
{
	Status.Action.GetIsochPipe[0] = FALSE;
	for (UCHAR i = 0; i < Interface.bNumEndpoints; i++)
	{
		if (WinUsb_QueryPipeEx(Handle.WinusbHandle, 0, i, &Handle.pipe) == TRUE) 
		{
			if (Handle.pipe.PipeType == UsbdPipeTypeIsochronous) 
			{
				Handle.IsochInPipe = Handle.pipe.PipeId;
				Status.Action.GetIsochPipe[0] = TRUE;
				i = Interface.bNumEndpoints;
			}
		}
		else
		{
			//Status.Action.QueryPipe[0] = HRESULT_FROM_WIN32(GetLastError());
			i = Interface.bNumEndpoints;
			throw "No Pipe";
		}
	}
}

VOID _USB_Device::CheckInterval()
{
	if (Handle.pipe.MaximumBytesPerInterval == 0 || Handle.pipe.Interval == 0)
	{
		Status.Action.GetInterval[0] = FALSE;
		throw "Wrong interval or Max Bytes per interval";
	}
	Status.Action.GetInterval[0] = TRUE;
}

VOID _USB_Device::SetTransferCharacteristics( uint32_t packetSize,  uint32_t microFrameSize)
{
	Handle.IsochInBytesPerInterval = ((ULONG)BackBufferSize8 > Handle.pipe.MaximumBytesPerInterval) ? Handle.pipe.MaximumBytesPerInterval : (ULONG)BackBufferSize8;
	Handle.IsochInBytesPerFrame = Handle.IsochInBytesPerInterval * ((ULONG)microFrameSize / (ULONG)Handle.pipe.Interval);
	Handle.IsochInTransferSize = Handle.IsochInBytesPerFrame * (ULONG)packetSize;
	if (Handle.IsochInTransferSize % Handle.IsochInBytesPerFrame == 0)
	{
		Handle.IsochInPacketCount = max(1,Handle.IsochInTransferSize / Handle.pipe.MaximumBytesPerInterval); //BufferSize / Handle.IsochInTransferSize; //assumption: no re-use of packages accross query
		Status.Action.EndAtFrame[0] =TRUE;
		Status.Action.SetTransferChars[0] = TRUE;
	}
	else
	{
		Status.Action.EndAtFrame[0] = FALSE;
		Status.Action.SetTransferChars[0] = FALSE;
		throw "Buffer not at end of frame";
	}
}

VOID _USB_Device::SetOverlappedStructure()
{
	Overlapped = new OVERLAPPED[Isoch_In_Transfer_Count];
	ZeroMemory(Overlapped, Isoch_In_Transfer_Count * sizeof(OVERLAPPED));
	if (Overlapped != NULL) Status.Action.SetOverlappedStructure[0] = TRUE; else throw "no Overlapped allocated";
}

VOID _USB_Device::SetOverlappedEvents()
{
	for (ULONG i = 0; i < Isoch_In_Transfer_Count; i++)
	{
		Overlapped[i].hEvent = CreateEvent(NULL, TRUE, FALSE, NULL);
		if (Overlapped[i].hEvent == NULL) { Status.Action.SetOverlappedEvents[0] = FALSE; throw "Event not created"; }
		else Status.Action.SetOverlappedEvents[0] = TRUE;
	}
}

VOID _USB_Device::SetIsochPackets()
{
	Packets = new USBD_ISO_PACKET_DESCRIPTOR[(uint32_t) Isoch_In_Transfer_Count*Handle.IsochInPacketCount];
	ZeroMemory(Packets, (uint32_t)Isoch_In_Transfer_Count*Handle.IsochInPacketCount * sizeof(USBD_ISO_PACKET_DESCRIPTOR));
	if (Packets == NULL) {Status.Action.SetIsochPackets[0] = FALSE; throw "No Packets could be created";}
	else Status.Action.SetIsochPackets[0] = TRUE;
}

VOID _USB_Device::RegisterIsochBuffer()
{
	if (BackBufferSize8  < (ULONG) (Handle.IsochInTransferSize))
	{Status.Action.RegisterIsochBuffer[0] = FALSE; throw "No or wrongly sized databuffer"; }
	
	BOOL result = WinUsb_RegisterIsochBuffer(Handle.WinusbHandle, Handle.IsochInPipe, BackBuffer, BackBufferSize8, &IsochBufferHandle);


	if (result == FALSE) { Status.Action.RegisterIsochBuffer[0] = FALSE; throw "No or wrongly sized databuffer"; }
	else Status.Action.RegisterIsochBuffer[0] = TRUE;
}

ULONG _USB_Device::GetCurrentFrameNumber()
{
	ULONG frameNumber; LARGE_INTEGER timeStamp;
	BOOL result = WinUsb_GetCurrentFrameNumber(Handle.WinusbHandle, &frameNumber, &timeStamp);
	if (result == FALSE)
	{
		Status.Errored[0] = TRUE;
		return -999;
	}
	return frameNumber;
}

VOID _USB_Device::ResetPipe()
{
	BOOL result = WinUsb_ResetPipe(Handle.WinusbHandle, Handle.IsochInPipe);
	if (result == FALSE)
	{
		Status.Errored[0] = TRUE;
		Status.Action.ResetPipe[0] = FALSE;
	}
	Status.Action.ResetPipe[0] = TRUE;
}


//To use the Class create a _USB_Device USB member. Then call the USB.Initialize(1023,1,1) eg. 1023 in 1 packet & 1 microframe (as only FS) or USB.Initialize(2046,2,1), ...

VOID _USB_Device::Initialize(int16_t* pBuffer16, uint32_t bufferSize8, uint32_t backBufferSize8, uint32_t packetSize, uint32_t microFrameSize)
{
	try {
		//Here dispose would be misplaced. The ~ would entail to delete the Screen member itself not its members.
		//The members should be disposed off in the DLLmain on the thread wich created them.
		//Also dont use Dispose inside the class but only in the DLLmain
		//if (Status.Initialized[0] ==TRUE) Dispose();
		Status.Errored[0] =FALSE;

		SetBuffers(pBuffer16, bufferSize8, backBufferSize8);
		GetHandle();
		GetDescriptor();
		GetInterface();
		GetIsochPipe();
		CheckInterval();
		SetTransferCharacteristics(packetSize, microFrameSize);
		SetOverlappedStructure();
		SetOverlappedEvents();
		SetIsochPackets();
		RegisterIsochBuffer();
		ResetPipe();

		Status.Initialized[0] =TRUE;
	}
	catch (...) 
	{
		Status.Started[0] = FALSE;
		Status.Stopped[0] = TRUE;
		Status.Errored[0] = TRUE;
		//Here dispose would be misplaced. The ~ would entail to delete the Screen member itself not its members.
		//The members should be disposed off in the DLLmain on the thread wich created them.
		//Also dont use Dispose inside the class but only in the DLLmain
		//Dispose();
	}
}

VOID _USB_Device::Dispose()
{
try{
		Status.Initialized[0] = FALSE;
		Status.Started[0] = FALSE;
		Status.Stopped[0] = TRUE;

		if(BackBuffer!=NULL) BackBuffer=NULL;
		if (BackBuffer16 != NULL) BackBuffer16=NULL;

		if (IsochBufferHandle != 0x0000000000000000) {BOOL Result = WinUsb_UnregisterIsochBuffer(IsochBufferHandle);}

		if (Packets != NULL) {delete[] Packets;}

		for (ULONG i = 0; i < Isoch_In_Transfer_Count; i++)
		{	if (Overlapped != NULL && Overlapped->Pointer !=NULL && Overlapped[i].hEvent != INVALID_HANDLE_VALUE) { CloseHandle(Overlapped[i].hEvent); } }

		if (Overlapped != NULL && Overlapped->Pointer != NULL ) { delete[] Overlapped; }
	
		CloseDevice(&Handle);
}catch(...){}
}

VOID _USB_Device::Start(uint32_t TaktTime, ULONG FrameNumber)
{
	try {
		if (Status.Initialized[0] ==FALSE || Status.Errored[0] ==TRUE) throw "USB device in Error or not initialized";

		Status.Started[0] = TRUE;
		
		uint64_t previousTime = (uint64_t) TaktTime + 1;
		TotalBytesTransferred = 0;
		AvgTaktTimeMeasured = 0;
		
		while (RequestToStop[0] == FALSE)
		{		

			previousTime = std::chrono::duration_cast<std::chrono::microseconds>(std::chrono::high_resolution_clock::
				now().time_since_epoch()).count();
			BackBufferLocked[0] = TRUE;
			DWORD numBytes = 0;
			for (ULONG i = 0; i < Isoch_In_Transfer_Count; i++)	//Loop in case more than one transfer is to be made. 
											//https://docs.microsoft.com/en-us/windows-hardware/drivers/usbcon/getting-set-up-to-use-windows-devices-usb
											//ie. ISOCH_TRANSFER_COUNT in website above. Ie. 3 transfers of 10ms. In our setup only 1 transfer is required.
			{				
				if (FrameNumber == 0)
				{
					Status.Action.ReadIsochPipe[0] = (uint8_t) WinUsb_ReadIsochPipeAsap(IsochBufferHandle, i * Handle.IsochInTransferSize, Handle.IsochInTransferSize, (i == 0) ? FALSE : TRUE,
						Handle.IsochInPacketCount, &Packets[i * Handle.IsochInPacketCount], &Overlapped[i]);
				}
				else
				{
					Status.Action.ReadIsochPipe[0] = (uint8_t) WinUsb_ReadIsochPipe(IsochBufferHandle, i * Handle.IsochInTransferSize, Handle.IsochInTransferSize, &FrameNumber,
						Handle.IsochInPacketCount, &Packets[i * Handle.IsochInPacketCount], &Overlapped[i]);
				}

				if (Status.Action.ReadIsochPipe[0] == FALSE && GetLastError() != ERROR_IO_PENDING) { throw "Failed to start a read operation"; }
				//while(GetLastError() == ERROR_IO_PENDING);
				for (ULONG i = 0; i < Isoch_In_Transfer_Count; i++) //Likewise to the above comment ISOCH_TRANFER_COUNT in our case is 1
				{
					BOOL result = WinUsb_GetOverlappedResult(Handle.WinusbHandle, &Overlapped[i], &numBytes, TRUE);
					if (result == FALSE) {ResetPipe();/*throw "Failed to read";*/}
					TotalBytesTransferred += numBytes;
				}

			}
			
			
			BackBufferLocked[0] = FALSE;
			if (Status.Errored[0] == TRUE || Status.Initialized[0] == FALSE) throw "Buffers not initialized or errored.";
			BackBufferFilled[0] = 1; //Triggers C# program to begin moving memory
			while (BackBufferFilled[0] == 1 && Status.Started[0] == 1); //Wait for the C# to have finished copying under condition the USB is still started otherwise hanging
			
			while (std::chrono::duration_cast<std::chrono::microseconds>(std::chrono::high_resolution_clock::
				now().time_since_epoch()).count() - previousTime <= (uint64_t)TaktTime) {uint8_t numBytes=0; };
			
			AvgTaktTimeMeasured = (uint64_t) (AvgTaktTimeMeasured + std::chrono::duration_cast<std::chrono::microseconds>(std::chrono::high_resolution_clock::
				now().time_since_epoch()).count() - previousTime) / 2;

		}

		RequestToStop[0] = FALSE;
		Status.Started[0] = FALSE;
		Status.Stopped[0] = TRUE;
		//Here dispose would be misplaced. The ~ would entail to delete the Screen member itself not its members.
		//The members should be disposed off in the DLLmain on the thread wich created them.
		//Also dont use Dispose inside the class but only in the DLLmain
		//Dispose();

	} catch (char const* e)
	{
		Status.Started[0] = FALSE;
		Status.Stopped[0] = TRUE;
		Status.Errored[0] = TRUE;
		//Here dispose would be misplaced. The ~ would entail to delete the Screen member itself not its members.
		//The members should be disposed off in the DLLmain on the thread wich created them.
		//Also dont use Dispose inside the class but only in the DLLmain
		//https://stackoverflow.com/questions/44957240/when-to-delete-pointer-in-try-catch-block
		//Dispose();
	}
}

VOID _USB_Device::Stop()
{
	if(Status.Started[0] == TRUE && Status.Errored[0] ==FALSE) {RequestToStop[0] = TRUE;}
}

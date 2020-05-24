#include "pch.h"
#include "USB.h"

VOID USB::USB_Init()
{
    ReadyToRead = false;
    BeginReading = false;
    EndReading = false;
    HasReadbufferTransfered = false;
    ReadyToReportState = false;
    ReportedState = false;
    CloseRequest = false;

    if (!CallOpenDevice()) { goto error; };
    if (!CallGetDescriptor()) { goto error; };
    if (!CallQueryInterfaceSettings()) { goto error; };
    if (!GetIsochPipes()) { goto error; };
    if (!GetIntervalInformation()) { goto error; };
    if (!SetIsochTransferReady()) { goto error; };
    if (!PrintSomeInterfaceSettings()) { goto error; };
    if (!CallResetPipe()) { goto error; };
    ReadyToReportState = true; //Some state settings are depending on SendIsochIntransfer   
    ReadyToRead = true; //Set signal Ready to Transfer

    do {
        if (BeginReading == (BOOL)true) //Wait for signal and keep loop running to avoid lock
        {
            EndReading = false;
            if (!SendIsochInTransfer()) { goto error; } //Read data from ADC
            EndReading = true; //Give signal data is read in from ADC
            while (HasReadbufferTransfered == false); //Wait for signal data is transferred  
            HasReadbufferTransfered = false;
        }
    } while (CloseRequest == false);
error:
    ReadyToReportState = true;
    do {

    } while (CloseRequest == false);
    Dispose();
}



VOID USB::USB_Result_Init()
{
    USB_Result.s.CallOpenDevice = 0;
    USB_Result.s.CallGetDescriptor = 0;
    USB_Result.s.CallQueryInterfaceSettings = 0;
    USB_Result.s.QueryPipe = 0;
    USB_Result.s.GetIntervalInformation = 0;

    USB_Result.s.noDevice = 0;
    USB_Result.s.ReadTransfer = 0;
    USB_Result.s.EndAtFrame = true;
    USB_Result.s.AllocateMemory = 0;
    USB_Result.s.RegisterIsochBuffer = 0;
    USB_Result.s.CallResetPipe = 0;
    USB_Result.s.SetOverlappedResult = 0;
    USB_Result.s.CallGetFrameNumber = 0;
    USB_Result.s.ReadIsochPipe = 0;
    USB_Result.s.GetOverlappedResult = 0;
    USB_Result.s.TransferCompleted = 0;
    USB_Result.s.UnregisterIsochBuffer = 0;
    USB_Result.s.FoundPipe = 0;
    USB_Result.s.FailedToStartRead = 0;
}

VOID USB::USB_hResult_Init()
{
    USB_hResult.s.GetIsochPipes = 0;
    USB_hResult.s.QueryPipe = 0;
    USB_hResult.s.GetIntervalInformation = 0;
}



BOOL USB::CallOpenDevice()
{
    USB_hResult.s.OpenDevice = OpenDevice(&deviceData, &USB_Result.s.noDevice);
    USB_Result.s.CallOpenDevice = (USB_hResult.s.OpenDevice < 0) ? false : true;
 
    return USB_Result.s.CallOpenDevice;
}
BOOL USB::CallGetDescriptor()
{
    USB_Result.s.CallGetDescriptor = WinUsb_GetDescriptor(deviceData.WinusbHandle, USB_DEVICE_DESCRIPTOR_TYPE, 0, 0, (PBYTE)&deviceDesc, sizeof(deviceDesc), &lengthReceived);
    
    if (USB_Result.s.CallGetDescriptor == FALSE || lengthReceived != sizeof(deviceDesc)) {
        USB_Result.s.CallGetDescriptor = false;
        CloseDevice(&deviceData);
    }

    return USB_Result.s.CallGetDescriptor;
}
BOOL USB::CallQueryInterfaceSettings()
{
    USB_Result.s.CallQueryInterfaceSettings = WinUsb_QueryInterfaceSettings(deviceData.WinusbHandle, 0, &interfaceDesc);

    if (FALSE == USB_Result.s.CallQueryInterfaceSettings) {
        CloseDevice(&deviceData);
    }
    return USB_Result.s.CallQueryInterfaceSettings;
}
BOOL USB::PrintSomeInterfaceSettings()
{
    USB_Device.bcdDevice = deviceDesc.bcdUSB;
    USB_Device.idProduct = deviceDesc.idProduct;
    USB_Device.idVendor = deviceDesc.idVendor;
    /* wprintf(L"Device found: VID_%04X&PID_%04X; bcdUsb %04X\n",
        deviceDesc.idVendor,
        deviceDesc.idProduct,
        deviceDesc.bcdUSB);*/
    return true;
}

BOOL USB::GetIsochPipes()
{
    UCHAR i;
    USB_hResult.s.QueryPipe = S_OK;
    USB_Result.s.FoundPipe = false;
    for (i = 0; i < interfaceDesc.bNumEndpoints; i++)
    {
        USB_Result.s.QueryPipe = WinUsb_QueryPipeEx(deviceData.WinusbHandle, 0,(UCHAR)i, &deviceData.pipe);

        if (USB_Result.s.QueryPipe == FALSE)
        {
            USB_hResult.s.QueryPipe = HRESULT_FROM_WIN32(GetLastError());
            USB_Result.s.FoundPipe = false;
            i = interfaceDesc.bNumEndpoints;
        } else if (deviceData.pipe.PipeType == UsbdPipeTypeIsochronous)
        {
            deviceData.IsochInPipe = deviceData.pipe.PipeId;
            USB_Result.s.FoundPipe = true;
        }
    }
    if (USB_Result.s.FoundPipe == false) {

        USB_Result.s.QueryPipe = false;
        CloseHandle(deviceData.DeviceHandle);
        CloseDevice(&deviceData);
    }
    return USB_Result.s.QueryPipe;
}

BOOL USB::GetIntervalInformation() {
    USB_hResult.s.GetIntervalInformation = S_OK;
    USB_Result.s.GetIntervalInformation = true;
    if (deviceData.pipe.MaximumBytesPerInterval == 0 || (deviceData.pipe.Interval == 0))
    {
        USB_hResult.s.GetIntervalInformation = E_INVALIDARG;
        USB_Result.s.GetIntervalInformation = false;
        CloseHandle(deviceData.DeviceHandle);
        CloseDevice(&deviceData);
    }
    return USB_Result.s.GetIntervalInformation;
}
BOOL USB::SetIsochTransferReady() {
    readBuffer = NULL;
    isochPackets = NULL;
    overlapped = NULL;
    isochReadBufferHandle = INVALID_HANDLE_VALUE;

    deviceData.IsochInBytesPerInterval = (ULONG)deviceData.pipe.MaximumBytesPerInterval;
    deviceData.IsochInBytesPerFrame = deviceData.IsochInBytesPerInterval * (ULONG)deviceData.pipe.Interval;
    deviceData.IsochInTransferSize = deviceData.IsochInBytesPerFrame * ISOCH_DATA_SIZE_MS;
    Sizeof_USB_readbuffer = deviceData.IsochInTransferSize * (RINGBUFFER1023 / deviceData.IsochInTransferSize); //taking max possible for 1023bytes // Isoch_Transfer_Count;
    deviceData.IsochInPacketCount = ISOCH_DATA_SIZE_MS * (RINGBUFFER1023 / 1023); //taking max packet count Isoch_Transfer_Count;
    Isoch_Transfer_Count_Max = RINGBUFFER1023 / 1023;

    USB_Result.s.EndAtFrame = true;
    if (Sizeof_USB_readbuffer % deviceData.IsochInBytesPerFrame != 0)
    {
        USB_Result.s.EndAtFrame = false;
        return USB_Result.s.TransferCompleted;
    }

    readBuffer = new UCHAR[Sizeof_USB_readbuffer];
    USB_Result.s.AllocateMemory = true;
    if (readBuffer == NULL)
    {
        USB_Result.s.AllocateMemory = false;
        return USB_Result.s.TransferCompleted;
    }
    ZeroMemory(readBuffer, Sizeof_USB_readbuffer);

    overlapped = new OVERLAPPED[Isoch_Transfer_Count_Max];
    ZeroMemory(overlapped, Isoch_Transfer_Count_Max * (sizeof(OVERLAPPED)));
    USB_Result.s.SetOverlappedResult = true;
    for (ULONG i = 0; i < Isoch_Transfer_Count_Max; i++)
    {
        overlapped[i].hEvent = CreateEvent(NULL, TRUE, FALSE, NULL);

        if ((*overlapped).hEvent == NULL)
        {
            USB_Result.s.SetOverlappedResult = false;
            return USB_Result.s.TransferCompleted;
        }
    }

    isochPackets = new USBD_ISO_PACKET_DESCRIPTOR[Isoch_Transfer_Count_Max * 1];
    ZeroMemory(isochPackets, Isoch_Transfer_Count_Max * 1 * sizeof(USBD_ISO_PACKET_DESCRIPTOR));
    USB_Result.s.RegisterIsochBuffer = WinUsb_RegisterIsochBuffer(deviceData.WinusbHandle, deviceData.IsochInPipe, readBuffer,
        Isoch_Transfer_Count_Max * deviceData.IsochInTransferSize, &isochReadBufferHandle);

    if (!USB_Result.s.RegisterIsochBuffer)
    {
        return USB_Result.s.TransferCompleted;
    }


    USB_Result.s.CallGetFrameNumber = WinUsb_GetCurrentFrameNumber(deviceData.WinusbHandle, &frameNumber, &timeStamp);

    if (!USB_Result.s.CallGetFrameNumber)
    {
        return USB_Result.s.TransferCompleted;
    }

    startFrame = frameNumber + 2;
    return true;

}


BOOL USB::SendIsochInTransfer() {


    
    USB_Result.s.TransferCompleted = false;
    USB_Result.s.ReadTransfer = true;
    USB_Result.s.FailedToStartRead = false;

    for (ULONG i = 0; i < Isoch_Transfer_Count; i++)
    {
        AsapTransfer = TRUE;
        if (AsapTransfer)
        {
           // Sleep(1);
            USB_Result.s.ReadIsochPipe = WinUsb_ReadIsochPipeAsap(isochReadBufferHandle,i * deviceData.IsochInTransferSize,deviceData.IsochInTransferSize, (i == 0) ? FALSE : TRUE, 1, &isochPackets[i * 1], &overlapped[i]);
        }
 /*       else
        {
            printf("Transfer starting at frame %d.\n", startFrame);
            result = WinUsb_ReadIsochPipe(isochReadBufferHandle,DeviceData->IsochInTransferSize * i,DeviceData->IsochInTransferSize,&startFrame,DeviceData->IsochInPacketCount,&isochPackets[i * DeviceData->IsochInPacketCount],&overlapped[i]);
            printf("Next transfer frame %d.\n", startFrame);
        }*/

        if (!USB_Result.s.ReadIsochPipe)
        {
            result_lastError = GetLastError();

            if (result_lastError != ERROR_IO_PENDING)
            {
                USB_Result.s.FailedToStartRead = true;
            }
        }
    }
    numBytes = 0;
    numBytes = 0; numBytes1 = 0;
    for (ULONG i = 0; i < Isoch_Transfer_Count; i++) {
        USB_Result.s.GetOverlappedResult = WinUsb_GetOverlappedResult(deviceData.WinusbHandle,&overlapped[i],&numBytes1,TRUE);

        if (!USB_Result.s.GetOverlappedResult)
        {
            result_lastError = GetLastError();
        }
    }

 //   Allow system time to count the numBytes before killing the thread
    while (numBytes < (Isoch_Transfer_Count * deviceData.IsochInTransferSize)) {
        //removed the Getoverlappedresult for performance reasons. This resulted in blue screen WDF-vialoation. 
        //With now includeing a while loop to wait for results to come in, blue screen is not occuring anymore.
    for (ULONG i = 0; i < Isoch_Transfer_Count; i++) {
        numBytes += isochPackets[i].Length;
    }
    }
    USB_Result.s.TransferCompleted = true;

    //printf("Transfer completed. Read %d bytes. \n\n", numBytes);
    //printf("Done\n\n");


    return USB_Result.s.TransferCompleted;
    
}

BOOL USB::CallResetPipe(void) {
    USB_Result.s.CallResetPipe = WinUsb_ResetPipe(deviceData.WinusbHandle, deviceData.IsochInPipe);

    return USB_Result.s.CallResetPipe;
}
VOID USB::ReportState(void) {

}

VOID USB::Dispose(void) {

    ReadyToRead = false;
    if (isochReadBufferHandle != INVALID_HANDLE_VALUE)
    {
        USB_Result.s.UnregisterIsochBuffer = WinUsb_UnregisterIsochBuffer(isochReadBufferHandle);
        if (!USB_Result.s.UnregisterIsochBuffer)
        {
            //("Failed to unregister isoch read buffer. \n");
        }
    }

    if (readBuffer != NULL)
    {
        delete[] readBuffer;
    }

    if (isochPackets != NULL)
    {
        delete[] isochPackets;
    }

    for (ULONG i = 0; i < Isoch_Transfer_Count; i++)
    {
        if (overlapped != NULL){
            if (overlapped[i].hEvent != NULL)
            {
                CloseHandle(overlapped[i].hEvent);
            }
        }
    }

    if (overlapped != NULL)
    {
        delete[] overlapped;
    }

    CloseDevice(&deviceData);
}

uint64_t USB::BinExtract(uint8_t* bfr, uint32_t start, uint32_t length)
{
    uint32_t i = 0;
    uint32_t pos = 0;

    uint64_t content = 0;

    for (uint32_t c = 0; c < length; c++) {
        i = (start+c) / 8;
        pos = (start+c) % 8;
        content = content | ((uint64_t)(bfr[i] >> pos) & (uint64_t)0x01)<<c;
    }

    return content;
}

void USB::BinConcat(uint8_t* bfr, uint32_t start, uint32_t length, uint64_t* content)
{
    for (uint32_t l = start; l < start + length; l++)
    {

        uint32_t i = l / 8;
        uint32_t pos = l % 8;


        bfr[i] = bfr[i] | ((uint8_t)((uint64_t)0x01 & (*content >> (l - start))) << (pos));
    }
}

BOOL USB::ReadbufferTransfered(UCHAR* USB_readbuffer, uint32_t* _ADC_Reading, uint32_t* _ADC_Tick, int32_t RB_L, int32_t RB_U)
{
    UCHAR* readbuffer;


    readbuffer = USB_readbuffer;


    uint32_t EndPos = 0; uint8_t Found = 0;
    uint32_t ADC_Reading_Pos = RB_L;
    uint32_t ADC_Tick_Pos = RB_L;

    uint64_t Measure = 0;
    uint64_t Tick = 0;


//     memcpy(readbuffer, readBuffer, 4092);
    while (EndPos < 8 * deviceData.IsochInTransferSize * Isoch_Transfer_Count - CheckPatternLength - MeasurePatternLength - TickPatternLength && ADC_Reading_Pos <= (uint32_t)RB_U)
    {
        Found = 0;
        while (Found != 1 && EndPos < 8 * deviceData.IsochInTransferSize * Isoch_Transfer_Count - CheckPatternLength - MeasurePatternLength - TickPatternLength && ADC_Reading_Pos <= (uint32_t)RB_U)
        {
            if (BinExtract(readBuffer, EndPos, CheckPatternLength) == (uint64_t)CheckPattern) { Found = 1; }
            EndPos++; //EndPos will hold the pointer at starting point
        }
        if (Found == 1) {
            EndPos -= 1; //to offset last EndPos++ in loop
            EndPos += CheckPatternLength;

            Measure = BinExtract(readBuffer, EndPos, MeasurePatternLength); EndPos += MeasurePatternLength;
            Tick = BinExtract(readBuffer, EndPos, TickPatternLength); EndPos += TickPatternLength;
            ADC_Reading[ADC_Reading_Pos] = (uint32_t)Measure; ADC_Reading_Pos++;
            ADC_Tick[ADC_Tick_Pos] = (uint32_t)Tick; ADC_Tick_Pos++;

        }
    }

    for (int32_t i = RB_L; i <= RB_U; i++)
    {
        _ADC_Reading[i] = ADC_Reading[i];
        _ADC_Tick[i] = ADC_Tick[i];
    }


    //       USB_readbuffer = readBuffer;
    //       Sizeof_USB_Readbuffer = &Sizeof_USB_readbuffer;
    return true; //the return here is to the C# program
    //The ReadbufferTransferred boolean of the USB program is set by different instruction SetReadBufferTransferred

}
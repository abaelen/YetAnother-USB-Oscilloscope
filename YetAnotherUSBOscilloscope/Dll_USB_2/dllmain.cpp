// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include "USB.h"
#include <vector>

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

_USB_Device USB_Device;
    //To use the Class create a _USB_Device USB_Device member. Then call the USB_Device.Initialize(1023,1,1) eg. 1023 in 1 packet & 1 microframe (as only FS) or USB_Device.Initialize(2046,2,1), ...    

//Below exposes all the public members and methods for external use. Call these from for example C# using
    
    /* [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_2.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void USB2_Device_Initialize(UInt32 Buffersize, UInt32 PacketSize, UInt32 MicroFrameSize);
    
    [DllImport(@"C:\Users\Gebruiker\Documents\OneDrive\Elec Projects\Github\YetAnother-USB-Oscilloscope\YetAnotherUSBOscilloscope\x64\Debug\Dll_USB_2.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void USB2_Device_Start(UInt32 TaktTime, int[] pRequestToStop = null, ulong FrameNumber = 0);*/



    //Keep data out of the dll export functions, as it increases speed of execution
    //To get values, pass by value
    //To send values, pass a pointer and set the value of the memory space of the pointer

extern "C" {
      __declspec(dllexport) uint8_t USB2_Device_Initialize(int16_t* pBuffer16, uint32_t bufferSize8, uint32_t backBufferSize8, uint32_t packetSize, uint32_t microFrameSize)
    {
        USB_Device.Initialize(pBuffer16, bufferSize8, backBufferSize8, packetSize, microFrameSize);
        return (uint8_t) USB_Device.Status.Initialized[0];
    }
    __declspec(dllexport) void USB2_Device_Start( uint32_t TaktTime, ULONG FrameNumber = 0)
    {
        USB_Device.Start(TaktTime,FrameNumber);
        USB_Device.Dispose(); 
        //Here dispose would be misplaced. The ~ would entail to delete the Screen member itself not its members.
        //The members should be disposed off in the DLLmain on the thread wich created them.
        //Also dont use Dispose inside the class but only in the DLLmain
        //https://stackoverflow.com/questions/44957240/when-to-delete-pointer-in-try-catch-block
    }
    __declspec(dllexport) void USB2_Device_Status_RequestToStop(uint8_t* pRequestToStop)
    {
        USB_Device.RequestToStop = pRequestToStop;
    }
    __declspec(dllexport) void USB2_Device_Stop()
    {
        USB_Device.Stop();
    }
    __declspec(dllexport) void USB2_Device_Dispose()
    {
        USB_Device.Dispose();
    }
    __declspec(dllexport) void USB2_Device_Status_Started(uint8_t* pStarted)
    {
        USB_Device.Status.Started = pStarted;
        //return (uint8_t)USB_Device.Status.Started;
    }
    __declspec(dllexport) void USB2_Device_Status_Stopped(uint8_t* pStopped)
    {
        USB_Device.Status.Stopped = pStopped;
        //return (uint8_t)USB_Device.Status.Stopped;
    }
    __declspec(dllexport) void USB2_Device_Status_Initialized(uint8_t* pInitialized)
    {
        USB_Device.Status.Initialized = pInitialized;
        //return (uint8_t)USB_Device.Status.Initialized;
    }
    __declspec(dllexport) void USB2_Device_Status_Errored(uint8_t* pErrored)
    {
        USB_Device.Status.Errored = pErrored;
        //return (uint8_t)USB_Device.Status.Errored;
    }
    __declspec(dllexport) void USB2_Device_Status_Action_DeviceFound(uint8_t* pDeviceFound)
    {
        USB_Device.Status.Action.DeviceFound = pDeviceFound;
        //return (uint8_t) USB_Device.Status.Action.DeviceFound[0];
    }
    __declspec(dllexport) void USB2_Device_Status_Action_GetDescriptor(uint8_t* pGetDescriptor)
    {
        USB_Device.Status.Action.GetDescriptor = pGetDescriptor;
        //return (uint8_t)USB_Device.Status.Action.GetDescriptor;
    }
    __declspec(dllexport) void USB2_Device_Status_Action_GetInterface(uint8_t* pGetInterface)
    {
        USB_Device.Status.Action.GetInterface= pGetInterface;
        //return (uint8_t)USB_Device.Status.Action.GetInterface;
    }
    __declspec(dllexport) void USB2_Device_Status_Action_GetIsochPipe(uint8_t* pGetIsochPipe)
    {
        USB_Device.Status.Action.GetIsochPipe= pGetIsochPipe;
        //return (uint8_t)USB_Device.Status.Action.GetIsochPipe;
    }
    __declspec(dllexport) void USB2_Device_Status_Action_GetInterval(uint8_t* pGetInterval)
    {
        USB_Device.Status.Action.GetInterval= pGetInterval;
        //return (uint8_t)USB_Device.Status.Action.GetInterval;
    }
    __declspec(dllexport) void USB2_Device_Status_Action_SetTransferChars(uint8_t* pSetTransferChars)
    {
        USB_Device.Status.Action.SetTransferChars= pSetTransferChars;
        //return (uint8_t)USB_Device.Status.Action.SetTransferChars;
    }
    __declspec(dllexport) void USB2_Device_Status_Action_SetOverlappedStructure(uint8_t* pSetOverlappedStructure)
    {
        USB_Device.Status.Action.SetOverlappedStructure= pSetOverlappedStructure;
        //return (uint8_t)USB_Device.Status.Action.SetOverlappedStructure;
    }
    __declspec(dllexport) void USB2_Device_Status_Action_SetOverlappedEvents(uint8_t* pSetOverlappedEvents)
    {
        USB_Device.Status.Action.SetOverlappedEvents= pSetOverlappedEvents;
        //return (uint8_t)USB_Device.Status.Action.SetOverlappedEvents;
    }
    __declspec(dllexport) void USB2_Device_Status_Action_SetIsochPackets(uint8_t* pSetIsochPackets)
    {
        USB_Device.Status.Action.SetIsochPackets= pSetIsochPackets;
        //return (uint8_t)USB_Device.Status.Action.SetIsochPackets;
    }
    __declspec(dllexport) void USB2_Device_Status_Action_RegisterIsochBuffer(uint8_t* pRegisterIsochBuffer)
    {
        USB_Device.Status.Action.RegisterIsochBuffer= pRegisterIsochBuffer;
        //return (uint8_t)USB_Device.Status.Action.RegisterIsochBuffer;
    }
    __declspec(dllexport) void USB2_Device_Status_Action_ResetPipe(uint8_t* pResetPipe)
    {
        USB_Device.Status.Action.ResetPipe= pResetPipe;
       //return (uint8_t)USB_Device.Status.Action.ResetPipe;
    }
    __declspec(dllexport) void USB2_Device_Status_Action_EndAtFrame(uint8_t* pEndAtFrame)
    {
        USB_Device.Status.Action.EndAtFrame= pEndAtFrame;
        //return (uint8_t)USB_Device.Status.Action.EndAtFrame;
    }
    __declspec(dllexport) void USB2_Device_Status_Action_ReadIsochPipe(uint8_t* pReadIsochPipe)
    {
        USB_Device.Status.Action.ReadIsochPipe= pReadIsochPipe;
        //return (uint8_t)USB_Device.Status.Action.ReadIsochPipe;
    }
    __declspec(dllexport) void USB2_Device_BackBufferFilled(uint8_t* pBackBufferFilled)
    {
        USB_Device.BackBufferFilled = pBackBufferFilled;
        //return (uint8_t)USB_Device.BackBufferFilled;
    }
    __declspec(dllexport) void USB2_Device_BackBufferLocked(uint8_t* pBackBufferLocked)
    {
        USB_Device.BackBufferLocked = pBackBufferLocked;
    }
}
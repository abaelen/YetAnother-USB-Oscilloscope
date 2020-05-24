// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include "USB.h"





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

USB U;

//Keep data out of the dll export functions, as it increases speed of execution

extern "C" {
    __declspec(dllexport) void USB_Init()
    {
        U.USB_Init();
    }

    __declspec(dllexport) BOOL USB_ReadyToRead()
    {
        return U.ReadyToRead;
    }
    __declspec(dllexport) BOOL USB_SetBeginReading(bool BeginReading)
    {
        U.BeginReading = BeginReading;
        return true;
    }
    __declspec(dllexport) BOOL USB_GetEndReading()
    {
        return U.EndReading;
    }
    __declspec(dllexport) BOOL USB_GetState(int* InResults, INT64* InhResults, uint16_t* USBVendor, uint16_t* USBProduct, uint16_t* USBbcd)
    {
       //InResults = U.USB_Result.a;
       //Values can not be passed by ref, but need to be passed into the memory space of provided pointer
        // ie. the calling thread exposes its memory to called DLL via pointer to native datatype! (eg. bool does not work)
       for (uint16_t i = 0; i < U.Sizeof_USB_Result; i++)
       {
           InResults[i] = U.USB_Result.a[i];
       }
       for (uint16_t i = 0; i < U.Sizeof_USB_hResult; i++)
       {
           InhResults[i] = U.USB_hResult.a[i];
       }
       *USBVendor = U.USB_Device.idVendor;
       *USBProduct = U.USB_Device.idProduct;
       *USBProduct = U.USB_Device.bcdDevice;

       U.ReportedState = true;
       return true;
    }
    __declspec(dllexport) bool USB_Sizeof_USB_Result(uint16_t* InSizeof_USB_Results, uint16_t* InSizeof_USB_hResults)
    {
        InSizeof_USB_Results[0] = U.Sizeof_USB_Result;
        InSizeof_USB_hResults[0] = U.Sizeof_USB_hResult;
        return true;
    }
    __declspec(dllexport) VOID USB_SetCloseRequest()
    {
        U.CloseRequest = true;
    }
    __declspec(dllexport) BOOL USB_ReadbufferTransfered(UCHAR* USB_readbuffer, uint32_t* _ADC_Reading, uint32_t* _ADC_Tick, int32_t RB_L, int32_t RB_U)
    {
        U.ReadbufferTransfered(USB_readbuffer, _ADC_Reading, _ADC_Tick, RB_L, RB_U);
        return true;
    }
    __declspec(dllexport) BOOL USB_SetReadbufferTransfered()
    {
        U.HasReadbufferTransfered = true; //Here the signal is given from C# to continue, this allowed C# to process the data remainin unaltered as pass is by ref
        return true;
    }
    __declspec(dllexport) VOID USB_Dispose()
    {
        U.Dispose();
    }
    __declspec(dllexport) BOOL USB_ReadyToReportState()
    {
        return U.ReadyToReportState;
    }
    __declspec(dllexport) BOOL USB_Set_Isoch_Transfer(uint32_t NumberOfSamples, uint32_t ADC_Check_Pattern, uint32_t ADC_Check_Pattern_Length, uint32_t ADC_Measure_Length, uint32_t ADC_Tick_Length)
    {
        U.CheckPattern = ADC_Check_Pattern;
        U.CheckPatternLength = ADC_Check_Pattern_Length;
        U.MeasurePatternLength = ADC_Measure_Length;
        U.TickPatternLength = ADC_Tick_Length;
        U.Isoch_Transfer_Count = (uint32_t)ceil((double)NumberOfSamples * ((double)U.CheckPatternLength + (double)U.MeasurePatternLength + (double)U.TickPatternLength) / 8 / (double)(U.deviceData.IsochInBytesPerInterval));
        return true;
    }
    __declspec(dllexport) uint32_t USB_GetIsochTransferCount()
    {
        return U.Isoch_Transfer_Count;
    }
}
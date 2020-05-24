#pragma once
#include "pch.h"
#include <stdio.h>
#include <time.h>
#include <stdint.h>

#define ISOCH_DATA_SIZE_MS   1

class USB
{
public:

    const uint32_t RINGBUFFER = 262144;
    uint32_t RINGBUFFER1023 = RINGBUFFER / 1024 * 1023;

    DEVICE_DATA           deviceData;
    HRESULT               hr;
    USB_DEVICE_DESCRIPTOR deviceDesc;
    USB_INTERFACE_DESCRIPTOR    interfaceDesc;
    HRESULT hresult_OpenDevice = 0;
    
    union _USB_Result_u {
        struct _USB_Result_s {
            BOOL CallOpenDevice;
            BOOL CallGetDescriptor;
            BOOL CallQueryInterfaceSettings;
            BOOL QueryPipe;
            BOOL GetIntervalInformation;          
            BOOL ReadTransfer;
            BOOL EndAtFrame;
            BOOL AllocateMemory;
            BOOL RegisterIsochBuffer;
            BOOL CallResetPipe;
            BOOL SetOverlappedResult;
            BOOL CallGetFrameNumber;
            BOOL ReadIsochPipe;
            BOOL GetOverlappedResult;
            BOOL TransferCompleted;
            BOOL UnregisterIsochBuffer;
            BOOL FoundPipe;
            BOOL FailedToStartRead;
            BOOL noDevice;
        } s;
        int a[19];
    } USB_Result;
    uint16_t Sizeof_USB_Result=19;

    union _USB_hResult_u {
        struct _USB_hResult_s {
            HRESULT OpenDevice;
            HRESULT GetIsochPipes;
            HRESULT QueryPipe;
            HRESULT GetIntervalInformation;
        } s;
        INT64 a[4];
    }USB_hResult;
    uint16_t Sizeof_USB_hResult = 4;

    struct _USB_Device {
        USHORT  idVendor;
        USHORT  idProduct;
        USHORT  bcdDevice;
    } USB_Device;

    DWORD result_lastError = 0;

    ULONG lengthReceived = 0;
    BOOL AsapTransfer = true;

    PUCHAR readBuffer;
    uint32_t Sizeof_USB_readbuffer;
    uint32_t Isoch_Transfer_Count = 0;
    uint32_t Isoch_Transfer_Count_Max=RINGBUFFER/1023;

    uint32_t* ADC_Reading = new uint32_t[RINGBUFFER];
    uint32_t* ADC_Tick = new uint32_t[RINGBUFFER];

    uint32_t CheckPattern = 0x55;
    uint32_t CheckPatternLength = 8; uint32_t MeasurePatternLength = 16; uint32_t TickPatternLength = 16;

    LPOVERLAPPED overlapped;
    ULONG numBytes = 0;
    ULONG numBytes1;
    WINUSB_ISOCH_BUFFER_HANDLE isochReadBufferHandle;
    PUSBD_ISO_PACKET_DESCRIPTOR isochPackets;
    clock_t start_time = clock();
    ULONG frameNumber;
    ULONG startFrame;
    LARGE_INTEGER timeStamp;
    ULONG totalTransferSize;

    BOOL ReadyToRead = 0;
    BOOL ReadyToReportState = 0;
    BOOL HasReadbufferTransfered = 0;
    BOOL EndReading = 0;

    BOOL CloseRequest = 0;
    BOOL ReportedState = 0;
    BOOL BeginReading=0;

    VOID USB_Result_Init(void);
    VOID USB_hResult_Init(void);

    void USB_Init(void);
    BOOL GetIsochPipes(void);
    BOOL GetIntervalInformation(void);
    BOOL SetIsochTransferReady(void);
    BOOL SendIsochInTransfer(void);
    BOOL CallOpenDevice(void);
    BOOL CallGetDescriptor(void);
    BOOL CallQueryInterfaceSettings(void);
    BOOL PrintSomeInterfaceSettings(void);
    BOOL CallResetPipe(void);
    BOOL ReadbufferTransfered(UCHAR* USB_readbuffer, uint32_t* ADC_Reading, uint32_t* ADC_Tick, int32_t RB_L, int32_t RB_U);

    uint64_t BinExtract(uint8_t* bfr, uint32_t start, uint32_t length);
    void BinConcat(uint8_t* bfr, uint32_t start, uint32_t length, uint64_t* content);

    VOID ReportState(void);
    VOID Dispose(void);
    
};
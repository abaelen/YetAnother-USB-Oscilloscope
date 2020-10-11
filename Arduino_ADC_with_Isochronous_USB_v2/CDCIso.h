/*
 * CDCIso.h
 *
 * Created: 23/04/2020 10:34:00
 *  Author: Gebruiker
 */ 


#ifndef CDCISO_H_
#define CDCISO_H_

#include <Arduino.h>
#include <USB/PluggableUSB.h>
#include <USB/USBCore.h>

typedef struct
{
//  Iso
  InterfaceDescriptor     dif;
  EndpointDescriptor      in;
  EndpointDescriptor      out;
} CDCIsoDescriptor;

union MemAddr {
  uint32_t* b32;
  uint8_t* b8;
};

class CDCIso_ : public PluggableUSBModule {
public:
  CDCIso_(void);
  bool begin(void);
  void enableInterrupt();
  size_t write(const uint8_t *buffer, size_t size);
  
  static const uint32_t Iso_TransferSize = 420; //limit to 1020, 1020 / 4byte = 511 datasets = 2byte ADC reading + 2byte Tick data
  static const uint8_t NumberOfPackets = 50;
   __attribute__((__aligned__(4))) uint8_t WriteBuffer[Iso_TransferSize];
  uint8_t Iso_PckCode;  
  __attribute__((__aligned__(4))) uint8_t DataBuffer[Iso_TransferSize]; //__attribute__((__section__(".bss_hram0"))) 
    /*Bits 31:0 – ADDR[31:0]: Data Pointer Address Value
    These bits define the data pointer address as an absolute word address in RAM.The two least significant bits must
    be zero to ensure the start address is 32-bit aligned.*/
     
  uint32_t Iso_PckSize;
  uint32_t pos=1021;
  uint16_t testiteration=0;
  uint16_t teststep=0;
  uint16_t teststep2=11730;

  uint32_t UsbConfiguration = 0;
  
  uint8_t epEPNum[2]; 
  USBSetup EP0_BK0;
  uint32_t* ptrEP0_BK0;
  MemAddr EP0_BK0_Addr;
  MemAddr EPn_BK1_Addr;

  union _ADC_Reg
  {
    uint8_t b8[2];
    uint16_t b16;
  } ADC_Reg;

  void ADC_init();
  uint8_t *ADCregH;
  uint8_t *ADCregL;

 
protected:
  int getInterface(uint8_t* interfaceNum);
    bool setup(USBSetup& setup);
  int getDescriptor(USBSetup& setup);
  void handleEndpoint(int ep);
  
private:
  uint32_t epIsoType[2];
  uint32_t send(uint32_t ep, const void *data, uint32_t len);
  
};


#endif /* CDCISO_H_ */

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
  
  
  uint8_t writebuffer[1023];
  uint8_t lenwritebuffer;
  uint8_t databuffer[1023];
    
  uint32_t iso_pcksize;
  uint8_t iso_pckcode;
  
  uint32_t usbConfiguration = 0;
  
  uint8_t epEPNum[2]; 
  USBSetup EP0_BK0;
  uint32_t* ptrEP0_BK0;
  MemAddr EP0_BK0_Addr;
  MemAddr EPn_BK1_Addr;

//USER ADDED //

    uint32_t ADC_Reading = 0; //10-bit
    uint32_t ADC_Tick = 255; //13-bit
    uint32_t ADC_Chk = 85; //8-bit

  
  uint8_t ADC_Bfr[1024];
    uint32_t ADC_Bfr_c=0;
  
  union _ADC_Reg
  {
    uint8_t b8[2];
    uint16_t b16;
  } ADC_Reg;
    _ADC_Reg ADC_reg[1024];
    uint32_t ADC_rTick[1024];   
    
  uint16_t ADC_Reading_Size = 12;
    uint16_t ADC_Tick_Size = 12;
    uint16_t ADC_Chk_Size = 7;

    uint8_t Continue=0;

  uint16_t Iteration = (uint16_t) (1024.0f / (float) (ADC_Reading_Size+ADC_Tick_Size+ADC_Chk_Size)*8.0f);
    
    uint64_t CSmpl=0;
    uint32_t TSmpl=255;
    void BinConcat(uint8_t* bfr, uint32_t start, uint32_t length, uint64_t* content);

    //USER*************


//*****************//

  
protected:
  int getInterface(uint8_t* interfaceNum);
    bool setup(USBSetup& setup);
  int getDescriptor(USBSetup& setup);
  void handleEndpoint(int ep);
  
private:
  uint32_t epIsoType[2];
  uint32_t send(uint32_t ep, const void *data, uint32_t len);
  
  char LastTransmitTimedOut[2] = {0,0};
};


#endif /* CDCISO_H_ */

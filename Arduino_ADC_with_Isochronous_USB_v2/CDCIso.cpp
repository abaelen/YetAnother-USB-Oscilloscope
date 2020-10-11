/*
 * CDCIso.cpp
 *
 * Created: 23/04/2020 10:34:21
 *  Author: Gebruiker
 */ 

#include <arduino.h>
#include "CDCIso.h"
#include <USB/USBCore.h>
#include <USB/PluggableUSB.h>
#include <USB/CDC.h>

CDCIso_::CDCIso_(void) : PluggableUSBModule(2,2,epIsoType)
{
  epIsoType[0] = USB_ENDPOINT_TYPE_BULK | USB_ENDPOINT_IN(0);
  epIsoType[1] = USB_ENDPOINT_TYPE_ISOCHRONOUS | USB_ENDPOINT_IN(0);
    PluggableUSB().plug(this);
}

int CDCIso_::getInterface(uint8_t* interfaceNum)
{

  interfaceNum[0] += 1;
  
  epEPNum[0]=pluggedEndpoint;
  epEPNum[1]=pluggedEndpoint+1;
  
  CDCIsoDescriptor _cdcIsoInterface = {

    // CDC Iso interface
    D_INTERFACE(uint8_t(pluggedInterface + 1), 2, 0xFF, 0, 0),
    D_ENDPOINT(USB_ENDPOINT_OUT(pluggedEndpoint), USB_ENDPOINT_TYPE_BULK, EPX_SIZE, 0),
    D_ENDPOINT(USB_ENDPOINT_IN (pluggedEndpoint+1), USB_ENDPOINT_TYPE_ISOCHRONOUS, Iso_TransferSize, 1) //Limit the max transfer to 1020 otherwise Windows will packetize to max allowed and throw insufficient buffer when reducing transfers to 1020. 1020 is choosen as it allows for 511 x 4byte transfer (2byte ADC + 2byte tick)
  };
  return USBDevice.sendControl(&_cdcIsoInterface, sizeof(_cdcIsoInterface));    

}

bool CDCIso_::setup(USBSetup& setup)
{
  return false;
}

int CDCIso_::getDescriptor(USBSetup& setup)
{
  return 0;
}

void CDCIso_::enableInterrupt() {
  USB->DEVICE.DeviceEndpoint[epEPNum[0]].EPINTENSET.bit.TRCPT0=1; //TO DO CHECK RIGHT BANK?
  USB->DEVICE.DeviceEndpoint[epEPNum[1]].EPINTENSET.bit.TRCPT1=1;
}

void CDCIso_::handleEndpoint(int ep) {
  if (ep==epEPNum[1]) 
  {
      //if (pos >= Iso_TransferSize*NumberOfPackets){
     //SerialUSB.println(pos);
    //*(uint32_t *) (EPn_BK1_Addr.b8+0x004) = *(uint32_t *) (EPn_BK1_Addr.b8+0x004)  | ((uint32_t) 0 << 14);;  //Byte count on position 0 to 13 set to length //USBCore equivalent: usbd.epBank1SetByteCount(ep, 0) or EP[ep].DeviceDescBank[1].PCKSIZE.bit.BYTE_COUNT = bc; 
    write(WriteBuffer,NumberOfPackets*Iso_TransferSize); //3 packets of 1020 each, 1020 / 4byte = 511 datasets = 2byte ADC reading + 2byte Tick data
    //}
  } 
}

size_t CDCIso_::write(const uint8_t *buffer, size_t size)
{
  testiteration++;
  uint32_t r = send(epEPNum[1], buffer, size);

  if (r > 0) {
    return r;
    } else {
    //setWriteError();
    return 0;
  }
}

uint32_t CDCIso_::send(uint32_t ep, const void *data, uint32_t len)
{
  uint32_t written = 0;
  uint32_t length = 0;

  if (!UsbConfiguration)
  return -1;
 // if (len > 16384)
 // return -1;

  #ifdef PIN_LED_TXL
  if (txLEDPulse == 0)
  digitalWrite(PIN_LED_TXL, LOW);

  txLEDPulse = TX_RX_LED_PULSE_MS;
  #endif

  // Flash area
  while (len != 0)
  {
  if (USB->DEVICE.DeviceEndpoint[epEPNum[1]].EPSTATUS.bit.BK1RDY) {
  while (!USB->DEVICE.DeviceEndpoint[epEPNum[1]].EPINTFLAG.bit.TRCPT1);}
  memset(&DataBuffer,0,sizeof(DataBuffer)/sizeof(DataBuffer[0]));
    
    if (len >= Iso_TransferSize/*Iso_PckSize-1*/) { //limit to 1020, 1020 / 4byte = 511 datasets = 2byte ADC reading + 2byte Tick data
      *(EPn_BK1_Addr.b8+0x004) = *(EPn_BK1_Addr.b8+0x004) | (0x1uL << 32); // Auto ZLP on position 31 //USBCore equivalent code: usbd.epBank1EnableAutoZLP(ep) or EP[ep].DeviceDescBank[1].PCKSIZE.bit.AUTO_ZLP = 1
      length =  min(Iso_TransferSize,Iso_PckSize); //FS isochronous datatransfer only supports 1023 even the register is set at 1024. The size is further reduced to 1020 as it is the first number that is dividable by 4, as the information length is 4 bytes.
    } else {
      length = len;
    }

    /* memcopy could be safer in multi threaded environment */
    memcpy(&DataBuffer, data, length);

  
    //set Byte count to length of data
    *(uint32_t *) (EPn_BK1_Addr.b8+0x004) = *(uint32_t *) (EPn_BK1_Addr.b8+0x004) | (length);  //Byte count on position 0 to 13 set to length //USBCore equivalent: usbd.epBank1SetByteCount(ep, 0) or EP[ep].DeviceDescBank[1].PCKSIZE.bit.BYTE_COUNT = bc; 


    // Clear the transfer complete flag
    USB->DEVICE.DeviceEndpoint[epEPNum[1]].EPINTFLAG.reg = ((0x3ul << 0) & (2 << 0));//USBCore equivalent code: usbd.epBank1AckTransferComplete(ep) or usb.DeviceEndpoint[ep].EPINTFLAG.reg = USB_DEVICE_EPINTFLAG_TRCPT(2);

    // RAM buffer is full, we can send data (IN)
    USB->DEVICE.DeviceEndpoint[epEPNum[1]].EPSTATUSSET.bit.BK1RDY=1; //USBCore equivalent code: usbd.epBank1SetReady(ep) or usb.DeviceEndpoint[ep].EPSTATUSSET.bit.BK1RDY = 1
    

  pos=0;
  while(pos<Iso_TransferSize)
  {
    if (ADC->INTFLAG.bit.RESRDY==1){//Time first!! for openGL
      WriteBuffer[pos]=*(((uint8_t *) &RTC->MODE0.COUNT.reg)+0); pos++;//*(((uint8_t*) &teststep2) +0);pos++;//*(((uint8_t *) &RTC->MODE0.COUNT.reg)+0); pos++;//*(((uint8_t*) &teststep2) +0);pos++;//
      WriteBuffer[pos]=*(((uint8_t *) &RTC->MODE0.COUNT.reg)+1); pos++;//*(((uint8_t*) &teststep2) + 1);pos++;//*(((uint8_t *) &RTC->MODE0.COUNT.reg)+1); pos++;//*(((uint8_t*) &teststep2) +1);pos++;//
      RTC->MODE0.COUNT.bit.COUNT=0;
      //if (teststep2 >= 65535) teststep2 = 11730; else teststep2=11730;
      WriteBuffer[pos]=*ADCregL;pos++;//*(((uint8_t*) &teststep) +0);pos++;//
      WriteBuffer[pos]=*ADCregH;pos++;//*(((uint8_t*) &teststep) +1);pos++;//
      //if (teststep >= 4095) teststep =0; else teststep+=12;
    }
   }
      written += length;
      len -= length;
      //data = (char *)data + length;
        
  }   
  return written;
}


void CDCIso_::ADC_init()
{
  
   ///***** ADC configuration*******
    //Clock configuration
    uint8_t *clkctrl; clkctrl = (uint8_t *) (0x40000C02UL); *clkctrl=(int8_t) 30; //set clckctrl register to 30 - ADC
    GCLK->CLKCTRL.bit.GEN = 0x3; //Change to clock generator 3
    while (GCLK->STATUS.reg & GCLK_STATUS_SYNCBUSY );
    //Bus configuration
    PM->APBCMASK.bit.ADC_=0x01; //enable APBCmask - already enabled by ArduinoCore with settings.
    //Reference settings
    ADC->REFCTRL.bit.REFSEL=0x2; //1/2 VDDANA (only for VDDANA > 2.0V)
    ADC->REFCTRL.bit.REFCOMP=0x00; //Reference buffer offset
    //Averaging control   
    ADC->AVGCTRL.bit.SAMPLENUM=0x00; //1 sample take, no addition
    ADC->AVGCTRL.bit.ADJRES=0x00; //no division as only 1 sample
    // Sampling control
    ADC->SAMPCTRL.bit.SAMPLEN=1; //no delay immedatie sampling
    // ADC settings
    ADC->CTRLB.bit.DIFFMODE=0x00; //no differential mode
    ADC->CTRLB.bit.LEFTADJ=0x00; //no left adjustment
    ADC->CTRLB.bit.FREERUN=0x01; //free run mode
    ADC->CTRLB.bit.CORREN=0x00; //enable gain and offset correction (set in seperate register)
    ADC->CTRLB.bit.RESSEL=0x00; //12-bit - 16 would mean oversampling, ie. time trade-off
    ADC->CTRLB.bit.PRESCALER=0x00; //Division by 4 - highest value: 8Mhz/4=2Mhz, max. operating mode f(GCK_ADC)
    ADC->WINCTRL.bit.WINMODE=0x00; //no window mode enabled
    ADC->CTRLA.bit.RUNSTDBY=0x00; //no running on standby
    //input control
    ADC->INPUTCTRL.bit.MUXPOS=0x0A; //pinA1 (ie. PB02 ie. AIN10) - take a pin that draws voltage from VADDA as otherwise accuracy may drop
    ADC->INPUTCTRL.bit.MUXNEG=0x18;  //ground as no differential - here internal is choosen, alt. I/O GND is that off digital path?? while GND = VADAGND??
    ADC->INPUTCTRL.bit.INPUTSCAN=0x00; //no pin scan
    ADC->INPUTCTRL.bit.INPUTOFFSET=0x00; //no pin scan so no offsetting requied
    ADC->INPUTCTRL.bit.GAIN=0xF; //reference is set at VADDA/2 ie 1.7V, so to have full scale measurement to 3.3 division by /2
    //event control
    ADC->EVCTRL.bit.STARTEI=0x00; //no event driven conversion (free running with polling under 1000 cycle)
    ADC->EVCTRL.bit.SYNCEI=0x00; //no event driven flush & conversion (free running with polling under 1000 cycle)
    ADC->EVCTRL.bit.RESRDYEO=0x00; //for now on zero, want to disable all interupts - to ensure RTOS, with continuous polling
    ADC->EVCTRL.bit.WINMONEO=0x00; //no window mode
    //Window control
    ADC->WINLT.bit.WINLT=0x00; //no window mode
    ADC->WINUT.bit.WINUT=0x00; //no window mode
    //Correction control
    ADC->GAINCORR.bit.GAINCORR=0x00; //no Gain correction
    ADC->OFFSETCORR.bit.OFFSETCORR=0x00; //no Offset correction
    //Debug control
    ADC->DBGCTRL.bit.DBGRUN=0x00; //no ADC during debugging

    //Setting pin AIN10=PB02 as Input (without pull-up or pull-down to avoid distortion) atmel page 384
      //set mux
      PORT->Group[1].PMUX[1].bit.PMUXE=0x01; //Attaching the PB02 to B peripherial, ie. AIN10
      //Set pin
      PORT->Group[1].DIR.bit.DIR&=~((uint32_t) 1<<3); //set direction of bit 3 to zero
      PORT->Group[1].PINCFG[2].bit.INEN=1; //configure as floating input
      PORT->Group[1].PINCFG[2].bit.PULLEN=0; //configure as floating input
      
      //PORT->Group[1].OUT.bit.OUT;
      //PORT->Group[1].IN.bit.IN;
      //PORT->Group[1].CTRL.bit.SAMPLING;
      //PORT->Group[1].PINCFG[2].bit.DRVSTR;
      PORT->Group[1].PINCFG[2].bit.PMUXEN=1; //enable pin


    //USB settings: follow ArduinoCore

    //DMA settings: for later

      //Bind RTC clock to 8MHz source and start it
            RTC->MODE0.COUNT.bit.COUNT=0x00;
            while (GCLK->STATUS.reg & GCLK_STATUS_SYNCBUSY );
            uint8_t *clkctrlID; clkctrlID = (uint8_t *) (0x40000C02UL); *clkctrlID=(int8_t) 4; //set clckctrl register to 30 - ADC
            GCLK->CLKCTRL.bit.GEN = 0x3; //Change to 8MHzclock generator 3
            while (GCLK->STATUS.reg & GCLK_STATUS_SYNCBUSY );
            GCLK->CLKCTRL.bit.CLKEN = 1;
      //Before enabling initialize the RTC mode
            RTC->MODE0.CTRL.bit.MODE=0x0; //32-bit count value
            //CTRL.CLKREP not valid in Mode0
            RTC->MODE0.CTRL.bit.PRESCALER=0x0; //DIV 1 we allow 8MHz
            RTC->MODE1.READREQ.bit.RCONT=1; //Continuous reading 

      //Enable the clock
            RTC->MODE0.CTRL.bit.ENABLE=0x01;
            while (GCLK->STATUS.reg & GCLK_STATUS_SYNCBUSY );
   
//Print_register();
      
  ADC->CTRLA.bit.ENABLE=1;
  while (GCLK->STATUS.reg & GCLK_STATUS_SYNCBUSY );
  
  ADC->SWTRIG.bit.FLUSH; //flushes the ADC
  delay(10);
  ADC->SWTRIG.bit.START; //starts the conversion
  delay(1);
  while (ADC->INTFLAG.bit.RESRDY==0);

//stabalizing ADC before printing
  //Serial.println(ADC->RESULT.reg);
    while (ADC->INTFLAG.bit.RESRDY==0);
  //Serial.println(ADC->RESULT.reg);
    while (ADC->INTFLAG.bit.RESRDY==0);
  //Serial.println(ADC->RESULT.reg);
    while (ADC->INTFLAG.bit.RESRDY==0);
  //Serial.println(ADC->RESULT.reg);
    while (ADC->INTFLAG.bit.RESRDY==0);
  //Serial.println(ADC->RESULT.reg);
    while (ADC->INTFLAG.bit.RESRDY==0);
  //Serial.println(ADC->RESULT.reg);

    //Clock configuration
    ADCregL = (uint8_t *) (0x4200401A);
    ADCregH = (uint8_t *) (0x4200401B);
    
  //Reset clock value to avoid quick overrun. 8Mhz clock will fill 32bit in 537sec.
    RTC->MODE0.COUNT.bit.COUNT=0; 

}

/*Begining of Auto generated code by Atmel studio */
#include <Arduino.h>
#include "CDCIso.h"
#include "CDCIso_event.h"


/*End of auto generated code by Atmel studio */


//Beginning of Auto generated function prototypes by Atmel Studio
//End of Auto generated function prototypes by Atmel Studio


    //ADC Memory settings
    uint32_t i=0;
    const uint16_t s=1023;

    uint8_t *ADCregH;
    uint8_t *ADCregL;
    //uint8_t ADCreg[s];

    //uint32_t Tickreg[s];

void ADC_init()
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
    i=0;
  //Reset clock value to avoid quick overrun. 8Mhz clock will fill 32bit in 537sec.
    RTC->MODE0.COUNT.bit.COUNT=0; 
}
void USB_init()
{
  
  for (int i = 0; i < sizeof(USBISO.ADC_Bfr)/sizeof(USBISO.ADC_Bfr[0]); i++)
  {
    USBISO.ADC_Bfr[i] = 0;
  }

  // for (int i = 0; i < (uint32_t) (1022*8)/(ADC_Reading_Size+ADC_Tick_Size+ADC_Chk_Size); i++)
  USBISO.ADC_Bfr_c = 0;
  for (int i=0;i<(uint32_t)(1022 * 8) / (USBISO.ADC_Reading_Size + USBISO.ADC_Tick_Size + USBISO.ADC_Chk_Size);i++)
  {
    while (ADC->INTFLAG.bit.RESRDY==0);
    
    USBISO.ADC_Tick = USBISO.ADC_Tick++;//(uint32_t) RTC->MODE0.COUNT.bit.COUNT - USBISO.ADC_Tick;
    //Tickreg[i]=RTC->MODE0.COUNT.bit.COUNT;
    USBISO.ADC_Reg.b8[0]=127;//*ADCregL;
    USBISO.ADC_Reg.b8[1]=0;//*ADCregH;
    //uint8_t ADCreg[i]=*ADCregL;i++;ADCreg[i]=*ADCregH;i++;
    
    USBISO.BinConcat(USBISO.ADC_Bfr, USBISO.ADC_Bfr_c, USBISO.ADC_Chk_Size, (uint64_t*)&USBISO.ADC_Chk);
    USBISO.ADC_Bfr_c = USBISO.ADC_Bfr_c + USBISO.ADC_Chk_Size;
    USBISO.BinConcat(USBISO.ADC_Bfr, USBISO.ADC_Bfr_c, USBISO.ADC_Reading_Size, (uint64_t*)&USBISO.ADC_Reg.b16);
    USBISO.ADC_Bfr_c = USBISO.ADC_Bfr_c + USBISO.ADC_Reading_Size;
    USBISO.BinConcat(USBISO.ADC_Bfr, USBISO.ADC_Bfr_c, USBISO.ADC_Tick_Size, (uint64_t*)&USBISO.ADC_Tick);
    USBISO.ADC_Bfr_c = USBISO.ADC_Bfr_c + USBISO.ADC_Tick_Size;
//  } else {
    //Serial.write(ADCreg,s); //Left out Tickreg data as SerialPlot doesnt take timestamp
    //delay(2);
    //i=0;
  }
  for (int i = 0; i<1023;i++) {
    USBISO.writebuffer[i] =  USBISO.ADC_Bfr[i];
  }
  
  USBISO.lenwritebuffer=sizeof(USBISO.writebuffer);
//  USBISO.write(USBISO.writebuffer,1023);
  USBISO.Continue = 0;
}
void ADC_Loop()
{

  for (int i=0;i<USBISO.Iteration ;i++)
  {
    while (ADC->INTFLAG.bit.RESRDY==0);
    USBISO.ADC_reg[i].b8[0]=*ADCregL;//127;//
    USBISO.ADC_reg[i].b8[1]=*ADCregH;//0;//
    USBISO.ADC_rTick[i] = (uint32_t) RTC->MODE0.COUNT.bit.COUNT;
    RTC->MODE0.COUNT.bit.COUNT=0; //needed to get reliable tick data, if one is incremental calc doesnt work.
  }
}


void USB_Loop()
{

  USBISO.ADC_Bfr_c = 0;
  for (int i=0;i<USBISO.Iteration ;i++)
  {   
    
      USBISO.BinConcat(USBISO.ADC_Bfr, USBISO.ADC_Bfr_c, USBISO.ADC_Chk_Size, (uint64_t*)&USBISO.ADC_Chk);
      USBISO.ADC_Bfr_c = USBISO.ADC_Bfr_c + USBISO.ADC_Chk_Size;
      USBISO.ADC_Reg = USBISO.ADC_reg[i];
      USBISO.BinConcat(USBISO.ADC_Bfr, USBISO.ADC_Bfr_c, USBISO.ADC_Reading_Size, (uint64_t*)&USBISO.ADC_Reg.b16);
      USBISO.ADC_Bfr_c = USBISO.ADC_Bfr_c + USBISO.ADC_Reading_Size;
      USBISO.ADC_Tick = USBISO.ADC_rTick[i];
      USBISO.BinConcat(USBISO.ADC_Bfr, USBISO.ADC_Bfr_c, USBISO.ADC_Tick_Size, (uint64_t*)&USBISO.ADC_Tick);
      USBISO.ADC_Bfr_c = USBISO.ADC_Bfr_c + USBISO.ADC_Tick_Size;
  }
  
  USBISO.Continue=0;
  for (int i=0; i<1023; i++)
  {
    USBISO.writebuffer[i]=USBISO.ADC_Bfr[i];
  }
  USBISO.Continue=1;
    
    //memset is slower
    //memset(USBISO.ADC_Bfr,0,1024);
    //memset(USBISO.ADC_reg,0,1024);
    //memset(USBISO.ADC_rTick,0,1024);
    for (int i = 0; i < 1024; i++)
    {
      
      USBISO.ADC_Bfr[i]=0;
      USBISO.ADC_reg[i].b16 = 0;
      USBISO.ADC_rTick[i] = 0;
    }

}


void setup() {
  // put your setup code here, to run once:
  USBISO.iso_pcksize=1024;
  USB_SetHandler(UDD_Handler_Iso);
  SerialUSB.print("test");
  delay(10);
  SerialUSB.print("test");
  delay(10);

  ADC_init();

  USB_init();


  }

  void loop() {
    // put your main code here, to run repeatedly:
  
  ADC_Loop();
  USB_Loop();

  }

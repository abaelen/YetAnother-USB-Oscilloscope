/*Begining of Auto generated code by Atmel studio */
#include <Arduino.h>
#include "CDCIso.h"
#include "CDCIso_event.h"
#include <stdint.h>
/*End of auto generated code by Atmel studio */


//Beginning of Auto generated function prototypes by Atmel Studio
//End of Auto generated function prototypes by Atmel Studio


    //ADC Memory settings

void setup() {
  // put your setup code here, to run once:
  USBISO.Iso_PckSize=1024; //Set to config the USBCore to Isochronous. Transfersize will be limited to 1020 = (2byte ADC readings + 2byte Tick data)x511
  USB_SetHandler(UDD_Handler_Iso);
  SerialUSB.print("test");
  delay(10);
  SerialUSB.print("test");
  delay(10);

  USBISO.ADC_init();

  memset(USBISO.WriteBuffer,0,sizeof(USBISO.WriteBuffer)/sizeof(USBISO.WriteBuffer[0]));
  RTC->MODE0.COUNT.bit.COUNT=0;
}


void loop() {
    // put your main code here, to run repeatedly:
   

  }

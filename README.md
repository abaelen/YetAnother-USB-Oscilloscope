# YetAnother USB Oscilloscope
 USB oscilloscope featuring isochronous USB with openGL

1) Rewritten the Arduino Isochronous code
- Implemented the ADC capturing into the Isochronous event, to avoid time losses due to eventhandling .
- Fixed some minor bugs (like memory alignment of write buffer)
2) Completely rewritten the WinUSB isochronous code
- Removed binary serialization, as only limited time gain vs. high handling complexity
- Improved C++ DLL  interface with C#
- Introduced better error handling
- Introduced shared buffer memory between C++ and C#
- Communication between Arduino and PC has been brought to multi-package writting (50 packets) of 420 bytes. To allow for intermediate screen updating and lowering USB communication overhead.
3) Improved OpenGL DLL
- Updated Glew library
- Introduced shared buffer memory between C++ and C#
- Moved data transformation from C++ to C# (worse performance vs lower complexity)
4) Introduced different reading modes
- Standard Mode / Roll mode
- Triggered Mode
- Memory Mode (10s memory buffer)

Next updates:
- Finalize exception handling
- Finetune code (UI parameter handling)
- Class definitions
- Introduce FFT transformation (fourier, frequency domain analysis)

<br>
<img src="https://raw.githubusercontent.com/abaelen/YetAnother-USB-Oscilloscope/tree/master/img/YetAnotherOscilloscope.png?raw=true" />
<br>

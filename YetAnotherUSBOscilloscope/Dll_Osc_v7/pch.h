// pch.h: This is a precompiled header file.
// Files listed below are compiled only once, improving build performance for future builds.
// This also affects IntelliSense performance, including code completion and many code browsing features.
// However, files listed here are ALL re-compiled if any one of them is updated between builds.
// Do not add files here that you will be updating frequently as this negates the performance advantage.

#ifndef PCH_H
#define PCH_H

#define SHADER_VERTEX_RASTER_PATH "C:\\Users\\Gebruiker\\Documents\\OneDrive\\Elec Projects\\Github\\YetAnother-USB-Oscilloscope\\YetAnotherUSBOscilloscope\\Dll_Osc_v7\\TransformRaster.vertexshader"
#define SHADER_COLOR_RASTER_PATH "C:\\Users\\Gebruiker\\Documents\\OneDrive\\Elec Projects\\Github\\YetAnother-USB-Oscilloscope\\YetAnotherUSBOscilloscope\\Dll_Osc_v7\\ColorRaster.fragmentshader"
#define SHADER_VERTEX_TRIGGER_PATH "C:\\Users\\Gebruiker\\Documents\\OneDrive\\Elec Projects\\Github\\YetAnother-USB-Oscilloscope\\YetAnotherUSBOscilloscope\\Dll_Osc_v7\\TransformTrigger.vertexshader"
#define SHADER_COLOR_TRIGGER_PATH "C:\\Users\\Gebruiker\\Documents\\OneDrive\\Elec Projects\\Github\\YetAnother-USB-Oscilloscope\\YetAnotherUSBOscilloscope\\Dll_Osc_v7\\ColorTrigger.fragmentshader"
#define SHADER_VERTEX_DATA_PATH "C:\\Users\\Gebruiker\\Documents\\OneDrive\\Elec Projects\\Github\\YetAnother-USB-Oscilloscope\\YetAnotherUSBOscilloscope\\Dll_Osc_v7\\TransformData.vertexshader"
#define SHADER_COLOR_DATA_PATH "C:\\Users\\Gebruiker\\Documents\\OneDrive\\Elec Projects\\Github\\YetAnother-USB-Oscilloscope\\YetAnotherUSBOscilloscope\\Dll_Osc_v7\\ColorData.fragmentshader"

#define WIN_W 682
#define WIN_H 682
#define VERTEX_BUFFER_SIZE 3004 //
#define COLOR_BUFFER_SIZE 4506 //VERTEX_BUFFER_SIZE / 2 * 3;
#define RINGBUFFER_SIZE 262144 //2^18
#define VSCALE 10
#define TSCALE 10

// add headers that you want to pre-compile here
#include "framework.h"



#endif //PCH_H

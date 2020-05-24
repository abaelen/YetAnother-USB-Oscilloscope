#pragma once
#include "pch.h"
#include <stdint.h>
#include <math.h>
#include "GL/glew.h"


class Data {
public:
	struct Reading
	{
		uint16_t ADC_Reading;
		uint32_t ADC_Tick; //for 1us for 10sec, measured at 1Mhz, ie. 1us, 24-bit tick is required
	};

	const uint32_t RB_Size = (uint32_t)RINGBUFFER_SIZE; // 262144;// (18 bit)
	
	uint32_t RB_U = 0; //Necessary to have at first run new data input on 0
	uint32_t RB_L = 0; // we will not use [0] buffer, we start with one step behind

	uint32_t* ADC_Reading = new uint32_t[RB_Size+1]; uint32_t Sizeof_ADC_Reading;
	uint32_t* ADC_Tick = new uint32_t[RB_Size+1]; uint32_t Sizeof_ADC_Tick;

	//equivalent to AutoResetEvent(false/true)
	bool ReadingLock = false;
	bool DataProcessed = true;
//	bool ResetNumberOfSamples = false;
	bool Suspended = true;

	uint16_t ADC_res = (uint16_t)pow(2, 12); uint16_t ADC_Vref = 3300; uint16_t zeroVolt = 2500; uint16_t VoltDiv = 500; 
	float TimeDiv = 1000.0f; uint16_t TScale = TSCALE; uint32_t ADC_Clock = 8000000; uint16_t VScale = VSCALE;

	// Transformation Variables declaration
	float dfx = 0.0f; int32_t px = 0; int32_t dpx = 0; float cdpx=0; //cdpx is a float counter to allow screen progression in case of dpx=0, ie. buffer not able to capture one pixel of data
	float ADC_res_V = (float)ADC_Vref / (float) ADC_res; /*(float)ADC_Vref / 2^ADC_res; - for easy reference*/
	float V_Interval = ADC_res_V / (VoltDiv * VScale) / 2.0f;
	float KSamplePerSec = 0.0f;

	static const uint16_t Win_W = WIN_W;
	static const uint16_t Win_H = WIN_H;
	GLfloat g_vertex_buffer_raster[(int) VERTEX_BUFFER_SIZE];
	GLfloat g_color_buffer_raster[(int) COLOR_BUFFER_SIZE]; 
	GLfloat g_vertex_buffer_data[(Win_W + 1) * 6];
	GLfloat g_color_buffer_data[(Win_W + 1) * 12];
	GLushort g_indices[(Win_W + 4) * 4];

	double mean = 0; 
	double dev = 0; 
	double sdev = 0; 
	double var = 0; 
	double sd = 0;

	bool OptionFilterOutliers = true;

	void InitCollectData(void);
	void WaitForData(void);
	void InitTransfData(void);
	void ContTransformData(void);
	void FilterOutliers(void);
	void Dispose(void);
	void GetData(uint32_t* Readings, uint32_t* Ticks, int32_t RB_L, int32_t RB_U);
};


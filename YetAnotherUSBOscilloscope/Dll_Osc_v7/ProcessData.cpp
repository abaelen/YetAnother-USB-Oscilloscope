#include "pch.h"
#include "ProcessData.h"
#include <stdlib.h>

void Data::InitCollectData(void) {
	ADC_Reading[0] = 0; //Samples[0].ADC_Reading = 0;
	ADC_Tick[0] = 0; //Samples[0].ADC_Tick = 0; //to ensure no bufferunderrun 
}


void Data::GetData(uint32_t* Readings, uint32_t* Ticks, int32_t _RB_L, int32_t _RB_U) {
	RB_L = _RB_L;
	RB_U = _RB_U;
	
	for (int32_t i = RB_L; i <= RB_U; i++)
	{
		ADC_Reading[i] = Readings[i];
		ADC_Tick[i] = Ticks[i];
	}
}

// Initialization of transformation variables
void Data::InitTransfData(void) {
	//for (int32_t x = Win_W; x >= 0; x--) {
	for (int32_t x = 0; x <= Win_W; x++) {
		g_vertex_buffer_data[6 * x + 0] = 2.0f * (float)x / (float)(Win_W)-1.0f;
		g_vertex_buffer_data[6 * x + 2] = 2.0f * (float)x / (float)(Win_W)-1.0f;
		g_vertex_buffer_data[6 * x + 4] = 2.0f * (float)x / (float)(Win_W)-1.0f;

		g_vertex_buffer_data[6 * x + 1] = 0.0f;
		g_vertex_buffer_data[6 * x + 3] = 0.0f;
		g_vertex_buffer_data[6 * x + 5] = 0.0f;

		g_indices[4 * x + 0] = 4 * x + 0 - x;
		g_indices[4 * x + 1] = 4 * x + 1 - x;
		g_indices[4 * x + 2] = 4 * x + 1 - x;
		g_indices[4 * x + 3] = 4 * x + 2 - x;
	}
}
void Data::ContTransformData(void) {
	//Initialize average counter
	int32_t avg_count[Win_W + 1];
	for (int32_t x = Win_W; x >= 0; x--) {
		avg_count[x] = 0;
	}

	uint64_t cumTick = 0;
	for (int32_t i = RB_L; i <= RB_U; i++) cumTick += ADC_Tick[i];
	KSamplePerSec = (float)(RB_U - RB_L + 1) / cumTick * ADC_Clock / 1000;
	dfx = (float)(cumTick) * (1000.0f / ADC_Clock) / (TimeDiv * TScale) * 2.0f;//dfx = (float)(Samples[RB_U].ADC_Tick - Samples[RB_L].ADC_Tick) * (1000.0f / ADC_freq) / (TimeDiv * TScale) * 2.0f;
	if (dfx > 2.0f) { dfx = 2.0f; } //In case of overload, refresh whole screen
	if (dfx < 0.0f) { dfx = 0.0f; } //In case of tick reset, ignore data
	dpx = (int32_t)floor((dfx / 2.0f * (float)Win_W));
	if (cdpx > 1.0f) 
	//if (cdpx > 1.0f)
	{ 
		dpx = 1; cdpx = 0; 
	} //if dpx=0, ie; the number of samples in buffer dont ask for a shift in pixel on the graph, the number of samples per float is increased until a one screen pixel shift is granted
	//Shift all data with the timeshift to left
	
	for (int32_t x = dpx; x <= Win_W; x++) {
		g_vertex_buffer_data[6 * (x - dpx) + 1] = g_vertex_buffer_data[6 * x + 1];
		g_vertex_buffer_data[6 * (x - dpx) + 3] = g_vertex_buffer_data[6 * x + 3];
		g_vertex_buffer_data[6 * (x - dpx) + 5] = g_vertex_buffer_data[6 * x + 5];
	}
	for (int32_t x = Win_W - dpx ; x <= Win_W; x++) {
		if (x >0) {
			g_vertex_buffer_data[6 * x + 1] = 0;
			g_vertex_buffer_data[6 * x + 3] = 0;
			g_vertex_buffer_data[6 * x + 5] = 0;
		}
	}

	//Add the new read data
	cumTick = 0;
	for (int32_t i = RB_U; i >= RB_L; i--) {
		//calculate the screen position, offsetting from RB_U (in case of overload it will start full screen refresh)
		if (cumTick + ADC_Tick[i] > pow(2, 64) - 1) {
			cumTick = 0;
		}
		cumTick += ADC_Tick[i];
		px = (int32_t)Win_W - (((float)(cumTick - ADC_Tick[i]) * (1000.0f / ADC_Clock) / (TimeDiv * TScale)) * Win_W);
		if (px >= 0 and px <= Win_W) { //In case px<0: screen overload, ignore data oldest data. In case of px > Win_W : Tick reset, ignored data	
			g_vertex_buffer_data[6 * (px)+3] += ((float)ADC_Reading[i] * ADC_res_V - (zeroVolt - (VoltDiv * VScale / 2))) / (VoltDiv * VScale) * 2.0f - 1.0f; 
			avg_count[px]++;
		}
		if ((int32_t)floor((dfx / 2.0f * (float)Win_W)) == 0) 
		{ 
			cdpx += (dfx / 2.0f * (float)Win_W);
			//cdpx += dfx; 
		} //maintain count of screenprogression in float space
	}
	//Average the read data
	for (int32_t x = 0; x <= Win_W; x++) {
		if (avg_count[x] > 0)
		{
			g_vertex_buffer_data[6 * (x)+3] /= (avg_count[x]);
			g_vertex_buffer_data[6 * (x)+1] = (g_vertex_buffer_data[6 * (x)+3]) - V_Interval;
			g_vertex_buffer_data[6 * (x)+5] = (g_vertex_buffer_data[6 * (x)+3]) + V_Interval;
		}
	}

}
void Data::FilterOutliers(void)
{
	for (int32_t x = 1; x <= Win_W; x++)
	{
		if (g_vertex_buffer_data[6 * (x)+3] > 1.0f || g_vertex_buffer_data[6 * (x)+3] < -1.0f)
		{
			g_vertex_buffer_data[6 * (x)+3] = g_vertex_buffer_data[6 * (x - 1) + 3];
			g_vertex_buffer_data[6 * (x)+1] = (g_vertex_buffer_data[6 * (x)+3]);// -V_Interval;//doing V_interval creates nasty blocks
			g_vertex_buffer_data[6 * (x)+5] = (g_vertex_buffer_data[6 * (x)+3]);// +V_Interval;
		}
	}



/*	mean = 0; dev = 0; sdev = 0; var = 0;  sd = 0;
	for (int32_t x = 0; x <= Win_W; x++) 
	{
		mean += g_vertex_buffer_data[6 * (x)+3];
	}
	mean /= (Win_W + 1);
	for (int32_t x = 0; x <= Win_W; x++)
	{
		dev = pow((g_vertex_buffer_data[6 * (x)+3] - mean), 2);
		sdev = sdev + dev;
	}
	var = sdev / (Win_W + 1 - 1);
	sd = sqrt(var);
	for (int32_t x = 0; x <= Win_W; x++)
	{
		if (g_vertex_buffer_data[6 * (x) + 3 ] > 5* sd) 
		{
			g_vertex_buffer_data[6 * (x)+3] = mean;
			g_vertex_buffer_data[6 * (x)+1] = (g_vertex_buffer_data[6 * (x)+3]) - V_Interval;
			g_vertex_buffer_data[6 * (x)+5] = (g_vertex_buffer_data[6 * (x)+3]) + V_Interval;
		}
	}*/
}


void Data::Dispose(void)
{
	free(ADC_Reading);
	free(ADC_Tick);
}
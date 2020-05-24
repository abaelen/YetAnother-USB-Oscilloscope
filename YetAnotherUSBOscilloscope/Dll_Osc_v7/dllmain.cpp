// dllmain.cpp : Defines the entry point for the DLL application.
// dllmain.cpp : Defines the entry point for the DLL application.

#include "pch.h"
#include "ProcessScreen.h"
#include "ProcessData.h"
#include "ProcessMonitor.h"
#include "GlfwWindow.h"



BOOL APIENTRY DllMain(HMODULE hModule,
	DWORD  ul_reason_for_call,
	LPVOID lpReserved
)
{

	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:

	case DLL_THREAD_ATTACH:

	case DLL_THREAD_DETACH:

	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;

}

Screen S;
Data D;
Window W;
Process P;


//TO DO: !!! Keep data out of the dll export functions, as it increases speed of execution.
//However what to do with function combining three classes? - Create masterclass?

extern "C"
{
	__declspec(dllexport) void OGL_Window_Init()
	{
//		D.ResetNumberOfSamples = false;
		D.Suspended = true;

		if (QueryPerformanceFrequency(&P.PC_Freq));
		
		W.Init(D.Win_H,D.Win_H);
		
		S.InitParams(D.g_vertex_buffer_raster, sizeof(D.g_color_buffer_raster),
			D.g_color_buffer_raster, sizeof(D.g_color_buffer_raster),
			D.g_vertex_buffer_data, sizeof(D.g_vertex_buffer_data),
			D.g_color_buffer_data, sizeof(D.g_color_buffer_data),
			D.g_indices, sizeof(D.g_indices), D.Win_H, D.Win_W);
		S.GenerateRaster();
		D.InitCollectData();
		S.ColorData();
		D.InitTransfData();
		S.InitScreen();
		do {
			// Clear the screen. It's not mentioned before Tutorial 02, but it can cause flickering, so it's there nonetheless.		
			glClear(GL_COLOR_BUFFER_BIT);
																				
			while (D.Suspended) {  //DoEvents is not stable for use //Suspended is a switch
				glClear(GL_COLOR_BUFFER_BIT);
				S.BuildSuspendedScreen();
				if (P.msSleep != -999) { Sleep((uint32_t)P.msSleep); }
				W.Draw();
				D.DataProcessed = true;
			}; 
																				if (QueryPerformanceCounter(&P.PC_Start));
			while (D.ReadingLock); // holding until GetData is called by C# program => this is the break
									//ReadingLock is below in GetData and called via DLLmain SendData
			D.ReadingLock = true; /*ReadingLock is a swith */					if (QueryPerformanceCounter(&P.PC_Stop[0]));
			D.ContTransformData();												if (QueryPerformanceCounter(&P.PC_Stop[1]));
			if (D.OptionFilterOutliers == true) { D.FilterOutliers(); }
			S.BuildScreen();													if (QueryPerformanceCounter(&P.PC_Stop[2]));
			if (P.msSleep != -999) { Sleep((uint32_t) P.msSleep); }				if (QueryPerformanceCounter(&P.PC_Stop[3]));
			W.Draw();															if (QueryPerformanceCounter(&P.PC_Stop[4]));
			D.DataProcessed = true; //This is the signal to C# program to continue
			P.CalcTmr();
			W.FPS();
		} // Check if the ESC key was pressed or the window was closed
		while (W.CloseReq()); //CloseReq is a switch

		S.Dispose();
		W.Terminate();
		D.Dispose();
	}

	__declspec(dllexport) void OGL_Window_SetPos(int x, int y)
	{
		W.GlfwSetWindowPos(x, y);
	};
	__declspec(dllexport) void OGL_Window_Dispose(void)
	{
		W.GlfwSetWindowShouldClose();
	};
	__declspec(dllexport) HWND GetWin32Window(void)
	{
		return W.GlfwGetWin32Window();
	};
	__declspec(dllexport) bool SendData(uint32_t* Readings, uint32_t* Ticks,int32_t RB_L, int32_t RB_U)
	{
		D.GetData(Readings, Ticks, RB_L, RB_U);
		return true;
	};
	__declspec(dllexport) BOOL DataProcessed(void)
	{
		return D.DataProcessed;
	};
	__declspec(dllexport) BOOL SetSuspended(bool Suspended)
	{
		D.Suspended = Suspended;
		return D.Suspended;
	};
	__declspec(dllexport) BOOL SetReadingLock(bool ReadingLock)
	{
		D.ReadingLock = ReadingLock;
		return D.ReadingLock;
	};
	__declspec(dllexport) BOOL SetDataProcessed(bool DataProcessed)
	{
		D.DataProcessed = DataProcessed;
		return D.DataProcessed;
	};

	__declspec(dllexport) BOOL SendTransformParams(uint16_t ADC_bitres, uint16_t ADC_Vref, uint16_t zeroVolt, uint16_t VoltDiv, float TimeDiv, uint32_t ADC_Clock,  bool optionExtrapolated, float msSleep, bool optionFilterOutliers)
	{
		//uint16_t ADC_res = (uint16_t)pow(2, 8); uint16_t ADC_Vref = 5000; uint16_t zeroVolt = 2500; uint16_t VoltDiv = 500;
		//float TimeDiv = 1000.0f; uint16_t TScale = TSCALE; uint32_t ADC_freq = 350000; uint16_t VScale = VSCALE;
		D.ADC_res = pow(2,ADC_bitres);
		D.ADC_Vref = ADC_Vref;
		D.zeroVolt = zeroVolt;
		D.VoltDiv = VoltDiv;
		D.TimeDiv = TimeDiv;
		D.ADC_Clock = ADC_Clock;

		S.optionExtrapolated = optionExtrapolated;
		D.OptionFilterOutliers = optionFilterOutliers;

		//Updating all dependencies. Below variables could be made functions but for speed reasons choosen to update only when updated.
		D.ADC_res_V = (float)D.ADC_Vref / D.ADC_res; /*(float)ADC_Vref / ADC_res; - for easy reference*/
		D.V_Interval = D.ADC_res_V / (D.VoltDiv * D.VScale) / 2.0f;

		P.msSleep = msSleep;

		return true;
	}

	__declspec(dllexport) BOOL GetStats(float* InStats)
	{
		float* Stats;

		Stats = InStats;

		if (W.nbFrames > 0) {
			Stats[0] = (float) W.Frames[W.nbFrames - 1];
		}
		
		Stats[1] = D.dpx;
		Stats[2] = D.dfx/2.0f;
		Stats[3] = D.ADC_res_V; //transform from float to Volt
		Stats[4] = (float)P.TmrBuildScreen;
		Stats[5] = (float)P.TmrSuspended;
		Stats[6] = (float)P.TmrDrawScreen;
		Stats[7] = (float)P.TmrTransfData;
		Stats[8] = (float)P.TmrWaitForData;
		Stats[9] = (float)D.KSamplePerSec;

		P.TmrWaitForData = 0;
		P.TmrTransfData = 0;
		P.TmrBuildScreen = 0;
		P.TmrSuspended = 0;
		P.TmrDrawScreen = 0;
		P.TmrCount = 0; //Reset for following call

		return 1;
	}
}

// dllmain.cpp : Defines the entry point for the DLL application.
// dllmain.cpp : Defines the entry point for the DLL application.

#include "pch.h"
#include "ProcessScreen.h"
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
Window W;
//Process P;


//TO DO: !!! Keep data out of the dll export functions, as it increases speed of execution.
//However what to do with function combining three classes? - Create masterclass?

extern "C"
{
	__declspec(dllexport) void OGL_Window_Init(uint32_t win_W, float* vertexBufferData, uint32_t vertexBufferDataSize, float* vertexBufferTrigger, uint32_t vertexBufferTriggerSize)
	{
//		D.ResetNumberOfSamples = false;

		if (QueryPerformanceFrequency(&W.PC_Freq));

		W.Init(win_W,win_W);
		
		S.InitParams(win_W, vertexBufferData, vertexBufferDataSize, vertexBufferTrigger, vertexBufferTriggerSize);
		S.GenerateRaster();
		S.ColorData();
		S.ColorTrigger();
		//S.InitIndices();
		S.InitScreen();

		S.Suspended[0]=1;
		S.ScreenDrawn[0]=0;
		S.Extrapolate[0]=0;
		while (W.CloseReq()==false)
		{
			
			if (S.Suspended[0]==true)
				S.BuildSuspendedScreen();
			else
			{
				S.BuildScreen();
			}
			S.ScreenDrawn[0] = true;
			
			W.Draw();
			W.Time = W.Timer_Stop();
			if (W.Time < 4 && W.Time > 0) Sleep(4-W.Time);
			W.Timer_Start();
			//P.CalcTmr();
			W.FPS();
			std::unique_lock<std::mutex> lck(S.mutex_);
			S.cv.wait(lck,[]{return !S.ScreenDrawn[0];}); //puts into a waiting pattern
		}

		//Proper placement of disposal mechanism.
		//On the destuctor of the class ~ would entail to delete the Screen member itself not its members.
		//The members should be disposed off in the DLLmain on the thread wich created them.
		//Also dont use Dispose inside the class but only in the DLLmain
		S.Dispose();
		W.Terminate();
	}


	/* This function serves as a signal generator to continue the drawing of the window
	 * The function is part of a two way mutex lock which notifies to unlock, based on the condition of the Screendraw. 
	 * This condition is best placed to avoid spurious or lost wakeups
	 */
	__declspec(dllexport) void OGL_ScreenDrawn(bool screenDrawn)
	{
		std::unique_lock<std::mutex> lck(S.mutex_);
		S.ScreenDrawn[0]=screenDrawn;
		S.cv.notify_one();
	}
	__declspec(dllexport) void OGL_pScreenDrawn(bool* pScreenDrawn)
	{
		S.ScreenDrawn = pScreenDrawn;
	}
	__declspec(dllexport) void OGL_Suspended(bool* pSuspended)
	{
		S.Suspended = pSuspended;
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

	__declspec(dllexport) void OGL_Window_Frames(float* pWindow_Frames)
	{
		W.Frames = pWindow_Frames;
	}
	__declspec(dllexport) void OGL_Extrapolate(bool* pExtrapolate)
	{
		S.Extrapolate = pExtrapolate;
	}
	__declspec(dllexport) void OGL_LastDataPosition(uint32_t* pLastDataPosition)
	{
		S.LastDataPosition = pLastDataPosition;
	}
}

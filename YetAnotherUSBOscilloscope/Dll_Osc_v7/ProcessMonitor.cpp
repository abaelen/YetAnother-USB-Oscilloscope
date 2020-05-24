#include "pch.h"
#include "ProcessMonitor.h"

void Process::CalcTmr()
{
	TmrCount++;
	TmrWaitForData = TmrWaitForData * (TmrCount-1) / TmrCount + ((double)(PC_Stop[0].QuadPart - PC_Start.QuadPart) / (double)(PC_Freq.QuadPart) *1000.0f) / TmrCount;
	TmrTransfData = TmrTransfData * (TmrCount - 1) / TmrCount + ((double)(PC_Stop[1].QuadPart - PC_Stop[0].QuadPart) / (double)(PC_Freq.QuadPart) * 1000.0f) / TmrCount;
	TmrBuildScreen = TmrBuildScreen * (TmrCount - 1) / TmrCount + ((double)(PC_Stop[2].QuadPart - PC_Stop[1].QuadPart) / (double)(PC_Freq.QuadPart) * 1000.0f) / TmrCount;
	TmrSuspended = TmrSuspended * (TmrCount - 1) / TmrCount + ((double)(PC_Stop[3].QuadPart - PC_Stop[2].QuadPart) / (double)(PC_Freq.QuadPart) * 1000.0f) / TmrCount;
	TmrDrawScreen = TmrDrawScreen * (TmrCount - 1) / TmrCount + ((double)(PC_Stop[4].QuadPart - PC_Stop[3].QuadPart) / (double)(PC_Freq.QuadPart) * 1000.0f) / TmrCount;
}
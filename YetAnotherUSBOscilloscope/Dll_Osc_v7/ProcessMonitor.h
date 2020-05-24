#pragma once
#include "pch.h"

class Process {
public:
	LARGE_INTEGER PC_Start; LARGE_INTEGER PC_Stop[5]; BOOL result; LARGE_INTEGER PC_Freq;
	double TmrWaitForData = 0; double TmrTransfData = 0; double TmrBuildScreen = 0; double TmrSuspended = 0;  double TmrDrawScreen=0; double TmrCount=0;

	void CalcTmr(void);

	float msSleep=-999;
};
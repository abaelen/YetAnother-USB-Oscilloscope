#pragma once
#ifndef PROCESSSCREEN_H
#define PROCESSSCREEN_H

#include "pch.h"
#include <windows.h>
#include <stdio.h>
#include <stdlib.h>
#include <TCHAR.h>
#include "atlstr.h"
#include <time.h>
#include <math.h>
#include <stdint.h>
#include <condition_variable>
#include <iostream>

// Include GLEW
#include <GL/glew.h>

// Include GLM
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
using namespace glm;

#include <common/shader.hpp>

//#include "Dll_Osc_Global_Params.h"

class Screen {

	//Buffers
		public:GLuint VAO[1];
		public:GLuint rasterbuffer[2];
		public:GLuint triggerbuffer[2];
		public:GLuint databuffer[2];

	//Load and Compile GSL program for shaders
		public:GLuint ShaderRaster; //= LoadShaders("C:\\Users\\Gebruiker\\source\\repos\\Osc_v5\\Dll_Osc_v6\\TransformRaster.vertexshader", "C:\\Users\\Gebruiker\\source\\repos\\Osc_v5\\Dll_Osc_v6\\ColorRaster.fragmentshader");
		public:GLuint ShaderTrigger; //
		public:GLuint ShaderData; //= LoadShaders("C:\\Users\\Gebruiker\\source\\repos\\Osc_v5\\Dll_Osc_v6\\TransformData.vertexshader", "C:\\Users\\Gebruiker\\source\\repos\\Osc_v5\\Dll_Osc_v6\\ColorData.fragmentshader");

		public:uint32_t Win_W = 682; //default set to 682 is changed by initialization
		public:uint32_t Win_H = 682;

	//OpenGL buffers binded to the data and raster buffers through Binding statements
		public:float* g_vertex_buffer_raster;
		public:float* g_color_buffer_raster;
		public:float* g_vertex_buffer_trigger;
		public:float* g_color_buffer_trigger;
		public:float* g_vertex_buffer_data;
		public:float* g_color_buffer_data;
		public:uint32_t sizeof_g_vertex_buffer_raster;
		public:uint32_t sizeof_g_color_buffer_raster;
		public:uint32_t sizeof_g_vertex_buffer_trigger;
		public:uint32_t sizeof_g_color_buffer_trigger;
		public:uint32_t sizeof_g_vertex_buffer_data;
		public:uint32_t sizeof_g_color_buffer_data;

		public:std::mutex mutex_;
		public:std::condition_variable cv;

		public:bool* Suspended;
		public:bool* ScreenDrawn;
		public:bool* Extrapolate;
		public:uint32* LastDataPosition;
	
		//public: GLenum glGetError();
		
	//con/de-structor, copy & assignment:
		public: Screen();
		public: ~Screen();
		private: Screen(const Screen& that) = delete;
		private: Screen& operator=(const Screen& that) = delete;


	//Initializes the window size and the accompagnying buffers for the raster, color and data. The data is a passed in pointer to the transformed read data
		public:	void InitParams(uint32_t win_W, float* vertexBufferData, uint32_t vertexBufferDataSize, float* vertexBufferTrigger, uint32_t vertexBufferTriggerSize);
	//Method to draw the raster line along the X-axis
		public:void DrawRasterLinesX(unsigned int y, int* a, int* b);
	//Method to draw the raster line along the Y-axis
		public:void DrawRasterLinesY(unsigned int x, int* a, int* b);
	//Method to draw the black lines along the Y-axis to create the dash lining accross the X-axis
		public:void DrawStripesX(unsigned int y1, int* a, int* b);
	//Method to draw the black lines along the X-axis to create the dash lining accross the Y-axis
		public:void DrawStripesY(unsigned int x1, int* a, int* b);
	//Method to generate the raster structure of the screen
		public:void GenerateRaster();
	//Method to generate the color data
		public:void ColorData(void);
	//Method to generate the color trigger
		public:void ColorTrigger(void);
	//Method to generate the Indices data (indices are used to link the datapoints to each other
		public:void InitIndices(void);
	//Method to initialize the screen buffers
		public:void InitScreen(void);
	//Method that builds the screen, basically binding the vertex buffer into the OGL
		public:void BuildScreen(void);
	//Method that builds the suspended screen
		public:void BuildSuspendedScreen(void);
	//Method to dispose the allocated memory
		public:void Dispose(void);
};


#endif //PROCESSSCREEN_H
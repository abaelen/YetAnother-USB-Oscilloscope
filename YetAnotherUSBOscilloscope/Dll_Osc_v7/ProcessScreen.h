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
#include <stdlib.h>

// Include GLEW
#include <GL/glew.h>

// Include GLM
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
using namespace glm;

#include <common/shader.hpp>

//#include "Dll_Osc_Global_Params.h"

class Screen {
public:

	GLuint VAO[1];
	GLuint rasterbuffer[2];
	GLuint databuffer[2];
	GLuint ebo[1]; // element buffer object


	//Load and Compile GSL program for shaders
	GLuint ShaderRaster; //= LoadShaders("C:\\Users\\Gebruiker\\source\\repos\\Osc_v5\\Dll_Osc_v6\\TransformRaster.vertexshader", "C:\\Users\\Gebruiker\\source\\repos\\Osc_v5\\Dll_Osc_v6\\ColorRaster.fragmentshader");
	GLuint ShaderData; //= LoadShaders("C:\\Users\\Gebruiker\\source\\repos\\Osc_v5\\Dll_Osc_v6\\TransformData.vertexshader", "C:\\Users\\Gebruiker\\source\\repos\\Osc_v5\\Dll_Osc_v6\\ColorData.fragmentshader");

	//Shared data to be initialized, so that not everytime everything needs to be passed on
	GLfloat* g_vertex_buffer_raster;
	GLfloat* g_color_buffer_raster;
	GLfloat* g_vertex_buffer_data;
	GLfloat* g_color_buffer_data;
	uint32_t sizeof_g_vertex_buffer_raster;
	uint32_t sizeof_g_color_buffer_raster;
	uint32_t sizeof_g_vertex_buffer_data;
	uint32_t sizeof_g_color_buffer_data;

	GLushort* g_indices;
	uint32_t sizeof_g_indices;

	uint16_t Win_H;
	uint16_t Win_W;

	bool optionExtrapolated=false;

	void InitParams(GLfloat* vertex_raster, uint32_t sizeof_vertex_raster, GLfloat* color_raster, uint32_t sizeof_color_raster, GLfloat* vertex_data, uint32_t sizeof_vertex_data, GLfloat* color_data, uint32_t sizeof_color_data, GLushort* indices, uint32_t sizeof_indices, const uint16_t win_H, const uint16_t win_W);
	void DrawRasterLinesX(unsigned int y, int* a, int* b);
	void DrawRasterLinesY(unsigned int x, int* a, int* b);
	void DrawStripesX(unsigned int y1, int* a, int* b);
	void DrawStripesY(unsigned int x1, int* a, int* b);
	void GenerateRaster();
	void ColorData(void);
	void InitScreen(void);
	void BuildScreen(void);
	void BuildSuspendedScreen(void);
	void Dispose(void);
};


#endif //PROCESSSCREEN_H
#pragma once

#include "pch.h"

// Include GLEW
#include <GL/glew.h>

// Include GLFW
#include <GLFW/glfw3.h>
#include <GLFW/glfw3native.h>

class Window {
public:
	GLFWwindow* window;
	
	//to keep number of frames
	double lastTime = glfwGetTime();
	int nbFrames = 0;
	double Frames[1000] = { 0 };
	int cFrames = 0;


	void Draw(void);
	void Init(const uint16_t Win_W, const uint16_t Win_H);
	void FPS(void);
	bool CloseReq(void);
	void Terminate(void);
	void GlfwSetWindowPos(int x, int y);
	void GlfwSetWindowShouldClose(void);
	HWND GlfwGetWin32Window(void);
	void GlfwPollEvent(void);
};

#pragma once

#include "pch.h"

// Include GLEW
#include <GL/glew.h>

// Include GLFW
#include <GLFW/glfw3.h>
#include <GLFW/glfw3native.h>

class Window {

public:GLFWwindow* window;
	
	//Params to keep Frame Per seconsds
		public:double lastTime = glfwGetTime();
		public:int nbFrames = 0;
		public:float* Frames=0;
		public:int cFrames = 0;

		public: LARGE_INTEGER PC_Start; 
		public: LARGE_INTEGER PC_Stop[5]; 
		public: LARGE_INTEGER PC_Freq;
		public: double Time;

	//con/de-structor, copy & assignment:
		public: Window();
		public: ~Window();
		private: Window(const Window& that) = delete;
		private: Window& operator=(const Window& that) = delete;

	//Method that draws the screen by swapping the buffers and polling events
		public:void Draw(void);
	//Method that initializes the screen by creating a screenhandle and initializing the window
		public:void Init(const uint16_t Win_W, const uint16_t Win_H);
	//Method that tracks the PFS
		public:void FPS(void);
	//Method that starts the timer for keeping FPS to min 60hz
		public: void Timer_Start(void);
	//Method that stops the timer for keeping FPS to min 60hz
		public: double Timer_Stop(void);
	//Method that captures a closerequest, to pass on to terminate the window
		public:bool CloseReq(void);
	//Method that calls the GLFW termination procedure
		public:void Terminate(void);
	//Method that sets the position of the window
		public:void GlfwSetWindowPos(int x, int y);
	//Method that sets the close flag of the window, which on its turn will trigger CloseReq which triggers Terminate
		public:void GlfwSetWindowShouldClose(void);
	//Method that allows to get the handle over the window
		public:HWND GlfwGetWin32Window(void);
	//Method that polls for events
		public:	void GlfwPollEvent(void);
};

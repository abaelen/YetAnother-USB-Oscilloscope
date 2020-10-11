#include "pch.h"
#include "GlfwWindow.h"
#include <stdio.h>

Window::Window()
{
	//Empty
}
Window::~Window()
{
	//Here dispose would be misplaced. The ~ would entail to delete the Screen member itself not its members.
	//The members should be disposed off in the DLLmain on the thread wich created them.
	//Also dont use Dispose inside the class but only in the DLLmain
	//Terminate();
}

void Window::Init(const uint16_t Win_W, const uint16_t Win_H)
{
	// Initialise GLFW
	if (!glfwInit())
	{
		fprintf(stderr, "Failed to initialize GLFW\n");
		getchar();
		//return -1;
	}

	//glfwWindowHint(GLFW_SAMPLES, 4); //Anti-aliasing better performance if disabled and provides nicer drawing
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GL_TRUE); // To make MacOS happy; should not be needed //disabled as linewidth would not work
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
	glfwWindowHint(GLFW_DECORATED, 0);
	//glfwWindowHint(GLFW_RESIZABLE, 0);
	glfwWindowHint(GLFW_FLOATING, 1);
	glfwWindowHint(0x0002200C, 1); //GFLW_SCALE_TO_MONITOR


	// Open a window and create its OpenGL context
	window = glfwCreateWindow(Win_H, Win_W, "Tutorial 01", NULL, NULL);
	if (window == NULL) {
		fprintf(stderr, "Failed to open GLFW window. If you have an Intel GPU, they are not 3.3 compatible. Try the 2.1 version of the tutorials.\n");
		getchar();
		glfwTerminate();
		//return -1;
	}


	glfwSetWindowPos(window, 68, 93);
	glfwMakeContextCurrent(window);

	// Initialize GLEW
	if (glewInit() != GLEW_OK) {
		fprintf(stderr, "Failed to initialize GLEW\n");
		getchar();
		glfwTerminate();
		//return -1;
	}

	// Ensure we can capture the escape key being pressed below
	glfwSetInputMode(window, GLFW_STICKY_KEYS, GL_TRUE);
	glfwFocusWindow(window);
	// Dark blue background
	glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
}
void Window::Draw(void) 
{
	glfwSwapBuffers(window);
	glfwPollEvents();
}
void Window::FPS(void)
{
	// Measure speed
	double currentTime = glfwGetTime();

	cFrames++;
	if (currentTime - lastTime >= 1) 
	{ 
		if(Frames !=NULL) Frames[0] = 1000 / float(cFrames);
		cFrames = 0;
		lastTime = glfwGetTime();
	}
}
void Window::Timer_Start(void)
{
	if (QueryPerformanceCounter(&PC_Start));
}
double Window::Timer_Stop(void)
{
	if (QueryPerformanceCounter(&PC_Stop[0]));
	return ((double)(PC_Stop[0].QuadPart - PC_Start.QuadPart) / (double)(PC_Freq.QuadPart) * 1000.0f);
}
bool Window::CloseReq(void)
{
	return (glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS || glfwWindowShouldClose(window) != 0);
}
void Window::Terminate(void)
{
	glfwTerminate();
}
void Window::GlfwSetWindowPos(int x, int y)
{
	glfwSetWindowPos(window, x, y);
}
void Window::GlfwSetWindowShouldClose(void)
{
	glfwSetWindowShouldClose(window,1);
}
HWND Window::GlfwGetWin32Window(void)
{
	return glfwGetWin32Window(window);
}
void Window::GlfwPollEvent()
{
	glfwPollEvents();
}
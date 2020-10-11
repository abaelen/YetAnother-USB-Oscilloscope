#include "pch.h"
#include "ProcessScreen.h"

Screen::Screen() 
{
	//empty
}
Screen::~Screen()
{
	//Here dispose would be misplaced. The ~ would entail to delete the Screen member itself not its members.
	//The members should be disposed off in the DLLmain on the thread wich created them.
	//Also dont use Dispose inside the class but only in the DLLmain
	//Dispose();
}
void Screen::InitParams(uint32_t win_W, float* vertexBufferData, uint32_t vertexBufferDataSize,float * vertexBufferTrigger, uint32_t vertexBufferTriggerSize)
{
	Win_H = win_W;
	Win_W = win_W;
	g_vertex_buffer_raster = new float[VERTEX_BUFFER_SIZE]; //vertex_raster;
	sizeof_g_vertex_buffer_raster = VERTEX_BUFFER_SIZE;
	g_color_buffer_raster = new float[COLOR_BUFFER_SIZE];
	sizeof_g_color_buffer_raster = COLOR_BUFFER_SIZE;

	g_vertex_buffer_trigger = vertexBufferTrigger;
	sizeof_g_vertex_buffer_trigger = vertexBufferTriggerSize;
	//g_color_buffer_trigger = new float[(uint64_t)3 * 4];
	g_color_buffer_trigger = new float[(uint64_t)3 * vertexBufferTriggerSize / 2];
	//sizeof_g_color_buffer_trigger = 3 * 4;
	sizeof_g_color_buffer_trigger = 3 * vertexBufferTriggerSize / 2;

	g_vertex_buffer_data = vertexBufferData;
	sizeof_g_vertex_buffer_data = vertexBufferDataSize;
	g_color_buffer_data = new float[(uint64_t)3 * vertexBufferDataSize / 2];
	sizeof_g_color_buffer_data = 3 * vertexBufferDataSize / 2;

	ShaderRaster = LoadShaders(SHADER_VERTEX_RASTER_PATH, SHADER_COLOR_RASTER_PATH);
	ShaderTrigger = LoadShaders(SHADER_VERTEX_TRIGGER_PATH, SHADER_COLOR_TRIGGER_PATH);
	ShaderData = LoadShaders(SHADER_VERTEX_DATA_PATH, SHADER_COLOR_DATA_PATH);

}

void Screen::Dispose(void)
{
	glDisableVertexAttribArray(0);
	glDisableVertexAttribArray(1);
	glDisableVertexAttribArray(2);
	glDisableVertexAttribArray(3);
	glDisableVertexAttribArray(4);
	glDisableVertexAttribArray(5);


	glDeleteBuffers(2, rasterbuffer);
	glDeleteProgram(ShaderRaster);
	glDeleteBuffers(2, triggerbuffer);
	glDeleteProgram(ShaderTrigger);
	glDeleteBuffers(2, databuffer);
	glDeleteProgram(ShaderData);
	glDeleteVertexArrays(1, VAO);

	if (g_vertex_buffer_raster != NULL) delete[] g_vertex_buffer_raster;
	if (g_color_buffer_raster != NULL)  delete[] g_color_buffer_raster;
	if (g_color_buffer_trigger != NULL) delete[] g_color_buffer_trigger;
	if (g_color_buffer_data != NULL)  delete[] g_color_buffer_data;
}


void Screen::DrawRasterLinesX(unsigned int y, int* a, int* b) { //unsigned int y, int *pa, int *pb
	g_vertex_buffer_raster[*a] = -1.0f;										(*a)++;
	g_vertex_buffer_raster[*a] = (float)y / Win_H * 2.0f - 1.0f;			(*a)++;
	g_vertex_buffer_raster[*a] = (float)(Win_W - 1) / Win_W * 2.0f - 1.0f;	(*a)++;
	g_vertex_buffer_raster[*a] = (float)y / Win_H * 2.0f - 1.0f;			(*a)++;

	g_color_buffer_raster[*b] = 1.0f;			(*b)++;
	g_color_buffer_raster[*b] = 1.0f;			(*b)++;
	g_color_buffer_raster[*b] = 1.0f;			(*b)++;
	g_color_buffer_raster[*b] = 1.0f;			(*b)++;
	g_color_buffer_raster[*b] = 1.0f;			(*b)++;
	g_color_buffer_raster[*b] = 1.0f;			(*b)++;
}
void Screen::DrawRasterLinesY(unsigned int x, int* a, int* b) { //unsigned int x, int *pa, int *pb
	g_vertex_buffer_raster[*a] = (float)x / Win_W * 2.0f - 1.0f;							(*a)++;
	g_vertex_buffer_raster[*a] = -1.0f;														(*a)++;
	g_vertex_buffer_raster[*a] = (float)x / Win_W * 2.0f - 1.0f;							(*a)++;
	g_vertex_buffer_raster[*a] = (float)(Win_H - 1) / Win_H * 2.0f - 1.0f;					(*a)++;

	g_color_buffer_raster[*b] = 1.0f;			(*b)++;
	g_color_buffer_raster[*b] = 1.0f;			(*b)++;
	g_color_buffer_raster[*b] = 1.0f;			(*b)++;
	g_color_buffer_raster[*b] = 1.0f;			(*b)++;
	g_color_buffer_raster[*b] = 1.0f;			(*b)++;
	g_color_buffer_raster[*b] = 1.0f;			(*b)++;
}
void Screen::DrawStripesX(unsigned int y1, int* a, int* b)
{
	for (unsigned int y2 = y1 + 2; y2 < (y1 + (uint16_t)(Win_H / 10)); y2 = y2 + 2) {
		g_vertex_buffer_raster[*a] = (float)((uint16_t)(Win_W / 10)) / Win_W * 2.0f - 1.0f;					(*a)++; //*2 to start at second line
		g_vertex_buffer_raster[*a] = (float)y2 / Win_H * 2.0f - 1.0f;										(*a)++;
		g_vertex_buffer_raster[*a] = (float)(Win_W - 1 * (uint16_t)(Win_W / 10)) / Win_W * 2.0f - 1.0f; 	(*a)++;
		g_vertex_buffer_raster[*a] = (float)y2 / Win_H * 2.0f - 1.0f;										(*a)++;

		g_color_buffer_raster[*b] = 0.0f;			(*b)++;
		g_color_buffer_raster[*b] = 0.0f;			(*b)++;
		g_color_buffer_raster[*b] = 0.0f;			(*b)++;
		g_color_buffer_raster[*b] = 0.0f;			(*b)++;
		g_color_buffer_raster[*b] = 0.0f;			(*b)++;
		g_color_buffer_raster[*b] = 0.0f;			(*b)++;
	}
}
void Screen::DrawStripesY(unsigned int x1, int* a, int* b)
{
	for (unsigned int x2 = x1 + 2; x2 < (x1 + (uint16_t)(Win_W / 10)); x2 = x2 + 2) {
		g_vertex_buffer_raster[*a] = (float)x2 / Win_W * 2.0f - 1.0f;									(*a)++; //*2 to start at second line
		g_vertex_buffer_raster[*a] = (float)((uint16_t)(Win_H / 10)) / Win_H * 2.0f - 1.0f; 			(*a)++;
		g_vertex_buffer_raster[*a] = (float)x2 / Win_W * 2.0f - 1.0f;									(*a)++;
		g_vertex_buffer_raster[*a] = (float)(Win_H - 1 * (uint16_t)(Win_H / 10)) / Win_H * 2.0f - 1.0f; (*a)++;

		g_color_buffer_raster[*b] = 0.0f;			(*b)++;
		g_color_buffer_raster[*b] = 0.0f;			(*b)++;
		g_color_buffer_raster[*b] = 0.0f;			(*b)++;
		g_color_buffer_raster[*b] = 0.0f;			(*b)++;
		g_color_buffer_raster[*b] = 0.0f;			(*b)++;
		g_color_buffer_raster[*b] = 0.0f;			(*b)++;
	}
}
void Screen::GenerateRaster(void) {
	int a = 0; int b = 0; float c = 0.0f; signed int f = 0; int a1 = 0;
	for (int y = 0; y <= Win_H; y = y + (uint16_t)(Win_H / 10)) {
		DrawRasterLinesX(y, &a, &b);
	}
	for (int x = 0; x <= Win_W; x = x + (uint16_t)(Win_W / 10)) {
		DrawRasterLinesY(x, &a, &b);
	}

	for (int y1 = 0; y1 <= 10 * (uint16_t)(Win_H / 10); y1 = y1 + (uint16_t)(Win_H / 10)) {
		DrawStripesX(y1, &a, &b);
	}
	for (int x1 = 0; x1 <= 10 * (uint16_t)(Win_W / 10); x1 = x1 + (uint16_t)(Win_W / 10)) {
		DrawStripesY(x1, &a, &b);
	}

	//DrawRasterLinesX(5 * (uint16_t)(Win_H / 10) + 1, &a, &b); // no fat line on 0 X-axis
	DrawRasterLinesX(5 * (uint16_t)(Win_H / 10) + 0, &a, &b);
	//DrawRasterLinesX(5 * (uint16_t)(Win_H / 10) - 1, &a, &b); // no fat line on 0 X-axis
	//	DrawRasterLinesY(5 * ux(10) + 1, &a, &b);
	//	DrawRasterLinesY(5 * ux(10) + 0, &a, &b);
	//	DrawRasterLinesY(5 * ux(10) - 1, &a, &b);

	/*char str[5]; TCHAR Tstr[4 * 5]; USES_CONVERSION;
	sprintf(str, "%i", a);
	_tcscpy(Tstr, A2T(str));
	MessageBox(NULL, Tstr, L"values", MB_OK);
	sprintf(str, "%i", b);
	_tcscpy(Tstr, A2T(str));
	MessageBox(NULL, Tstr, L"values", MB_OK);*/
}
void Screen::ColorData(void) {
	for (unsigned int i = 0; i < 6*Win_W/2; i++) {
		g_color_buffer_data[(i * 3) + 0] = 1.0f;// 0.329412f;
		g_color_buffer_data[(i * 3) + 1] = 1.0f;//0.329412f;
		g_color_buffer_data[(i * 3) + 2] = 0.0f;//0.329412f;
	}
}
void Screen::ColorTrigger(void)
{
	for (unsigned int i = 0; i < sizeof_g_color_buffer_trigger / 3; i++)
	{
		g_color_buffer_trigger[(i * 3) + 0] = 0.0f;//0.329412f;
		g_color_buffer_trigger[(i * 3) + 1] = 120.0f/255.0f;//0.329412f;
		g_color_buffer_trigger[(i * 3) + 2] = 215.0f / 255.0f;//0.329412f;
	}
}

/*void Screen::InitIndices(void)
{
	for (int32_t x = 0; x < Win_W; x++) {

		g_indices[4 * x + 0] = 4 * x + 0 - x;//0:0	4:3
		g_indices[4 * x + 1] = 4 * x + 1 - x;//1:1	5:4
		g_indices[4 * x + 2] = 4 * x + 1 - x;//2:1	6:4
		g_indices[4 * x + 3] = 4 * x + 2 - x;//3:2	7:5
	}
}*/	
void Screen::InitScreen(void)
{
	
	//VAO Generalte Bind
	glGenVertexArrays(1, VAO);
	glBindVertexArray(VAO[0]);

	//VBO Generate and Bind
	glGenBuffers(2, rasterbuffer);

	glBindBuffer(GL_ARRAY_BUFFER, rasterbuffer[0]);
	glBufferData(GL_ARRAY_BUFFER, sizeof(float) * sizeof_g_vertex_buffer_raster, g_vertex_buffer_raster, GL_STATIC_DRAW);
	glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 0, (void*)0);

	glBindBuffer(GL_ARRAY_BUFFER, rasterbuffer[1]);
	glBufferData(GL_ARRAY_BUFFER, sizeof(float) * sizeof_g_color_buffer_raster, g_color_buffer_raster, GL_STATIC_DRAW);
	glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 0, (void*)0);

	//VBO Generate and Bind
	glGenBuffers(2, triggerbuffer);

	glBindBuffer(GL_ARRAY_BUFFER, triggerbuffer[0]);
	glBufferData(GL_ARRAY_BUFFER, sizeof(float) * sizeof_g_vertex_buffer_trigger, g_vertex_buffer_trigger, GL_DYNAMIC_DRAW);
	glVertexAttribPointer(4, 2, GL_FLOAT, GL_FALSE, 0, (void*)0);

	glBindBuffer(GL_ARRAY_BUFFER, triggerbuffer[1]);
	glBufferData(GL_ARRAY_BUFFER, sizeof(float) * sizeof_g_color_buffer_trigger, g_color_buffer_trigger, GL_STATIC_DRAW);
	glVertexAttribPointer(5, 3, GL_FLOAT, GL_FALSE, 0, (void*)0);

	//	glBindVertexArray(VAO[1]);

	//VBO Generate and Bind
	glGenBuffers(2, databuffer);

	for (int i = 1; i < sizeof_g_vertex_buffer_data; i += 2) g_vertex_buffer_data[i] = 0.25;

	glBindBuffer(GL_ARRAY_BUFFER, databuffer[1]);
	glBufferData(GL_ARRAY_BUFFER, sizeof(float)*sizeof_g_vertex_buffer_data, g_vertex_buffer_data, GL_DYNAMIC_DRAW);
	//glBufferData(GL_ARRAY_BUFFER, sizeof_g_vertex_buffer_data, NULL, GL_DYNAMIC_DRAW);
	//glBufferSubData(GL_ARRAY_BUFFER, 0, sizeof_g_vertex_buffer_data, g_vertex_buffer_data);
	glVertexAttribPointer(3, 2, GL_FLOAT, GL_FALSE, 0, (void*)0);


	glBindBuffer(GL_ARRAY_BUFFER, databuffer[0]);
	glBufferData(GL_ARRAY_BUFFER, sizeof(float) * sizeof_g_color_buffer_data, g_color_buffer_data, GL_STATIC_DRAW);
	//glBufferData(GL_ARRAY_BUFFER, sizeof_g_color_buffer_data, NULL, GL_STATIC_DRAW);
	//glBufferSubData(GL_ARRAY_BUFFER, 0, sizeof_g_color_buffer_data, g_color_buffer_data);
	glVertexAttribPointer(2, 3, GL_FLOAT, GL_FALSE, 0, (void*)0);

	//Indices Generate and Bindg
	//glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ebo[0]);
	//glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(GLushort) * (Win_W) * 4, &g_indices[0], GL_STATIC_DRAW);

	glEnableVertexAttribArray(0);	
	glEnableVertexAttribArray(1);
	glEnableVertexAttribArray(2);
	glEnableVertexAttribArray(3);
	glEnableVertexAttribArray(4);
	glEnableVertexAttribArray(5);
	

}
void Screen::BuildScreen(void)
{

	glClear(GL_COLOR_BUFFER_BIT);
	glUseProgram(ShaderRaster);
	glDrawArrays(GL_LINES, 0, sizeof_g_vertex_buffer_raster /2);
	//float rands = (float)(rand()%100)/100.0;
	//for (int i = 1; i < sizeof_g_vertex_buffer_data; i += 2) g_vertex_buffer_data[i] = rands;
		
	glBindBuffer(GL_ARRAY_BUFFER, triggerbuffer[0]);
	glBufferData(GL_ARRAY_BUFFER, sizeof(float) * sizeof_g_vertex_buffer_trigger, g_vertex_buffer_trigger, GL_DYNAMIC_DRAW);
	glUseProgram(ShaderTrigger);
	glDrawArrays(GL_LINES, 0, sizeof_g_vertex_buffer_trigger/2); //with lines count is divided by number of vertices in tupple


	glBindBuffer(GL_ARRAY_BUFFER, databuffer[1]); //to ensure data is bound
	glBufferData(GL_ARRAY_BUFFER, sizeof(float) * sizeof_g_vertex_buffer_data, g_vertex_buffer_data, GL_DYNAMIC_DRAW);
	glUseProgram(ShaderData);
	if (Extrapolate[0] == 1) 
		glDrawArrays(GL_LINE_STRIP, 0, LastDataPosition[0] / 2);
	else
		glDrawArrays(GL_POINTS, 0, LastDataPosition[0]);
	

	glFinish();
	// Swap buffers

}
void Screen::BuildSuspendedScreen(void)
{
	glUseProgram(ShaderRaster);
	glDrawArrays(GL_LINES, 0, sizeof_g_vertex_buffer_raster / 2);

	glFinish();
	// Swap buffers
}

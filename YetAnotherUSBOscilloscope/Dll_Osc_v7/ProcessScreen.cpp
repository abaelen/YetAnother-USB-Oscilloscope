#include "pch.h"
#include "ProcessScreen.h"

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

	DrawRasterLinesX(5 * (uint16_t)(Win_H / 10) + 1, &a, &b);
	DrawRasterLinesX(5 * (uint16_t)(Win_H / 10) + 0, &a, &b);
	DrawRasterLinesX(5 * (uint16_t)(Win_H / 10) - 1, &a, &b);
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
	for (unsigned int i = 0; i < Win_W; i++) {
		g_color_buffer_data[(i * 9) + 0] = 1.0f;// 0.329412f;
		g_color_buffer_data[(i * 9) + 1] = 1.0f;//0.329412f;
		g_color_buffer_data[(i * 9) + 2] = 0.0f;//0.329412f;
		g_color_buffer_data[(i * 9) + 3] = 1.0f;//1.0f;
		g_color_buffer_data[(i * 9) + 4] = 1.0f;//1.0f;
		g_color_buffer_data[(i * 9) + 5] = 0.0f;
		g_color_buffer_data[(i * 9) + 6] = 1.0f;//1.0f;
		g_color_buffer_data[(i * 9) + 7] = 1.0f;//1.0f;
		g_color_buffer_data[(i * 9) + 8] = 0.0f;
		g_color_buffer_data[(i * 9) + 9] = 1.0f;//0.329412f;
		g_color_buffer_data[(i * 9) + 10] = 1.0f;//0.329412f;
		g_color_buffer_data[(i * 9) + 11] = 0.0f;//0.329412f;
	}
}
void Screen::InitScreen(void)
{
	//VAO Generalte Bind
	glGenVertexArrays(1, VAO);
	glBindVertexArray(VAO[0]);

	//VBO Generate and Bind
	glGenBuffers(2, rasterbuffer);

	glBindBuffer(GL_ARRAY_BUFFER, rasterbuffer[0]);
	glBufferData(GL_ARRAY_BUFFER, sizeof_g_vertex_buffer_raster, g_vertex_buffer_raster, GL_STATIC_DRAW);
	glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 0, (void*)0);

	glBindBuffer(GL_ARRAY_BUFFER, rasterbuffer[1]);
	glBufferData(GL_ARRAY_BUFFER, sizeof_g_color_buffer_raster, g_color_buffer_raster, GL_STATIC_DRAW);
	glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 0, (void*)0);


	//	glBindVertexArray(VAO[1]);

	//VBO Generate and Bind
	glGenBuffers(2, databuffer);
	glGenBuffers(1, ebo);

	glBindBuffer(GL_ARRAY_BUFFER, databuffer[1]);
	//	glBufferData(GL_ARRAY_BUFFER, sizeof_g_vertex_buffer_data, g_vertex_buffer_data, GL_DYNAMIC_DRAW);
	glBufferData(GL_ARRAY_BUFFER, sizeof_g_vertex_buffer_data, NULL, GL_DYNAMIC_DRAW);
	glBufferSubData(GL_ARRAY_BUFFER, 0, sizeof_g_vertex_buffer_data, g_vertex_buffer_data);
	glVertexAttribPointer(3, 2, GL_FLOAT, GL_FALSE, 0, (void*)0);


	glBindBuffer(GL_ARRAY_BUFFER, databuffer[0]);
	//	glBufferData(GL_ARRAY_BUFFER, sizeof_g_color_buffer_data, g_color_buffer_data, GL_STATIC_DRAW);
	glBufferData(GL_ARRAY_BUFFER, sizeof_g_color_buffer_data, NULL, GL_STATIC_DRAW);
	glBufferSubData(GL_ARRAY_BUFFER, 0, sizeof_g_color_buffer_data, g_color_buffer_data);
	glVertexAttribPointer(2, 3, GL_FLOAT, GL_FALSE, 0, (void*)0);

	//Indices Generate and Bindg
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ebo[0]);
	glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(GLushort) * (Win_W + 4) * 4, &g_indices[0], GL_STATIC_DRAW);

	glEnableVertexAttribArray(0);	
	glEnableVertexAttribArray(1);
	glEnableVertexAttribArray(2);
	glEnableVertexAttribArray(3);

}
void Screen::BuildScreen(void)
{
	glUseProgram(ShaderRaster);
	glDrawArrays(GL_LINES, 0, sizeof_g_vertex_buffer_raster / sizeof(float));

	//			glBindVertexArray(VAO[1]);
	glBindBuffer(GL_ARRAY_BUFFER, databuffer[1]);
	glBufferSubData(GL_ARRAY_BUFFER, 0, sizeof_g_vertex_buffer_data, g_vertex_buffer_data);
	//glBufferData(GL_ARRAY_BUFFER, sizeof_g_vertex_buffer_data, g_vertex_buffer_data, GL_DYNAMIC_DRAW);
	glBindBuffer(GL_ARRAY_BUFFER, databuffer[0]);
	glBufferSubData(GL_ARRAY_BUFFER, 0, sizeof_g_color_buffer_data, g_color_buffer_data);
	//glBufferData(GL_ARRAY_BUFFER, sizeof_g_color_buffer_data, g_color_buffer_data, GL_STATIC_DRAW);


	glUseProgram(ShaderData);
	//	glDrawArrays(GL_LINES, 0, sizeof_g_vertex_buffer_data / sizeof(float));
	if (optionExtrapolated == true) 
	{
		glDrawElements(GL_LINE_STRIP, 4 * Win_W, GL_UNSIGNED_SHORT, 0); //OpenGL does not support GL_UNSIGNED_INT and indices is short
	}
	else
	{
		glDrawElements(GL_LINES, 4 * Win_W, GL_UNSIGNED_SHORT, 0); //OpenGL does not support GL_UNSIGNED_INT and indices is short
	}

	glFinish();
	// Swap buffers
}
void Screen::BuildSuspendedScreen(void)
{
	glUseProgram(ShaderRaster);
	glDrawArrays(GL_LINES, 0, sizeof_g_vertex_buffer_raster / sizeof(float));

	glFinish();
	// Swap buffers
}
void Screen::Dispose(void) 
{
	glDisableVertexAttribArray(0);
	glDisableVertexAttribArray(1);
	glDisableVertexAttribArray(2);
	glDisableVertexAttribArray(3);
	// Cleanup VBO and shader
	glDeleteBuffers(2, rasterbuffer);
	glDeleteProgram(ShaderRaster);
	glDeleteBuffers(2, databuffer);
	glDeleteBuffers(1, ebo);
	glDeleteProgram(ShaderData);
	glDeleteVertexArrays(1, VAO);
	//glDeleteVertexArrays(1, &VAO_Data);
}
void Screen::InitParams(GLfloat* vertex_raster, uint32_t sizeof_vertex_raster, GLfloat* color_raster, uint32_t sizeof_color_raster, GLfloat* vertex_data, uint32_t sizeof_vertex_data, GLfloat* color_data, uint32_t sizeof_color_data, GLushort* indices, uint32_t sizeof_indices, const uint16_t win_H, const uint16_t win_W)
{
	Win_H = win_H;
	Win_W = win_W;
	g_vertex_buffer_raster = vertex_raster;
	sizeof_g_vertex_buffer_raster=sizeof_vertex_raster;
	g_color_buffer_raster =color_raster;
	sizeof_g_color_buffer_raster = sizeof_color_raster;
	g_vertex_buffer_data =vertex_data;
	sizeof_g_vertex_buffer_data = sizeof_vertex_data;
	g_color_buffer_data = color_data;
	sizeof_g_color_buffer_data = sizeof_color_data;
	g_indices =indices;
	sizeof_g_indices = sizeof_indices;

	ShaderRaster = LoadShaders(SHADER_VERTEX_RASTER_PATH, SHADER_COLOR_RASTER_PATH);
	ShaderData = LoadShaders(SHADER_VERTEX_DATA_PATH, SHADER_COLOR_DATA_PATH);

}
#version 450 core
layout (location = 0) in vec4 aPos;

out vec4 vertexColor;

void main()
{
    gl_Position = aPos;
    vertexColor = vec4(0.5, 0.0, 0.0, 1.0);
}
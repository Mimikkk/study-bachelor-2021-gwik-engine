#version 450 core

layout (location = 0) in vec3 v_Position;
layout (location = 1) in vec2 v_TextureCoordinate;
layout (location = 2) in vec3 v_Normal;

uniform mat4 u_Projection;
uniform mat4 u_View;
uniform mat4 u_Model;

out vec2 f_TextureCoordinate;

void main()
{
    gl_Position = u_Projection * u_View * u_Model *vec4(v_Position, 1);
    f_TextureCoordinate = v_TextureCoordinate;
}
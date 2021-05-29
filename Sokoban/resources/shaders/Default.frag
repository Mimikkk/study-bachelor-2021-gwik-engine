#version 450 core

uniform sampler2D u_Texture0;

in vec2 f_TextureCoordinate;

out vec4 f_Color;

void main()
{
    f_Color = texture(u_Texture0, f_TextureCoordinate);
} 
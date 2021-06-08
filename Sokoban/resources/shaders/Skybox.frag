#version 450 core

uniform samplerCube skybox;

in vec3 f_TextureCoordinate;

out vec4 f_fragColor;

void main()
{
    f_fragColor = texture(skybox, f_TextureCoordinate);
}
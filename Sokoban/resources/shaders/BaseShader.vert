#version 330 core
layout (location = 0) in vec3 v_Pos;
layout (location = 1) in vec2 v_TexCoords;
layout (location = 2) in vec3 v_Normal;

uniform mat4 u_Projection;
uniform mat4 u_View;
uniform mat4 u_Model;

out vec3 f_Normal;
out vec2 f_TexCoords;
out vec3 f_Pos;

void main()
{
    gl_Position = u_Projection * u_View * u_Model * vec4(v_Pos, 1.0);

    f_Pos = vec3(u_Model * vec4(v_Pos, 1.0));
    f_TexCoords = v_TexCoords;
    f_Normal = mat3(transpose(inverse(u_Model))) * v_Pos;
}
#version 430 core

uniform sampler2D diffuseTexture;

uniform float brightness;

//in vec3 pos;
in vec3 n;
in vec2 uvs;

out vec4 color;

void main()
{
	color = texture2D(diffuseTexture, uvs, 0.0) * brightness;
}
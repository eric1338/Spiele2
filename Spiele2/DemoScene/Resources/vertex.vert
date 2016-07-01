#version 430 core				

uniform mat4 camera;

in vec3 position;
in vec3 normal;
in vec2 uv;

uniform vec3 instancePosition;
uniform float instanceScale;

out vec3 pos;
out vec3 n;
out vec2 uvs;

void main() 
{
	vec3 posi = position * instanceScale + instancePosition;

	pos = posi;
	n = normal;
	uvs = uv;

	gl_Position = camera * vec4(posi, 1.0);
}
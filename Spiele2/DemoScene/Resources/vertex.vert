#version 430 core				

uniform mat4 camera;

in vec3 vertexPosition;
in vec3 vertexNormal;
in vec2 vertexUV;

uniform vec3 instancePosition;
uniform float instanceScale;

uniform mat3 instanceRotation;

uniform float time;

out vec3 pos;
out vec3 n;
out vec2 uvs;

void main()
{
	vec3 rotatedPosition = vertexPosition * instanceRotation;
	vec3 rotatedNormal = vertexNormal * instanceRotation;

	vec3 posi = rotatedPosition * instanceScale + instancePosition;

	pos = posi;
	n = rotatedNormal;
	uvs = vertexUV;

	gl_Position = camera * vec4(posi, 1.0);
}
#version 430 core				

uniform mat4 camera;

in vec3 vertexPosition;
in vec3 vertexNormal;
in vec2 vertexUV;

uniform vec3 instancePosition;
uniform float instanceScale;

uniform vec3 polePosition;
uniform vec3 windDirection;
uniform float waveSpeed;
uniform float waveAmplitude;
uniform float time;

out vec3 pos;
out vec3 n;
out vec2 uvs;

// todo: Normale berechnen

void main()
{
	vec3 position = vertexPosition * instanceScale + instancePosition;

	float waveFrequency = 1.5;
	float flagScale = 0.9;

	float distanceToPole = length(vec3(vertexPosition.x, 0, vertexPosition.z)) * instanceScale;

	float amplitude = waveAmplitude * min((distanceToPole * 12), 1);
	float waveFactor = sin(waveSpeed * time + distanceToPole * waveFrequency) * amplitude;

	vec3 normalizedWindDirection = normalize(vec3(windDirection.x, 0, windDirection.z));
	vec3 offset = vec3(normalizedWindDirection.z * -waveFactor, position.y, normalizedWindDirection.x * waveFactor);
	
	vec3 vectorToPole = normalize(windDirection) * distanceToPole * flagScale;
	vec3 xzPosition = polePosition + vectorToPole + offset;

	position = vec3(xzPosition.x, position.y, xzPosition.z);

	pos = position;
	n = vertexNormal;
	uvs = vertexUV;

	gl_Position = camera * vec4(position, 1.0);
}
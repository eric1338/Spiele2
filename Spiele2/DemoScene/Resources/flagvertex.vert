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

	float waveFrequency = 1.4;

	float flagScale = 0.9;

	float distanceToPole = length(vec3(vertexPosition.x * instanceScale, 0, vertexPosition.z * instanceScale));

	vec3 flatPosition = vec3(position.x, 0, position.z);

	float amplitude = waveAmplitude * min((distanceToPole * 4), 0.5);

	float waveFactor = sin(waveSpeed * time + waveFrequency * distanceToPole) * amplitude;

	vec3 normalizedPosition = normalize(flatPosition);

	vec3 offs = vec3(normalizedPosition.z * -waveFactor, position.y, normalizedPosition.x * waveFactor);
	
	vec3 newDVec = normalize(windDirection) * distanceToPole * flagScale;
	vec3 finalXZ = polePosition + newDVec + offs;

	vec3 finalPosi = vec3(finalXZ.x, position.y, finalXZ.z);

	position = finalPosi;

	position = vec3(position.x + waveSpeed, position.y + waveAmplitude, position.z);

	pos = position;
	n = vertexNormal;
	uvs = vertexUV;

	gl_Position = camera * vec4(position, 1.0);
}
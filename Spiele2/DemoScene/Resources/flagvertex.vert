#version 430 core				

uniform mat4 camera;

in vec3 position;
in vec3 normal;
in vec2 uv;

uniform vec3 instancePosition;
uniform float instanceScale;

uniform float time;

out vec3 pos;
out vec3 n;
out vec2 uvs;

void main() 
{
	vec3 posi = position * instanceScale + instancePosition;

	vec3 wind = vec3(1, 0, 0.5);
	vec3 mast = vec3(0, 0, 0);
	float flagScale = 0.9;

	// todo: Normale berechnen
	// andere Namen

	vec3 dVec = vec3(posi.x, 0, posi.z) - vec3(mast.x, 0, mast.z);

	vec3 newDVec = dVec * flagScale;

	float trig = sin(2 * time + length(dVec)) * 0.8;

	vec3 nPosi = normalize(posi);

	vec3 offs = vec3(nPosi.z * -trig, 0, nPosi.x * trig);

	vec3 finalXZ = mast + newDVec + offs;

	vec3 finalPosi = vec3(finalXZ.x, posi.y, finalXZ.z);

	posi = finalPosi;

	pos = posi;
	n = normal;
	uvs = uv;

	gl_Position = camera * vec4(posi, 1.0);
}
#version 430 core
uniform vec3 cameraPosition;

uniform vec3 materialColor;

uniform vec3 lightDirection;
uniform vec3 lightColor;
uniform float ambientFactor;

in vec3 pos;
in vec3 n;
in vec2 uvs;

out vec4 color;

float lambert(vec3 n, vec3 l)
{
	return max(0, dot(n, l));
}

float specular(vec3 n, vec3 l, vec3 v, float shininess)
{
	vec3 r = reflect(-l, n);
	return pow(max(0, dot(r, v)), shininess);
}

void main()
{
	vec3 normal = normalize(n);
	vec3 v = normalize(cameraPosition - pos);

	float lam = lambert(normal, -lightDirection);
	float diff = lambert(normal, -lightDirection);

	vec3 l = vec3(0, 3, 1);

	vec4 materialColor2 = vec4(materialColor, 1);

	vec4 diffuse = materialColor2 * vec4(lightColor, 1) * lam;

	vec4 ambient = ambientFactor * vec4(lightColor, 1) * materialColor2;

	color = diffuse * vec4(lightColor, 1) + ambient;
}
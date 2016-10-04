#version 430 core
uniform vec3 cameraPosition;

uniform vec3 materialColor;

uniform vec3 lightDirection;
uniform vec3 lightColor;
uniform float ambientFactor;

uniform vec3 lightningBugPosition;

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
	//if(0 > dot(n, l)) return 0;
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

	vec4 materialColor4 = vec4(materialColor, 1);

	vec4 diffuse = materialColor4 * vec4(lightColor, 1) * lam;

	vec4 ambient = materialColor4 * vec4(lightColor, 1) * ambientFactor;

	float diffuseColorFactor = (diff > 0.9) ? 1 : (diff > 0.5) ? 0.5 : 0;

	color = diffuse * vec4(lightColor, 1) * diffuseColorFactor + ambient;

	// toon-shading
	if(abs(dot(normal, v)) < 0.24) color = vec4(0, 0, 0, 1);

}
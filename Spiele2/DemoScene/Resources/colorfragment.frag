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

	vec4 materialColor2 = vec4(materialColor, 1);

	vec4 diffuse = materialColor2 * vec4(lightColor, 1) * lam;

	vec4 ambient = ambientFactor * vec4(lightColor, 1) * materialColor2;

	color = diffuse * vec4(lightColor, 1) + ambient;

	//toon shading == discrete (quantized) steps of diffuse lighting
	//vec4 maxColor = materialColor2 * vec4(lightColor, 1);
	//color = (diff > 0.9) ? maxColor : (diff > 0.5) ? 0.5 * maxColor : ambient;

	//float spec = specular(normal, l, v, 100);
	//if(spec > 0.8) color = vec4(1);

	//cel shading == detect edges and color them
	//if(abs(dot(normal, v)) < 0.18)
	//{
	//	//color = vec4(0, 0, 0, 1);
	//}
}
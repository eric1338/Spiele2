#version 430 core
uniform vec3 cameraPosition;

uniform sampler2D diffuseTexture;
//uniform sampler2D texN;
//uniform sampler2D texS;

uniform vec3 lightDirection;
uniform vec3 lightColor;

//in vec3 pos;
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
	//vec3 v = normalize(cameraPosition - pos);

	float lam = lambert(normal, -lightDirection);
	
	vec4 mat = texture2D(diffuseTexture, uvs, 0.0);

	vec4 diffuse = mat * vec4(lightColor, 1) * lam;

	vec4 ambient = 0.5 * vec4(lightColor, 1) * mat;

	color = diffuse * vec4(lightColor, 1) + ambient;

	//float cl = myColor.r * 0.213 + myColor.g * 0.715 + myColor.b * 0.07;
	//color = vec4(cl, cl, cl, 1);
}
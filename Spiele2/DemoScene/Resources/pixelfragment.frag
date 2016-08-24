#version 430 core
uniform vec3 cameraPosition;

uniform sampler2D diffuseTexture;
//uniform sampler2D texN;
//uniform sampler2D texS;

uniform float specularFactor;

uniform vec3 lightDirection;
uniform vec3 lightColor;
uniform float ambientFactor;

uniform float pixelFactor;

uniform vec3 playerPosition;
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
	
	//float diff = lambert(normal, -lightDirection); // fuer cell
	
	vec4 materialColor = texture2D(diffuseTexture, uvs, 0.0);

	vec4 lightColor4 = vec4(lightColor, 1);

	float spec = specular(normal, -lightDirection, v, 100) * specularFactor;
	vec4 diffuse = materialColor * lightColor4 * lam + lightColor4 * spec;

	vec4 ambient = ambientFactor * 2 * lightColor4 * materialColor;

	//color = diffuse + ambient;
	//color = vec4(lam, lam, lam, 1);

	float uvx = round(uvs.x * 50) / 50;
	float uvy = round(uvs.y * 50) / 50;

	color = texture2D(diffuseTexture, vec2(uvx, uvy), 0.0);
}
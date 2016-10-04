#version 430 core
uniform vec3 cameraPosition;

uniform sampler2D diffuseTexture;
uniform sampler2D specularTexture;

uniform float specularFactor;

uniform vec3 lightDirection;
uniform vec3 lightColor;
uniform float ambientFactor;

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
	
	vec4 materialColor = texture2D(diffuseTexture, uvs, 0.0);

	vec4 lightColor4 = vec4(lightColor, 1);

	float shininessFactor = 2;

	vec4 specularColor = texture2D(specularTexture, uvs, 0.0);

	float redFactor = specularColor.r * shininessFactor;
	float greenFactor = specularColor.g * shininessFactor;
	float blueFactor = specularColor.b * shininessFactor;

	float specRed = lightColor4.r * specular(normal, -lightDirection, v, redFactor * 100) * redFactor * specularFactor;
	float specGreen = lightColor4.g * specular(normal, -lightDirection, v, greenFactor * 100) * greenFactor * specularFactor;
	float specBlue = lightColor4.b * specular(normal, -lightDirection, v, blueFactor * 100) * blueFactor * specularFactor;

	vec4 specColor = vec4(specRed, specGreen, specBlue, lightColor4.a);

	vec4 diffuse = materialColor * lightColor4 * lam + specColor;
	
	vec4 ambient = materialColor * lightColor4 * ambientFactor;

	float lightningBugFactor = max((1.5 - length(pos - lightningBugPosition)) / 1.5, 0);
	vec4 lightningBugColor = vec4(0.9, 1, 0.4, 1) * lightningBugFactor * 0.4;

	color = diffuse + ambient + materialColor * lightningBugColor;
}
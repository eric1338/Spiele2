#version 430 core
uniform vec3 cameraPosition;

uniform sampler2D diffuseTexture;
uniform sampler2D specularTexture;

uniform float specularFactor;

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
	
	//float diff = lambert(normal, -lightDirection); // fuer cell
	
	vec4 materialColor = texture2D(diffuseTexture, uvs, 0.0);

	vec4 lightColor4 = vec4(lightColor, 1);


	//spec
	float shininessFactor = 2;

	vec4 specularColor = texture2D(specularTexture, uvs, 0.0);

	float redFactor = specularColor.r * shininessFactor;
	float greenFactor = specularColor.g * shininessFactor;
	float blueFactor = specularColor.b * shininessFactor;

	float specRed = lightColor4.r * specular(normal, -lightDirection, v, redFactor * 100) * redFactor * specularFactor;
	float specGreen = lightColor4.g * specular(normal, -lightDirection, v, greenFactor * 100) * greenFactor * specularFactor;
	float specBlue = lightColor4.b * specular(normal, -lightDirection, v, blueFactor * 100) * blueFactor * specularFactor;

	//float specRed = lightColor4.r * redFactor * specularFactor;
	//float specGreen = lightColor4.g * greenFactor * specularFactor;
	//float specBlue = lightColor4.b * blueFactor * specularFactor;

	vec4 specColor = vec4(specRed, specGreen, specBlue, lightColor4.a);

	vec4 diffuse = materialColor * lightColor4 * lam + specColor;

	vec4 ambient = ambientFactor * 2 * lightColor4 * materialColor;

	color = diffuse + ambient;
	//color = vec4(lam, lam, lam, 1);

	//float lf = max((1.5 - length(pos - lightningBugPosition)) / 1.5, 0);
	//vec4 lbc = vec4(0.8, 1, 0.2, 1) * lf;

	//color = diffuse * vec4(lightColor, 1) + ambient + materialColor * lbc;

	//toon shading == discrete (quantized) steps of diffuse lighting
	//vec4 maxColor = materialColor * vec4(lightColor, 1);
	//color = (diff > 0.9) ? maxColor : (diff > 0.5) ? 0.5 * maxColor : ambient;

	//vec3 l = vec3(0, 3, 1);
	//float spec = specular(normal, l, v, 100);
	//if(spec > 0.8) color = vec4(1);

	//cel shading == detect edges and color them
	//if(abs(dot(normal, v)) < 0.18)
	//{
	//	//color = vec4(0, 0, 0, 1);
	//}

	//float cl = myColor.r * 0.213 + myColor.g * 0.715 + myColor.b * 0.07;
	//color = vec4(cl, cl, cl, 1);
}
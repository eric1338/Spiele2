#version 430 core
uniform vec3 cameraPosition;

uniform sampler2D diffuseTexture;
uniform sampler2D specularTexture;

uniform float specularFactor;

uniform vec3 lightDirection;
uniform vec3 lightColor;
uniform float ambientFactor;

uniform vec3 lightningBugPosition;

uniform vec3 viewDirection;
uniform vec3 playerPosition;

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

	vec3 nv = normalize(vec3(viewDirection.x, 0, viewDirection.z));
	vec3 nv2 = normalize(vec3(pos.x, 0, pos.z) - vec3(playerPosition.x, 0, playerPosition.z));

	float alpha = lambert(nv2, nv);
	color = vec4(materialColor.rgb, pow(alpha, 64));
}
#version 430 core
uniform vec3 cameraPosition;

uniform sampler2D diffuseTexture;
//uniform sampler2D texN;
//uniform sampler2D texS;

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
	
	//float diff = lambert(normal, -lightDirection); // fuer cell
	
	vec4 materialColor = texture2D(diffuseTexture, uvs, 0.0);

	vec4 lightColor4 = vec4(lightColor, 1);

	float spec = specular(normal, -lightDirection, v, 100) * specularFactor;
	vec4 diffuse = materialColor * lightColor4 * lam + lightColor4 * spec;

	vec4 ambient = ambientFactor * 2 * lightColor4 * materialColor;
	

	float lightningBugFactor = max((1.5 - length(pos - lightningBugPosition)) / 1.5, 0);
	vec4 lightningBugColor = vec4(0.9, 1, 0.4, 1) * lightningBugFactor * 0.4;

	color = diffuse + ambient + materialColor * lightningBugColor;
	//color = vec4(lam, lam, lam, 1);

	// distance-fog
	//float na = min(max(2 - (length(playerPosition - vec3(-3, 1, -3) - pos) / 10), 0), 1);
	//color = vec4(color.x, color.y, color.z, na);


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
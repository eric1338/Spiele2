#version 430 core
uniform vec3 cameraPosition;

uniform sampler2D diffuseTexture;
uniform sampler2D specularTexture;

uniform float specularFactor;

uniform vec3 lightDirection;
uniform vec3 lightColor;
uniform float ambientFactor;

uniform vec3 lightningBugPosition;

uniform vec3 test;
uniform vec3 test2;

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
	float d = 0.001;
	float ux, uy;
	float red = 0;
	float green = 0;
	float blue = 0;
	float alpha = 0;
	int factor;
	int sum = 0;
	vec4 textureColor;

	int degree = 4;

	for (int i = -degree; i < degree + 1; i++) {
		for (int j = -degree; j < degree + 1; j++) {
			
			ux = uvs.x + i * d;
			uy = uvs.y + j * d;

			factor = (degree + degree + 1) - (abs(i) + abs(j));

			textureColor = texture2D(diffuseTexture, vec2(ux, uy), 0.0);

			red += textureColor.r * factor;
			green += textureColor.g * factor;
			blue += textureColor.b * factor;
			alpha += textureColor.a * factor;

			sum += factor;
		}
	}
	
	vec4 blurColor = vec4(red / sum, green / sum, blue / sum, alpha / sum);

	vec3 normal = normalize(n);

	vec3 v = normalize(cameraPosition - pos);

	float lam = lambert(normal, -lightDirection);
	
	vec4 materialColor = texture2D(diffuseTexture, uvs, 0.0);

	vec4 lightColor4 = vec4(lightColor, 1);

	float spec = specular(normal, -lightDirection, v, 100) * specularFactor;
	vec4 diffuse = blurColor * lightColor4 * lam + lightColor4 * spec;

	vec4 ambient = blurColor * lightColor4  * ambientFactor;

	float lightningBugFactor = max((1.5 - length(pos - lightningBugPosition)) / 1.5, 0);
	vec4 lightningBugColor = vec4(0.9, 1, 0.4, 1) * lightningBugFactor * 0.4;
	
	color = diffuse + ambient + blurColor * lightningBugColor;
}
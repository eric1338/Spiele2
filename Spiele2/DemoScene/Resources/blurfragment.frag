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

	float spec = specular(normal, -lightDirection, v, 100) * specularFactor;
	vec4 diffuse = materialColor * lightColor4 * lam + lightColor4 * spec;

	vec4 ambient = ambientFactor * 2 * lightColor4 * materialColor;

	//color = diffuse + ambient;
	//color = vec4(lam, lam, lam, 1);

	float d = 0.01;
	float ux = uvs.x;
	float uy = uvs.y;

	vec4 mid = texture2D(diffuseTexture, uvs, 0.0);
	vec4 left = texture2D(diffuseTexture, vec2(ux - d, uy), 0.0);
	vec4 right = texture2D(diffuseTexture, vec2(ux + d, uy), 0.0);
	vec4 top = texture2D(diffuseTexture, vec2(ux, uy + d), 0.0);
	vec4 bottom = texture2D(diffuseTexture, vec2(ux, uy - d), 0.0);

	float red = (mid.r + left.r + right.r + top.r + bottom.r) / 5;
	float green = (mid.g + left.g + right.g + top.g + bottom.g) / 5;
	float blue = (mid.b + left.b + right.b + top.b + bottom.b) / 5;
	float alpha = (mid.a + left.a + right.a + top.a + bottom.a) / 5;

	color = vec4(red, green, blue, alpha);
}
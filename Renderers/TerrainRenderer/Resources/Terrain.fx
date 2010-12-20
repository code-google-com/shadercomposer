
//
struct VertexShader_Input
{
	float3 positionWorld : POSITION0;
	float3 normal : NORMAL;
};

struct VertexShader_Output
{
	float4 position : SV_POSITION;
	
	float3 positionWorld : TEXCOORD1;
	float3 normal : TEXCOORD0;
};

struct PixelShader_Output
{
	float4 color : SV_TARGET;
};

// Textures
Texture2D noiseTexture;
Texture2D grassTexture;
Texture2D sandTexture;
Texture2D snowTexture;
Texture2D snowRockTexture;

SamplerState textureSampler
{
	Filter = ANISOTROPIC; MaxAnisotropy = 16;
	AddressU = Wrap; AddressV = Wrap;
};

SamplerState noiseSampler
{
	Filter = MIN_MAG_MIP_LINEAR;
	AddressU = Mirror; AddressV = Mirror;
};

// Sample from a detail texture at a given world space position
float4 SampleDetail(float4 positionWorldSpace, Texture2D textureObject)
{
	float4 value = 0;

	value += textureObject.Sample(textureSampler, positionWorldSpace.xz / 32.0f) * 0.5;
	value += textureObject.Sample(textureSampler, positionWorldSpace.xz / 16.0f) * 0.5;
	value += textureObject.Sample(textureSampler, positionWorldSpace.xz / 8.0f);
	value += textureObject.Sample(textureSampler, positionWorldSpace.xz / 4.0f);
	value += textureObject.Sample(textureSampler, positionWorldSpace.xz / 2.0f);
	value += textureObject.Sample(textureSampler, positionWorldSpace.xz / 1.0f);

	return value / 5.0;
}

// Sample the noise value at a given world space postition
float SampleNoise(float4 positionWorldSpace)
{
	float value = 0;

	value += noiseTexture.Sample(noiseSampler, positionWorldSpace.xz / float2(32.0f, 32.0f)).x * 0.5;
	value += noiseTexture.Sample(noiseSampler, positionWorldSpace.xz / float2(16.0f, 16.0f)).x;
	value += noiseTexture.Sample(noiseSampler, positionWorldSpace.xz / float2(4.0f, 4.0f)).x * 1.5;
	value += noiseTexture.Sample(noiseSampler, positionWorldSpace.xz / float2(1.0f, 1.0f)).x;

	return value / 4.0;
}

//
float4x4 matViewProjection;

float3 cameraPositionWorld;

//
VertexShader_Output VShader (VertexShader_Input input)
{
	VertexShader_Output output = (VertexShader_Output)0;

	output.position = mul(float4(input.positionWorld, 1), matViewProjection);

	output.positionWorld = input.positionWorld;
	output.normal = input.normal;
	
	return output;
}

///[ShaderGraph]///

PixelShader_Output PShader (VertexShader_Output input)
{
	PixelShader_Output output = (PixelShader_Output)0;
	
	float3 position = input.positionWorld;
	float3 normal = normalize(input.normal);
	float3 toCamera = normalize(cameraPositionWorld - input.positionWorld);
	float3 light = normalize(float3(0.2, -0.2, 0.2));

	// Blinn-Phong
	//float3 materialDiffuse = float3(0.5, 0.2, 0.6);
	//float3 materialSpecular = float3(0.5, 0.5, 0.5);
	//float materialPower = 10;
	//float3 ambient = float3(0.2, 0.2, 0.2);
	//float3 emissive = float3(0, 0, 0);
	//float3 dirLightColor = float3(1, 1, 1);

	//float3 halfway = normalize(light + toCamera);
	//float3 diffuse = saturate(dot(normal, light)) * materialDiffuse;
	//float3 specular = pow(saturate(dot(normal, halfway)), materialPower) * materialSpecular;
	//float3 color = (saturate(ambient + diffuse) + specular) * dirLightColor + emissive;
	//output.color = float4(color, 1);

	//float angle = dot(normal, toCamera);
	//output.color = float4(0.3 + float3(angle, angle, angle) * 0.7, 1);

	output.color = getColor(float4(position, 1), float4(normal, 0), float4(cameraPositionWorld,1));

	return output;
}

RasterizerState rasterizerStateNoCull
{
	CullMode = None;
	FillMode = Solid;
};

technique11 Technique1
{
	pass Pass1
	{
		SetVertexShader( CompileShader( vs_4_0, VShader() ) );
		SetGeometryShader( NULL );
		SetPixelShader( CompileShader( ps_4_0, PShader() ) );

		SetRasterizerState( rasterizerStateNoCull );
	}
}
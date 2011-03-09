
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
	output.color.w = 1.0;

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
Shader "FresnelSurfaceSolidTextured" {
	Properties {
		_TintAlbedo ("Tint Albedo", Vector) = (1,1,1,1)
		_MainTex ("Albedo Texture", 2D) = "white" {}
		[HDR] _TintEmission ("Tint Emission", Vector) = (1,1,1,1)
		_EmissionTex ("Emission Texture", 2D) = "white" {}
		[HDR] _TintFresnel ("Tint Fresnel", Vector) = (1,1,1,1)
		_FresnelTex ("Fresnel Texture", 2D) = "white" {}
		_FresnelScale ("Fresnel Scale", Float) = 1
		_FresnelPower ("Fresnel Power", Float) = 5
		_Smooth ("Smooth", Float) = 0
		_Metal ("Metal", Float) = 0
		_Grayscale ("Grayscale", Float) = 0
		[HideInInspector] __dirty ("", Float) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4x4 unity_ObjectToWorld;
			float4x4 unity_MatrixVP;
			float4 _MainTex_ST;

			struct Vertex_Stage_Input
			{
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Vertex_Stage_Output
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
			};

			Vertex_Stage_Output vert(Vertex_Stage_Input input)
			{
				Vertex_Stage_Output output;
				output.uv = (input.uv.xy * _MainTex_ST.xy) + _MainTex_ST.zw;
				output.pos = mul(unity_MatrixVP, mul(unity_ObjectToWorld, input.pos));
				return output;
			}

			Texture2D<float4> _MainTex;
			SamplerState sampler_MainTex;

			struct Fragment_Stage_Input
			{
				float2 uv : TEXCOORD0;
			};

			float4 frag(Fragment_Stage_Input input) : SV_TARGET
			{
				return _MainTex.Sample(sampler_MainTex, input.uv.xy);
			}

			ENDHLSL
		}
	}
	Fallback "Diffuse"
	//CustomEditor "ASEMaterialInspector"
}
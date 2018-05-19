// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "WoWEdit/Terrain/Low" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_MainTex2("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.0
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_ChunkCoordX ("Chunk Coord X", Float) = 0.0
		_ChunkCoordY("Chunk Coord Y", Float) = 0.0
	}
	SubShader 
	{
		Tags 
		{
			"RenderType"="Opaque"
		}
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert addshadow

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MainTex2;

		struct Input 
		{
			float2 uv_MainTex;
			float2 uv2_MainTex2;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _ChunkCoordX;
		float _ChunkCoordY;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
		// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutput o) 
		{
			float2 UVs;
			UVs.x = IN.uv2_MainTex2.x;
			UVs.y = IN.uv2_MainTex2.y;

			fixed4 c = tex2D (_MainTex2, UVs) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}

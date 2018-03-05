Shader "WoWEdit/WoWTerrain"
{
	Properties
	{
	[Header(Texture Layers)]
	[NoScaleOffset] _MainTex("Layer 0", 2D) = "black" {}
	[NoScaleOffset] _BlendTex1("Layer 1", 2D) = "black" {}
	[NoScaleOffset] _BlendTex2("Layer 2", 2D) = "black" {}
	[NoScaleOffset] _BlendTex3("Layer 3", 2D) = "black" {}

	[Header(Layer Tiling)]
	_MainTexTiling("Layer 0", Range(1,10)) = 1
	_BlendTex1Tiling("Layer 1", Range(1,10)) = 1
	_BlendTex2Tiling("Layer 2", Range(1,10)) = 1
	_BlendTex3Tiling("Layer 3", Range(1,10)) = 1

	[Header(Alpha Masks)]
	[NoScaleOffset] _BlendTexAmount1("Alpha 1", 2D) = "black" {}
	[NoScaleOffset] _BlendTexAmount2("Alpha 2", 2D) = "black" {}
	[NoScaleOffset] _BlendTexAmount3("Alpha 3", 2D) = "black" {}

	// Toggles //
	//[Header(WoWedit Toggles)]
	//[Toggle(VERTEX_COLOR_ON)] _VertexColor("Vertex Color Display", Float) = 1
	}

	SubShader
	{
		
		Tags{ "RenderType" = "Opaque" }
		
		LOD 200

		Name "FORWARD"
		CGPROGRAM


		#pragma surface surf Standard vertex:vert addshadow 
		#pragma target 3.0
		#pragma multi_compile VERTEX_COLOR_ON VERTEX_COLOR_OFF

		#include "Lighting.cginc"

		sampler2D _MainTex;
		sampler2D _BlendTex1;
		sampler2D _BlendTexAmount1;
		sampler2D _BlendTex2;
		sampler2D _BlendTexAmount2;
		sampler2D _BlendTex3;
		sampler2D _BlendTexAmount3;
		half _MainTexTiling;
		half _BlendTex1Tiling;
		half _BlendTex2Tiling;
		half _BlendTex3Tiling;

		struct Input
		{
			float2 uv_MainTex;
			float3 vertexColor;
		};

		struct v2f {
			float4 pos : SV_POSITION;
			fixed4 color : COLOR;
		};

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input,o);
			o.vertexColor = v.color; // Save the Vertex Color in the Input for the surf() method
		}

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 mainCol = tex2D(_MainTex, IN.uv_MainTex*_MainTexTiling);
			fixed4 tex1Col = tex2D(_BlendTex1, IN.uv_MainTex*_BlendTex1Tiling);
			fixed4 tex1Amount = tex2D(_BlendTexAmount1, IN.uv_MainTex);
			fixed4 tex2Col = tex2D(_BlendTex2, IN.uv_MainTex*_BlendTex2Tiling);
			fixed4 tex2Amount = tex2D(_BlendTexAmount2, IN.uv_MainTex);
			fixed4 tex3Col = tex2D(_BlendTex3, IN.uv_MainTex*_BlendTex3Tiling);
			fixed4 tex3Amount = tex2D(_BlendTexAmount3, IN.uv_MainTex);

			fixed4 mainOutput = mainCol.rgba * (1.0 - tex1Amount.a);
			fixed4 blendOutput1 = tex1Col.rgba * tex1Amount.a;
			fixed4 mainOutput1 = (mainOutput + blendOutput1) * (1.0 - tex2Amount.a);
			fixed4 blendOutput2 = tex2Col.rgba * tex2Amount.a;
			fixed4 mainOutput2 = (mainOutput1 + blendOutput2) * (1.0 - tex3Amount.a);
			fixed4 blendOutput3 = tex3Col.rgba * tex3Amount.a;
			fixed4 mainOutput3 = mainOutput2 + blendOutput3;

			//#ifdef VERTEX_COLOR_ON
			o.Albedo = mainOutput3.rgb * IN.vertexColor.rgb * 2;
			//#else
			//		o.Albedo = mainOutput3.rgb;
			//#endif
			//o.Emission = mainOutput3.rgb * IN.vertexColor.rgb * 0.5;
		}
		ENDCG
			
	}


	Fallback "VertexLit"
}
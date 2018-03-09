Shader "WoWEdit/WoWTerrain"
{
	Properties
	{
	[Header(Texture Layers)]
	[NoScaleOffset] _layer0("Layer 0", 2D) = "black" {}
	[NoScaleOffset] _layer1("Layer 1", 2D) = "black" {}
	[NoScaleOffset] _layer2("Layer 2", 2D) = "black" {}
	[NoScaleOffset] _layer3("Layer 3", 2D) = "black" {}

	[Header(Layer Tiling)]
	layer0scale("Layer 0", Range(1,10)) = 1
	layer1scale("Layer 1", Range(1,10)) = 1
	layer2scale("Layer 2", Range(1,10)) = 1
	layer3scale("Layer 3", Range(1,10)) = 1

	[Header(Alpha Masks)]
	[NoScaleOffset] _blend1("Alpha 1", 2D) = "black" {}
	[NoScaleOffset] _blend2("Alpha 2", 2D) = "black" {}
	[NoScaleOffset] _blend3("Alpha 3", 2D) = "black" {}

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

		sampler2D _layer0;
		sampler2D _layer1;
		sampler2D _blend1;
		sampler2D _layer2;
		sampler2D _blend2;
		sampler2D _layer3;
		sampler2D _blend3;
		half layer0scale;
		half layer1scale;
		half layer2scale;
		half layer3scale;

		struct Input
		{
			float2 uv_layer0;
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
			float2 trimmedUV = { 1-IN.uv_layer0.x, IN.uv_layer0.y };

			fixed4 mainCol = tex2D(_layer0, IN.uv_layer0*layer0scale);
			fixed4 tex1Col = tex2D(_layer1, IN.uv_layer0*layer1scale);
			fixed4 tex1Amount = tex2D(_blend1, trimmedUV);
			fixed4 tex2Col = tex2D(_layer2, IN.uv_layer0*layer2scale);
			fixed4 tex2Amount = tex2D(_blend2, trimmedUV);
			fixed4 tex3Col = tex2D(_layer3, IN.uv_layer0*layer3scale);
			fixed4 tex3Amount = tex2D(_blend3, trimmedUV);

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
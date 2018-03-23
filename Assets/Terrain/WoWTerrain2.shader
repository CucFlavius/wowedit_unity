Shader "WoWEdit/WoWTerrain2"
{
	Properties
	{
		[Header(Texture Layers)]
		[NoScaleOffset] _layer0("Layer 0", 2D) = "black" {}
		[NoScaleOffset] _layer1("Layer 1", 2D) = "black" {}
		[NoScaleOffset] _layer2("Layer 2", 2D) = "black" {}
		[NoScaleOffset] _layer3("Layer 3", 2D) = "black" {}

		[Header(Layer Tiling)]
		layer0scale("layer0scale", Float) = 2
		layer1scale("layer1scale", Float) = 2
		layer2scale("layer2scale", Float) = 2
		layer3scale("layer3scale", Float) = 2

		[Header(Height Textures)]
		[NoScaleOffset] _height0("Height 0", 2D) = "black" {}
		[NoScaleOffset] _height1("Height 1", 2D) = "black" {}
		[NoScaleOffset] _height2("Height 2", 2D) = "black" {}
		[NoScaleOffset] _height3("Height 3", 2D) = "black" {}

		[Header(Alpha Masks)]
		[NoScaleOffset] _blend1("Alpha 1", 2D) = "black" {}
		[NoScaleOffset] _blend2("Alpha 2", 2D) = "black" {}
		[NoScaleOffset] _blend3("Alpha 3", 2D) = "black" {}

		heightScale("HeightScale", Vector) = (0,0,0,0)
		heightOffset("HeightOffset", Vector) = (1,1,1,1)

		[NoScaleOffset] _shadowMap("Shadow Map", 2D) = "black"{}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		//Cull Off
		LOD 200
		Name "FORWARD"
		CGPROGRAM
		
		#pragma surface surf Lambert vertex:vert addshadow 
		#pragma target 3.0
		#pragma multi_compile VERTEX_COLOR_ON VERTEX_COLOR_OFF
		#include "Lighting.cginc"

		sampler2D _height0;
		sampler2D _height1;
		sampler2D _height2;
		sampler2D _height3;
		sampler2D _layer0;
		sampler2D _layer1;
		sampler2D _layer2;
		sampler2D _layer3;
		sampler2D _blend1;
		sampler2D _blend2;
		sampler2D _blend3;
		sampler2D _shadowMap;
		float4 heightOffset;
		float4 heightScale;
		float layer0scale;
		float layer1scale;
		float layer2scale;
		float layer3scale;

		struct Input
		{
			float2 uv_layer0;
			float3 vertexColor;
		};

		struct v2f 
		{
			float4 pos : SV_POSITION;
			fixed4 color : COLOR;
		};

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input,o);
			o.vertexColor = v.color; // Save the Vertex Color in the Input for the surf() method
		}

		void surf(Input IN, inout SurfaceOutput o)
		{
			float2 UVs = { IN.uv_layer0.x, IN.uv_layer0.y };	// Fixed UVs

			float shadowMap = tex2D(_shadowMap, UVs).a;
			float3 shadowMapRGB = float3(shadowMap, shadowMap, shadowMap);

			float2 tc0 = UVs * (8.0 / layer0scale);
			float2 tc1 = UVs * (8.0 / layer1scale);
			float2 tc2 = UVs * (8.0 / layer2scale);
			float2 tc3 = UVs * (8.0 / layer3scale);

			float3 blendTex;
			half alphaMap1 = tex2D(_blend1, UVs).a;
			half alphaMap2 = tex2D(_blend2, UVs).a;
			half alphaMap3 = tex2D(_blend3, UVs).a;
			blendTex = float3(alphaMap1, alphaMap2, alphaMap3);

			float3 one3 = { 1.0, 1.0, 1.0 };
			float4 one4 = { 1.0, 1.0, 1.0, 1.0 };
			float4 zero = { 0,0,0,0 };

			float sum = blendTex.x + blendTex.y + blendTex.z;
			float clamp_result = clamp(sum, 0, 1);
			float4 layer_weights = float4(1.0 - clamp_result,blendTex);
			float4 layer_pct = float4(layer_weights.x * (tex2D(_height0, tc0).a * heightScale[0] + heightOffset[0])
			, layer_weights.y * (tex2D(_height1, tc1).a * heightScale[1] + heightOffset[1])
			, layer_weights.z * (tex2D(_height2, tc2).a * heightScale[2] + heightOffset[2])
			, layer_weights.w * (tex2D(_height3, tc3).a * heightScale[3] + heightOffset[3])
			);

			float4 max1 = max(layer_pct.x, layer_pct.y);
			float4 max2 = max(layer_pct.y, layer_pct.z);
			float4 max3 = max(max1, max2);
			float4 layer_pct_max = max3;
			float4 scale = one4 - clamp(layer_pct_max - layer_pct, 0, 1);
			layer_pct = layer_pct * scale;
			float4 sum2 = dot(one4, layer_pct);
			layer_pct = layer_pct / float4(sum2);

			float4 weightedLayer_0 = tex2D(_layer0, tc0) * layer_pct.x;
			float4 weightedLayer_1 = tex2D(_layer1, tc1) * layer_pct.y;
			float4 weightedLayer_2 = tex2D(_layer2, tc2) * layer_pct.z;
			float4 weightedLayer_3 = tex2D(_layer3, tc3) * layer_pct.w;

			// these are used later in the shader. left in to emphasise that different layers contribute to different blends
			float metalBlend = weightedLayer_0.a + weightedLayer_1.a;
			float specBlend = weightedLayer_2.a + weightedLayer_3.a;

			// and combine weighted layers with vertex color and a constant factor to have the final diffuse layer
			float3 matDiffuse = (weightedLayer_0.rgb + weightedLayer_1.rgb + weightedLayer_2.rgb + weightedLayer_3.rgb) * IN.vertexColor.rgb * 2.0; // * 2.0 because mccv goes from 0.0 to 1.0
			//float3 matDiffuseShadow = (weightedLayer_0.rgb + weightedLayer_1.rgb + weightedLayer_2.rgb + weightedLayer_3.rgb) * IN.vertexColor.rgb * 2.0 -(shadowMapRGB /3); // * 2.0 because mccv goes from 0.0 to 1.0
			o.Albedo = matDiffuse;
		}
		ENDCG	
	}
	Fallback "VertexLit"
}
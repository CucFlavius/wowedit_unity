Shader "WoWEdit/Terrain/High"
{
	Properties
	{
		[Header(WoWEdit)]
		_WireThickness("Wire Thickness", RANGE(0, 800)) = 100
		_LineColor("Wire Color", Color) = (1.0, 1.0, 1.0, 1.0)

		[Header(Texture Layers)]
		[NoScaleOffset] _layer0("Layer 0", 2D) = "black" {}
		[NoScaleOffset] _layer1("Layer 1", 2D) = "black" {}
		[NoScaleOffset] _layer2("Layer 2", 2D) = "black" {}
		[NoScaleOffset] _layer3("Layer 3", 2D) = "black" {}

		[Header(Layer Tiling)]
		layer0scale("layer0scale", Float) = 1
		layer1scale("layer1scale", Float) = 1
		layer2scale("layer2scale", Float) = 1
		layer3scale("layer3scale", Float) = 1

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
		Tags
		{
			"RenderType" = "Transparent"
		}
		//Cull Off
		LOD 200
		Name "FORWARD"
		CGPROGRAM
		
		#pragma surface surf Lambert vertex:vert addshadow 
		#pragma target 3.0
		#pragma multi_compile VERTEX_COLOR_ON VERTEX_COLOR_OFF
		#pragma multi_compile WIREFRAME_ON WIREFRAME_OFF
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

		float _terrainVertexColorOn;

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

			float sum = dot(float3(1.0, 1.0, 1.0), blendTex);
			float clamp_result = clamp(sum, 0, 1);
			float4 layer_weights = float4(1.0 - clamp_result,blendTex);
			float4 layer_pct = float4(layer_weights.x * (tex2D(_height0, tc0).a * heightScale[0] + heightOffset[0])
									, layer_weights.y * (tex2D(_height1, tc1).a * heightScale[1] + heightOffset[1])
									, layer_weights.z * (tex2D(_height2, tc2).a * heightScale[2] + heightOffset[2])
									, layer_weights.w * (tex2D(_height3, tc3).a * heightScale[3] + heightOffset[3])
									);

			float4 max1 = max(layer_pct.x, layer_pct.y);
			float4 max2 = max(layer_pct.z, layer_pct.w);
			float4 max3 = max(max1, max2);
			float4 layer_pct_max = max3;
			float4 scale = float4(1.0, 1.0, 1.0, 1.0) - clamp(layer_pct_max - layer_pct, 0, 1);
			layer_pct = layer_pct * scale;
			float sum2 = dot(float4(1.0, 1.0, 1.0, 1.0), layer_pct);
			layer_pct = layer_pct / float4(sum2, sum2, sum2, sum2);

			float4 weightedLayer_0 = tex2D(_layer0, tc0) * layer_pct.x;
			float4 weightedLayer_1 = tex2D(_layer1, tc1) * layer_pct.y;
			float4 weightedLayer_2 = tex2D(_layer2, tc2) * layer_pct.z;
			float4 weightedLayer_3 = tex2D(_layer3, tc3) * layer_pct.w;

			// these are used later in the shader. left in to emphasise that different layers contribute to different blends
			float metalBlend = weightedLayer_0.a + weightedLayer_1.a;
			float specBlend = weightedLayer_2.a + weightedLayer_3.a;

			// and combine weighted layers with vertex color and a constant factor to have the final diffuse layer
			float3 vertColor = IN.vertexColor.rgb; // _terrainVertexColor
			if (_terrainVertexColorOn == 0)
			{
				vertColor = 0.5;
			}
			float3 matDiffuse = (weightedLayer_0.rgb + weightedLayer_1.rgb + weightedLayer_2.rgb + weightedLayer_3.rgb) * vertColor * 2.0; // * 2.0 because mccv goes from 0.0 to 1.0
			//float3 matDiffuseShadow = (weightedLayer_0.rgb + weightedLayer_1.rgb + weightedLayer_2.rgb + weightedLayer_3.rgb) * IN.vertexColor.rgb * 2.0 -(shadowMapRGB /3); // * 2.0 because mccv goes from 0.0 to 1.0
			//float3 matDiffuse = weightedLayer_0.rgb + weightedLayer_1.rgb + weightedLayer_2.rgb + weightedLayer_3.rgb;
			o.Albedo = matDiffuse;
		}
		ENDCG

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha

			// Wireframe shader based on the the following
			// http://developer.download.nvidia.com/SDK/10/direct3d/Source/SolidWireframe/Doc/SolidWireframe.pdf

			CGPROGRAM
			#pragma vertex vert
			#pragma geometry geom
			#pragma fragment frag
			#include "UnityCG.cginc"

			float _WireThickness;
			float _terrainWireframeOn;

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2g
			{
				float4 projectionSpaceVertex : SV_POSITION;
				float4 worldSpacePosition : TEXCOORD1;
			};

			struct g2f
			{
				float4 projectionSpaceVertex : SV_POSITION;
				float4 worldSpacePosition : TEXCOORD0;
				float4 dist : TEXCOORD1;
			};


			v2g vert(appdata v)
			{
				v2g o;
				//UNITY_SETUP_INSTANCE_ID(v);
				//UNITY_INITIALIZE_OUTPUT(v2g, o);
				o.projectionSpaceVertex = UnityObjectToClipPos(v.vertex);
				o.worldSpacePosition = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}

			[maxvertexcount(3)]
			void geom(triangle v2g i[3], inout TriangleStream<g2f> triangleStream)
			{
				float2 p0 = i[0].projectionSpaceVertex.xy / i[0].projectionSpaceVertex.w;
				float2 p1 = i[1].projectionSpaceVertex.xy / i[1].projectionSpaceVertex.w;
				float2 p2 = i[2].projectionSpaceVertex.xy / i[2].projectionSpaceVertex.w;

				float2 edge0 = p2 - p1;
				float2 edge1 = p2 - p0;
				float2 edge2 = p1 - p0;

				// To find the distance to the opposite edge, we take the
				// formula for finding the area of a triangle Area = Base/2 * Height, 
				// and solve for the Height = (Area * 2)/Base.
				// We can get the area of a triangle by taking its cross product
				// divided by 2.  However we can avoid dividing our area/base by 2
				// since our cross product will already be double our area.
				float area = abs(edge1.x * edge2.y - edge1.y * edge2.x);

				float camDist = distance(i[0].worldSpacePosition, _WorldSpaceCameraPos);

				float wireThickness = (800 - _WireThickness) * camDist / (100-camDist) * 30;
				g2f o;
				o.worldSpacePosition = i[0].worldSpacePosition;
				o.projectionSpaceVertex = i[0].projectionSpaceVertex;
				o.dist.xyz = float3((area / length(edge0)), 0.0, 0.0) * o.projectionSpaceVertex.w * wireThickness;
				o.dist.w = 1.0 / o.projectionSpaceVertex.w;
				triangleStream.Append(o);

				o.worldSpacePosition = i[1].worldSpacePosition;
				o.projectionSpaceVertex = i[1].projectionSpaceVertex;
				o.dist.xyz = float3(0.0, (area / length(edge1)), 0.0) * o.projectionSpaceVertex.w * wireThickness;
				o.dist.w = 1.0 / o.projectionSpaceVertex.w;
				triangleStream.Append(o);

				o.worldSpacePosition = i[2].worldSpacePosition;
				o.projectionSpaceVertex = i[2].projectionSpaceVertex;
				o.dist.xyz = float3(0.0, 0.0, (area / length(edge2))) * o.projectionSpaceVertex.w * wireThickness;
				o.dist.w = 1.0 / o.projectionSpaceVertex.w;
				triangleStream.Append(o);
			}

			uniform fixed4 _LineColor;

			fixed4 frag(g2f i) : SV_Target
			{
			float minDistanceToEdge = min(i.dist[0], min(i.dist[1], i.dist[2])) * i.dist[3];
			float dist = distance(_WorldSpaceCameraPos, i.worldSpacePosition);
			// Early out if we know we are not on a line segment.
			if (minDistanceToEdge > 0.9)
			{
				discard;
			}
			//return _LineColor;
			//return max(1 - (dist / 100), 0.5);

			_LineColor.a = max(1 - (dist / 30), 0) * _terrainWireframeOn;

			return _LineColor;
		}
		ENDCG
		}
	}
	Fallback "VertexLit"
}
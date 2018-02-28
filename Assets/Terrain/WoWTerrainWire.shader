// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "WoWEdit/WoWTerrainWire"
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

	_WireColor("WireColor", Color) = (1,0,0,1)
		_Color("Color", Color) = (1,1,1,1)

	// Toggles //
	//[Header(WoWedit Toggles)]
	//[Toggle(VERTEX_COLOR_ON)] _VertexColor("Vertex Color Display", Float) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200
		//Blend One OneMinusDstAlpha
		Blend One OneMinusDstAlpha
		Pass
			{
				CGPROGRAM
#include "UnityCG.cginc"
#pragma target 4.0
#pragma vertex verte
#pragma geometry geom
#pragma fragment frag


				half4 _WireColor, _Color;
				half        _MinVisDistance;
				half        _MaxVisDistance;
			struct v2g
			{
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct g2f
			{
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
				float3 dist : TEXCOORD1;
			};

			v2g verte(appdata_full v)
			{
				v2g OUT;
				OUT.pos = UnityObjectToClipPos(v.vertex);
				OUT.uv = v.texcoord;

				return OUT;
			}

			[maxvertexcount(3)]
			void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream)
			{

				float2 WIN_SCALE = float2(_ScreenParams.x / 2.0, _ScreenParams.y / 2.0);

				//frag position
				float2 p0 = WIN_SCALE * IN[0].pos.xy / IN[0].pos.w;
				float2 p1 = WIN_SCALE * IN[1].pos.xy / IN[1].pos.w;
				float2 p2 = WIN_SCALE * IN[2].pos.xy / IN[2].pos.w;


				//barycentric position
				float2 v0 = p2 - p1;
				float2 v1 = p2 - p0;
				float2 v2 = p1 - p0;
				//triangles area
				float area = abs(v1.x*v2.y - v1.y * v2.x);

				g2f OUT;
				OUT.pos = IN[0].pos;
				OUT.uv = IN[0].uv;
				OUT.dist = float3(area / length(v0),0,0);
				triStream.Append(OUT);

				OUT.pos = IN[1].pos;
				OUT.uv = IN[1].uv;
				OUT.dist = float3(0,area / length(v1),0);
				triStream.Append(OUT);

				OUT.pos = IN[2].pos;
				OUT.uv = IN[2].uv;
				OUT.dist = float3(0,0,area / length(v2));
				triStream.Append(OUT);

			}

			half4 frag(g2f IN) : COLOR
			{
				//distance of frag from triangles center
				float d = min(IN.dist.x , min(IN.dist.y, IN.dist.z));
				//fade based on dist from center
				float I = exp2(-5*d*d);

				return lerp(_Color, _WireColor, I);
			}

				ENDCG

			}


				CGPROGRAM


#pragma surface surf Standard  addshadow 
#pragma vertex vert
#pragma target 4.0

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

#ifdef VERTEX_COLOR_ON
				o.Albedo = mainOutput3.rgb * IN.vertexColor.rgb * 2;
#else
				o.Albedo = mainOutput3.rgb;
#endif
			}
			ENDCG

	}
	Fallback "VertexLit"
}
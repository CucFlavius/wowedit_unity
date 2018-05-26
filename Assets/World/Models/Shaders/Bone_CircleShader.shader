Shader "W/Circle Anti-Aliasing"
{
	Properties
	{
		_BoundColor("Bound Color", Color) = (0,0.5843137254901961,1,1)
		_BgColor("Background Color", Color) = (0.1176470588235294,0,0.5882352941176471,1)
		_circleSizePercent("Circle Size Percent", Range(0, 100)) = 50
		_border("Anti Alias Border Threshold", Range(0.00001, 5)) = 0.01
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 100

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
		// make fog work
#pragma multi_compile_fog

#include "UnityCG.cginc"

		struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	float _border;

	fixed4 _BoundColor;
	fixed4 _BgColor;
	float _circleSizePercent;

	struct v2f
	{
		float2 uv : TEXCOORD0;
	};

	v2f vert(
		float4 vertex : POSITION, // vertex position input
		float2 uv : TEXCOORD0, // texture coordinate input
		out float4 outpos : SV_POSITION // clip space position output
	)
	{
		v2f o;
		o.uv = uv;
		outpos = UnityObjectToClipPos(vertex);
		return o;
	}

	float2 antialias(float radius, float borderSize, float dist)
	{
		float t = smoothstep(radius + borderSize, radius - borderSize, dist);
		return t;
	}

	fixed4 frag(v2f i, UNITY_VPOS_TYPE screenPos : VPOS) : SV_Target
	{
		float4 col;
	float2 center = _ScreenParams.xy / 2;

	float maxradius = length(center);

	float radius = maxradius * (_circleSizePercent / 100);

	float dis = distance(screenPos.xy, center);

	if (dis > radius) {
		float aliasVal = antialias(radius, _border, dis);
		col = lerp(_BoundColor, _BgColor, aliasVal); //NOT needed but incluse just incase
	}
	else {
		float aliasVal = antialias(radius, _border, dis);
		col = lerp(_BoundColor, _BgColor, aliasVal);
	}
	return col;

	}
		ENDCG
	}
	}
}
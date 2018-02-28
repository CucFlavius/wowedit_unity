// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Test"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
	}

		SubShader
	{
		Tags{ "Queue" = "Geometry" "RenderType" = "Opaque" "ForceNoShadowCasting" = "True" }
		LOD 200

		Pass
	{
		Name "FORWARD"
		Tags{ "LightMode" = "ForwardBase" }

		CGPROGRAM

#pragma target 2.0

#pragma multi_compile_fwdbase

#pragma multi_compile KEYWORD_OFF KEYWORD_ON

#pragma vertex vert
#pragma fragment frag

		half4 _Color;

	struct vin
	{
		float4 vertex : POSITION;
	};

	struct vout
	{
		float4 pos : SV_POSITION;
	};

	vout vert(vin v)
	{
		vout o;
		UNITY_INITIALIZE_OUTPUT(vout, o);

		o.pos = UnityObjectToClipPos(v.vertex);

		return o;
	};

	half4 frag(vout i) : SV_Target
	{
#ifdef KEYWORD_ON
		return half4(0,0,0,1);
#else
		return _Color;
#endif
	};

	ENDCG
	}
	}
		FallBack "Unlit/Color"
}

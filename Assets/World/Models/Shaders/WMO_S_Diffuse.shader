// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

///////////////////////////////////////////////////////
///////////		   WMO Surface Shader		///////////
///////////				Diffuse				///////////
///////////////////////////////////////////////////////

Shader "WoWEdit/WMO/S_Diffuse" 
{
	Properties 
	{
		_MainTex ("_MainTex", 2D) = "white" {}

		[Header(Blending Mode)]
		MySrcMode("SrcMode", Float) = 0
		MyDstMode("DstMode", Float) = 0
	}
	SubShader 
	{
		Tags 
		{ 
			"Queue" = "AlphaTest" 
			"IgnoreProjector" = "True"  
			"RenderType"="TransparentCutout"
		}

		LOD 200
		Cull Off
		Zwrite On
		AlphaToMask On
		CGPROGRAM

		#pragma surface surf Lambert fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input 
		{
			float2 uv_MainTex;
		};

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			//float _Cutoff = 0.5f;
			//c.a = (c.a - _Cutoff) / max(fwidth(c.a), 0.0001) + 0.5;
			o.Albedo = c.rgb;
			//o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
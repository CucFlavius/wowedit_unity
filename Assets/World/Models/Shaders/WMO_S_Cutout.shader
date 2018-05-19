// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

///////////////////////////////////////////////////////
///////////		   WMO Surface Shader		///////////
///////////				CutOut				///////////
//////////  With smoothed edges using MSAA  ///////////
///////////////////////////////////////////////////////

Shader "WoWEdit/WMO/S_CutOut" 
{
	Properties 
	{
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
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

		#pragma surface surf Lambert alphatest:_Cutoff fullforwardshadows
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
			float _Cutoff = 0.5f;
			c.a = (c.a - _Cutoff) / max(fwidth(c.a), 0.0001) + 0.5;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
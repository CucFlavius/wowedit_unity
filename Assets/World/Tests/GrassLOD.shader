// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "GrassLOD"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.897
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_AlphaTexture("AlphaTexture", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform sampler2D _AlphaTexture;
		uniform float4 _AlphaTexture_ST;
		uniform float _Cutoff = 0.897;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float4 transform34 = mul(unity_ObjectToWorld,float4( ase_vertex3Pos , 0.0 ));
			float2 uv_TexCoord35 = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
			v.vertex.xyz += ( float4( float3(0.1,0,0.2) , 0.0 ) * sin( ( transform34 + _Time.y ) ) * ( 1.0 - uv_TexCoord35.y ) ).xyz;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 tex2DNode2 = tex2D( _TextureSample0, uv_TextureSample0 );
			o.Albedo = tex2DNode2.rgb;
			o.Alpha = 1;
			float temp_output_18_0 = ( 1.0 - unity_LODFade.x );
			float ifLocalVar19 = 0;
			if( temp_output_18_0 == 1.0 )
				ifLocalVar19 = 0.0;
			else if( temp_output_18_0 < 1.0 )
				ifLocalVar19 = temp_output_18_0;
			float2 uv_AlphaTexture = i.uv_texcoord * _AlphaTexture_ST.xy + _AlphaTexture_ST.zw;
			float ifLocalVar11 = 0;
			if( ifLocalVar19 > tex2D( _AlphaTexture, uv_AlphaTexture ).a )
				ifLocalVar11 = 1.0;
			clip( ( tex2DNode2.a - ifLocalVar11 ) - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13801
1;680;1244;304;912.8058;490.3994;1.75245;True;True
Node;AmplifyShaderEditor.PosVertexDataNode;32;391.0859,-472.3177;Float;False;0;0;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LODFadeNode;16;-974.596,83.39874;Float;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;20;-776.3353,173.1375;Float;False;Constant;_Float1;Float 1;3;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;34;646.3818,-390.4206;Float;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TimeNode;31;302.2186,-214.429;Float;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;18;-824.7985,38.27811;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;12;-360.3205,421.1753;Float;False;Constant;_Float0;Float 0;4;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;35;-418.615,-145.1667;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ConditionalIfNode;19;-575.1404,-4.311015;Float;False;False;5;0;FLOAT;0.0;False;1;FLOAT;1.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;29;455.5577,-312.0084;Float;False;2;2;0;FLOAT4;0.0;False;1;FLOAT;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.SamplerNode;5;-695.1569,296.3448;Float;True;Property;_AlphaTexture;AlphaTexture;2;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;2;-387.0363,5.869342;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;36;-150.4901,-125.8898;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SinOpNode;28;553.1371,-170.8667;Float;False;1;0;FLOAT4;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.ConditionalIfNode;11;-310.3877,250.8182;Float;False;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.Vector3Node;22;-322.4514,-323.6492;Float;False;Constant;_Vector0;Vector 0;3;0;0.1,0,0.2;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SinTimeNode;21;-82.80965,-377.0014;Float;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;13;-62.19543,244.944;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;59.41714,-167.5764;Float;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0;False;2;FLOAT;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.WorldPosInputsNode;25;129.711,-378.223;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;316.382,1.095585;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;GrassLOD;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;True;Back;0;0;False;0;0;Masked;0.897;True;True;0;False;TransparentCutout;AlphaTest;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;34;0;32;0
WireConnection;18;0;16;1
WireConnection;19;0;18;0
WireConnection;19;3;20;0
WireConnection;19;4;18;0
WireConnection;29;0;34;0
WireConnection;29;1;31;2
WireConnection;36;0;35;2
WireConnection;28;0;29;0
WireConnection;11;0;19;0
WireConnection;11;1;5;4
WireConnection;11;2;12;0
WireConnection;13;0;2;4
WireConnection;13;1;11;0
WireConnection;23;0;22;0
WireConnection;23;1;28;0
WireConnection;23;2;36;0
WireConnection;0;0;2;0
WireConnection;0;10;13;0
WireConnection;0;11;23;0
ASEEND*/
//CHKSM=A058A6A4DA9F94FC0C36F608A7CCCB7D4E8F7C32
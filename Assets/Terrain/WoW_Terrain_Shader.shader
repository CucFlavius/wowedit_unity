Shader "Machinima/WoWTerrain"
{
	Properties
	{
	_MainTex ("_MainTex", 2D) = "black" {}
	_MainTexTiling("_MainTexTiling", Range(1,10)) = 1
         _BlendTex1 ("_BlendTex1", 2D) = "black" {}
		 _BlendTex1Tiling ("_BlendTex1Tiling", Range(1,10)) = 1
		 _BlendTexAmount1 ("_BlendTexAmount1", 2D) = "black" {}
         _BlendTex2 ("_BlendTex2", 2D) = "black" {}
		 _BlendTex2Tiling ("_BlendTex2Tiling", Range(1,10)) = 1
		 _BlendTexAmount2 ("_BlendTexAmount2", 2D) = "black" {}
         _BlendTex3 ("_BlendTex3", 2D) = "black" {}
		 _BlendTex3Tiling ("_BlendTex3Tiling", Range(1,10)) = 1
		 _BlendTexAmount3 ("_BlendTexAmount3", 2D) = "black" {}		 

     }
	 
	 
	 
	 
	 
     SubShader
     {	
		
         Tags {"RenderType"="Opaque" }
         LOD 200
         
        // ZWrite Off
         //Blend SrcAlpha OneMinusSrcAlpha
         //Blend One One
         //Blend SrcAlpha OneMinusSrcAlpha
 
         
         CGPROGRAM
        // #pragma surface surf Lambert addshadow fullforwardshadows
		 #pragma surface surf Standard vertex:vert addshadow 
		 #pragma target 3.0
		 
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
		
		
		   void vert (inout appdata_full v, out Input o)
         {
             UNITY_INITIALIZE_OUTPUT(Input,o);
             o.vertexColor = v.color; // Save the Vertex Color in the Input for the surf() method
         }
 
    void surf (Input IN, inout SurfaceOutputStandard o)
    {
      fixed4 mainCol = tex2D(_MainTex, IN.uv_MainTex*_MainTexTiling);
      fixed4 tex1Col = tex2D(_BlendTex1, IN.uv_MainTex*_BlendTex1Tiling);       
	  fixed4 tex1Amount = tex2D(_BlendTexAmount1, IN.uv_MainTex);  
      fixed4 tex2Col = tex2D(_BlendTex2, IN.uv_MainTex*_BlendTex2Tiling);       
	  fixed4 tex2Amount = tex2D(_BlendTexAmount2, IN.uv_MainTex); 
      fixed4 tex3Col = tex2D(_BlendTex3, IN.uv_MainTex*_BlendTex3Tiling);       
	  fixed4 tex3Amount = tex2D(_BlendTexAmount3, IN.uv_MainTex);  	 	  
      
      fixed4 mainOutput = mainCol.rgba * (1.0 - tex1Amount.a) ;
      fixed4 blendOutput1 = tex1Col.rgba * tex1Amount.a;
	  fixed4 mainOutput1 = (mainOutput + blendOutput1) * (1.0 - tex2Amount.a);
      fixed4 blendOutput2 = tex2Col.rgba * tex2Amount.a;
	  fixed4 mainOutput2 = (mainOutput1 + blendOutput2) * (1.0 - tex3Amount.a);  
	  fixed4 blendOutput3 = tex3Col.rgba * tex3Amount.a;
	  fixed4 mainOutput3 = mainOutput2 + blendOutput3;
	  
	  //fixed4 vertexColorBlend = IN.vertexColor.rgb;
	  
	  //o.Albedo = mainOutput3.rgb * IN.vertexColor.rgb *2;
	  o.Albedo = mainOutput3.rgb * IN.vertexColor.rgb ;
	  //o.Albedo = IN.vertexColor.rgb * 2;
      //o.Alpha = mainOutput.a + blendOutput1.a + blendOutput2.a + blendOutput3.a;
    }

			
	ENDCG
		 
		 
     } 
	 
	 
	 
	 
     Fallback "VertexLit"
 }
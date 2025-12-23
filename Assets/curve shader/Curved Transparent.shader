Shader "Unlit/CurvedUnlit Transparent"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		
		ZWrite On
		
		

		Pass
		{
			
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "CurvedCode.cginc"

			// Add the following line to enable transparency
			

			ENDCG

		}
	}
}

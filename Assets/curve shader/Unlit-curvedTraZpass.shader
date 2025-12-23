// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/Transparent_Curved_Zpass" {
Properties {
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
}

SubShader {
    Tags {"Queue"="Transparent" "RenderType"="Transparent"}
    LOD 100

    ZWrite Off

    Blend SrcAlpha OneMinusSrcAlpha



    Stencil {
            Ref 1
            Comp always
            Pass replace
            }

    Pass {
        

        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_fog

            
			#include "CurvedCode.cginc"


        ENDCG
    }
}

}

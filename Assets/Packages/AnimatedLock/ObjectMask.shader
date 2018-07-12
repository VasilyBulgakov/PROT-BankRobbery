// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ObjectMask" 
{
    Properties 
	{
         _Color ("Color", Color) = (1,1,1,1)
		 _Alpha ("Alpha", Float) = 0
    }

    SubShader 
	{
	    Tags { "RenderType" = "Transparent" "Queue" = "Transparent"  }
        Pass 
		{
		    Cull Back
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            
            uniform float4 _Color;
			float _Alpha;

            float4 vert(float4 v:POSITION) : SV_POSITION 
			{
                return UnityObjectToClipPos (v);
            }

            fixed4 frag() : COLOR 
			{
                return float4(_Color.xyz,_Alpha);
                //return float4(_Color.x,_Color.y,_Color.z,_Color.w + _AlphaOffset);
            }

            ENDCG
        }
    }
	FallBack "Diffuse"
}
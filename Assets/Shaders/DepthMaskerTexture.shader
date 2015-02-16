Shader "DepthMask/Masker Texture" {
    Properties {
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_Cutoff ("Base Alpha Cutoff", Float) = 0.5
	}
	SubShader {
        Tags {"Queue" = "Transparent-1" }     
        Lighting Off
        ZTest LEqual
        ZWrite On
        ColorMask 0
        Pass 
		{
			AlphaTest LEqual [_Cutoff]
			SetTexture [_MainTex]
			{
				combine texture * primary, texture
			}
		
			/*CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			uniform sampler2D _MainTex;
			
			struct vertexInput
			{
				float4 position : POSITION;
				float4 texCoord : TEXCOORD0;
			};
			
			struct vertexOutput
			{
				float4 position : POSITION;
				float4 texCoord : TEXCOORD0;
			};
			
			vertexOutput vert(vertexInput input) : POSITION
			{
				vertexOutput output;
				
				output.texCoord = input.texCoord;
				output.position = mul(UNITY_MATRIX_MVP, input.position);
				return output;
			}
			
			fixed4 frag(vertexOutput input) : COLOR
			{
				float4 texColor = tex2D(_MainTex, input.texCoord.xy);
				return fixed4(texColor.r, texColor.g, texColor.b, texColor.a);	
			}
			
			ENDCG*/
		}
    }
}

Shader "DepthMask/Maskee" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	}
    SubShader {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True"}     
        Lighting Off
        ZTest LEqual
        ZWrite On
        Blend SrcAlpha OneMinusSrcAlpha
		
        Pass 
		{		
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			uniform float4 _Color;
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
				return fixed4(_Color.r * texColor.r, _Color.g * texColor.g, _Color.b * texColor.b, _Color.a * texColor.a);	
			}
			
			ENDCG
		}
	}
	Fallback "Transparent/VertexLit"
}
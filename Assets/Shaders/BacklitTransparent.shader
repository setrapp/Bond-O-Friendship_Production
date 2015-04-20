Shader "Backlit/Backlit Transparent" {
	Properties {
		_Color ("Main Color (RGB) Alpha (A)", Color) = (1,1,1,1)
		_MainTex ("Main (RGB) Trans (A)", 2D) = "white" {}
		_BackingShadowColor ("Backing Shadow (RGB) Alpha (A)", Color) = (1,1,1,1)
		_BackingShadowTex("Backing (RGB) Trans (A)", 2D) = "white" {}
		_BackingLightColor ("Backing Shadow (RGB) Alpha (A)", Color) = (1,1,1,1)
	}
    SubShader {
   		Tags{"Queue" = "Transparent" "RenderType" = "Transparent"}
        //Lighting Off
        //ZTest LEqual
        ZWrite On
        Blend SrcAlpha OneMinusSrcAlpha
		
		// First light pass
        Pass 
		{		
			Tags{"LightMode" = "ForwardBase"}
			
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			uniform float4 _Color;
			uniform float4 _BackingShadowColor;
			uniform float4 _BackingLightColor;
			uniform sampler2D _MainTex;
			uniform sampler2D _BackingShadowTex;
			
			struct vertexInput
			{
				float4 pos	: POSITION;
				float4 uv	: TEXCOORD0;
				float3 norm	: NORMAL;
			};
			
			struct vertexOutput
			{
				float4 pos 		: POSITION;
				float4 uv		: TEXCOORD0;
				float4 worldPos	: TEXCOORD1;
				float3 norm		: NORMAL;
			};
			
			vertexOutput vert(vertexInput input) : POSITION
			{
				vertexOutput output;
				
				output.uv = input.uv;
				output.pos = mul(UNITY_MATRIX_MVP, input.pos);
				output.worldPos = mul(_Object2World, input.pos);
								
				output.norm = normalize(mul(float4(input.norm, 0.0), _World2Object).xyz);
				
				return output;
			}
			
			float4 frag(vertexOutput input) : COLOR
			{
				float4 lightPos = _WorldSpaceLightPos0;
				
				float attenuation = 1.0;
				float3 lightDir = normalize(lightPos.xyz);
				
				// TODO make this actually work with point lights.
				
				if (lightPos.w != 0)
				{
					float3 toLight = lightPos.xyz - input.worldPos.xyz;
					float dist = length(toLight);
					lightDir = toLight / dist;
					attenuation = 1.0 / dist;
				}
				
				float normDotLight = dot(input.norm, lightDir.xyz);
			
				// Sample the base texture.
				float4 texColor = tex2D(_MainTex, input.uv.xy);
				
				// Sample the shadow texture and calculate its contribution based on the transparency of the main color.
				float4 shadowColor = tex2D(_BackingShadowTex, input.uv.xy);
				float shadowFactor = 0;//shadowColor.a * _BackingShadowColor.a * 0;//(1 - _Color.a);
				shadowColor = float4(shadowColor.r * _BackingShadowColor.r * shadowFactor, shadowColor.g * _BackingShadowColor.g * shadowFactor, shadowColor.b * _BackingShadowColor.b * shadowFactor, shadowColor.a * texColor.a * _BackingShadowColor.a);
				
				// Fake extra lighting to make the object brighten as the main color fades, as if a light was hitting from behind.
				float backLightFactor = ((1 - (shadowColor.a)) - texColor.a) * (_BackingLightColor.a / 0.5);
				backLightFactor *= backLightFactor > 0;
				float3 backLightColor = float3(_BackingLightColor.r * backLightFactor, _BackingLightColor.g * backLightFactor, _BackingLightColor.b * backLightFactor);
				
				// Compute the base color.
				float4 diffuseColor = float4(normDotLight * _Color.r, normDotLight * _Color.g, normDotLight * _Color.b, _Color.a);
				
				// Create the final color by subtracting away the shadows and adding the back lighting.
				float4 finalColor = float4(diffuseColor.r * (texColor.r - shadowColor.r + backLightColor.r), diffuseColor.g * (texColor.g - shadowColor.g + backLightColor.g), diffuseColor.b * (texColor.b - shadowColor.b + backLightColor.b), diffuseColor.a * (texColor.a + shadowColor.a));
				
				return finalColor;
			}
			
			ENDCG
		}
		
		// All later light pass
        Pass 
		{		
			Tags{"LightMode" = "ForwardAdd"}
			Blend SrcAlpha SrcAlpha
			
			CGPROGRAM
			
			// TODO make multiple lights work.
			
			ENDCG
		}
	}
	Fallback "Transparent/Diffuse"
}

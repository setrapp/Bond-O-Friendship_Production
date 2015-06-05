Shader "DepthMask/MaskerAlphaGradient" {

	Properties {

		//Colour of the Plane
		_Color ("Color", Color) = (0.0,0.0,0.0,1.0)

		//2 Players' positions
		[HideInInspector]_P1Pos ("Player1 Pos", Vector) = (0.0, 0.0, 0.0, 0.0)
		[HideInInspector]_P2Pos ("Player2 Pos", Vector) = (0.0, 0.0, 0.0, 0.0)
		//_Time("Time", Float) = 1.0f

		// 2 closest luminus calculated via script
		[HideInInspector]_L1Pos ("Luminus1 Pos", Vector) = (0.0, 0.0, 0.0, 0.0)
		[HideInInspector]_L2Pos ("Luminus2 Pos", Vector) = (0.0, 0.0, 0.0, 0.0)

		//Tweakable variables for transparency created around players
		_Constant("Ambience", Float) = -1.0
		_Quadratic("Inner Only", Float) = -10.0
		_Linear("Inner and Outer", Float) = 10.0

		//Pulsing effect
		_PulseRate("Pulse Rate", Float) = 3.0
		_PulseWidth("Pulse Width", Float) = 5.0
		

	}	
	SubShader {
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
		
		Blend SrcAlpha OneMinusSrcAlpha		

		Pass {	
			

			CGPROGRAM

			//pragmas
			#pragma vertex vert
			#pragma fragment frag

			//user declared variables
			uniform float4 _Color;
			uniform float4 _P1Pos; 
			uniform float4 _P2Pos;
			uniform float _Constant;
			uniform float _Linear;
			uniform float _Quadratic;
			uniform float _PulseRate;
			uniform float _PulseWidth;
			
			//uniform float _Time;


			//base input structs
			struct vertexInput{
				float4 vertex: POSITION;
				float4 normal: NORMAL;
			};

			struct vertexOutput{
				float4 pos: SV_POSITION;
				float4 normal: NORMAL;
				float4 worldPos: COLOR0;
			};

			//vertex function
			vertexOutput vert(vertexInput vi){
				vertexOutput vo;
			
				vo.normal = normalize(mul(vi.normal, _World2Object));
				vo.pos = mul(UNITY_MATRIX_MVP, vi.vertex);
				vo.worldPos = mul(_Object2World, vi.vertex);
				return vo;
			}


			//fragment function
			float4 frag(vertexOutput vo) : COLOR
			{
				float alpha = 1;

				float4 color;

				//float4 toP1 = normalize(_P1Pos - vo.worldPos);
				//float4 toP2 = normalize(_P2Pos - vo.worldPos);
				float4 toP1 = _P1Pos - vo.worldPos;
				float4 toP2 = _P2Pos - vo.worldPos;

				//float normDotTo1 = dot(vo.normal, toP1);
				//float normDotTo2 = dot(vo.normal, toP2);
				
				float toP1Length = length(toP1.xyz);
				float toP2Length = length(toP2.xyz);

				//more organic, less light-like
				//float normDotTo;
				
				//Additive mean
				//normDotTo = (normDotTo1 + normDotTo2) / 2;

				//Multiplicative (much lower when far away)
				//normDotTo = pow(normDotTo1 * normDotTo2, 0.5);
				
				//alpha = CalcAlpha(normDotTo,_Exponent,_Exp_Coeff,_Ambience);
				//alpha = 1 - ((pow(normDotTo, _Exponent ) * _Exp_Coeff) + normDotTo*_Ambience);

				alpha = 1 - ( _Constant + (_Linear/toP1Length + _Quadratic/(toP1Length*toP1Length)) + (_Linear/toP2Length + _Quadratic/(toP2Length*toP2Length)) ) + sin(_Time.y*_PulseRate)*_PulseWidth/100; 

				color = float4(_Color.rgb,alpha);
				return color;
			}

			
			ENDCG
		}
	}

	//fallback
	Fallback "Diffuse"

}
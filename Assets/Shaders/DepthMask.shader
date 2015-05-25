Shader "DepthMask/MaskerAlphaGradient" {

	Properties {

		//Colour of the Plane
		_Color ("Color", Color) = (0.0,0.0,0.0,1.0)

		//2 Players' positions
		_P1Pos ("Player1 Pos", Vector) = (0.0, 0.0, 0.0, 0.0)
		_P2Pos ("Player2 Pos", Vector) = (0.0, 0.0, 0.0, 0.0)
		// 2 closest luminus calculated via script
		_L1Pos ("Luminus1 Pos", Vector) = (0.0, 0.0, 0.0, 0.0)
		_L2Pos ("Luminus2 Pos", Vector) = (0.0, 0.0, 0.0, 0.0)

		//Visibility around Players
		_Exponent("Sharpness Radius", Float) = 4.0
		_Exp_Coeff("Sharpness Multiplier", Float) = 30.0
		_Ambience("Ambience", Float) = 0.8
		_RingCoeff("Outer Radius", Float) = 5


		//Visibility around Luminus

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
			uniform float _Exponent;
			uniform float _Exp_Coeff;
			uniform float _Ambience;
			uniform float _RingCoeff;


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

			float CalcAlpha(float normDotTo, float _Exponent, float _Exp_Coeff, float _Ambience)
			{
				return 1 - ((pow(normDotTo, _Exponent ) * _Exp_Coeff) + normDotTo*_Ambience); 
			}

			//fragment function
			float4 frag(vertexOutput vo) : COLOR
			{
				float alpha = 1;

				float4 color;

				float4 toP1 = normalize(_P1Pos - vo.worldPos);
				float4 toP2 = normalize(_P2Pos - vo.worldPos);

				float normDotTo1 = dot(vo.normal, toP1);
				float normDotTo2 = dot(vo.normal, toP2);
				
				//Uncomment this part for a more subtle cellular look
				//if (normDotTo1 < 0.25)
				//{
				//	normDotTo1 = 0;
				//}
				//if (normDotTo2 < 0.25)
				//{
				//	normDotTo2 = 0;
				//}
				//float alpha1 = 1 - normDotTo1;
				//float alpha2 = 1 - normDotTo2;
				//
				//alpha = 1 - (pow(normDotTo1, 2) + pow(normDotTo2, 2));
				

				//more organic, less light-like
				float normDotTo;
				
				//Additive mean
				normDotTo = (normDotTo1 + normDotTo2) / 2;

				//Multiplicative (much lower when far away)
				//normDotTo = pow(normDotTo1 * normDotTo2, 0.5);
				
				alpha = CalcAlpha(normDotTo,_Exponent,_Exp_Coeff,_Ambience);
				//alpha = 1 - ((pow(normDotTo, _Exponent ) * _Exp_Coeff) + normDotTo*_Ambience);

				alpha = min(alpha , CalcAlpha(1/_RingCoeff,_Exponent,_Exp_Coeff,_Ambience));

				//Uncomment this part for hard edges around players
				//if (normDotTo < 1/_RingCoeff)
				//{
				//	alpha = 0.9;
				//}

				color = float4(_Color.rgb,alpha);
				return color;
			}

			
			ENDCG
		}
	}

	//fallback
	Fallback "Diffuse"

}
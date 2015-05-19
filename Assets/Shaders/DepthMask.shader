Shader "DepthMask/MaskerAlphaGradient" {

	Properties {

		_Color ("Color", Color) = (0.0,0.0,0.0,1.0)
		_P1Pos ("P1 Position", Vector) = (0.0, 0.0, 0.0, 0.0)
		_P2Pos ("P2 Position", Vector) = (0.0, 0.0, 0.0, 0.0)
		//_HypSquared ("Hypotenuse Squared", Float) = 0.0

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

				float4 toP1 = normalize(_P1Pos - vo.worldPos);
				float4 toP2 = normalize(_P2Pos - vo.worldPos);

				float normDotTo1 = dot(vo.normal, toP1);
				float normDotTo2 = dot(vo.normal, toP2);


				
				//More light like behaviour
				//if (normDotTo1 < 0.05)
				//{
				//	normDotTo1 = 0;
				//}
				//if (normDotTo2 < 0.05)
				//{
				//	normDotTo2 = 0;
				//}
				//
				//float alpha1 = 1 - normDotTo1;
				//float alpha2 = 1 - normDotTo2;
				//
				//alpha = 1 - (pow(normDotTo1, 2) + pow(normDotTo2, 2));
				

				//more organic, less light-like
				float normDotTo = (dot(vo.normal, toP1) + dot(vo.normal, toP2)) / 2;
				alpha = 1 - ((pow(normDotTo, 8 ) * 50000) + normDotTo/2);
				if (normDotTo < .2)
				{
					alpha = 0.9;
				}


				color = float4(_Color.rgb,alpha);

				return color;
			}
			ENDCG
		}
	}

	//fallback
	Fallback "Diffuse"

}
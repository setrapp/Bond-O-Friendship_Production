Shader "Shader Tutorials/1b - Normal Color" {
	Properties {
		_Color ("Color", Color) = (1.0,1.0,1.0,1.0)
	}
	SubShader {
		Pass {
			CGPROGRAM
			
			//pragmas
			#pragma vertex vert
			#pragma fragment frag

			//user defined variables
			uniform float4 _Color;

			//base input structs
			struct vertexInput{
				float4 vertex: POSITION;
				float3 normal: NORMAL;
			};
			struct vertexOutput{
				float4 pos: SV_POSITION;
				float4 col: COLOR;
			};

			//vertex function
			vertexOutput vert(vertexInput vi){
				vertexOutput vo;
				vo.pos = mul(UNITY_MATRIX_MVP, vi.vertex);
				vo.col = float4(vi.normal, 1.0);
				return vo;
			}

			//fragment function
			float4 frag(vertexOutput vo) : COLOR
			{
				return vo.col;
			}

			ENDCG
		}
	}
	//fallback
	Fallback "Diffuse"
}
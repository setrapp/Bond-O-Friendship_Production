Shader "Shader Tutorials/ Aurora_Prototype" {
	Properties {
		_Color ("Top", Color) = (1.0, 1.0, 1.0, 1.0)
		_Color ("Bottom", Color) = (0.0, 0.0, 0.0, 1.0)
	}
	SubShader {
		Pass{
			Tags { "LightMode" = "ForwardBase" }
			CGPROGRAM
// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it does not contain a surface program or both vertex and fragment programs.
#pragma exclude_renderers gles
			//#pragma vertex vert
			#pragma fragment frag

			//user defined variables
			uniform float4 _Color;

			//unity defined variables
			uniform float4 _LightColor0;

			//base input structs
			struct vertexInput{
				float4 vertex: POSITION;
				float ratio;
			};
			struct vertexOutput{
				float4 col: COLOR;
			};

			vertex function
			vertexOutput vert(vertexInput vi){
			
			}

			float4 frag(vertexOutput vo): COLOR
			{
				
				return vo.col;
			}

			ENDCG
	}
	}
}
Shader "Player View Stencil/Diffuse1" {
	Properties {
		_Color ("Main Color", Color) = (1, 1, 1, 1)
	}
	SubShader {
		Tags { "RenderType" = "Opaque" "Queue"="Geometry"}
		//
		Pass {
			Stencil {
				Ref 1
				Comp equal
				Pass keep
				Fail keep
				ZFail keep
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			fixed4 _Color;
			sampler2D _CameraDepthTexture;

			struct appdata {
				float4 vertex : POSITION;
				float4 texcoord0 : TEXCOORD0;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float4 texcoord0 : TEXCOORD0;
			};
			v2f vert(appdata v) {
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord0 = v.texcoord0;
				return o;
			}
			half4 frag(v2f i) : COLOR {
				float depth = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.texcoord0.xy));
				return depth;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
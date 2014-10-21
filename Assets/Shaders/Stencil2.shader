Shader "Player View Stencil/Stencil2" {
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Geometry-1s"}
		Pass {
			Stencil {
				Ref 2
				Comp always
				Pass replace
				ZFail decrWrap
			}
			Blend Zero One
			ZWrite On
			ZTest LEqual
			ColorMask 0
		}
	} 
}

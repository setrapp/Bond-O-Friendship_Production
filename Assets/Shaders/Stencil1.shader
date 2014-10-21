Shader "Player View Stencil/Stencil1" {
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Geometry-1"}
		Pass {
			Stencil {
				Ref 1
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

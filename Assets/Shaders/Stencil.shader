Shader "Player View Stencil/Stencil" {
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Geometry"}
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

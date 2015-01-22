Shader "Stencil/WinterStencil" {
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Transparent"}
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

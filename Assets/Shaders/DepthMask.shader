Shader "DepthMask/MaskerAlphaGradient" {

	Properties {

		//Colour of the Plane
		_Color ("Color", Color) = (0.0,0.0,0.0,1.0)

		//multipliers
		_Mul("Multipliers", Vector) = (1.0, 1.0, 1.0, 1.0)

		//2 Players' positions calculated via script
		[HideInInspector]_P1Pos ("Player1 Pos", Vector) = (0.0, 0.0, 0.0, 0.0)
		[HideInInspector]_P2Pos ("Player2 Pos", Vector) = (0.0, 0.0, 0.0, 0.0)
		//_Time("Time", Float) = 1.0f

		// 2 closest lumini calculated via script
		[HideInInspector]_L1Pos ("Luminus1 Pos", Vector) = (0.0, 0.0, 0.0, 0.0)
		[HideInInspector]_L2Pos ("Luminus2 Pos", Vector) = (0.0, 0.0, 0.0, 0.0)

		//Overall Ambience
		_Constant("Overall Ambience", Float) = -1.0

		//Player tweaks
		_PlayerQuadratic("Player Inner Radius Only", Float) = -10.0
		_PlayerLinear("Player Inner and Outer Radii", Float) = 10.0

		//Luminus tweaks
		_LuminusQuadratic("Luminus Inner Radius Only", Float) = -10.0
		_LuminusLinear("Luminus Inner and Outer Radii", Float) = 10.0

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

			uniform float4 _Mul;

			uniform float4 _P1Pos; 
			uniform float4 _P2Pos;
			uniform float4 _L1Pos;
			uniform float4 _L2Pos;

			uniform float _Constant;

			uniform float _PlayerLinear;
			uniform float _PlayerQuadratic;
			uniform float _LuminusLinear;
			uniform float _LuminusQuadratic;

			uniform float _PulseRate;
			uniform float _PulseWidth;


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

				//positions
				float4 toP1 = _P1Pos - vo.worldPos;
				float4 toP2 = _P2Pos - vo.worldPos;
				float4 toL1 = _L1Pos - vo.worldPos;
				float4 toL2 = _L2Pos - vo.worldPos;
				
				//distances
				float toP1Length = length(toP1.xyz);
				float toP2Length = length(toP2.xyz);
				float toL1Length = length(toL1.xyz);
				float toL2Length = length(toL2.xyz);

				//Independent contributions
				float p1Contribution = _Mul.x * (_PlayerLinear/toP1Length + _PlayerQuadratic/(toP1Length*toP1Length));
				float p2Contribution = _Mul.y * (_PlayerLinear/toP2Length + _PlayerQuadratic/(toP2Length*toP2Length));
				float l1Contribution = _Mul.z * (_LuminusLinear/toL1Length + _LuminusQuadratic/(toL1Length*toL1Length));
				float l2Contribution = _Mul.w * (_LuminusLinear/toL2Length + _LuminusQuadratic/(toL2Length*toL2Length));

				//noise
				float noise = sin(_Time.y*_PulseRate)*_PulseWidth/100;

				//Overall alpha: 1- (ambience + contributions) + noise
				alpha = 1 - ( _Constant + p1Contribution + p2Contribution + l1Contribution + l2Contribution ) + noise; 

				color = float4(_Color.rgb, alpha * _Color.a);
				return color;
			}

			
			ENDCG
		}
	}

	//fallback
	Fallback "Diffuse"

}
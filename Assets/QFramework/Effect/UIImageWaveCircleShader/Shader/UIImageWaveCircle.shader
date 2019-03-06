Shader "UI/UIImageWaveCircle" 
{
	Properties 
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		[Toggle(FlowOutwards)] _FlowOutward("Flow Outwards", Int) = 1
		_CenterX("Wave Center X", Range(-1, 2)) = 0.5
		_CenterY("Wave Center Y", Range(-1, 2)) = 0.5
		_WaveLength("Wave Length", Range(0.0001, 5)) = 0.02
		_WaveAmplitude("Wave Amplitude", Range(0, 2)) = 0.02
		_WavePhase("Wave Phase", Range(0, 1)) = 0
		_WaveSpeed("Wave Speed", Range(0, 100)) = 1

		[HideInInspector] _StencilComp("Stencil Comparison", Float) = 8
		[HideInInspector] _Stencil("Stencil ID", Float) = 0
		[HideInInspector] _StencilOp("Stencil Operation", Float) = 0
		[HideInInspector] _StencilWriteMask("Stencil Write Mask", Float) = 255
		[HideInInspector] _StencilReadMask("Stencil Read Mask", Float) = 255

		[HideInInspector] _ColorMask("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
	}

	SubShader 
	{
		Tags 
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Stencil 
		{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask[_ColorMask]

		Pass
		{
			Name "UIImage_Dynamic_Wave_Circle"
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag_worldposition
			#pragma target 2.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"
            #include "Assets/QFramework/Framework/5.ShaderLib/CGInclude/UI.cginc"
            
			uniform int _FlowOutward;
			uniform float _CenterX;
			uniform float _CenterY;
			uniform float _WaveLength;
			uniform float _WaveAmplitude;
			uniform float _WavePhase;
			uniform float _WaveSpeed;
                     
			fixed4 ProcessColor(float2 uv,float4 inColor,float4 worldPosition) 
			{
				float offsetX = uv.x - _CenterX;
				float offsetY = uv.y - _CenterY;
                
				float dis = sqrt(abs(offsetX * offsetX + offsetY * offsetY));

				float v1;
				if (_FlowOutward != 0) {
					v1 = frac(dis / _WaveLength + _WavePhase - _Time*_WaveSpeed);
				}
				else {
					v1 = frac(dis / _WaveLength + _WavePhase + _Time*_WaveSpeed);
				}
				
				float vSin = _WaveAmplitude * sin(v1*6.2832f);
				float2 coordOffset = float2(vSin*offsetX/dis, vSin*offsetY/dis);
				fixed4 color = tex2D(_MainTex, uv + coordOffset);

				color.a *= UnityGet2DClipping(worldPosition.xy, _ClipRect);

		        #ifdef UNITY_UI_ALPHACLIP
				clip(color.a - 0.001);
		        #endif

				return color;
			}
			ENDCG
		}
	}
}

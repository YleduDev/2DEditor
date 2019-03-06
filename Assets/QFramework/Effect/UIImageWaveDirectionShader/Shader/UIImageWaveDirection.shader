Shader "UI/UIImageWave" 
{
	Properties 
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		[Toggle(HorizontalWave)] _IsHorizontalWave("Horizontal Wave", Int) = 1
		[Toggle(PositiveWave)] _IsPositiveWave("Positive Direction Wave", Int) = 1
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
		Tags {
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
			Name "UIImage_Dynamic_Wave_Direction"
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag_worldposition
			#pragma target 2.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"
            #include "Assets/QFramework/Framework/5.ShaderLib/CGInclude/UI.cginc"

			#pragma multi_compile __ UNITY_UI_ALPHACLIP

			uniform int _IsHorizontalWave;
			uniform int _IsPositiveWave;
			uniform float _WaveLength;
			uniform float _WaveAmplitude;
			uniform float _WavePhase;
			uniform float _WaveSpeed;

         
			fixed4 ProcessColor(float2 uv,float4 inColor,float4 worldPosition)
			{
				fixed4 color;

				float validTime;
				
				if (_IsPositiveWave != 0) 
				{
					validTime = _Time * _WaveSpeed;
				}
				else 
				{
					validTime = -_Time * _WaveSpeed;
				}
				
				if (_IsHorizontalWave != 0) 
				{
					float v1 = frac(uv.x / _WaveLength + _WavePhase + validTime);
					float vSin = -_WaveAmplitude * sin(v1*6.2832f);
					color = tex2D(_MainTex, float2(uv.x, uv.y + vSin));
				}
				else 
				{
					float v1 = frac(uv.y / _WaveLength + _WavePhase + validTime);
					float vSin = -_WaveAmplitude * sin(v1*6.2832f);
					color = tex2D(_MainTex, float2(uv.x + vSin, uv.y));
				}

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
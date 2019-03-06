Shader "UI/UIImageRelief"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		[Toggle(ConfigKernel)] _GrayRelief("Gray Relief", Int) = 1
		_ReliefAngle("Relief Angle", Range(0, 360)) = 0
		_ReliefHeight("Relief Height", Range(0, 0.1)) = 0.01
		_ReliefCount("Relief Count", Range(1, 500)) = 0.5
		_ReliefAlpha("Relief Alpha", Range(0, 1)) = 1

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
		    Name "UIImage_Static_Relief" 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag_worldposition
			#pragma target 2.0
			
			#include "UnityCG.cginc"
			#include "UnityUI.cginc"
		    #include "Assets/QFramework/Framework/5.ShaderLib/CGInclude/UI.cginc"

			uniform int _GrayRelief;
			uniform float _ReliefAngle;
			uniform float _ReliefHeight;
			uniform float _ReliefCount;
			uniform float _ReliefAlpha;
                     
			fixed4 ProcessColor (float2 uv,float4 inColor,float4 worldPosition) : SV_Target
			{
				float offsetX = cos(_ReliefAngle / 180.0f * 3.1415926f) * _ReliefHeight;
				float offsetY = sin(_ReliefAngle / 180.0f * 3.1415926f) * _ReliefHeight;

				float4 colMainTex = tex2D(_MainTex, uv);
				float4 colDeleteTex = tex2D(_MainTex, uv + float2(offsetX, offsetY));

				fixed4 color;
				float4 canColor = ((colDeleteTex - colMainTex) + float4(1, 1, 1, 1)) * 0.5f;

				if (_GrayRelief != 0) {
					float validGray = canColor.r * 0.299f + canColor.g * 0.875f + canColor.b * 0.114f;
					float grayScale = floor(validGray * _ReliefCount) / _ReliefCount;
					color = float4(float4(grayScale, grayScale, grayScale, _ReliefAlpha) * inColor);
				}
				else {
					color = float4(float4(canColor.rgb, _ReliefAlpha) * inColor);
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

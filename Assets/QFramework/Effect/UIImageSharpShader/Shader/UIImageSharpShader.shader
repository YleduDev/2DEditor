Shader "UI/UIImageSharp" 
{
	Properties 
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_LineWidth("line Width", Range(0, 0.25)) = 0.001
		[Toggle(ConfigKernel)] _ConfigKernel("Config Kernel", Int) = 0

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
			Name "UIImage_Static_Sharp"

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag_worldposition
			#pragma target 2.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"
            #include "Assets/QFramework/Framework/5.ShaderLib/CGInclude/UI.cginc"


			uniform float sharpKernel[9];
			static float LaplacianKernel[9] = {
				-1.0f, -1.0f, -1.0f,
				-1.0f, 8.0f, -1.0f,
				-1.0f, -1.0f, -1.0f
			};

			uniform fixed4 _Color;
			uniform float _LineWidth;
			uniform int _ConfigKernel;

			fixed4 ProcessColor(float2 uv,float4 inColor,float4 worldPosition)
			{
				float4 col11 = tex2D(_MainTex, float2(uv.x - _LineWidth, uv.y + _LineWidth));
				float4 col12 = tex2D(_MainTex, float2(uv.x, uv.y + _LineWidth));
				float4 col13 = tex2D(_MainTex, float2(uv.x + _LineWidth, uv.y + _LineWidth));
				float4 col21 = tex2D(_MainTex, float2(uv.x - _LineWidth, uv.y));
				float4 col22 = tex2D(_MainTex, uv);
				float4 col23 = tex2D(_MainTex, float2(uv.x + _LineWidth, uv.y));
				float4 col31 = tex2D(_MainTex, float2(uv.x - _LineWidth, uv.y - _LineWidth));
				float4 col32 = tex2D(_MainTex, float2(uv.x, uv.y - _LineWidth));
				float4 col33 = tex2D(_MainTex, float2(uv.x + _LineWidth, uv.y - _LineWidth));

				fixed4 color;
				
				if (_ConfigKernel != 0) 
				{
					color = (col22
						+ (sharpKernel[0] * col11 + sharpKernel[1] * col12 + sharpKernel[2] * col13 +
							sharpKernel[3] * col21 + sharpKernel[4] * col22 + sharpKernel[5] * col23 +
							sharpKernel[6] * col31 + sharpKernel[7] * col32 + sharpKernel[8] * col33)
						+ _TextureSampleAdd) * inColor;
				}
				else
				{
					color = (col22
						+ (LaplacianKernel[0] * col11 + LaplacianKernel[1] * col12 + LaplacianKernel[2] * col13 +
							LaplacianKernel[3] * col21 + LaplacianKernel[4] * col22 + LaplacianKernel[5] * col23 +
							LaplacianKernel[6] * col31 + LaplacianKernel[7] * col32 + LaplacianKernel[8] * col33)
						+ _TextureSampleAdd) * inColor;
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
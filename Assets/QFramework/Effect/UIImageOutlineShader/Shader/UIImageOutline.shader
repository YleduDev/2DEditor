// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UI/Outline"
{
	Properties
	{
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0

		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

		_OutlineColor("Outline Color", Color) = (1,1,1,1)
		_OutlineWidth("Outline Width", Float) = 2
		_Threshold("Threashold", Range(0, 1)) = 0.5
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}
		
		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag_worldposition

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"
            #include "Assets/QFramework/Framework/5.ShaderLib/CGInclude/UI.cginc"
            		
			fixed4 _Color;

			float4 _MainTex_TexelSize;

			float4 _OutlineColor;
			float _OutlineWidth;
			float _Threshold;

			fixed4 ProcessColor(float2 uv, float4 inColor,float4 worldPosition) : SV_Target
			{
				half4 color = (tex2D(_MainTex, uv) + _TextureSampleAdd) * inColor;

				color.a *= UnityGet2DClipping(worldPosition.xy, _ClipRect);
				
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				float width = _MainTex_TexelSize.z, height = _MainTex_TexelSize.w;

				if (color.a <= _Threshold)
				{
					half2 dir[8] = {{ 0,1},{1,1},{1,0},{1,-1},{0,-1},{-1,-1},{-1,0},{-1,1}};
				
					for (int i = 0; i < 8;i++)
					{
						float offset = float2(dir[i].x / width,dir[i].y / height);
						offset *= _OutlineWidth;

						half4 nearby = (tex2D(_MainTex,uv + offset) + _TextureSampleAdd) * inColor;
						if (nearby.a > _Threshold)
						{
							color = _OutlineColor;
							break;
						}
					}
				}

				return color;
			}
		ENDCG
		}
	}
}

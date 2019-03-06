Shader "UI/UIImageOutlineAlpha"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_OutlineColor("Outline Color", Color) = (1,0,0,1)
		_OutlineWidth("Outline Width", Range(0, 0.5)) = 0
		[Toggle(DecayImage)] _DecayImage("Decay Image", Int) = 1
		
		[HideInInspector] _StencilComp("Stencil Comparison", Float) = 8
		[HideInInspector] _Stencil("Stencil ID", Float) = 0
		[HideInInspector] _StencilOp("Stencil Openration", Float) = 0
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
		    Name "UIImage_Outline_Alpha"
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag_worldposition
			#pragma target 2.0
			
			#include "UnityCG.cginc"
			#include "UnityUI.cginc"
			#include "Assets/QFramework/Framework/5.ShaderLib/CGInclude/UI.cginc"
			
			uniform fixed4 _OutlineColor;
			uniform float _OutlineWidth;
			uniform int _DecayImage;
			
			fixed4 ProcessColor (float2 uv,float4 inColor,float4 worldPosition) : SV_Target
			{
				float4 col11 = tex2D(_MainTex, float2(uv.x - _OutlineWidth, uv.y + _OutlineWidth));
				float4 col12 = tex2D(_MainTex, float2(uv.x, uv.y + _OutlineWidth));
				float4 col13 = tex2D(_MainTex, float2(uv.x + _OutlineWidth, uv.y + _OutlineWidth));
				float4 col21 = tex2D(_MainTex, float2(uv.x - _OutlineWidth, uv.y));
				float4 col22 = tex2D(_MainTex, uv);
				float4 col23 = tex2D(_MainTex, float2(uv.x + _OutlineWidth, uv.y));
				float4 col31 = tex2D(_MainTex, float2(uv.x - _OutlineWidth, uv.y - _OutlineWidth));
				float4 col32 = tex2D(_MainTex, float2(uv.x, uv.y - _OutlineWidth));
				float4 col33 = tex2D(_MainTex, float2(uv.x + _OutlineWidth, uv.y - _OutlineWidth));
												
				half4 color = (col22 + _TextureSampleAdd) * inColor;

				float judgeAlpha = abs(col11.a + col12.a + col13.a + col21.a + (col22.a * -8.0f) + col23.a + col31.a + col32.a + col33.a);
				if (judgeAlpha >= 0.0001f && (_DecayImage != 0 || col22.a <= 0.0001f)) {
					color = _OutlineColor;
					float validAplhaJudge = (col11.a + col12.a + col13.a + col21.a + col22.a*2.0f + col23.a + col31.a + col32.a + col33.a)*0.1f;
					if (_DecayImage == 0) {
						validAplhaJudge *= 3.0f;
					}

					color.a = smoothstep(0, 1, validAplhaJudge);
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

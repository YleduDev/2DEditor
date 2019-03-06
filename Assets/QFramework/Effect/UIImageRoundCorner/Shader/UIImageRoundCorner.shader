// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UI/RoundCorner"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)

        _RoundedRadius("Rounded Radius", Range(0, 256)) = 64
        
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
            CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag_worldposition

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"
            #include "Assets/QFramework/Framework/5.ShaderLib/CGInclude/UI.cginc"

			#pragma multi_compile __ UNITY_UI_ALPHACLIP

            fixed4 _Color;

            float _RoundedRadius;

            float4 _MainTex_TexelSize;
            
            fixed4 ProcessColor(float2 uv,float4 inColor,float4 worldPosition) : SV_Target
            {
                half4 color = (tex2D(_MainTex, uv) + _TextureSampleAdd) * inColor;

                color.a *= UnityGet2DClipping(worldPosition.xy, _ClipRect);

				#ifdef UNITY_UI_ALPHACLIP
                clip(color.a - 0.001);
				#endif

                float width = _MainTex_TexelSize.z;
                float height = _MainTex_TexelSize.w;

                float x = uv.x * width;
                float y = uv.y * height;

                float r = _RoundedRadius;

                //左下角
                if (x < r && y < r)
                {
                    if ((x - r) * (x - r) + (y - r) * (y - r) > r * r)
                        color.a = 0;
                }

                //左上角
                if (x < r && y > (height - r))
                {
                    if ((x - r) * (x - r) + (y - (height - r)) * (y - (height - r)) > r * r)
                        color.a = 0;
                }

                //右下角
                if (x > (width - r) && y < r)
                {
                    if ((x - (width - r)) * (x - (width - r)) + (y - r) * (y - r) > r * r)
                        color.a = 0;
                }

                //右上角
                if (x > (width - r) && y > (height - r))
                {
                    if ((x - (width - r)) * (x - (width - r)) + (y - (height - r)) * (y - (height - r)) > r * r)
                        color.a = 0;
                }

                return color;
            }
            ENDCG
        }
    }
}
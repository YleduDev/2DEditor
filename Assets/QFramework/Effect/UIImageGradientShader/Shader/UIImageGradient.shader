Shader "UI/UIImageGradient"
{
    Properties 
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _StartColor("Start Color", Color) = (1,1,1,1)
        _EndColor("End Color", Color) = (1,1,1,1)
        [Toggle(VertialDirection)] _VerticalDirection("Vertical Direction", Int) = 0
        
        [HideInInspector] _StencilComp("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask("Stencil Write mask", Float) = 255
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
            Name "UIImage_Gradient"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"
            
			#include "Assets/QFramework/Framework/5.ShaderLib/CGInclude/UI.cginc"

            uniform fixed4 _StartColor;
            uniform fixed4 _EndColor;
            uniform int _VerticalDirection;
            
            fixed4 ProcessColor(float2 uv,float4 inColor)
            {
                float4 tex = tex2D(_MainTex, uv);
                
                float validFactor = uv.x;
                if (_VerticalDirection != 0)
                {
                    validFactor = uv.y;
                }
                
                float4 validBlendColor = lerp(_StartColor,_EndColor,validFactor) * inColor;
                
                fixed4 color = tex * validBlendColor;
                
                color.a *= UnityGet2DClipping(uv.xy, _ClipRect);
                
                #ifdef UNITY_UI_ALPHACLIP
                clip(color.a - 0.001);
                #endif
                
                return color;
            }
            ENDCG
        }
    }
}
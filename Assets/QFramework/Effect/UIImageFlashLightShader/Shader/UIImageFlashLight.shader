// http://blog.sina.com.cn/s/blog\_471132920101d8zf.html

Shader "UI/UIImageFlashLight"
{
    Properties 
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
     
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
            #pragma vertex vert_transform_tex
            #pragma fragment frag
            #pragma target 2.0
            
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"
            #include "Assets/QFramework/Framework/5.ShaderLib/CGInclude/UI.cginc"

            
            uniform fixed4 _StartColor;
            uniform fixed4 _EndColor;
            uniform int _VerticalDirection;
            
            //必须放在使用其的 frag函数之前，否则无法识别。
            //核心：计算函数，角度，uv,光带的x长度，间隔，开始时间，偏移，单次循环时间
            float inFlash(float angle,float2 uv,float xLength,int interval,int beginTime, float offX, float loopTime )
            {
                //亮度值
                float brightness =0;
               
                //倾斜角
                float angleInRad = 0.0174444 * angle;
               
                //当前时间
                float currentTime = _Time.y;
           
                //获取本次光照的起始时间
                int currentTimeInt = _Time.y/interval;
                currentTimeInt *=interval;
               
                //获取本次光照的流逝时间 = 当前时间 - 起始时间
                float currentTimePassed = currentTime -currentTimeInt;
                if(currentTimePassed >beginTime)
                {
                    //底部左边界和右边界
                    float xBottomLeftBound;
                    float xBottomRightBound;

                    //此点边界
                    float xPointLeftBound;
                    float xPointRightBound;
                   
                    float x0 = currentTimePassed-beginTime;
                    x0 /= loopTime;
           
                    //设置右边界
                    xBottomRightBound = x0;
                   
                    //设置左边界
                    xBottomLeftBound = x0 - xLength;
                   
                    //投影至x的长度 = y/ tan(angle)
                    float xProjL;
                    xProjL= (uv.y)/tan(angleInRad);

                    //此点的左边界 = 底部左边界 - 投影至x的长度
                    xPointLeftBound = xBottomLeftBound - xProjL;
                    //此点的右边界 = 底部右边界 - 投影至x的长度
                    xPointRightBound = xBottomRightBound - xProjL;
                   
                    //边界加上一个偏移
                    xPointLeftBound += offX;
                    xPointRightBound += offX;
                   
                    //如果该点在区域内
                    if(uv.x > xPointLeftBound && uv.x < xPointRightBound)
                    {
                        //得到发光区域的中心点
                        float midness = (xPointLeftBound + xPointRightBound)/2;
                       
                        //趋近中心点的程度，0表示位于边缘，1表示位于中心点
                        float rate= (xLength -2*abs(uv.x - midness))/ (xLength);
                        brightness = rate;
                    }
                }
                brightness= max(brightness,0);
               
                //返回颜色 = 纯白色 * 亮度
                float4 col = float4(1,1,1,1) *brightness;
                return brightness;
            }
            
            fixed4 ProcessColor(float2 uv,float4 inColor)
            {
                float4 outp;
                
                float4 tex = tex2D(_MainTex, uv);
                
                //传进i.uv等参数，得到亮度值
                float tmpBrightness;
                tmpBrightness = inFlash(75,uv,0.25,5,2,0.15,0.7);
                
                //图像区域，判定设置为 颜色的A > 0.5,输出为材质颜色+光亮值
                if(tex.w >0.5)
                    outp  = tex + float4(1,1,1,1) * tmpBrightness;
                //空白区域，判定设置为 颜色的A <=0.5,输出空白
                else
                    outp = float4(0,0,0,0);                
                
                #ifdef UNITY_UI_ALPHACLIP
                clip(outp.a - 0.001);
                #endif
                
                return outp;
            }
            ENDCG
        }
	}
}

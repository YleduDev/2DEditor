/****************************************************************************
 * Copyright (c) 2018 liangxie
 * 
 * http://liangxiegame.com
 * https://github.com/liangxiegame/QFramework
 * 
 * 	Gray = R * 0.299 + G * 0.587 + B * 0.114
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 ****************************************************************************/

// Shader 名字
Shader "UI/DefaultGray"
{
	// 属性
	Properties 
	{
		// 贴图(UIImage 的 Sprite)
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		
		// 灰度权重，提供给编辑器使用
		_GrayFactor("Gray Factor", Range(0, 1)) = 1 

		// UIImage Shader 默认额配置，不用管。(模板测试需要的值)
		[HideInInspector] _StencilComp("Stencil Comparison", Float) = 8
		[HideInInspector] _Stencil("Stencil ID", Float) = 0
		[HideInInspector] _StencilOp("Stencil Operation", Float) = 0
		[HideInInspector] _StencilWriteMask("Stencil Write Mask", Float) = 255
		[HideInInspector] _StencilReadMask("Stencil Read Mask", Float) = 255

		[HideInInspector] _ColorMask("Color Mask", Float) = 15 //这样这个组件就支持mask的使用了

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
	}

	// 子着色器
	SubShader 
	{
		// UI 默认 标签
		Tags 
		{
			// 渲染序列：透明物体(UI 需要 Alpha 通道)
			"Queue" = "Transparent" 
			// 不接受 Projector 组件的投影
			"IgnoreProjector" = "True"
			// 渲染类型：透明
			"RenderType" = "Transparent"
			// 预览类型:面片(没介绍)
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		// UI 默认 模板测试 (不用管)
		Stencil 
		{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}

		// UI 默认
		// 控制多边形的哪一面应该被剔除(即不绘制）
		Cull Off // 关闭剔除
		Lighting Off // 关闭灯光
		
		ZWrite Off // 关闭 Z 值的写入(半透明，详情看日志上的图片)
		ZTest[unity_GUIZTestMode] // 深度测试模式 
		
		// 混合，详情看日志图片
		Blend SrcAlpha OneMinusSrcAlpha 
		// 颜色遮罩，选定红绿蓝中是否不被输出
		ColorMask[_ColorMask]

		// Pass 通道
		Pass
		{
			// Pass 名字
			Name "UIImage_Static_Gray"
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			// 选择 ShaderModel 2.0 (不用管)
			#pragma target 2.0

			// 引入两个 Shader 库
			#include "UnityCG.cginc"
			#include "UnityUI.cginc"
			#include "Assets/QFramework/Framework/5.ShaderLib/CGInclude/Gray.cginc"
			#include "Assets/QFramework/Framework/5.ShaderLib/CGInclude/UI.cginc"

			// 灰度值
			uniform float _GrayFactor;			
			
			fixed4 ProcessColor(float2 uv,float4 inColor)
			{
			    // tex2D(纹理 ,uv) = 纹理的像素颜色值
			    float4 inputColor = tex2D(_MainTex, uv);
			    
				// 生成灰度颜色(不用考虑  lerp 只看 float4里的就好
				fixed4 color = gray(_GrayFactor,inputColor);
				
				// alpha 赋值
	            color.a = inputColor.a;
	            
			    return color;
			} 

			ENDCG
		}
	}
}
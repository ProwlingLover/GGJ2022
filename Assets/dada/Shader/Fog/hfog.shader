// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Fog With Depth Texture" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _FogDensity ("Fog Density", Float) = 1.0
        _FogColor ("Fog Color", Color) = (1, 1, 1, 1)
        _FogStart ("Fog Start", Float) = 0.0
        _FogEnd ("Fog End", Float) = 1.0
    }
    SubShader {
        CGINCLUDE

        #include "UnityCG.cginc"

        // 脚本传递过来这个参数
        // (0, bottomLeft);
        // (1, bottomRight);
        // (2, topRight);
        // (3, topLeft);
        float4x4 _FrustumCornersRay;

        sampler2D _MainTex;
        half4 _MainTex_TexelSize;
        sampler2D _CameraDepthTexture;
        half _FogDensity;
        fixed4 _FogColor;
        float _FogStart;
        float _FogEnd;

        struct v2f {
            float4 pos : SV_POSITION;
            half2 uv : TEXCOORD0;
            half2 uv_depth : TEXCOORD1;
            // 保存了插值后的像素向量
            float4 interpolatedRay : TEXCOORD2;
        };

        v2f vert(appdata_img v) {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);

            o.uv = v.texcoord;
            o.uv_depth = v.texcoord;

            // unity左下角0,0 右上角1,1
            //判断是不是directx平台，若是开启了抗锯齿，若是此时使用了渲染到纹理技术
            //那么此时纹理不会翻转，在顶点着色器中加入这样的处理
            #if UNITY_UV_STARTS_AT_TOP
            if (_MainTex_TexelSize.y < 0)
            o.uv_depth.y = 1 - o.uv_depth.y;
            #endif


            int index = 0;
            if (v.texcoord.x < 0.5 && v.texcoord.y < 0.5) {
                index = 0;
            } else if (v.texcoord.x > 0.5 && v.texcoord.y < 0.5) {
                index = 1;
            } else if (v.texcoord.x > 0.5 && v.texcoord.y > 0.5) {
                index = 2;
            } else {
                index = 3;
            }

            #if UNITY_UV_STARTS_AT_TOP
            if (_MainTex_TexelSize.y < 0)
            index = 3 - index;
            #endif

            // 插值后的像素向量
            o.interpolatedRay = _FrustumCornersRay[index];

            return o;
        }


        fixed4 frag(v2f i) : SV_Target {
            // LinearEyeDepth：视角空间下的线性深度值，
            // SAMPLE_DEPTH_TEXTURE：深度纹理采样
            float linearDepth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv_depth));

            // 1.摄像机的世界空间中的位置
            // 2.空间世界下该像素相对于摄像机的偏移量，把他们相加，获得的就是该像素的世界坐标
            float3 worldPos = _WorldSpaceCameraPos + linearDepth * i.interpolatedRay.xyz;

            // 雾的浓度
            float fogDensity = (_FogEnd - worldPos.y) / (_FogEnd - _FogStart); 
            // 限制到0~1之间
            fogDensity = saturate(fogDensity * _FogDensity);

            fixed4 finalColor = tex2D(_MainTex, i.uv);
            // 雾的浓度=0的时候，显示原颜色
            finalColor.rgb = lerp(finalColor.rgb, _FogColor.rgb, fogDensity);

            return finalColor;
        }

        ENDCG

        Pass {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM  

            #pragma vertex vert 
            #pragma fragment frag 

            ENDCG  
        }
    } 
    FallBack Off
}
// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Roystan/Toon/Ghost"
{
    Properties
    {
        [HDR]
		_Color("Color", Color) = (1,1,1,1)
        _rimstep("边缘宽度", Float) = 1
        _rimddx("边缘抖动", Float) = 1
        _Dspeed("抖动速度", Float) = 10
        _noiseScale("_noiseScale", Float) = 1
        _PosScale("位移频率", Float) = 1
        _MainTex ("Noise", 2D) = "white" {}
    }
    SubShader
    {
	    Tags {  }
        // Cull Off
	
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"

        Pass
        {
            ZWrite On
		    Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			#include "Lighting.cginc"

            
            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 viewdir : TEXCOORD1;
				float3 worldNormal : NORMAL;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _noiseScale;
			float4 _Color;
			float _rimstep;
			float _rimddx;
			float _Dspeed;
			float _PosScale;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = normalize(mul((float3x3)UNITY_MATRIX_M, v.normal));
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;  
                o.viewdir = _WorldSpaceCameraPos.xyz - worldPos; 
                float noise = tex2Dlod(_MainTex,float4(v.uv.x * _noiseScale + _Time.x *_Dspeed , v.uv.y * _noiseScale , 0.0,0.0)).r;
                // worldPos.y += noise;
                o.vertex.xyz += noise * _PosScale * o.worldNormal;
                return o;
            }


            float4 frag (v2f i) : SV_Target
            {

				float NOL = dot(i.worldNormal, _WorldSpaceLightPos0);
                float LNdotL  = NOL * 0.5 + 0.5;
                float NOV  = max(0,dot(i.worldNormal,normalize (i.viewdir)));

                float Rim = 1-NOV;
                float LRim = (NOV) * 0.5+0.5 ;
                float noise = tex2D(_MainTex,float2(i.uv.x * _noiseScale + _Time.x *_Dspeed , i.uv.y * _noiseScale)).r;
                float width1 = step( LRim  , _rimstep );
                float width2 = step(LRim  , _rimstep + 1 * 0.05 * _rimddx  * noise  );
                // float width2 = step( LRim  , _rimstep + cos(_Time.y * 1 ) * 0.05 * _rimddx  * noise );


                float4 col1 = lerp(0, 1 ,width1);           
                float4 col2 = lerp(0, 1 ,width2);           
                return float4((clamp(col1 + col2 ,0 ,1)) * _Color.xyz, 1.0) ;

            }
            ENDCG
        }
    }
}

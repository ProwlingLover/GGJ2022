Shader "Roystan/Toon/Lit"
{
    Properties
    {
		_Color("Color", Color) = (1,1,1,1)
		shadowColor("阴影颜色", Color) = (0,0,0.0,1)
        _MainTex ("MainTex", 2D) = "white" {}
        Ramp ("Ramp", 2D) = "white" {}
    }
    SubShader
    {
        Tags 
		{ 
			"RenderType" = "Opaque"
			"LightMode" = "ForwardBase"
		}

		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"

        Pass
        {
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
				float3 worldNormal : NORMAL;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D Ramp;
            float4 Ramp_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = normalize(mul((float3x3)UNITY_MATRIX_M, v.normal));
                return o;
            }

			float4 _Color;
			float4 shadowColor;

            float4 frag (v2f i) : SV_Target
            {

				float NdotL = dot(i.worldNormal, _WorldSpaceLightPos0);
                float LNdotL  = NdotL * 0.5 + 0.5;
                float Rampmap = tex2D(Ramp , LNdotL);
                
				float4 light =  lerp(shadowColor , _LightColor0 , Rampmap);

                float4 col = tex2D(_MainTex, i.uv);
                return (col * _Color) * (light + unity_AmbientSky);
            }
            ENDCG
        }
    }
}

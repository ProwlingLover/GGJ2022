 
 
Shader "scenes/FogSceneObj"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DepthStartFog("StartFog",float) = 0
		_DepthEndFog("EndFog",float) = 0.2
		_FogColor("FogColor",Color) = (1,1,1,1)
 
		_LineStartFog("LineStartFog",float) = 0
		_LineEndFog  ("LineEndFog",float) = 50
		
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
 
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 uvLM:TEXCOORD1;
			};
 
			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				#ifdef LIGHTMAP_ON
				half2 uvLM:TEXCOORD1;
				#endif
				float3 worldPos:TEXCOORD2;
				fixed LineDistance : TEXCOORD3;
			};
 
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _DepthStartFog;
			fixed _DepthEndFog;
			fixed4 _FogColor;
			fixed _LineStartFog;
			fixed _LineEndFog;
 
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
 
				#ifdef LIGHTMAP_ON
				o.uvLM = v.uvLM.xy*unity_LightmapST.xy + unity_LightmapST.zw;
				#endif
 
				o.LineDistance = distance(o.worldPos.xyz, _WorldSpaceCameraPos.xyz);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
 
				fixed4 col = tex2D(_MainTex, i.uv);
				#ifdef LIGHTMAP_ON
				fixed3 lm = (DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uvLM)));
				col.rgb *= lm;
				#endif
				fixed Depthfog =saturate( (i.worldPos.y - _DepthStartFog) / (_DepthEndFog - _DepthStartFog));
				col.rgb = lerp(_FogColor.rgb, col.rgb, Depthfog);
 
				fixed Linefog = saturate((i.LineDistance - _LineStartFog)/ (_LineEndFog - _LineStartFog));
				col.rgb = lerp( col.rgb, _FogColor.rgb, Linefog);
				return col;
			}
			ENDCG
		}
	}
}
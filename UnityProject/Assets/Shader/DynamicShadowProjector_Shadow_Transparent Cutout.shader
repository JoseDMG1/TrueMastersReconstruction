// Approximate capture mask; native shader source was not recovered. See Knowledge/dynamic-shadow-core.md.
Shader "DynamicShadowProjector/Shadow/Transparent Cutout" {
 Properties { [HideInInspector] _Cull ("Cull",Float)=2 _Color ("Shadow",Color)=(0,0,0,1) _MainTex ("Alpha",2D)="white" {} _Cutoff ("Cutoff",Range(0,1))=.5 }
 SubShader { Tags { "RenderType"="TransparentCutout" } Pass { Cull [_Cull] ZWrite Off Blend DstColor Zero
 HLSLPROGRAM
 #pragma vertex vert
 #pragma fragment frag
 #include "UnityCG.cginc"
 sampler2D _MainTex;float4 _MainTex_ST;float4 _Color;float _Cutoff;
 struct appdata {float4 vertex:POSITION;float2 uv:TEXCOORD0;};
 struct v2f {float4 pos:SV_POSITION;float2 uv:TEXCOORD0;};
 v2f vert(appdata v) {v2f o;o.pos=UnityObjectToClipPos(v.vertex);o.uv=TRANSFORM_TEX(v.uv,_MainTex);return o;}
 float4 frag(v2f i):SV_Target {float alpha=tex2D(_MainTex,i.uv).a;clip(alpha-_Cutoff);alpha=1; return float4(lerp(float3(1,1,1),_Color.rgb,saturate(alpha*_Color.a)),1);}
 ENDHLSL
 } } Fallback Off
}

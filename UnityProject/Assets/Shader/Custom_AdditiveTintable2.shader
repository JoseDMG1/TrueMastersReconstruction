Shader "Custom/AdditiveTintable2" {
 Properties {
  _MainTex ("Color (RGB) Alpha (A)", 2D) = "white" {}
  _TintColor ("Tint Color (RGB)", Color) = (1,1,1,1)
  _InvFade ("Soft Particles Factor", Range(0.01,3)) = 1
 }
 // Reconstructed Built-In additive particle pass; original GPU program unavailable.
 SubShader {
  Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
  Blend SrcAlpha One
  Cull Off
  Lighting Off
  ZWrite Off
  ColorMask RGB
  Pass {
   CGPROGRAM
   #pragma vertex vert
   #pragma fragment frag
   #pragma multi_compile_particles
   #pragma multi_compile_fog
   #include "UnityCG.cginc"
   sampler2D _MainTex;
   float4 _MainTex_ST;
   fixed4 _TintColor;
   float _InvFade;
   struct appdata { float4 vertex:POSITION; fixed4 color:COLOR; float2 uv:TEXCOORD0; };
   struct v2f {
    float4 vertex:SV_POSITION;
    fixed4 color:COLOR;
    float2 uv:TEXCOORD0;
    UNITY_FOG_COORDS(1)
    #ifdef SOFTPARTICLES_ON
    float4 projected:TEXCOORD2;
    #endif
   };
   #ifdef SOFTPARTICLES_ON
   UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
   #endif
   v2f vert(appdata v) {
    v2f o;
    o.vertex=UnityObjectToClipPos(v.vertex);
    o.color=v.color*_TintColor;
    o.uv=TRANSFORM_TEX(v.uv,_MainTex);
    UNITY_TRANSFER_FOG(o,o.vertex);
    #ifdef SOFTPARTICLES_ON
    o.projected=ComputeScreenPos(o.vertex);
    COMPUTE_EYEDEPTH(o.projected.z);
    #endif
    return o;
   }
   fixed4 frag(v2f i):SV_Target {
    #ifdef SOFTPARTICLES_ON
    float sceneDepth=LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture,UNITY_PROJ_COORD(i.projected)));
    i.color.a*=saturate(_InvFade*(sceneDepth-i.projected.z));
    #endif
    fixed4 c=tex2D(_MainTex,i.uv)*i.color;
    UNITY_APPLY_FOG_COLOR(i.fogCoord,c,fixed4(0,0,0,0));
    return c;
   }
   ENDCG
  }
 }
 Fallback Off
}

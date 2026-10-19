// Candidate reconstruction, not recovered Nyahoon Asset Store source.
// Projection and multiply setup adapted from Nyahoon ProjectorForLWRP (MIT).
// Copyright (c) 2019 Nyahoon Games Pte. Ltd. See LICENSE in this directory.
// Linear depth falloff and explicit volume clipping are reconstruction choices.
#ifndef RECONSTRUCTED_PROJECTOR_CANDIDATE_INCLUDED
#define RECONSTRUCTED_PROJECTOR_CANDIDATE_INCLUDED
#include "UnityCG.cginc"
float4x4 unity_Projector;
float4x4 unity_ProjectorClip;
sampler2D _ShadowTex;
float _Alpha;
float _Ambient;
float _ClipScale;

struct CandidateVertex { float4 vertex : POSITION; };
struct CandidateInterpolators
{
    float4 position : SV_POSITION;
    float4 projected : TEXCOORD0;
    float4 depthProjection : TEXCOORD1;
    UNITY_FOG_COORDS(2)
};
CandidateInterpolators CandidateProjectorVertex(CandidateVertex input)
{
    CandidateInterpolators output;
    output.position = UnityObjectToClipPos(input.vertex);
    output.projected = mul(unity_Projector, input.vertex);
    output.depthProjection = mul(unity_ProjectorClip, input.vertex);
    UNITY_TRANSFER_FOG(output, output.position);
    return output;
}
float4 CandidateProjectorFragment(CandidateInterpolators input) : SV_Target
{
    // Projectors select whole intersecting receivers; constrain actual fragments.
    clip(input.projected.w - 0.000001);
    float2 uv = input.projected.xy / input.projected.w;
    clip(float4(uv, 1.0 - uv));
    clip(input.depthProjection.w - 0.000001);
    float depth = input.depthProjection.x / input.depthProjection.w;
    clip(depth);
    clip(1.0 - depth);
    float3 cookie = saturate(tex2Dproj(_ShadowTex, UNITY_PROJ_COORD(input.projected)).rgb);
    float nearFade = saturate(depth * max(_ClipScale, 0.0));
    float strength = saturate(_Alpha) * nearFade * saturate(1.0 - depth);
    float3 attenuation = lerp(float3(1.0, 1.0, 1.0), max(cookie, saturate(_Ambient)), strength);
    float4 output = float4(attenuation, 1.0);
    UNITY_APPLY_FOG_COLOR(input.fogCoord, output, float4(1.0, 1.0, 1.0, 1.0));
    return output;
}
#endif

// Candidate reconstruction. See README.md and LICENSE. Not yet imported/tested.
Shader "DynamicShadowProjector/Projector/Dynamic/Shadow With Linear Falloff"
{
    Properties
    {
        [HideInInspector] _ShadowTex ("Runtime Shadow Texture", 2D) = "white" {}
        _ClipScale ("Near Clip Sharpness", Float) = 100
        _Alpha ("Shadow Darkness", Range(0, 1)) = 1
        _Ambient ("Ambient", Range(0.01, 1)) = .3
        _Offset ("Offset", Range(-10, 0)) = -1
        _OffsetSlope ("Offset Slope Factor", Range(-1, 0)) = -1
        [HideInInspector] _DSPMainTex ("Receiver Texture", 2D) = "white" {}
        [HideInInspector] _DSPBaseAlpha ("Receiver Alpha", Vector) = (1,0,0,1)
        [HideInInspector] _DSPCull ("__cull", Float) = 2
    }
    SubShader
    {
        Tags { "Queue"="Transparent-1" "RenderType"="Transparent" }
        Pass
        {
            ZWrite Off
            ZTest LEqual
            ColorMask RGB
            Blend DstColor Zero
            Offset [_OffsetSlope], [_Offset]
            Cull [_DSPCull]
            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex CandidateProjectorVertex
            #pragma fragment CandidateProjectorFragment
            #pragma multi_compile_fog
            #include "ProjectorCandidate.cginc"
            ENDHLSL
        }
    }
    Fallback Off
}

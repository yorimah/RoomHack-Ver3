Shader "Custom/URP_SpriteSimple"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
    }
        SubShader
        {
            Tags { "RenderPipeline" = "UniversalRenderPipeline" "Queue" = "Transparent" }
            Pass
            {
                Blend SrcAlpha OneMinusSrcAlpha
                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

                TEXTURE2D(_MainTex);
                SAMPLER(sampler_MainTex);
                float4 _MainTex_ST;
                float4 _Color;

                struct Attributes
                {
                    float4 positionOS : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct Varyings
                {
                    float4 positionHCS : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                Varyings vert(Attributes input)
                {
                    Varyings o;
                    o.positionHCS = TransformObjectToHClip(input.positionOS);
                    o.uv = TRANSFORM_TEX(input.uv, _MainTex);
                    return o;
                }

                half4 frag(Varyings input) : SV_Target
                {
                    half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                    return tex * _Color;
                }
                ENDHLSL
            }
        }
}

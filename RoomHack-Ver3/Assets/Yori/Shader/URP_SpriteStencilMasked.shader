Shader "Custom/URP_SpriteStencilMasked"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1, 1, 1, 1)
    }

        SubShader
        {
            Tags
            {
                "RenderPipeline" = "UniversalRenderPipeline"
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderType" = "Transparent"
            }

            Pass
            {
                Name "SpriteStencilPass"
                Blend SrcAlpha OneMinusSrcAlpha
                ZWrite Off
                Cull Off

                Stencil
                {
                    Ref 1
                    Comp NotEqual
                    Pass Keep
                }

                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 2.0

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
                    Varyings output;
                    output.positionHCS = TransformObjectToHClip(input.positionOS);
                    output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                    return output;
                }

                half4 frag(Varyings input) : SV_Target
                {
                    half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                    return texColor * _Color;
                }
                ENDHLSL
            }
        }
}

Shader "Custom/SpriteBlackWhenStencil"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
    }

        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            LOD 100

            Pass
            {
                Name "SpriteBlackStencil"

                Blend SrcAlpha OneMinusSrcAlpha
                ZWrite Off
                Cull Off

                Stencil
                {
                    Ref 1
                    Comp Equal
                    Pass Keep
                }

                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float4 position : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _Color;

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.position = TransformObjectToHClip(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                half4 frag(v2f i) : SV_Target
                {
                    half4 texColor = tex2D(_MainTex, i.uv) * _Color;

                    // ステンシル値が1（＝視界内）なら黒くする
                    return half4(0, 0, 0, texColor.a);
                }
                ENDHLSL
            }

            // 2パス目：ステンシル≠1（視界外）では通常の色で描く
            Pass
            {
                Name "SpriteNormal"

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
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float4 position : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _Color;

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.position = TransformObjectToHClip(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                half4 frag(v2f i) : SV_Target
                {
                    return tex2D(_MainTex, i.uv) * _Color;
                }
                ENDHLSL
            }
        }
}

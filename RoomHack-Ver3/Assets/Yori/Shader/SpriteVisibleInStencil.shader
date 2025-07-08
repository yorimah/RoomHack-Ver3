Shader "Custom/SpriteVisibleInStencil"
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

            // パス①：ステンシル=1（視界内）のとき、元の色で描画
            Pass
            {
                Name "VisibleInStencil"

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
                    // 通常の見た目で描画
                    return tex2D(_MainTex, i.uv) * _Color;
                }
                ENDHLSL
            }

            // パス②：ステンシル≠1（視界外）のとき、黒で描画
            Pass
            {
                Name "HiddenOutsideStencil"

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
                #pragma fragment fragBlack
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

                half4 fragBlack(v2f i) : SV_Target
                {
                    // アルファは元画像のまま、色だけ黒にする
                    float alpha = tex2D(_MainTex, i.uv).a * _Color.a;
                    return float4(0, 0, 0, alpha);
                }
                ENDHLSL
            }
        }
}

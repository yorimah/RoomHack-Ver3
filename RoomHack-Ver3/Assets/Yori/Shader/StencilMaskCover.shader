Shader "Custom/StencilMaskCover"
{
    Properties
    {
        _NoiseTex("Noise Texture", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "Queue" = "Overlay" "RenderType" = "Opaque" }

        Stencil
        {
            Ref 1
            Comp NotEqual
            Pass Keep
        }

        Pass
        {
            ZTest Always
            ZWrite Off
            Cull Off
            ColorMask RGBA

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _NoiseTex;
            float4 _NoiseTex_ST; // テクスチャのTiling, Offsetを使うとき用

            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            float _ScrollSpeed = 0.5; // スクロール速度（自由に変えてください）

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                float2 uv = o.pos.xy / o.pos.w;
                uv = uv * 0.5 + 0.5;

                // 時間でX方向にスクロール
                uv.x += _Time.y * _ScrollSpeed;

                o.uv = TRANSFORM_TEX(uv, _NoiseTex);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_NoiseTex, i.uv);
                return col;
            }
            ENDHLSL
        }
    }
}

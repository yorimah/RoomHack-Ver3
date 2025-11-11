Shader "Custom/AlwaysVisibleText_URP"
{
    Properties
    {
        _MainTex("Font Texture", 2D) = "white" {}
    }

        SubShader
    {
        Tags
        {
            "Queue" = "Overlay"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            Name "AlwaysVisibleText"
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            ZTest Always

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color; // Textの色
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                half4 texCol = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                if (texCol.a <= 0.001 || i.color.a <= 0.001)
                    discard;

                half4 col;
                col.rgb = i.color.rgb;       // Textの色を反映
                col.a = texCol.a * i.color.a; // アルファはテクスチャ×頂点色

                return col;
            }
            ENDHLSL
        }
    }
}

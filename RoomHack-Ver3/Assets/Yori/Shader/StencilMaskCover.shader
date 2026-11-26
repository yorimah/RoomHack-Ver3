Shader "Custom/StencilMaskCover"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _MaskColor("Mask Color (RGB)", Color) = (0,0,0,1)
        _MaskAlpha("Mask Alpha", Range(0,1)) = 0.6
    }

        SubShader
        {
            Tags
            {
                "Queue" = "Overlay"
                "RenderType" = "Transparent"
                "RenderPipeline" = "UniversalPipeline"
            }

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
                Blend SrcAlpha OneMinusSrcAlpha

                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                sampler2D _MainTex; // ← 定義だけ
                fixed4 _MaskColor;
                float _MaskAlpha;

                struct appdata { float4 vertex : POSITION; };
                struct v2f { float4 pos : SV_POSITION; };

                v2f vert(appdata v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 col = _MaskColor;
                    col.a = _MaskAlpha;
                    return col;
                }
                ENDHLSL
            }
        }
}
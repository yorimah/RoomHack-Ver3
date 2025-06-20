Shader "Custom/StencilMaskCover"
{
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

            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return fixed4(0, 0, 0, 1); // 黒で塗りつぶす
            }
            ENDHLSL
        }
    }
}
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

            // �p�X�@�F�X�e���V��=1�i���E���j�̂Ƃ��A���̐F�ŕ`��
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
                    // �ʏ�̌����ڂŕ`��
                    return tex2D(_MainTex, i.uv) * _Color;
                }
                ENDHLSL
            }

            // �p�X�A�F�X�e���V����1�i���E�O�j�̂Ƃ��A���ŕ`��
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
                    // �A���t�@�͌��摜�̂܂܁A�F�������ɂ���
                    float alpha = tex2D(_MainTex, i.uv).a * _Color.a;
                    return float4(0, 0, 0, alpha);
                }
                ENDHLSL
            }
        }
}

Shader "Custom/WriteStencil"
{
    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Geometry+10" }

        Pass
        {
            Stencil
            {
                Ref 1
                Comp always
                Pass replace
            }

            ColorMask 0 // 色は描かない（ステンシルだけ書く）
        }
    }
}

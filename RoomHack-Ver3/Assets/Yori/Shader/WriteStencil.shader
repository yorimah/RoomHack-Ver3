Shader "Custom/WriteStencil"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    }

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

            ColorMask 0
        }
    }
}
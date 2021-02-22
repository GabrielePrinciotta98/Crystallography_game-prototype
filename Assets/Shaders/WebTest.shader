Shader "Custom/WebTest"
{
    SubShader
    {
        Pass
        {
            CGPROGRAM

            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag

            half4 frag(v2f_customrendertexture IN) : SV_Target
            {  
                return half4(1.0,0.0,0.0,1.0); 
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}

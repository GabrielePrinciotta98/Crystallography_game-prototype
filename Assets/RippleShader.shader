Shader "Custom/RippleShader"
{
    SubShader
    {
        Pass
        {
            Lighting Off
            CGPROGRAM

            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag

        //uniform float centerY;
        //uniform float centerZ;
            uniform int n_atoms;
            uniform float4 centers[100];
            float distances[100];
            float4 frag(v2f_customrendertexture IN) : COLOR
            {
           
            float2 pos = float2(IN.globalTexcoord.x,
                                IN.globalTexcoord.y);
            int i;
            
            for (i = 0; i < n_atoms; i++) {
                centers[i] = float4(centers[i].x, centers[i].y / 6.61, centers[i].z / 20, 0.0);
            }

            for (i = 0; i < n_atoms; i++) {
                distances[i] = distance(pos, float2 (centers[i].z, centers[i].y)) * 50;
            }

            float w = 0;

            for (i = 0; i < n_atoms; i++) {
                w += -sin(distances[i])/ distances[i];
            }
            
            return float4((w + 1.0) / 2.0, (w + 1.0) / 2.0, (w + 1.0) / 2.0, 1);
           
            }

            ENDCG
        }

    }
}


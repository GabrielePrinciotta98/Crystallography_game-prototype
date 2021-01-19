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

        //uniform half centerY;
        //uniform half centerZ;
            uniform int n_atoms;
            
            uniform half4 centers[100];
            half distances[100];
            
            half4 frag(v2f_customrendertexture IN) : SV_Target
            {
            
            half2 pos = half2(IN.globalTexcoord.x,
                                IN.globalTexcoord.y);
            int i;
            /*
            for (i = 0; i < n_atoms; i++) {
                centers[i] = half4(centers[i].x, centers[i].y / 14.0, centers[i].z / 22.0, 0.0);
            }
            */
            for (i = 0; i < n_atoms; i++) {
                distances[i] = distance(pos, half2 (centers[i].z, centers[i].y)) * (1500 / centers[i].x);
            }

            half w = 0;

            for (i = 0; i < n_atoms; i++) {
                w += -sin(distances[i])/ distances[i];

            }
            //return half4(0, n_atoms, 0, 1);
            return half4((w + 1.0) / 2.0, (w + 1.0) / 2.0, (w + 1.0) / 2.0, 1);
            //return half4(n_atoms, n_atoms, n_atoms, 1);*/

            //return half4(1, 0, 0, 1);
            }

            ENDCG
        }

    }
}


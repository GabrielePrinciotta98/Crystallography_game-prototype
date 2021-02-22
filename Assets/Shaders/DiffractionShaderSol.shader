Shader "Custom/DiffractionShaderSol"
{
    
    SubShader
    {
         Pass
        {
            CGPROGRAM

            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag

            static const half3 k0 = half3(1,0,0);
            static const half PI = 3.14159265;
            static const half lambda = 0.5;
            uniform half4 atomsPoss[200];
            uniform int nAtoms = 0;
            //half3 pos[100];
            half phi[200];


            half3 ks(half2 screenCoords)
            {
                return normalize(half3(screenCoords*4.0, 1.0));
            }

            half2 e_Pow_ix(half x)
            {
                return half2(cos(x), sin(x));
            }


            half4 frag(v2f_customrendertexture IN) : SV_Target
            {
                //mapping delle coordinate texture da [0,1] a [-1.+1]
                const half2 screenCoords = 2.0 * (half2(IN.globalTexcoord.x, IN.globalTexcoord.y) - half2(0.5,0.5));
                int i;
                half2 I = half2(1.0, 0.0);
                half3 s = (ks(screenCoords)-k0)/lambda;
                
                for (i = 0; i < nAtoms; i++) {
                    phi[i] = -2.0 * PI * dot(atomsPoss[i], s);
                }
                for (i = 0; i < nAtoms; i++) {
                    I += e_Pow_ix(phi[i]);
                }

                return half4(1,1,1,2) - half4(dot(I,I) / ((nAtoms+1) * (nAtoms+1)), dot(I, I) / ((nAtoms+1) * (nAtoms+1)), dot(I, I) / ((nAtoms+1) * (nAtoms+1)),1);
                //c = half2(1, 0);
                //return half4(atomsPos[i].x/1, atomsPos[i].y / 10, atomsPos[i].z / 10, 1);
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}

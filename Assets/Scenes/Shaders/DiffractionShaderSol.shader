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
            half2 c = half2(1.0, 0.0);


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
                half2 screenCoords = 2.0 * (half2(IN.globalTexcoord.x, IN.globalTexcoord.y) - half2(0.5,0.5));
                int i;
                /*
                for (i = 0; i < nAtoms; i++) {
                    pos[i] = half3(atomsPos[i].x, atomsPos[i].y, atomsPos[i].z);
                }

                */
                //half3 r1 = half3(5, 0, 0);
                for (i = 0; i < nAtoms; i++) {
                    phi[i] = (2.0 * PI) / lambda * dot(atomsPoss[i], k0 - ks(screenCoords));
                }
                for (i = 0; i < nAtoms; i++) {
                    c += e_Pow_ix(phi[i]);
                }

                return half4(1,1,1,2) - half4(dot(c,c) / (nAtoms * nAtoms), dot(c, c) / (nAtoms * nAtoms), dot(c, c) / (nAtoms * nAtoms),1);
                //c = half2(1, 0);
                //return half4(atomsPos[i].x/1, atomsPos[i].y / 10, atomsPos[i].z / 10, 1);
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}

Shader "Custom/DiffractionShaderCrystal"
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
            uniform float lambda = 0.5;
            uniform half zoom = 1;
            uniform half pwr = 0; 
            uniform int K = 5;
            uniform int R;
            uniform int M;
            uniform half4 atomsPos[60];
            uniform int n_atoms = 0;
            uniform half4 a;
            uniform half4 c;
            half phi[60];
            
            half3 ks(half2 screenCoords)
            {
                return normalize(half3(screenCoords*16/zoom, 1.0));
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
                const half2 center = half2(0,0);
                half d = distance(screenCoords, center);
                
                half2 I = half2(1.0, 0.0);
                half3 s = (ks(screenCoords)-k0)/lambda;
                
                for (i = 0; i < n_atoms; i++) {
                    phi[i] = -2.0 * PI * dot(atomsPos[i], s);

                }
                for (i = 0; i < n_atoms; i++) {
                    I += e_Pow_ix(phi[i]);
                }
                
                const half3 b = half3(0,K,0);
                
                
                I *= sin(PI * R * dot(a, s)) / sin(PI * dot(a,s)) *
                     sin(PI * R * dot(b, s)) / sin(PI * dot(b,s)) *
                     sin(PI * R * dot(c, s)) / sin(PI * dot(c,s));
                
                const float vign = 1 - smoothstep(0.50, 1.1, d);
                pwr *= vign;

                //return half4(1,0,0,1);
                
                return half4(1,1,1,1) - half4(dot(I,I)/((n_atoms+1)*M*(n_atoms+1)*M),
                                            dot(I, I)/((n_atoms+1)*M*(n_atoms+1)*M),
                                            dot(I, I)/((n_atoms+1)*M*(n_atoms+1)*M),
                                            1) * pwr;
               
                /*
                 I *= sin(PI * 10 * dot(a, s)) / sin(PI * dot(a,s)) *
                     sin(PI * 10 * dot(b, s)) / sin(PI * dot(b,s)) *
                     sin(PI * 10 * dot(c, s)) / sin(PI * dot(c,s));
                //pwr = pow(2, pwr);
                const float vign = 1 - smoothstep(0.50, 1.1, d);
                //pwr *= vign;
            
                return half4(1,1,1,1) - half4(dot(I,I),
                                            dot(I, I),
                                            dot(I, I),
                                            1) * pwr;
                */
                /*
                return half4(dot(I,I),
                                            dot(I, I),
                                            dot(I, I),
                                            1) * pwr;
                */
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}

Shader "Custom/DiffractionShaderCrystalSol"
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
            uniform half _lambda = 0.5;
            uniform half4 atomsPoss[20];
            uniform int nAtoms = 0;
            uniform half4 _a;
            uniform half4 _c;
            uniform half _zoom = 1;
            uniform half _pwr = 0; 
            uniform int _K = 5;
            uniform int _R = 4;
            uniform int _M = 1;
            half phi[20];


            half3 ks(half2 screenCoords)
            {
                return normalize(half3(screenCoords*_zoom, 1.0));
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
                half3 s = (ks(screenCoords)-k0)/_lambda;
                
                for (i = 0; i < nAtoms; i++) {
                    phi[i] = -2.0 * PI * dot(atomsPoss[i], s);
                }
                for (i = 0; i < nAtoms; i++) {
                    I += e_Pow_ix(phi[i]);
                }
                const half3 _b = half3(0,_K,0);
                
                I *= sin(PI * _R * dot(_a, s)) / sin(PI * dot(_a,s)) *
                     sin(PI * _R * dot(_b, s)) / sin(PI * dot(_b,s)) *
                     sin(PI * _R * dot(_c, s)) / sin(PI * dot(_c,s));
                
                return half4(1,1,1,2) - half4(dot(I,I)/((nAtoms+1)*_M*(nAtoms+1)*_M)*_pwr,
                                            dot(I, I)/((nAtoms+1)*_M*(nAtoms+1)*_M)*_pwr,
                                            dot(I, I)/((nAtoms+1)*_M*(nAtoms+1)*_M)*_pwr,
                                            1);
                                            
                //return half4(1, 0, 0, 1);

            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}

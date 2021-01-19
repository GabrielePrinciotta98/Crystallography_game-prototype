Shader "Custom/RippleShader1"
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
            uniform int nAtoms;
            uniform half4 centerss[100];
           

            half distances[100];
            half4 frag(v2f_customrendertexture IN) : COLOR
            {
           
            half2 pos = half2(IN.globalTexcoord.x,
                                IN.globalTexcoord.y);
            int i;
            /*
            for (i = 0; i < nAtoms; i++) {
                centerss[i] = half4(centerss[i].x, centerss[i].y / 14.0, 1-((centerss[i].z+10)/-22), 0.0);
            }
            */
            for (i = 0; i < nAtoms; i++) {
                distances[i] = distance(pos, half2 (centerss[i].z, centerss[i].y)) * (1500 / centerss[i].x);
            }

            half w = 0;

            for (i = 0; i < nAtoms; i++) {
                w += -sin(distances[i])/ distances[i];

            }
            //return half4(0, nAtoms, 0, 1);
            return half4((w + 1.0) / 2.0, (w + 1.0) / 2.0, (w + 1.0) / 2.0, 1);
            //return half4(nAtoms, nAtoms, nAtoms, 1);
            //return half4(w, w, w, 1);
            }

            ENDCG
        }

    }
}


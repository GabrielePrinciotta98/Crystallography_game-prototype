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

        //uniform float centerY;
        //uniform float centerZ;
            uniform int nAtoms;
            uniform float4 centerss[100];
            uniform float redd;

            float distances[100];
            float4 frag(v2f_customrendertexture IN) : COLOR
            {
           
            float2 pos = float2(IN.globalTexcoord.x,
                                IN.globalTexcoord.y);
            int i;
            float greeen = nAtoms;
            for (i = 0; i < nAtoms; i++) {
                centerss[i] = float4(centerss[i].x, centerss[i].y / 14.0, 1-((centerss[i].z+10)/-22), 0.0);
            }

            for (i = 0; i < nAtoms; i++) {
                distances[i] = distance(pos, float2 (centerss[i].z, centerss[i].y)) * 50;
            }

            float w = 0;

            for (i = 0; i < nAtoms; i++) {
                w += -sin(distances[i])/ distances[i];

            }
            //return float4(0, nAtoms, 0, 1);
            return float4((w + 1.0) / 2.0, (w + 1.0) / 2.0, (w + 1.0) / 2.0, 1);
            //return float4(nAtoms, nAtoms, nAtoms, 1);
            //return float4(w, w, w, 1);
            }

            ENDCG
        }

    }
}


using System;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [SerializeField] private Material mat;
    private EmitterCone emitter;
    private EmitterConeSol emitterSol;
    private float newX, value;
    private float lambda = 0.5f, power = 1f;

    private static readonly int Color = Shader.PropertyToID("_Color");

    void Start()
    {
        emitter = FindObjectOfType<EmitterCone>();
        emitterSol = FindObjectOfType<EmitterConeSol>();
        mat.mainTexture.wrapModeV = TextureWrapMode.Clamp;  

    }

    void Update()
    {
        if (!emitter.GetPowerOn() || !emitterSol.GetPowerOn())
        {
            mat.SetColor(Color, UnityEngine.Color.black);
        }
        else
        {
            Color curColor = mat.GetColor(Color);
            float H, S;
            UnityEngine.Color.RGBToHSV(curColor, out H, out S, out value);
            if (Math.Abs(value - 1.0f) > 0.00001f)
            {
                value += 0.005f;
            }
            
            if (Math.Abs(newX - 1) < 0.00001f)
                newX = 0;

            newX += 0.001f;
            mat.mainTextureOffset = new Vector2(newX, 0.15f + -2.15f / -6 * (power - 3));
            mat.mainTextureScale = new Vector2(1 + 3 / 1.7f * (lambda - 0.3f), 0.7f + 4.3f / -6 * (power - 3));
            mat.SetColor(Color, UnityEngine.Color.HSVToRGB(0.75f / 1.7f * (lambda - 0.3f), 1, value, true));
        }
    }
    
    public void SetPwr(float p)
    {
        power = p;
    }

    public void SetLambda(float l)
    {
        lambda = l;
    }
}

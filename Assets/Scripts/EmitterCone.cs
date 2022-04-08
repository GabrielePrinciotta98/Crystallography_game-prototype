using System;
using UnityEngine;

public class EmitterCone : MonoBehaviour
{
    public bool GameStart { get; set; }

    private Vector3 newScale, newPos;
    private float zoom = 4f, power = 1f, lambda = 0.5f;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    private Renderer _renderer;
    private Collider _collider;
    private static readonly int Color = Shader.PropertyToID("_Color");
    private bool powerOn;
    private float powerLevel;
    public Tripod tripod;
    private HUDManager hudManager;
    
    private void Start()
    {
        
        _renderer = GetComponentInChildren<Renderer>();
        _renderer.enabled = false;

        _collider = GetComponent<Collider>();
        _collider.enabled = false;

    }

    public void SetZoom(float z)
    {
        zoom = z;
    }

    public void SetPwr(float p)
    {
        power = p;
    }

    public void SetLambda(float l)
    {
        lambda = l;
    }
    
    
    private void Update()
    {
        if (GameStart)
        {
            _collider.enabled = true;
        }
        
        if (!powerOn) return;
        if (Math.Abs(powerLevel - 0.5f) > 0.0001f)
        {
            powerLevel += 0.002f;
        }


        var emitterConeTransform = transform;
        newScale = emitterConeTransform.localScale;
        

        newScale.x = 0.4f + 0.4f / 9f * (zoom - 1f);
        newScale.z = 0.4f + 0.4f / 9f * (zoom - 1f);


        emitterConeTransform.localScale = newScale;
        
 
        SetEmissionColor(powerLevel);

    }

    private void SetEmissionColor(float alpha)
    {
        Material mat = _renderer.material;
        mat.SetColor(Color, 
            UnityEngine.Color.HSVToRGB(0.75f/1.7f*(lambda-0.3f), 0.2f+alpha, 1f, true));
        mat.SetColor(EmissionColor, 
            UnityEngine.Color.HSVToRGB(0.75f/1.7f*(lambda-0.3f), 0.4f+alpha, 1f, true));
        var color = mat.color;
        mat.SetColor(Color,
            new Color(color.r, color.g, color.b, 0.4f + 0.6f*(power-1)/9));
    }

    private void OnMouseDown()
    {
        if (powerOn) return;
        powerOn = true;
        _renderer.enabled = true;
        tripod.Click();
        hudManager = FindObjectOfType<HUDManager>();
        hudManager.CheckEmittersOn();
    }

    private void OnMouseEnter()
    {
        tripod.ChangeMaterial(1);
    }

    private void OnMouseExit()
    {
        tripod.ChangeMaterial(0);
        
    }

    public bool GetPowerOn()
    {
        return powerOn;
    }
}

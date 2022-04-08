using UnityEngine;

public class Shadow : MonoBehaviour
{
    public Color color;
    private Renderer _renderer;
    private Transform shadowTransform;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.enabled = true;
        _renderer.material.color = color;
        shadowTransform = transform;
        shadowTransform.localPosition = Vector3.zero;
    }

    private void Update()
    {
        //impedisci all'ombra di non scendere sotto al pavimento
        Vector3 clampedPosition = shadowTransform.position;
        clampedPosition.y = -1.8f;
        shadowTransform.position = clampedPosition;
        
    }
}

using UnityEngine;

public class TripodPosition : MonoBehaviour
{

    private float zoom = 4f;
    private Vector3 newPos;
    private Transform tripodTransform;

    private void Start()
    {
        tripodTransform = transform;
    }

    void Update()
    {
        newPos = tripodTransform.localPosition;

        //[1,10] -> [46, 41]
        newPos.x = 46f + -5f / 9f * (zoom - 1f);
        tripodTransform.localPosition = newPos;
    }
    
    public void SetZoom(float z)
    {
        zoom = z;
    }
}

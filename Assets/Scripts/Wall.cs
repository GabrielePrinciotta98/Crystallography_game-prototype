using UnityEngine;

public class Wall : MonoBehaviour
{
    private float zoom = 4f;
    private Vector3 newPos;
    private Transform wallTransform;

    private void Start()
    {
        wallTransform = transform;
    }

    void Update()
    {
        newPos = wallTransform.localPosition;

        //[1,10] -> [-31, -25]
        newPos.x = -31f + 6f / 9f * (zoom - 1f);
        wallTransform.localPosition = newPos;
    }
    
    public void SetZoom(float z)
    {
        zoom = z;
    }
}

using UnityEngine;

public class WallSol : MonoBehaviour
{
    private float zoom = 4f;
    private Vector3 newPos;
    private Transform wallSolTransform;

    private void Start()
    {
        wallSolTransform = transform;
    }

    void Update()
    {
        newPos = wallSolTransform.localPosition;
        
        //[1,10] -> [-31, -25]
        newPos.x = -31f + 6f / 9f * (zoom - 1f);
        wallSolTransform.localPosition = newPos;
    }
    
    public void SetZoom(float z)
    {
        zoom = z;
    }
}

using UnityEngine;

public class TripodPositionSol : MonoBehaviour
{

    private float zoom = 4f;
    private Vector3 newPos;
    private Transform tripodSolTransfom;

    private void Start()
    {
        tripodSolTransfom = transform;
    }

    void Update()
    {
        newPos = tripodSolTransfom.localPosition;
        
        //[1,10] -> [46, 41]
        newPos.x = 46f + -5f / 9f * (zoom - 1f);
        tripodSolTransfom.localPosition = newPos;
    }
    
    public void SetZoom(float z)
    {
        zoom = z;
    }
}

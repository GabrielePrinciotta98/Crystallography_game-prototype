using UnityEngine;

public class Bond : MonoBehaviour
{

    public GameObject Start { get; set; } //atomo di partenza

    public GameObject End { get; set; } //atomo di arrivo

    private void Update()
    {
        //POSIZIONE
        Vector3 startPosition = Start.transform.position;
        Vector3 endPosition = End.transform.position;
        transform.position = (startPosition + endPosition) / 2; 
        
        //SCALA
        float distance = Vector3.Distance(startPosition, endPosition);
        var bondTransform = transform;
        var localScale = bondTransform.localScale;
        localScale = new Vector3(
            localScale.x,
            distance / 2, 
            localScale.z  
        );
        bondTransform.localScale = localScale;

        //ROTAZIONE
        bondTransform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.Normalize(endPosition - startPosition));
    }
}

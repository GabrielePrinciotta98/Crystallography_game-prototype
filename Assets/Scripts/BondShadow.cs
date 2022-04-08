using UnityEngine;

public class BondShadow : MonoBehaviour
{
    public GameObject Start { get; set; } //atomo di partenza

    public GameObject End { get; set; } //atomo di arrivo


    void Update()
    {
        //POSIZIONE
        Vector3 startPosition = Start.transform.GetChild(0).position;
        Vector3 endPosition = End.transform.GetChild(0).position;
        transform.position = (startPosition + endPosition) / 2;

        //SCALA
        float distance = Vector3.Distance(startPosition, endPosition);
        var bondShadowTransform = transform;
        var localScale = bondShadowTransform.localScale;
        localScale = new Vector3(
            localScale.x,
            distance / 2,
            localScale.z 
        );
        bondShadowTransform.localScale = localScale;

        //ROTAZIONE
        bondShadowTransform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.Normalize(endPosition - startPosition));
        var localEulerAngles = bondShadowTransform.localEulerAngles;
        bondShadowTransform.localEulerAngles = new Vector3(90, localEulerAngles.y, localEulerAngles.z);
    }
}

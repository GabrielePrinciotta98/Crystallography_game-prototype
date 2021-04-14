using System;
using System.Collections;
using System.Collections.Generic;
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
        transform.localScale = new Vector3(
            transform.localScale.x,
            distance / 2,
            transform.localScale.z
        );

        //ROTAZIONE
        transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.Normalize(endPosition - startPosition));
        transform.localEulerAngles = new Vector3(90, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
}

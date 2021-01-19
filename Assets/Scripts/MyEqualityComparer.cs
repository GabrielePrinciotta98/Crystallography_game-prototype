using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEqualityComparer : IEqualityComparer<Vector4>
{
    
    public bool Equals(Vector4 x, Vector4 y)
    {
        bool equals = false;
        Debug.Log("x: " + x);
        Debug.Log("y: " + y);
        Vector2 v1 = new Vector2(x.x, x.y);
        Vector2 v2 = new Vector2(y.x, y.y);
        Debug.Log("v1: "+ v1);
        Debug.Log("v2: " +v2);
        Debug.Log("distance: " + Vector2.Distance(v1, v2));
        if (Vector2.Distance(v1, v2) < 1)
        {
            equals = true;
           
        }

        Debug.Log(equals);
        return equals;
    }

    public int GetHashCode(Vector4 obj)
    {
        return 1;
    }
}

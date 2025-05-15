using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryCheck : MonoBehaviour
{
    Bounds barrierBounds = new Bounds();

    
    void Start()
    {
        barrierBounds = GetComponent<MeshRenderer>().bounds;

    }

    public bool IsWithinBoundaries(Marker marker)
    {
        Bounds markerBounds = marker.gameObject.GetComponent<MeshRenderer>().bounds;
        if (markerBounds == null)
        {
            return false;
        }
        bool inBounds = barrierBounds.Intersects(markerBounds);
        return inBounds;
    }

}

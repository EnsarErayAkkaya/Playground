using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RailConnectionPoint
{
    public Vector3 endPoint, directionPoint;
    public Rail rail,nextRail;
    
    public float Angle()
    {
        return Vector3.Angle(directionPoint, endPoint);
    }
    
        
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RailConnectionPoint
{
    public Vector3 endPoint;
    public Rail rail,nextRail; 
    public float extraEngle;
}

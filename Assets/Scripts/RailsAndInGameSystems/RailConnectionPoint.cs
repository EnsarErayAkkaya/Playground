using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RailConnectionPoint:MonoBehaviour
{
    public Vector3 point
    {
        get{ return transform.position; }
        set{ transform.position = value; }
    }
    public Rail rail;
    public RailConnectionPoint connectedPoint; 
    public float extraAngle;
    public bool isInput,isHighlighted;
    [SerializeField]GameObject highlightObj;

    public void Highlight()
    {
        highlightObj.SetActive(true);
        isHighlighted = true;
    }
    public void Downlight()
    {
        highlightObj.SetActive(false);
        isHighlighted = false;
    }
}

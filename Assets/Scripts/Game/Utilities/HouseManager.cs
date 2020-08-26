using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    public float minX,maxX,minZ,maxZ,minY,maxY;

    void OnDrawGizmos()
    {
        Gizmos.DrawCube(new Vector3(minX,0,0),Vector3.one * 4);
        Gizmos.DrawCube(new Vector3(maxX,0,0),Vector3.one * 4);
        Gizmos.DrawCube(new Vector3(0,0,minZ),Vector3.one * 4);
        Gizmos.DrawCube(new Vector3(0,0,maxZ),Vector3.one * 4);
        Gizmos.DrawCube(new Vector3(0,minY,0),Vector3.one * 4);
        Gizmos.DrawCube(new Vector3(0,maxY,0),Vector3.one * 4);
    }
}

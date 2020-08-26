using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGround : MonoBehaviour
{
    public float minX,maxX,minZ,maxZ;
    public PlaygroundType playgroundType;
    public bool CheckInPlayground(Transform t)
    {
        if(t.position.x > minX && t.position.x < maxX && t.position.z > minZ && t.position.z < maxZ)
        {
            return true;
        }
        else
        {
            Debug.Log("Not in limit: "+ t.name);
            return false;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawCube(new Vector3(minX,0,0),Vector3.one * 2);
        Gizmos.DrawCube(new Vector3(maxX,0,0),Vector3.one * 2);
        Gizmos.DrawCube(new Vector3(0,0,minZ),Vector3.one * 2);
        Gizmos.DrawCube(new Vector3(0,0,maxZ),Vector3.one * 2);
    }
}

using System.Collections;
using System.Collections.Generic;
using BezierSolution;
using UnityEngine;

public class Rail : MonoBehaviour
{
    BezierSpline splineManager;
    RailManager railManager;

    public Vector3 endPoint;
    
    [SerializeField] bool isStart;
    void Start()
    {
        splineManager = FindObjectOfType<BezierSpline>();
        railManager = FindObjectOfType<RailManager>();
        if(!isStart)
            ConnectRailToClosest();
    }
    // ADD TRAIN TRACK
    void AddBezierSplinePoint()
    {

    }
    // ROTATE RAIL
    public void RotateRail()
    {

    }
    // DELETE RAIL
    public void DeleteRail()
    {

    }

    public void ConnectRailToClosest()
    {
        Rail connectingRail = railManager.rails[railManager.rails.Count-1];

        transform.position = connectingRail.transform.position + connectingRail.transform.right * connectingRail.endPoint.x;
        transform.rotation = connectingRail.transform.rotation;
        railManager.rails.Add(this);
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position,transform.right * endPoint.x);
    }
}

using System.Collections;
using System.Collections.Generic;
using BezierSolution;
using UnityEngine;

public class Rail : MonoBehaviour
{
    BezierSpline splineManager;
    RailManager railManager;
    public Vector3 endPoint;

    Rail connectedRail;
    BezierPoint bezierPoint;
    
    [SerializeField] bool isStart;
    void Start()
    {
        splineManager = FindObjectOfType<BezierSpline>();
        railManager = FindObjectOfType<RailManager>();
        if(!isStart)
        {
            ConnectRailToClosest();
            AddBezierSplinePoint();
        }
            
    }
    // ADD TRAIN TRACK
    void AddBezierSplinePoint()
    {
        // this will add a new point to Bezier Spline 
        // for train track
        bezierPoint = splineManager.InsertNewPointAt( splineManager.Count -1 );
        bezierPoint.transform.position = transform.position+ transform.right * endPoint.x  + new Vector3(0, railManager.lineHeight, 0); 
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
        connectedRail = railManager.rails[railManager.rails.Count-1];
        

        transform.position = connectedRail.transform.position + connectedRail.transform.right * connectedRail.endPoint.x;
        transform.rotation = connectedRail.transform.rotation;
        railManager.rails.Add(this);
    }
    void OnDrawGizmos()
    {
        //Gizmos.DrawRay(transform.position,transform.right * endPoint.x);
    }
}

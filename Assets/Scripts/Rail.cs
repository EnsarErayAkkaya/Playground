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
    bool isPlacing,isSearching;
    void Start()
    {
        splineManager = FindObjectOfType<BezierSpline>();
        railManager = FindObjectOfType<RailManager>();
        if(!isStart)
            isPlacing = true;
    }
    void FixedUpdate()
    {
        if(isPlacing)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit))
            {
                transform.position = new Vector3(hit.point.x, railManager.railHeight, hit.point.z);
            }
        }
        if(isSearching)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position,railManager.connectionDistance);
            Rail closestRail = null;
            foreach (Collider item in colliders)
            {
                Rail r = item.GetComponent<Rail>();
                if( r != null && r != this )
                {
                    if(  closestRail == null || Vector3.Distance(transform.position, closestRail.transform.position) 
                        > Vector3.Distance(transform.position, r.transform.position) )
                        {
                            closestRail = r;
                        }
                }
            }
            connectedRail = closestRail;
            isSearching = false;
            ConnectRailToClosest();
            AddBezierSplinePoint();
        }
    }

    void Update()
    {
        if(isPlacing)
        {
            if(Input.GetMouseButtonDown(0))
            {
                isPlacing = false;
                isSearching = true;
            }
        }
    }
    // ADD TRAIN TRACK
    void AddBezierSplinePoint()
    {
        // this will add a new point to Bezier Spline 
        // for train track
        bezierPoint = splineManager.InsertNewPointAt( splineManager.Count );
        bezierPoint.transform.position = transform.position + transform.right * endPoint.x  + new Vector3(0, railManager.lineHeight, 0); 
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
        transform.position = connectedRail.transform.position + connectedRail.transform.right * connectedRail.endPoint.x;
        transform.rotation = connectedRail.transform.rotation;
        railManager.rails.Add(this);
    }
    void OnDrawGizmos()
    {
        if(isPlacing)
            Gizmos.DrawWireSphere(transform.position, railManager.connectionDistance);
    }
}

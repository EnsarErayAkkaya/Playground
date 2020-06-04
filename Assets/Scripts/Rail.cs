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
    
    public bool isSearching;
    void Start()
    {
        splineManager = FindObjectOfType<BezierSpline>();
        railManager = FindObjectOfType<RailManager>();
    }
    void FixedUpdate()
    {
        if(isSearching)
        {
            // Etrafındaki colliderları bul ve en takındaki rayı seç
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
            // Eğer yakında bir ray varsa bağlan
            if(closestRail != null)
            {
                connectedRail = closestRail;
                isSearching = false;
                ConnectRailToClosest();
                AddBezierSplinePoint();
            }
            // Yoksa kendini yok et
            else
            {
                Debug.Log("Yakında bağlannılabilecek bir ray bulunamadı");
                Destroy(gameObject);
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
    
}

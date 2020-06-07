using BezierSolution;
using UnityEngine;

public class Rail : MonoBehaviour,IInteractible
{
    SplineManager splineManager;
    RailManager railManager;
    ObjectPlacementManager placementManager;

    public Vector3 endPoint;
    Rail previousRail, nextRail;
    BezierPoint bezierPoint,bezierStartPoint;
    bool isSearching, isFirst;
    [SerializeField] bool isStatic;
    [SerializeField] float rotateAngle;
    void Start()
    {
        splineManager = FindObjectOfType<SplineManager>();
        railManager = FindObjectOfType<RailManager>();
        placementManager = FindObjectOfType<ObjectPlacementManager>();

        isFirst = railManager.IsFirstRail();
     
        if(!isStatic)
            placementManager.PlaceMe(gameObject, PlacementType.Rail);
        if(isFirst && isStatic)
        {
            SetStartingSplinePoints();   
        }
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
                    if(  (closestRail == null || Vector3.Distance(transform.position, closestRail.transform.position) 
                        > Vector3.Distance(transform.position, r.transform.position)) && r.nextRail == null )
                        {
                            closestRail = r;
                        }
                }
            }
            // Eğer yakında bir ray varsa bağlan
            if(closestRail != null)
            {
                previousRail = closestRail;
                ConnectRailToClosest();
                AddBezierSplinePoint();
            }
            // Yoksa kendini yok et
            else
            {
                if(isFirst)
                {
                    SetStartingSplinePoints();   
                }
                else
                {
                    Debug.Log("Yakında bağlannılabilecek bir ray bulunamadı");
                    Destroy(gameObject);
                }
            }
            railManager.AddRail(this);
            isSearching = false;
        }
    }

    // ADD TRAIN TRACK
    void AddBezierSplinePoint()
    {
        // this will add a new point to Bezier Spline for train track
        // Get previous rails bezier point Index to place new point at correct location
        //
        int pointIndex = previousRail.bezierPoint.Internal_Index + 1;
        Vector3 pos = transform.position + transform.right * endPoint.x;
        bezierPoint = splineManager.InsertNewPoint(pos, pointIndex);
    }
    void UpdateBezierPointPosition()
    {
        splineManager.UpdateBezierPoint(bezierPoint, transform.position + transform.right * endPoint.x);
        if(bezierStartPoint != null)
            splineManager.UpdateBezierPoint(bezierStartPoint, transform.position);
    }
    // DELETE RAIL
    public void Destroy()
    {
        // If this rail is static you cant delete it 
        if(isStatic)
            return;
        
        // if this is first rail we need to delete startingBezierPoint
        // and give to make our end point starting point
        if(isFirst)
        {
            splineManager.RemovePoint(bezierStartPoint.Internal_Index);
            if(nextRail != null)
            {
                nextRail.bezierStartPoint = bezierPoint;
                nextRail.isFirst = true;
            }
        }
        else
        {
            // if this is not starting point 
            // delete this rail and end bezier Point
            splineManager.RemovePoint(bezierPoint.Internal_Index);
        }

        //Clean previousRails NextRail data
        if( previousRail != null )
        {
            previousRail.nextRail = null;
        }
        // Clean nextRails previousRail data
        if( nextRail != null)
        {
            nextRail.previousRail = null;
        }
        // Remove from list
        railManager.RemoveRail(this);

        Destroy(gameObject);    
    }
    public void ConnectRailToClosest()
    {
        transform.position = previousRail.transform.position + previousRail.transform.right * previousRail.endPoint.x;
        ///
        transform.rotation = previousRail.transform.rotation;/// this line can change
        ///
        previousRail.nextRail = this;
    }
    void SetStartingSplinePoints()
    {
        Vector3 endPos = transform.position + transform.right * endPoint.x; 
        BezierPoint[] startingPoints =splineManager.SetSpline(transform.position,endPos);
        bezierStartPoint = startingPoints[0];
        bezierPoint = startingPoints[1];
    }
    public void Search()
    {
        isSearching = true;
    }

    public void Rotate()
    {
        transform.RotateAround(transform.position, transform.up, rotateAngle);
        UpdateBezierPointPosition();
    }
}

using BezierSolution;
using UnityEngine;
/// TODO:
/// 1-Dönen raylar için üçüncü bir spline noktası eklenmeli kırılma noktası gibi.
/// 2-Gene dönen raylar için uç noktanın sahip olduğu açıyı bulan ve bir sonraki raya o açıyı atayan bir fonksyon geliştirilmeli.
public class Rail : MonoBehaviour,IInteractible
{
    SplineManager splineManager;
    RailManager railManager;
    ObjectPlacementManager placementManager;

    
    Rail previousRail, nextRail;
    // bezierStartPoint sadece ilk raysa bir değere sahip olur
    BezierPoint bezierPoint,bezierStartPoint;
    BezierPoint[] extraBezierPoints;
    [SerializeField] Vector3 endPoint;
    [SerializeField] bool isStatic, hasExtraPoints, askUserWhichWay;
    [SerializeField] float rotateAngle;
    
    [Header("Additinional Points")]
    //Bu noktlar eğer varsa oyuncu tarafından seçilecek gidiş yönü 
    [SerializeField]Vector3[] extraPoints;
    [SerializeField]Vector3[] choosingPoints;
    bool isSearching, isFirst;
    void Start()
    {
        splineManager = FindObjectOfType<SplineManager>();
        railManager = FindObjectOfType<RailManager>();
        placementManager = FindObjectOfType<ObjectPlacementManager>();

        // Eğer hasExtraPoint true ise o zaman liste oluştur.
        if(hasExtraPoints)
        {
            extraBezierPoints = new BezierPoint[extraBezierPoints.Length];
            // Noktaların konumlarını ayarla
            for (int i = 0; i < extraPoints.Length; i++)
            {
                extraPoints[i] = transform.position + transform.right * extraPoints[i].x + transform.forward * extraPoints[i].z;
            }
        }
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
        int pointIndex = previousRail.bezierPoint.Internal_Index + 1;
        if(hasExtraPoints)
        {
            extraBezierPoints = splineManager.InsertNewPoints(extraPoints, pointIndex);
        }
        // if there is only one point on this rail
        else
        {
            // this will add a new point to Bezier Spline for train track
            // Get previous rails bezier point Index to place new point at correct location
            //
            
            Vector3 pos = transform.position + transform.right * endPoint.x + transform.forward * endPoint.z;
            bezierPoint = splineManager.InsertNewPoint(pos, pointIndex);
        }
        
    }
    void UpdateBezierPointPosition()
    {
        if(hasExtraPoints)
        {
            for (int i = 0; i < extraBezierPoints.Length; i++)
            {
                splineManager.UpdateBezierPoint(extraBezierPoints[i]
                    , transform.position + transform.right * extraPoints[i].x + transform.forward * extraPoints[i].z);
            }
        }
        // if there is only one point on this rail
        else
        {
            splineManager.UpdateBezierPoint(bezierPoint, transform.position + transform.right * endPoint.x + transform.forward * endPoint.z);
        }

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
        transform.position = previousRail.transform.position + previousRail.transform.right * previousRail.endPoint.x 
            + previousRail.transform.forward * endPoint.z;
        ///
        transform.rotation = previousRail.transform.rotation;/// this line can change
        ///
        previousRail.nextRail = this;
        
        if(!askUserWhichWay)
                    AddBezierSplinePoint();
        else{

        }
    }
    void SetStartingSplinePoints()
    {
        Vector3 endPos = transform.position + transform.right * endPoint.x + transform.forward * endPoint.z; 
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
    void OnDrawGizmos()
    {
        if(hasExtraPoints)
        {
            foreach (Vector3 item in extraPoints)
            {
                Gizmos.DrawWireSphere(item, .6f);
            }
        }
        else
        {
            Gizmos.DrawWireSphere(endPoint,1);
        }
    }
}

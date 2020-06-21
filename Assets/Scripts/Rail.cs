using System.Collections.Generic;
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
    ObjectChooser objectChooser;

    // Point we conencted 
    RailConnectionPoint connectedPoint;
    [SerializeField] int railWayOptionsCount;
    int currentRailWayOption;

    bool isSearching;
    public bool isFirst, collidingWithInteractible;
    [SerializeField] bool isStatic;
    [SerializeField] float rotateAngle;
    // Bir sonraki rayların bağlanabileceği noktaların serisi
    [SerializeField]RailConnectionPoint[] connectionPoints;
    [SerializeField] MeshRenderer mesh;
    void Start()
    {
        splineManager = FindObjectOfType<SplineManager>();
        railManager = FindObjectOfType<RailManager>();
        placementManager = FindObjectOfType<ObjectPlacementManager>();
        objectChooser = FindObjectOfType<ObjectChooser>();

        isFirst = railManager.IsFirstRail();
     
        if(!isStatic)
            placementManager.PlaceMe(gameObject, PlacementType.Rail);
    }
    void OnTriggerStay(Collider other)
    {
        if( !collidingWithInteractible && other.CompareTag("Interactible"))
        {
            Debug.Log("0");
            collidingWithInteractible = true;
            CollidingWithAnother();
        }   
    }
    void OnTriggerExit(Collider other)
    {
        if( collidingWithInteractible && other.CompareTag("Interactible"))
        {
            Debug.Log("1");
            collidingWithInteractible = false;
            NotCollidingWithAnother();
        }   
    }
    
    void FixedUpdate()
    {
        if(isSearching)
        {
            // Etrafındaki colliderları bul ve en yakındaki rayı seç
            Collider[] colliders = Physics.OverlapSphere(transform.position,railManager.connectionDistance);
            RailConnectionPoint closestPoint = null;
            foreach (Collider item in colliders)
            {
                Rail r = item.GetComponent<Rail>();
                if( r != null && r != this )
                {
                    for (int i = 0; i < r.connectionPoints.Length; i++)
                    {
                        if( (closestPoint == null || Vector3.Distance(transform.position, closestPoint.endPoint) 
                            > Vector3.Distance(transform.position, r.connectionPoints[i].endPoint)) && r.connectionPoints[i].nextRail == null )
                            {
                                closestPoint = r.connectionPoints[i];
                            }
                    }
                }
            }
            // Eğer yakında bir ray varsa bağlan
            if(closestPoint != null)
            {
                connectedPoint = closestPoint;
                ConnectRailToClosest();
            }
            railManager.AddRail(this);
            isSearching = false;
        }
    }
    public Rail GetNextRail()
    {
        return connectionPoints[currentRailWayOption].nextRail;
    }
    public bool HasNextRail()
    {
        if(connectionPoints[currentRailWayOption].nextRail != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // This will increment currentWayOption 
    // And will call setSpline
    //
    public void ChangeCurrentOption()
    {
        currentRailWayOption++;
        if(currentRailWayOption >= railWayOptionsCount )
        {
            currentRailWayOption = 0;
        }
        else if (currentRailWayOption < 0){
            currentRailWayOption = railWayOptionsCount -1;
        }
        splineManager.SetSpline(currentRailWayOption);
    }
    // DELETE RAIL
    public void Destroy()
    {
        // If this rail is static you cant delete it 
        if(isStatic)
            return;

        //Clean previousRails NextRail data
        if( connectedPoint != null )
        {
            connectedPoint.nextRail = null;
        }
        // For every connection point find next rail and make it null
        for (int i = 0; i < connectionPoints.Length; i++)
        {
            if(connectionPoints[i].nextRail != null)
                connectionPoints[i].nextRail.connectedPoint = null;
        }

        // Remove from list
        railManager.RemoveRail(this);

        Destroy(gameObject);    
    }
    public void ConnectRailToClosest()
    {
        transform.position = connectedPoint.rail.transform.position + connectedPoint.rail.transform.right * connectedPoint.endPoint.x 
            + connectedPoint.rail.transform.forward * connectedPoint.endPoint.z;
        transform.rotation = Quaternion.Euler(0, connectedPoint.Angle(), 0);
        
        connectedPoint.nextRail = this;
    }
    public void Search()
    {
        isSearching = true;
    }

    public void Rotate()
    {
        transform.RotateAround(transform.position, transform.up, rotateAngle);
    }
    void OnDrawGizmos()
    {
        for (int i = 0; i < connectionPoints.Length; i++)
        {
            Gizmos.DrawWireSphere(transform.position + transform.right * connectionPoints[i].endPoint.x 
            + transform.forward * connectionPoints[i].endPoint.z + transform.up * connectionPoints[i].endPoint.y, .5f);
            Gizmos.DrawLine( transform.position + transform.right * connectionPoints[i].directionPoint.x 
                + transform.forward * connectionPoints[i].directionPoint.z + transform.up * connectionPoints[i].directionPoint.y, transform.position + transform.right * connectionPoints[i].endPoint.x 
                + transform.forward * connectionPoints[i].endPoint.z + transform.up * connectionPoints[i].endPoint.y);
        }
        
    }

    public void  Glow( bool b)
    {
        if(b)
        {
            mesh.material.SetInt("Vector1_114B864B", 1);
        }
        else{
            mesh.material.SetInt("Vector1_114B864B", 0);
        }
    }
    void CollidingWithAnother()
    {
        mesh.material.SetColor("Color_A7182EB8", Color.red);
        Glow(false);
    }
    void NotCollidingWithAnother()
    {
        mesh.material.SetColor("Color_A7182EB8", Color.white);
        if(objectChooser.AmITheChoosenOne(this))
        {
            Glow(true);
        }
    }
}
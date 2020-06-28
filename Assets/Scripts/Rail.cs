using System.Collections;
using System.Collections.Generic;
using BezierSolution;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
public class Rail : MonoBehaviour,IInteractible
{
    [SerializeField]SplineManager splineManager;
    RailManager railManager;
    ObjectPlacementManager placementManager;
    ObjectChooser objectChooser;

    // Point we conencted 
    
    //all ways count
    [SerializeField] int railWayOptionsCount;
    int currentRailWayOption;// active way

    bool isSearching;
    public bool isFirst, collidingWithInteractible;
    [SerializeField] bool isStatic;
    [SerializeField] float rotateAngle;
    // Bir sonraki rayların bağlanabileceği noktaların serisi
    [SerializeField]RailConnectionPoint[] connectionPoints;
    [SerializeField] MeshRenderer mesh;
    void Start()
    {
        railManager = FindObjectOfType<RailManager>();
        placementManager = FindObjectOfType<ObjectPlacementManager>();
        objectChooser = FindObjectOfType<ObjectChooser>();

        currentRailWayOption = 1;

        //Eğer static değilse        
        if(!isStatic){
            //yerleştir
            //placementManager.PlaceMe(gameObject, PlacementType.Rail);
            //ilk raymı ona bak
            isFirst = railManager.IsFirstRail();
            //eğer ilk ray değilse
            //if(!isFirst)// açısını en son rayın devamı olarak değiştir
                //SetRailAngle(railManager.GetLastRail().GetCurrentConnectionPoint());
        }
    }
    void OnTriggerStay(Collider other)
    {
        if( !collidingWithInteractible && other.CompareTag("Interactible"))
        {
            collidingWithInteractible = true;
            //Sonradan eklenen objeyi yok et ve uyarı ver
            CollidingWithAnother();
        }   
    }
    void OnTriggerExit(Collider other)
    {
        if( collidingWithInteractible && other.CompareTag("Interactible"))
        {
            collidingWithInteractible = false;
            NotCollidingWithAnother();
        }   
    }
    
   
    public Rail GetNextRail()
    {
        return connectionPoints[currentRailWayOption].connectedPoint.rail;
    }
    public bool HasNextRail()
    {
        if(connectionPoints[currentRailWayOption].connectedPoint != null)
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
            currentRailWayOption = 1;
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

        CleanConnections();

        // Remove from list
        railManager.RemoveRail(this);

        Destroy(gameObject);    
    }
    public void CleanConnections()
    {
        //Clean connectionPoints, connectedPoints 
        foreach (RailConnectionPoint item in connectionPoints)
        {
            if(item.connectedPoint != null)
            {
                item.connectedPoint.connectedPoint = null;
                item.connectedPoint = null;
            }
        }
    }
    public void Search()
    {
        isSearching = true;
    }

    public void Rotate()
    {
        transform.RotateAround(transform.position, transform.up, rotateAngle);
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
    public RailConnectionPoint GetCurrentConnectionPoint()
    {
        return connectionPoints[currentRailWayOption];
    }
    public RailConnectionPoint[] GetConnectionPoints()
    {
        return connectionPoints;
    }
    public RailConnectionPoint[] GetFreeConnectionPoints()
    {
        return connectionPoints.Where(s => s.connectedPoint == null).ToArray();
    }
    public void HighlightConnectionPoints()
    {
        // highlight only available point ( doesnt have next or previousRail)
        foreach (RailConnectionPoint item in GetFreeConnectionPoints())
        {
            item.Highlight();
        }          
        // rs listesini highlight et //
    }
    public void HighlightConnectionPoints(RailConnectionPoint[] rs)
    {
        // highlight only available point ( doesnt have next or previousRail)
        foreach (RailConnectionPoint item in rs)
        {
            item.Highlight();
        }          
        // rs listesini highlight et //
    }
    public void DownlightConnectionPoints()
    {
        // Downlight highlighted points
        foreach (RailConnectionPoint item in GetConnectionPoints().Where(h => h.isHighlighted))
        {
            item.Downlight();
        }    
    }
}
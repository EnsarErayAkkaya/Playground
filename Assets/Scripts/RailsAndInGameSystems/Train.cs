using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierSolution;
public class Train : InteractibleBase
{
    [HideInInspector]public TrainManager trainManager;
    [SerializeField] BezierWalkerWithSpeed walker;
    public Locomotiv locomotiv;
    //[SerializeField] BezierSpline trainSpline;
    public Rail rail;
    bool started;
    
    void Start()
    {
        trainManager = FindObjectOfType<TrainManager>();
        SetSpeed();
    }
    void OnTriggerEnter(Collider other)
    {
         if(other.transform.CompareTag("Interactible"))
        {
            CollidableBase collidedObject = null;

            if(other.GetComponent<CollidableBase>() != null )
                collidedObject = other.GetComponent<CollidableBase>();
            else if(other.transform.parent.GetComponent<CollidableBase>() != null)
                collidedObject = other.transform.parent.GetComponent<CollidableBase>();
            else
                collidedObject = other.transform.parent.parent.GetComponent<CollidableBase>();

            if( lastCollided == null || (collidedObject.GetHashCode() != lastCollided.GetHashCode()) || Time.time - lastCollisionTime > .9f )
            {
                lastCollided =  collidedObject;
                lastCollisionTime = Time.time;
                if(!this.isStatic) // çarpıştığım obje statik ve ben değilsem
                {
                    if(this.creationTime > collidedObject.creationTime) // oluşmuşum ve çarpmışım
                    {
                        Destroy();
                    }
                    else  if(this.lastEditTime > collidedObject.creationTime) // kıpırdamışım ve çarpmışım
                    {
                        Destroy();
                    } 
                } 
            }
        }
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            if(rail == null)
            {
                rail = FindObjectOfType<RailManager>().GetFirstRail();
                Debug.Log("Selecting first rail, there is no attached rail to " + gameObject.name);
            }

            walker.spline = rail.GetComponent<BezierSpline>();

            walker.move = true;
            started = true;

            StartCoroutine( WaitForLocomotive() );
        }
        if(started && walker.spline.splineEnded && rail.HasNextRail() )
        {
            BezierSpline exSpline = walker.spline;
            
            Rail nextRail = rail.GetNextRail();

            if( nextRail.GetConnectionPoints().Length > 2 && nextRail.GetOutputConnectionPoints().Length  == 1 )
            {
                nextRail.SetRailWayOptionAuto(rail.GetCurrentConnectionPoint().connectedPoint);
            }
            
            rail = nextRail;
            
            walker.spline = rail.GetComponent<BezierSpline>();
            walker.NormalizedT = 0;
            exSpline.SetPathEndedFalse();
        }
        if(started && walker.spline.splineEnded && !rail.HasNextRail())
        {
            locomotiv.move = false;
        }
    }

    public void StopTrain()
    {
        if(started == true)
        {
            walker.move = false;
        }
    }
    public void ResumeTrain()
    {
        if(started == true)
        {
            walker.move = true;
        }
    }
    public void StartTrain()
    {
        if(started == false)
        {
            if(rail == null)
            {
                rail = FindObjectOfType<RailManager>().GetFirstRail();
                Debug.Log("Selecting first rail, there is no attached rail to " + gameObject.name);
            }

            walker.spline = rail.GetComponent<BezierSpline>();

            walker.move = true;
            started = true;

            StartCoroutine( WaitForLocomotive() );
        }
    }

    IEnumerator WaitForLocomotive()
    {
        yield return new WaitForSeconds(.1f);
        locomotiv.move = true;
    }
    
    public void SetSpeed()
    {
        if(trainManager.speedType == SpeedType.x)
        {
            walker.speed = trainManager.normalSpeed;
        }
        else if(trainManager.speedType == SpeedType.x2)
        {
            walker.speed = trainManager.middleSpeed;
        }
        else if(trainManager.speedType == SpeedType.x3)
        {
            walker.speed = trainManager.fastSpeed;
        }
        locomotiv.SetSpeed();
    }
    public override void Destroy()
    {
        trainManager.RemoveTrain(this);

        Destroy(gameObject);
    }
}
[System.Serializable]
public enum SpeedType
{
    x,x2,x3
}

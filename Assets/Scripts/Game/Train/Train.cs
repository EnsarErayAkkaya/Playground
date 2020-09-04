using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierSolution;
public class Train : InteractibleBase
{
    [HideInInspector]public TrainManager trainManager;
    [SerializeField] BezierWalkerWithSpeed walker;
    public Locomotiv locomotiv;
    public ParticleSystem particleSystem;
    public Rail rail;
    bool started;
    public TrainType trainType;
    public uint startingRailId;
    
    void Start()
    {
        particleSystem.Stop();
        trainManager = FindObjectOfType<TrainManager>();
        SetSpeed();
    }
    void OnTriggerEnter(Collider other)
    {
        CollidableBase collidedObject = null;

        if(other.GetComponent<CollidableBase>() != null )
        {
            collidedObject = other.GetComponent<CollidableBase>();
            OnCollision(collidedObject);
        }
        else if(other.transform.parent.GetComponent<CollidableBase>() != null)
        {
            collidedObject = other.transform.parent.GetComponent<CollidableBase>();
            OnCollision(collidedObject);
        }
    }
    void OnCollision(CollidableBase collidedObject)
    {
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
    public void Update()
    {
        if(started && walker.spline.splineEnded)
        {   
            BezierSpline exSpline = walker.spline;
            if(rail.HasNextRail() == true )
            {                
                Rail nextRail = rail.GetNextRail();

                if( nextRail.GetConnectionPoints().Length > 2 && nextRail.GetOutputConnectionPoints().Length  == 1 )
                {
                    nextRail.SetRailWayOptionAuto(rail.GetCurrentConnectionPoint().connectedPoint);
                }

                rail = nextRail;
                walker.NormalizedT = 0;
                walker.spline = rail.GetComponent<BezierSpline>();
            }
            else
            {
                StopTrain();
                trainManager.OnTrainRouteFinished(rail);    
                walker.spline = null;
                started = false;
            }
            exSpline.SetPathEndedFalse();
        }
        
    }

    public void StopTrain()
    {
        if(started == true)
        {
            walker.move = false;
            locomotiv.move = false;
            particleSystem.Stop();
        }
    }
    public void ResumeTrain()
    {
        if(started == true)
        {
            walker.move = true;
            locomotiv.move = true;
            particleSystem.Play();
        }
    }
    public void StartTrain()
    {
        if(started == false)
        {
            if(rail == null)
            {
                rail = FindObjectOfType<RailManager>().GetRails()[0];
                Debug.Log("Selecting first rail, there is no attached rail to " + gameObject.name);
            }

            walker.spline = rail.GetComponent<BezierSpline>();

            walker.move = true;
            started = true;

            particleSystem.Play();

            StartCoroutine( WaitForLocomotive() );
        }
    }

    IEnumerator WaitForLocomotive()
    {
        yield return new WaitForSeconds(.2f);
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

        Destroy(transform.parent.gameObject);
    }
}
[System.Serializable]
public enum SpeedType
{
    x,x2,x3
}

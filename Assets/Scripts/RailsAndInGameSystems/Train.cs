using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierSolution;
public class Train : MonoBehaviour
{
    [SerializeField] BezierWalkerWithSpeed walker;
    [SerializeField] Locomotiv locomotiv;
    [SerializeField] BezierSpline trainSpline;
    public Rail rail;
    bool started;
    public SpeedType speedType = SpeedType.x;
    [SerializeField] float normalSpeed, middleSpeed, fastSpeed;
    void Start()
    {
        SetSpeed();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            if(rail == null)
            {
                Debug.LogError("there is no attached rail to " + gameObject.name);
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
    IEnumerator WaitForLocomotive()
    {
        yield return new WaitForSeconds(.1f);
        locomotiv.move = true;
    }
    public void ChangeSpeed()
    {
        if(speedType == SpeedType.x) speedType = SpeedType.x2;   
        else if(speedType == SpeedType.x2) speedType = SpeedType.x3;
        else if(speedType == SpeedType.x3) speedType = SpeedType.x;

        locomotiv.SetSpeed();

        SetSpeed();
    }
    void SetSpeed()
    {
        if(speedType == SpeedType.x)
        {
            walker.speed = normalSpeed;
        }
        else if(speedType == SpeedType.x2)
        {
            walker.speed = middleSpeed;
        }
        else if(speedType == SpeedType.x3)
        {
            walker.speed = fastSpeed;
        }
    }
}
[System.Serializable]
public enum SpeedType
{
    x,x2,x3
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierSolution;
public class Train : MonoBehaviour
{
    [SerializeField] BezierWalkerWithSpeed walker;
    [SerializeField] Locomotiv locomotiv;
    public Rail rail;
    bool started;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            rail = FindObjectOfType<RailManager>().GetFirstRail();
            walker.spline = rail.GetComponent<BezierSpline>();
            walker.move = true;
            started = true;
            StartCoroutine( WaitForLocomotive() );
        }
        if(started && walker.spline.splineEnded && rail.HasNextRail() )
        {
            BezierSpline exSpline = walker.spline;
            rail = rail.GetNextRail();
            
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
}

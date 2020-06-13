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
            locomotiv.move = true;
            started = true;
        }
        if(started && walker.spline.splineEnded && rail.HasNextRail() )
        {
            BezierSpline exSpline = walker.spline;
            rail = rail.GetNextRail();
            
            walker.spline = rail.GetComponent<BezierSpline>();
            walker.NormalizedT = 0;
            exSpline.SetPathEndedFalse();
        }
    }
}

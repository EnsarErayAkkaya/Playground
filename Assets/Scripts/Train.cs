using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierSolution;
public class Train : MonoBehaviour
{
    [SerializeField] BezierWalkerWithSpeed walker;
    [SerializeField] BezierWalkerLocomotion locomotion;
    public Rail rail;
    bool started;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            rail = FindObjectOfType<RailManager>().GetFirstRail();
            walker.spline = rail.GetComponent<BezierSpline>();
            walker.move = true;
            locomotion.move = true;
            started = true;
        }
        if(started && walker.spline.splineEnded && rail.HasNextRail() )
        {
            walker.spline.SetPathEndedFalse();
            rail = rail.GetNextRail();
            walker.spline = rail.GetComponent<BezierSpline>();
            walker.NormalizedT = 0;
        }
    }
}

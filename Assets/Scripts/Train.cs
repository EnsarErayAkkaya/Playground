using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierSolution;

public class Train : MonoBehaviour
{
    [SerializeField] BezierWalkerWithSpeed walker;
    [SerializeField] BezierWalkerLocomotion locomotion;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            FindObjectOfType<BezierSpline>().ConstructLinearPath();
            walker.move = true;
            locomotion.move = true;
        }
    }
}

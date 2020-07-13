﻿using UnityEngine;
using BezierSolution;
using System.Collections.Generic;

public class SplineManager : MonoBehaviour
{
    [System.Serializable]
    public struct BezierPointArray{
        public BezierPoint[] bezierPoints;
        public RailConnectionPoint start, end;
        public GameObject track;
    }
    public BezierSpline bezierSpline;
    [SerializeField] Rail rail;
    //Bu liste raydaki yolların bir listesidir.
    [SerializeField] List<BezierPointArray> bezierPointsList;
    int bezierPointsListIndex;

    /// <summary>
    /// Set Rail way with start and end points of spline.
    /// </summary>

    public void SetSpline(RailConnectionPoint s, RailConnectionPoint e)
    {
        HideTracks();
        int j = 0;
        foreach (BezierPointArray item in bezierPointsList)
        {
            if(item.start == s && item.end == e)
            {
                for (int i = 0; i < item.bezierPoints.Length; i++)
                {
                    item.bezierPoints[i].gameObject.SetActive(true);
                }

                if(item.track != null)
                    item.track.SetActive(true);

                bezierPointsListIndex = j;
            }
            else
            {
                for (int i = 0; i < item.bezierPoints.Length; i++)
                {
                    item.bezierPoints[i].gameObject.SetActive(false);
                }
            }
            j++;
        }
        // set end point as new output point
        rail.SetCurrentOutputPoint(e);

        bezierSpline.Refresh();
    }
    public void HideTracks()
    {
        foreach (var item in bezierPointsList)
        {
            item.track.SetActive(false);
        }
    }
    public void ShowTrack()
    {
        if(bezierPointsList.Count > 1)
            bezierPointsList[bezierPointsListIndex].track.SetActive(true);
        
    }
}

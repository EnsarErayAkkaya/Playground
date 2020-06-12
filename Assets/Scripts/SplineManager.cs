using UnityEngine;
using BezierSolution;
using System.Collections.Generic;

public class SplineManager : MonoBehaviour
{
    [System.Serializable]
    public struct BezierPointArray{
        public BezierPoint[] bezierPoints;
    }
    public BezierSpline bezierSpline;
    //Bu liste raydaki yolların bir listesidir.
    [SerializeField] List<BezierPointArray> bezierPointsList; 

    /// <summary>
    /// Call this when currentRailWayOption change.
    /// </summary>  
    public void SetSpline(int activeOption)
    {
        for (int i = 0; i < bezierPointsList.Count; i++)
        {
            for (int j = 0; j < bezierPointsList[i].bezierPoints.Length; j++)
            {
                if(i == activeOption)
                {
                    bezierPointsList[i].bezierPoints[j].gameObject.SetActive(true);
                }
                else
                {
                    bezierPointsList[i].bezierPoints[j].gameObject.SetActive(false);
                }
            }
        }
        bezierSpline.Refresh();
    }
}

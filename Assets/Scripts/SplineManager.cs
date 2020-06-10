using UnityEngine;
using BezierSolution;
public class SplineManager : MonoBehaviour
{
    [SerializeField]BezierSpline bezierSpline;
    [SerializeField] float lineHeight;
    /// <summary>
    /// Call this only for starting rail.
    /// </summary>  
    public BezierPoint[] SetSpline(Vector3 startPos, Vector3 endPos)
    {
        bezierSpline.Initialize(2);
        InsertNewPoints(new Vector3[]{startPos, endPos},0);
        return new BezierPoint[] {bezierSpline[0], bezierSpline[1]};
    }
    /// <summary>
    /// Call this for inserting only ONE point.
    /// </summary>        
    public BezierPoint InsertNewPoint(Vector3 pos, int index)
    {
        BezierPoint bezierPoint = bezierSpline.InsertNewPointAt( index );
        bezierPoint.transform.position = pos  + new Vector3(0,lineHeight,0);
        return bezierPoint;
    }
    /// <summary>
    /// Call this for points one after another.
    /// </summary>
    public BezierPoint[] InsertNewPoints(Vector3[] poses, int startingIndex)
    {
        BezierPoint[] points = new BezierPoint[poses.Length];
        for (int i = 0; i < poses.Length; i++)
        {
            points[i] = InsertNewPoint(poses[i], startingIndex);
            startingIndex++;
        }
        return points;
    }
    public void RemovePoint(int index)
    {
        bezierSpline.RemovePointAt(index);
    }
    public void UpdateBezierPoint(BezierPoint point, Vector3 pos)
    {
        point.position = pos + new Vector3(0,lineHeight,0);
    }
}

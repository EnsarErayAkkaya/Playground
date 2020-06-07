using UnityEngine;
using BezierSolution;
public class SplineManager : MonoBehaviour
{
    [SerializeField]BezierSpline bezierSpline;
    [SerializeField] float lineHeight; 
    public BezierPoint[] SetSpline(Vector3 startPos, Vector3 endPos)
    {
        bezierSpline.Initialize(2);
        bezierSpline[0].position = startPos + new Vector3(0,lineHeight,0);
        bezierSpline[1].position = endPos + new Vector3(0,lineHeight,0);
        return new BezierPoint[] {bezierSpline[0], bezierSpline[1]};
    }

    public BezierPoint InsertNewPoint(Vector3 pos, int index)
    {
        BezierPoint bezierPoint = bezierSpline.InsertNewPointAt( index );
        bezierPoint.transform.position = pos  + new Vector3(0,lineHeight,0);
        return bezierPoint;
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

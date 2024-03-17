using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BezierCurve : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public Transform startTangent;
    public Transform endTangent;
    public int resolution = 10;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        DrawCurve();
    }

    void DrawCurve()
    {
        Vector3 lastPoint = startPoint.position;
        for (int i = 1; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            Vector3 point = CalculateBezierPoint(t, startPoint.position, startTangent.position, endTangent.position, endPoint.position);
            Gizmos.DrawLine(lastPoint, point);
            lastPoint = point;
        }
    }

    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }
}

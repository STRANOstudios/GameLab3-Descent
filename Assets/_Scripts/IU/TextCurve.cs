using UnityEngine;
using TMPro;

public class TextCurve : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public Transform startPoint;
    public Transform endPoint;
    public Transform startTangent;
    public Transform endTangent;
    public float curveMultiplier = 1f;
    public int resolution = 10;

    void Update()
    {
        UpdateTextCurve();
    }

    void UpdateTextCurve()
    {
        Vector3[] vertices = textMeshPro.mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = vertices[i];
            float t = vertex.x / (float)resolution;
            Vector3 pointOnCurve = CalculateBezierPoint(t, startPoint.position, startTangent.position, endTangent.position, endPoint.position);
            vertex.y += pointOnCurve.y * curveMultiplier;
            vertices[i] = vertex;
        }
        textMeshPro.mesh.vertices = vertices;
        textMeshPro.mesh.RecalculateBounds();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BezierCurvesTest : MonoBehaviour
{
    public GameObject Target;
    public Vector3[] pos;
    [Range(0f, 1f)]
    public float curveValue;
    private void Start()
    {
        CurveCoroutine();
    }
    //3차 베지어 곡선 그래프
    private IEnumerator CurveCoroutine()
    {
        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            Target.transform.position = BezierCurve(pos, i);
            yield return null;
        }

    }

    private Vector3 BezierCurve(Vector3[] pos, float i)
    {
        Vector3 point1;
        Vector3 point2;
        Vector3 point3;

        Vector3 point4;
        Vector3 point5;

        Vector3 point6;

        point1 = Vector3.Lerp(pos[0], pos[1], curveValue);
        point2 = Vector3.Lerp(pos[1], pos[2], curveValue);
        point3 = Vector3.Lerp(pos[2], pos[3], curveValue);

        point4 = Vector3.Lerp(point1, point2, i);
        point5 = Vector3.Lerp(point2, point3, i);

        point6 = Vector3.Lerp(point4, point5, i);

        return point6;
    }
}

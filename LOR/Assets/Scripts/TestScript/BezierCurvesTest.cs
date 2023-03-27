using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Vector3 BezierCurve(Vector3[] pos, float index)
    {
        List<Vector3> points = new List<Vector3>(pos.Length);

        for (int i = 0; i < pos.Length; i++)
            points[i] = pos[i];

        for (int i = pos.Length; i > 0; i--)
        {
            for (int j = 0; j < i; j++)
            {
                points[i] = Vector3.Lerp(pos[j], pos[j + 1], index);
            }
        }
        return points[0];
    }
}

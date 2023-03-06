using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurvesTest : MonoBehaviour
{
    public Transform[] pos;
    public float curveValue;
    private void Start()
    {
        CurveCoroutine();
    }
    //3차 베지어 곡선 그래프
    private IEnumerator CurveCoroutine()
    {
        Vector3 point1;
        for (int i = 0; i < pos.Length; i++)
        {
            Vector3.Lerp(pos[0], pos[1], i);
        }
        yield return null;

    }
}

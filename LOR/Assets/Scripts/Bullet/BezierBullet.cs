using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierBullet : BaseBullet
{
    public Vector3[] targetPos;
    public float bezierSpeed = 1;
    private float processingValue = 0;


    public BezierBullet(Vector3[] pos)
    {
        targetPos = pos;
    }
    protected override void Start()
    {
        base.Start();
        StartCoroutine(BezierCoroutine());
    }
    private IEnumerator BezierCoroutine()
    {
        WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
        while (processingValue < 1f)
        {
            processingValue += Time.deltaTime * bezierSpeed;

            Vector3 pos = BezierCurve(targetPos, processingValue);
            Vector3 dir = pos - transform.position;

            if (dir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(dir.normalized);
            transform.position = pos;

            yield return waitForFixedUpdate;
        }

        while (true)
        {
            transform.position += transform.forward * speed;
            yield return null;
        }
    }

    private Vector3 BezierCurve(Vector3[] pos, float index)
    {
        Vector3[] vec = (Vector3[])pos.Clone();
        if (vec.Length <= 0)
        {
            processingValue = 1;
            return transform.forward;
        }
        for (int i = vec.Length - 1; i > 0; i--)
        {
            for (int j = 0; j < i; j++)
            {
                vec[j] = Vector3.Lerp(vec[j], vec[j + 1], index);
            }
        }
        return vec[0];
    }
}

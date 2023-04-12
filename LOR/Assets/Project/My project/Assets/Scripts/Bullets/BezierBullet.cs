using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierBullet : BaseBullet
{
    public Vector3[] poses;
    public float bezierSpeed;
    public GameObject target;
    private float bezierProcessing = 0;
    private void Start()
    {
        StartCoroutine(BezierCoroutine());
    }
    private IEnumerator BezierCoroutine()
    {
        while (bezierProcessing < 1)
        {
            if (target != null) poses[poses.Length - 1] = target.transform.position;
            Vector3 targetPos = BezierCurve(poses, bezierProcessing);
            transform.LookAt(targetPos);
            transform.position = targetPos;
            bezierProcessing += Time.deltaTime * bezierSpeed;
            yield return new WaitForFixedUpdate();
        }

        while (true)
        {
            transform.position += transform.forward * Time.deltaTime * speed;
            yield return null;
        }
    }
    private Vector3 BezierCurve(Vector3[] poses, float t)
    {
        Vector3[] vector3s = (Vector3[])poses.Clone();

        for (int i = vector3s.Length; i > 0; i--)
        {
            for (int j = 0; j < i - 1; j++)
            {
                vector3s[j] = Vector3.Lerp(vector3s[j], vector3s[j + 1], t);
            }
        }

        return vector3s[0];
    }

}

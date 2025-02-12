using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : BaseBullet
{
    public GameObject target;
    public float hommingPower;
    public float hommingDuration;
    protected override void Update()
    {
        base.Update();
        Move();
    }
    protected override void Move()
    {
        base.Move();
        if (target != null && hommingDuration > 0)
        {
            Vector3 pos = (transform.position- target.transform.position).normalized;
            transform.Rotate(pos * Time.deltaTime * hommingPower);
            hommingDuration -= Time.deltaTime;
        }

        transform.position += transform.forward * speed * Time.deltaTime;
    }
}

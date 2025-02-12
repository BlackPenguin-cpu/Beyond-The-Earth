using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : BaseBullet
{
    protected override void Update()
    {
        base.Update();
        Move();
    }
    protected override void Move()
    {
        base.Move();
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}

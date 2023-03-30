using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunEnemy : BaseEnemy
{
    public float attackDelay;
    private float nowAttackDelay;
    protected override void Update()
    {
        base.Update();
        nowAttackDelay -= Time.deltaTime;
        if (nowAttackDelay < 0)
        {
            nowAttackDelay = attackDelay;
        }

    }
    protected override void Move()
    {
        base.Move();
        transform.position += transform.forward * Time.deltaTime * speed;
    }
}

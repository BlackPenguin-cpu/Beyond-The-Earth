using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : BaseEnemy
{
    protected override void Die()
    {
        base.Die();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform obj = transform.GetChild(i);
            obj.parent = null;
            obj.GetComponent<Rigidbody>().AddForce(Random.insideUnitCircle * 5);
            Destroy(obj, 3);
        }
    }

    protected override void Hit()
    {
        base.Hit();
    }

    protected override void Move()
    {
        base.Move();
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        Die();
    }
}

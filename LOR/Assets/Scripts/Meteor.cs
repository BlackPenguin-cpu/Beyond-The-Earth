using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : Entity
{
    protected override void Die()
    {
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

    }

    protected override void Move()
    {
        throw new System.NotImplementedException();
    }
}

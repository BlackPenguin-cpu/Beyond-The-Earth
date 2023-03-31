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
            Destroy(obj.GetComponent<SpringJoint>());
            obj.GetComponent<Rigidbody>().AddForce(Random.insideUnitCircle * 40, ForceMode.Impulse);
            Destroy(obj.gameObject, 3);
        }
        GetComponent<Rigidbody>().AddForce(Random.insideUnitCircle * 40, ForceMode.Impulse);
        Destroy(gameObject, 3);
    }

    protected override void Hit()
    {
        base.Hit();
    }

    protected override void Move()
    {
        base.Move();
        transform.position += Vector3.back * speed * Time.deltaTime;
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        Die();
    }
}

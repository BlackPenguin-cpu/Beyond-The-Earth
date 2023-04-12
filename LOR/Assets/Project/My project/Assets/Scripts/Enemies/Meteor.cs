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
            Transform obj = transform.GetChild(0);
            obj.parent = null;
            obj.GetComponent<Rigidbody>().AddForce(Random.insideUnitSphere * 10, ForceMode.Impulse);
            Destroy(obj.gameObject, 3);
        }
        GetComponent<Rigidbody>().AddForce(Random.insideUnitSphere * 10,ForceMode.Impulse);
        Destroy(gameObject, 3);
    }
    void Update()
    {
        Move();
    }
    protected override void Move()
    {
        base.Move();
        transform.position += Vector3.back * Time.deltaTime * speed;
    }
}

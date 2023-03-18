using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : Entity
{
    public float attackDmg;
    protected virtual void Update()
    {
        Move();
    }

    protected override void Die()
    {
    }

    protected override void Hit()
    {
    }

    protected override void Move()
    {

    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out Player player))
        {
            player.Hp -= attackDmg;
        }
    }
}

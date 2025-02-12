using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : Entity
{
    public float lifeTime;
    public float atkDmg;
    public bool isPlayerBullet;
    protected virtual void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Die();
        }
    }
    protected override void Die()
    {
        Destroy(gameObject);
    }

    protected override void Hit()
    {
    }

    protected override void Move()
    {
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (isPlayerBullet && other.tag == "Enemy" || !isPlayerBullet && other.tag == "Player")
        {
            other.GetComponentInParent<Entity>().Hp -= atkDmg;
            Hp--;
        }
    }
}

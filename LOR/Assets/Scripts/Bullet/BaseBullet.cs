using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : Entity
{
    public bool isPlayerBullet;
    public float attackDmg;
    public float lifeTime = 3;
    protected virtual void Start()
    {
        Destroy(gameObject, lifeTime);
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(isPlayerBullet ? "Enemy" : "Player"))
        {
            other.GetComponent<Entity>().Hp -= attackDmg;
            Hp--;
        }
    }
}

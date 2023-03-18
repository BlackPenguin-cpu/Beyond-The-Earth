using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public abstract class Entity : MonoBehaviour
{
    public float maxHp;
    public float speed;
    [SerializeField]
    protected float hp;
    public virtual float Hp
    {
        get { return hp; }
        set
        {
            value = Mathf.Clamp(value, 0, maxHp);
            if (hp - value >= 0)
            {
                Hit();
            }

            hp = value;
            if (value <= 0)
            {
                Die();
            }
        }
    }
    protected abstract void Move();
    protected abstract void Die();
    protected abstract void Hit();
}

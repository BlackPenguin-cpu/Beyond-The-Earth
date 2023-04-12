using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public abstract class Entity : MonoBehaviour
{
    public float speed;
    public float maxHp;
    [SerializeField]
    protected float hp;
    protected bool isDie;
    public virtual float Hp
    {
        get
        {
            return hp;
        }
        set
        {
            if (hp - value > 0)
            {
                Hit();
            }
            value = Mathf.Clamp(value, 0, maxHp);
            hp = value;
            if (!isDie && hp <= 0)
            {
                isDie = true;
                Die();
            }
        }
    }

    protected abstract void Die();
    protected abstract void Move();
    protected abstract void Hit();
}

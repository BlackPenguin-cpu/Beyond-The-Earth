using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public float maxHp;
    public float hp;
    public float speed;
    protected virtual float Hp
    {
        get { return hp; }
        set
        {
            value = Mathf.Clamp(value, 0, maxHp);
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

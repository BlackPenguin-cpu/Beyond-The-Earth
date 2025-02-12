using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : Entity
{
    public float atkDmg;

    protected Player player;
    [SerializeField]
    protected ParticleSystem hitParticle;
    [SerializeField]
    protected ParticleSystem deadParticle;

    protected ItemObject[] itemObjects;
    protected virtual void Start()
    {
        player = Player.instance;
        itemObjects = Resources.LoadAll<ItemObject>("Items/");
    }
    public void ForceDie()
    {
        Die();
    }
    protected override void Die()
    {
        switch (Random.Range(0, 10))
        {
            case 0:
                Instantiate(itemObjects[0], transform.position, Quaternion.identity);
                break;
            case 1:
                Instantiate(itemObjects[1], transform.position, Quaternion.identity);
                break;
            case 2:
                Instantiate(itemObjects[2], transform.position, Quaternion.identity);
                break;
            case 3:
            case 4:
            case 5:
                Instantiate(itemObjects[3], transform.position, Quaternion.identity);
                break;
            default:
                break;
        }
        GameManager.instance.score += maxHp * 100;
        SoundManager.instance.PlaySound("EntityDead");
        Instantiate(deadParticle, transform.position, deadParticle.transform.rotation);
        Destroy(gameObject);
    }

    protected override void Hit()
    {
        hitParticle.Play();
    }

    protected override void Move()
    {
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            player.Hp -= atkDmg;
        }
    }
}

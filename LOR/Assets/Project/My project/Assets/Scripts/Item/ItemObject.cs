using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemObject : MonoBehaviour
{
    public float speed = 5;
    protected Player player;

    [SerializeField]
    private ParticleSystem actionParticle;
    protected virtual void Start()
    {
        player = Player.instance;
    }
    protected virtual void Update()
    {
        Move();
        if (transform.position.z < -5 + player.transform.position.z)
        {
            Action();
            Destroy(gameObject);
        }

    }
    protected virtual void Move()
    {
        transform.position += Vector3.back * speed * Time.deltaTime;
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SoundManager.instance.PlaySound("ItemUse");
            Instantiate(actionParticle, transform.position, actionParticle.transform.rotation);
            GameManager.instance.score += 1000;
            Action();
            Destroy(gameObject);
        }
    }

    protected abstract void Action();
}

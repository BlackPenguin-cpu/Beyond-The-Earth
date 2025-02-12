using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnemy : BaseEnemy
{
    public float atkDelay;
    public float bezierSphereScale;
    private float nowAtkDelay;

    private NormalBullet normalBullet;
    protected override void Start()
    {
        base.Start();
        normalBullet = Resources.Load<NormalBullet>("Bullet/NormalBullet");
    }
    private void Update()
    {
        nowAtkDelay -= Time.deltaTime;
        if (nowAtkDelay <= 0)
        {
            Attack();
            nowAtkDelay = atkDelay;
        }
        Move();
    }
    protected override void Move()
    {
        base.Move();
        transform.position += Vector3.back * Time.deltaTime * speed;
        transform.position += new Vector3(Mathf.Sin(Time.time), Mathf.Cos(Time.time), 0) * Time.deltaTime * 3;
    }
    private void Attack()
    {
        if (transform.position.z < player.transform.position.z) return;

        for (int i = 0; i < 10; i++)
        {
            Instantiate(normalBullet, transform.position, Quaternion.Euler(Random.Range(-20, 20), Random.Range(160, 200), 0));
        }
        Instantiate(normalBullet, transform.position, Quaternion.identity);
    }
}

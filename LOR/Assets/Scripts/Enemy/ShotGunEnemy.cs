using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunEnemy : BaseEnemy
{
    public float attackDelay;
    private float nowAttackDelay;

    private NormalBullet normalBullet;

    private void Start()
    {
        normalBullet = Resources.Load<NormalBullet>("Bullet/NormalBullet");
    }
    protected override void Update()
    {
        base.Update();
        nowAttackDelay -= Time.deltaTime;
        if (nowAttackDelay < 0)
        {
            nowAttackDelay = attackDelay;
            Attack();
        }
    }
    private void Attack()
    {
        if (transform.position.z < Player.instance.transform.position.z) return;
        for (int i = 0; i < 10; i++)
        {

            NormalBullet bullet = Instantiate(normalBullet, transform.position, Quaternion.Euler(Random.Range(-10, 10), Random.Range(-10, 10) + 180, transform.rotation.z));

            bullet.speed = 30;
            bullet.attackDmg = attackDmg;
            bullet.lifeTime = 3;
            bullet.isPlayerBullet = false;
        }
    }
    protected override void Move()
    {
        base.Move();
        transform.position += Vector3.back * Time.deltaTime * speed +
            new Vector3(Mathf.Cos(Time.time), Mathf.Sin(Time.time)) * 5 * Time.deltaTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierEnemy : BaseEnemy
{
    public float atkDelay;
    public float bezierSphereScale;
    private float nowAtkDelay;

    private BezierBullet bezierBullet;
    protected override void Start()
    {
        base.Start();
        bezierBullet = Resources.Load<BezierBullet>("Bullet/BezierBullet");
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
    }
    private void Attack()
    {
        if (transform.position.z < player.transform.position.z) return;
        
        Vector3[] poses = new Vector3[3];
        poses[0] = transform.position;
        poses[1] = transform.position + Random.insideUnitSphere * bezierSphereScale;
        poses[2] = player.transform.position;

        bezierBullet.isPlayerBullet = false;
        bezierBullet.atkDmg = atkDmg;
        bezierBullet.bezierSpeed = 1;
        bezierBullet.lifeTime = 5;
        bezierBullet.poses = poses;
        Instantiate(bezierBullet, transform.position, Quaternion.identity);
    }

}

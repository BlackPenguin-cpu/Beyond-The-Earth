using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnemy : BaseEnemy
{
    public float attackDelay;
    public float bezierAreaScale;
    private float nowAttackDelay;

    private BezierBullet bezierBullet;
    private Player player;
    private void Start()
    {
        player = Player.instance;
        bezierBullet = Resources.Load<BezierBullet>("Bullet/BezierBullet");
    }
    protected override void Update()
    {
        base.Update();
        nowAttackDelay += Time.deltaTime;

        if (nowAttackDelay > attackDelay && transform.position.z > player.transform.position.z)
        {
            Attack();
            nowAttackDelay = 0;
        }
    }
    private void Attack()
    {
        Vector3[] pos = new Vector3[3];
        pos[0] = transform.position;
        pos[1] = transform.position + Random.insideUnitSphere * bezierAreaScale;
        //pos[1] = (transform.position - player.transform.position) / 2 + Random.insideUnitSphere * bezierAreaScale;
        pos[2] = player.transform.position;


        var bezierBulletObj = Instantiate(bezierBullet.gameObject, transform.position, transform.rotation);
        BezierBullet bezierComponent = bezierBulletObj.GetComponent<BezierBullet>();
        bezierComponent.targetPos = pos;
        bezierComponent.isPlayerBullet = false;
        bezierBullet.maxHp = 1;
        bezierBullet.attackDmg = attackDmg;
    }
    protected override void Move()
    {
        base.Move();
        transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));
    }
}

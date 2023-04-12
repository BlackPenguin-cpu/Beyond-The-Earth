using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBoss : BaseBoss
{
    public float bezierSphereScale;

    private BezierBullet bezierBullet;
    protected override void Start()
    {
        base.Start();
        bezierBullet = Resources.Load<BezierBullet>("Bullet/BezierBullet");

        StartCoroutine(Pattern());
    }
    protected override void Update()
    {
        base.Update();
    }
    private void FixedUpdate()
    {
        Move();
    }
    protected override void Die()
    {
        GameManager.instance.StageClear();
        base.Die();
    }
    private IEnumerator Pattern()
    {
        float value = 0;
        while (value < 1)
        {
            transform.position = Vector3.Lerp(new Vector3(0, 40, 10), new Vector3(0, 0, 40), value);
            value += (Time.deltaTime / 4);
            yield return null;
        }
        yield return new WaitForSeconds(3);
        while (true)
        {
            Attack(2);
            yield return new WaitForSeconds(1 + hp / 1000);
            Attack(3);
            yield return new WaitForSeconds(1 + hp / 1000);
            Attack(5);
            StartCoroutine(BarrelRoll());
            yield return new WaitForSeconds(1 + hp / 1000);
        }
    }

    private IEnumerator BarrelRoll()
    {
        Vector3 rot = Vector3.zero;
        while (rot.z < 360)
        {
            transform.eulerAngles = rot;
            rot.z += 4;
            yield return null;
        }
        transform.eulerAngles = Vector3.zero;
    }
    protected override void Move()
    {
        base.Move();
        transform.position += new Vector3(Mathf.Cos(Time.time) * speed, Mathf.Sin(Time.time) * 5, 0) * Time.deltaTime;
    }
    private void Attack(int Count)
    {
        if (transform.position.z < player.transform.position.z) return;
        float bezierSpeed = Random.Range(0.5f, 3);
        for (int i = 0; i < Count; i++)
        {

            Vector3[] poses = new Vector3[3];
            poses[0] = transform.position;
            poses[1] = player.transform.position + Random.insideUnitSphere * bezierSphereScale;
            poses[2] = player.transform.position;

            bezierBullet.isPlayerBullet = false;
            bezierBullet.atkDmg = atkDmg;
            bezierBullet.bezierSpeed = bezierSpeed;
            bezierBullet.lifeTime = 5;
            bezierBullet.poses = poses;
            Instantiate(bezierBullet, transform.position, Quaternion.identity);
        }
    }
}

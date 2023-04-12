using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBoss : BaseBoss
{
    public float bezierSphereScale;
    public float rotSpeed;
    public Material PhaseMaterial;

    private MeshRenderer meshRenderer;
    [SerializeField]
    private MeshRenderer meshRenderer2;
    private BezierBullet bezierBullet;
    private HomingBullet homingBullet;

    protected override void Start()
    {
        base.Start();

        meshRenderer = GetComponent<MeshRenderer>();
        bezierBullet = Resources.Load<BezierBullet>("Bullet/BezierBullet");
        homingBullet = Resources.Load<HomingBullet>("Bullet/HomingBullet");

        StartCoroutine(Pattern());
    }
    protected override void Update()
    {
        base.Update();
        if (hp < maxHp / 2)
        {
            meshRenderer.material = PhaseMaterial;
            meshRenderer2.material = PhaseMaterial;
            transform.Rotate(0, 10f * rotSpeed * Time.deltaTime, 0);
            atkDmg = 20;
        }
        else
        {
            transform.Rotate(0, (maxHp / hp) * rotSpeed * Time.deltaTime, 0);
        }
    }
    private void FixedUpdate()
    {
        Move();
    }
    private IEnumerator Pattern()
    {
        float value = 0;
        while (value < 1)
        {
            transform.position = Vector3.Lerp(new Vector3(0, 40, 10), new Vector3(0, 0, 60), value);
            value += (Time.deltaTime / 4);
            yield return null;
        }
        yield return new WaitForSeconds(3);
        while (true)
        {
            int delay = 4;
            if (hp < maxHp / 2) delay--;
            yield return new WaitForSeconds(delay);
            for (int i = 0; i < 30; i++)
            {
                Attack1();
                yield return null;
            }
            yield return new WaitForSeconds(delay);
            for (int i = 0; i < 10; i++)
            {
                Attack2();
                yield return new WaitForSeconds(1 - i * 0.1f);
            }
            yield return new WaitForSeconds(delay);
            for (int i = 10; i > 0; i--)
            {
                Attack3(i);
                yield return new WaitForSeconds(0.5f);
            }
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

    protected override void Die()
    {
        GameManager.instance.StageClear();
        base.Die();
    }
    private void Attack1()
    {
        if (transform.position.z < player.transform.position.z) return;

        Vector3[] poses = new Vector3[5];
        poses[0] = transform.position;
        poses[1] = transform.position + Random.insideUnitSphere * bezierSphereScale;
        poses[3] = player.transform.position + Random.insideUnitSphere * bezierSphereScale;
        poses[4] = player.transform.position;

        bezierBullet.isPlayerBullet = false;
        bezierBullet.atkDmg = atkDmg;
        bezierBullet.bezierSpeed = 3;
        bezierBullet.lifeTime = 5;
        bezierBullet.poses = poses;
        Instantiate(bezierBullet, transform.position, Quaternion.identity);
    }

    private void Attack2()
    {
        if (transform.position.z < player.transform.position.z) return;
        for (int i = 0; i < 4; i++)
        {
            Vector3[] poses = new Vector3[5];
            poses[0] = transform.position;
            poses[1] = transform.position + Random.insideUnitSphere * bezierSphereScale + Vector3.back * 20;
            poses[4] = player.transform.position;

            bezierBullet.target = player.gameObject;
            bezierBullet.isPlayerBullet = false;
            bezierBullet.atkDmg = atkDmg;
            bezierBullet.bezierSpeed = 3;
            bezierBullet.lifeTime = 5;
            bezierBullet.poses = poses;
            Instantiate(bezierBullet, transform.position, Quaternion.identity);
        }
    }
    private void Attack3(float speed)
    {
        if (transform.position.z < player.transform.position.z) return;
        for (int i = 0; i < 4; i++)
        {
            Vector3[] poses = new Vector3[5];
            poses[0] = transform.position;
            poses[3] = player.transform.position + Random.insideUnitSphere * bezierSphereScale * 5;
            poses[4] = player.transform.position;

            bezierBullet.isPlayerBullet = false;
            bezierBullet.atkDmg = atkDmg;
            bezierBullet.bezierSpeed = i;
            bezierBullet.lifeTime = 5;
            bezierBullet.poses = poses;
            Instantiate(bezierBullet, transform.position, Quaternion.identity);
        }
    }

    protected override void Move()
    {
        base.Move();
        transform.position += new Vector3(Mathf.Cos(Time.time) * speed, Mathf.Sin(Time.time) * 5, 0) * Time.deltaTime;
    }
}

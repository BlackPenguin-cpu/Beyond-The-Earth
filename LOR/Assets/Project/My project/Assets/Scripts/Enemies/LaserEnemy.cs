using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserEnemy : BaseEnemy
{
    public float atkDistance = 40;

    private float attackDuration;

    private bool isAttacking;
    private LineRenderer lineRenderer;
    protected override void Start()
    {
        base.Start();
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        Move();
        if (transform.position.z > player.transform.position.z && Vector3.Distance(player.transform.position, transform.position) < atkDistance)
            isAttacking = true;
        else
            isAttacking = false;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, player.transform.position);
        lineRenderer.enabled = isAttacking;
        if (isAttacking)
        {
            Attack();
        }
    }
    private void Attack()
    {
        if (transform.position.z < player.transform.position.z) return;
        attackDuration += Time.deltaTime;
        lineRenderer.endColor = Color.Lerp(Color.cyan, Color.red, attackDuration / 3);

        if (attackDuration > 3)
        {
            player.Hp -= atkDmg;
            attackDuration = 0;
        }
    }
    protected override void Move()
    {
        base.Move();
        transform.position += Vector3.back * Time.deltaTime * speed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserEnemy : BaseEnemy
{
    private Player player;
    private LineRenderer lineRenderer;

    private float attackDuration;
    private bool isAttack;
    private void Start()
    {
        player = Player.instance;
        lineRenderer = GetComponent<LineRenderer>();
    }
    protected override void Update()
    {
        base.Update();

        if (player.transform.position.z > transform.position.z) isAttack = false;
        else
            isAttack = Vector3.Distance(player.transform.position, transform.position) < 60;
        lineRenderer.enabled = isAttack;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, player.transform.position);
        if (isAttack)
        {
            attackDuration += Time.deltaTime;
            lineRenderer.endColor = Color.Lerp(Color.cyan, Color.red, attackDuration / 2);
            if (attackDuration > 2)
            {
                attackDuration = 0;
                player.Hp -= attackDmg;
                lineRenderer.endColor = Color.cyan;
            }
        }
    }
    protected override void Move()
    {
        base.Move();
        transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));
    }

}

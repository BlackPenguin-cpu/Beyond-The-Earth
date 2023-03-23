using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ShootEnemy : MonoBehaviour
{
    [SerializeField]
    private float speed = 7;
    [SerializeField]
    private float attackDamage = 7;

    private Player player;
    private LineRenderer lineRenderer;

    private float attackDuration;
    private bool isAttack;
    private void Start()
    {
        player = Player.instance;
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));

        isAttack = Vector3.Distance(player.transform.position, transform.position) < 30;
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
                player.Hp -= attackDamage;
                lineRenderer.endColor = Color.cyan;
            }
        }

    }

}

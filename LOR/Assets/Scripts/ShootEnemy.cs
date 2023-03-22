using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnemy : MonoBehaviour
{
    [SerializeField]
    private float speed = 7;

    private Player player;
    private LineRenderer lineRenderer;

    private bool isAttack;
    private void Start()
    {
        player = GetComponent<Player>();
    }
    private void Update()
    {
        transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));

        isAttack = Vector3.Distance(player.transform.position, transform.position) < 30;
        lineRenderer.enabled = isAttack;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, player.transform.position);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChargeEnemy : BaseEnemy
{
    private Player player;
    void Start()
    {
        player = Player.instance;
    }
    protected override void Update()
    {
        base.Update();
        transform.LookAt(player.transform);
        transform.position += transform.forward * Time.deltaTime * speed;
    }
}

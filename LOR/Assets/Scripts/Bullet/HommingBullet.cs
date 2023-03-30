using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class HommingBullet : BaseBullet
{
    public float hommingPower;
    public float hommingDuration;

    private Player player;
    protected override void Start()
    {
        base.Start();
        player = Player.instance;
    }
    private void Update()
    {
        if (hommingDuration > 0)
        {
            hommingDuration -= Time.deltaTime;
            Vector3 vec = player.transform.position - transform.position;
            float rad = Mathf.Atan2(vec.x, vec.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.position, player.transform.position), hommingPower * Time.deltaTime);
        }

        transform.position += transform.forward * speed * Time.deltaTime;
    }

}

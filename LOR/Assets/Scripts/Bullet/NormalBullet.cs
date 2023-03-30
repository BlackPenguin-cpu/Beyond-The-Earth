using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : BaseBullet
{
    private void FixedUpdate()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }
}

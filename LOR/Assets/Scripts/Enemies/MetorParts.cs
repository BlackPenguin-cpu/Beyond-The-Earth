using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetorParts : BaseEnemy
{
    protected override void Hit()
    {
    }
    protected override void Die()
    {
        Destroy(gameObject);
    }
}

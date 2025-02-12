using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleItem : ItemObject
{
    protected override void Action()
    {
        player.invincibleTime = 5;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : ItemObject
{
    protected override void Action()
    {
        player.Hp += 15;
    }
}

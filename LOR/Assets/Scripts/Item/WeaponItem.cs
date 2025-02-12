using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : ItemObject
{
    protected override void Action()
    {
        if (player.weaponLevel < 4)
            player.weaponLevel++;
    }

}

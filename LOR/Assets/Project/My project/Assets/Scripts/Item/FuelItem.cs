using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelItem : ItemObject
{
    protected override void Action()
    {
        player.Fuel += 10;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPlus : CardUse
{
    public override void Use(Character sender, Character receiver)
    {
        base.Use(sender, receiver);

        sender.info.Hp += 8 * card.info.EffVal;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPlusPlus : CardUse
{
    public override void Use(Character sender, Character receiver)
    {
        base.Use(sender, receiver);

        sender.info.Hp += 12 * card.info.EffVal;      
    }
}

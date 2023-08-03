using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : CardUse
{
    public override void Use(Character sender, Character receiver)
    {
        base.Use(sender, receiver);

        sender.info.Hp += 4 * card.info.EffVal;
    }

}

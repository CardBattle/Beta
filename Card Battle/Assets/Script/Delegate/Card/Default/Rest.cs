using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rest : CardUse
{
    public override void Use(Character sender, Character receiver)
    {
        base.Use(sender, receiver);

        sender.info.Hp += 2 * card.info.EffVal;

        
    }

}

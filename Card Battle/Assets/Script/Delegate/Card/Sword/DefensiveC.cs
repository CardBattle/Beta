using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveC : CardUse
{
    public override void Use(Character sender, Character receiver)
    {
        base.Use(sender, receiver);

        if(card.info.Level >= 3)
            receiverCard.info.EffVal -= 2;
    }
}

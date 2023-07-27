using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : CardUse
{
    public override void Use(Character sender, Character receiver)
    {
        base.Use(sender, receiver);

        if (senderCard.info.Dice < receiverCard.info.Dice)
            receiverCard.info.EffVal -= 2;
        else receiverCard.info.EffVal -= 4;
    }
}

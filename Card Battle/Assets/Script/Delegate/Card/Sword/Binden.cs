using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Binden : CardUse
{
    public override void Use(Character sender, Character receiver)
    {
        base.Use(sender, receiver);
        if(receiverCard.info.Property == PropertyType.ATTACK)
            receiverCard.info.EffVal = 0;
    }
}

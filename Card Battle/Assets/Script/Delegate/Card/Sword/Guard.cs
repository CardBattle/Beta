using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : CardUse
{
    public override void Use(Character sender, Character receiver)
    {
        base.Use(sender, receiver);

        if (senderCard.info.Dice < receiverCard.info.Dice)
            receiverCard.info.EffVal -= card.info.EffVal;
        else 
        { 
            if(card.info.Level >= 3)
            {
                sender.info.Hp += CalculateDmg(receiver.info.AttackDmg, receiverCard.info.Dice, receiverCard.info.EffVal, CalculateEffect(receiverCard.info.Type, sender.info.Weapon));
            }
            else
            {
                receiverCard.info.EffVal -= card.info.EffVal * 2;
            }
        }
    }
}

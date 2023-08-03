using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : CardUse
{
    public override void Use(Character sender, Character receiver)
    {
        base.Use(sender, receiver);

        if(card.info.Level >= 2 && receiver.buffs.Count > 0)
        {
            foreach(var buff in receiver.buffs) 
            {
                if(buff.info.Type == BuffType.DEBUFF)
                {
                    card.info.EffVal += (card.info.EffVal - 3);
                    break;
                }
            }
        }
        receiver.info.Hp -= CalculateDmg(sender.info.AttackDmg, card.info.RandomDice, card.info.EffVal,
        CalculateEffect(card.info.Type, receiver.info.Weapon));

    }

}

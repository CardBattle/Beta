using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snapshot : CardUse
{
    public override void Use(Character sender, Character receiver)
    {
        int count = 2;

        if (card.info.Level >= 2)
        {
            if(card.info.Level >= 3)
                count++;
            foreach (var buff in sender.buffs)
            {
                if (buff.info.Id == 14)
                    count++;
            }
        }

        for (int i = 0; i < count; i++)
        {
            base.Use(sender, receiver);

            receiver.info.Hp -= CalculateDmg(sender.info.AttackDmg, card.info.RandomDice, card.info.EffVal,
            CalculateEffect(card.info.Type, receiver.info.Weapon));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrowshot : CardUse
{
    public override void Use(Character sender, Character receiver)
    {
        base.Use(sender, receiver);

        if (Random.Range(0, 10) <= 2) card.info.EffVal *= 2;

        receiver.info.Hp -= CalculateDmg(sender.info.AttackDmg, card.info.RandomDice, card.info.EffVal,
        CalculateEffect(card.info.Type, receiver.info.Weapon));

        Debug.Log("Arrowshot");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rest : CardUse
{
    public override void Use(Character sender, Character receiver)
    {
        base.Use(sender, receiver);

        sender.info.Hp += CalculateDmg(sender.info.Defense, card.info.RandomDice, card.info.EffVal, 1);

        Debug.Log("Rest");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frostbite : BuffUse
{
    public override void Use(Character character)
    {
        if (character.tag == "Enemy")
        {
            if (BattleManager.Bm.enemyDecision.card != null)
                BattleManager.Bm.enemyDecision.card.info.EffVal -= 2;
        }
        Debug.Log("Frostbite");
    }
}

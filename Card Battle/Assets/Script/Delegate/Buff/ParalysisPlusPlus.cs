using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalysisPlusPlus : BuffUse
{
    public override void Use(Character character)
    {
        if (character.tag == "Enemy")
        {
            if (BattleManager.Bm.enemyDecision.card != null)
                BattleManager.Bm.enemyDecision.card.info.Dice -= 1;
        }
        Debug.Log("ParalysisPlusPlus");
    }
}

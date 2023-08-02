using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcentrationBuffPlusPlus : BuffUse
{
    public override void Use(Character character)
    {
        if (character.tag == "Player")
        {
            if (BattleManager.Bm.playerDecision.card != null)
                BattleManager.Bm.playerDecision.card.info.EffVal *= 4;
        }
        else
        {
            if (BattleManager.Bm.enemyDecision.card != null)
                BattleManager.Bm.enemyDecision.card.info.EffVal *= 4;
        }

        Debug.Log("ConcentrationBuffPlusPlus");
    }
}

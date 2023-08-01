using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveN : BuffUse
{
    public override void Use(Character character)
    {
        if (character.tag == "Player")
        {
            if (BattleManager.Bm.playerDecision != null)
                BattleManager.Bm.playerDecision.card.info.EffVal -= 2;
        }
        else
        {
            if (BattleManager.Bm.enemyDecision != null)
                BattleManager.Bm.enemyDecision.card.info.EffVal -= 2;
        }

        Debug.Log("Defensive-N");
    }
}

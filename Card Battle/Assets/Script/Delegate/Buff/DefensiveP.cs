using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensivePPlus : BuffUse
{
    public override void Use(Character character)
    {
        if(character.tag == "Player")
        {
            if (BattleManager.Bm.playerDecision.card != null && BattleManager.Bm.playerDecision.card.info.Property == PropertyType.ATTACK)
                BattleManager.Bm.playerDecision.card.info.EffVal += 4;
        }
        else
        {
            if (BattleManager.Bm.enemyDecision.card != null && BattleManager.Bm.enemyDecision.card.info.Property == PropertyType.ATTACK)
                BattleManager.Bm.enemyDecision.card.info.EffVal += 4;
        }

        Debug.Log("Defensive-P");
    }
}

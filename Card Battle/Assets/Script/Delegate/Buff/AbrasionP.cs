using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbrasionP : BuffUse
{
    public override void Use(Character character)
    {
        if (character.tag == "Player")
        {
            if (BattleManager.Bm.playerDecision.card != null && BattleManager.Bm.playerDecision.card.info.Type == WeaponType.SWORD)
                BattleManager.Bm.playerDecision.card.info.EffVal += 1;
        }
        else
        {
            if (BattleManager.Bm.enemyDecision.card != null && BattleManager.Bm.enemyDecision.card.info.Type == WeaponType.SWORD)
                BattleManager.Bm.enemyDecision.card.info.EffVal += 1;
        }

        Debug.Log("AbrasionP");
    }
}

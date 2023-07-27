using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defense : CardUse
{
    public override void Use(Character sender, Character receiver)
    {
        base.Use(sender, receiver);

        BattleManager bm = BattleManager.Bm;

        if (card.defenseCheck == true)
        {
            if (bm.enemyDecision.card.info.Dice > bm.playerDecision.card.info.Dice)
                bm.enemyDecision.card.info.EffVal -= 1;
            else bm.enemyDecision.card.info.EffVal -= 2;

            Debug.Log("PlayerDefense");
        }
        else if (card.defenseCheck == false)
        {
            if (bm.enemyDecision.card.info.Dice < bm.playerDecision.card.info.Dice)
                bm.playerDecision.card.info.EffVal -= 1;
            else bm.playerDecision.card.info.EffVal -= 2;

            Debug.Log("EnemyDefense");
        }
    }

}

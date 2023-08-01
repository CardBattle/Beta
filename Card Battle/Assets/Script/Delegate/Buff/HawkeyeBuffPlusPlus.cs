using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HawkeyeBuffPlusPlus : BuffUse
{
    public override void Use(Character character)
    {
        if (character.tag == "Player")
        {
            if (BattleManager.Bm.playerDecision.card != null)
                BattleManager.Bm.playerDecision.card.info.Dice+= 3;
        }
        Debug.Log("HawkeyeBuff");
    }
}

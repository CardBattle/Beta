using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : BuffUse
{
    public override void Use(Character character)
    {
        if(character.tag == "Player")
        {
            BattleManager.Bm.playerDecision.card = null;
        }
        else
        {
            BattleManager.Bm.enemyDecision.card = null;
        }

        Debug.Log("Stun");
    }
}

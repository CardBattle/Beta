using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldStrike : CardUse
{
    public override void Use(Character sender, Character receiver)
    {
        BattleManager bm = BattleManager.Bm;
        if (sender.tag == "Player")
        {
            senderCard = bm.playerDecision.card;
            receiverCard = bm.enemyDecision.card;
        }
        else
        {
            senderCard = bm.enemyDecision.card;
            receiverCard = bm.playerDecision.card;
        }

        sender.GetComponent<SFXVFX>().play += PlaySFX;
        sender.GetComponent<SFXVFX>().play += delegate () { Instantiate(vfx, sender.transform.position, Quaternion.identity); };


        receiverCard.info.EffVal -= 1;

        if (senderCard.info.Dice > receiverCard.info.Dice)
        {
            if (card.info.buffs.Count > 0)
            {
                bool isExist = false;
                foreach (var buff in card.info.buffs)
                {
                    foreach (var exist in receiver.info.buffs)
                    {
                        if (exist.info.Id == buff.info.Id)
                        {
                            isExist = true;
                            exist.info.CurrentTurn += buff.info.Turns;
                            break;
                        }
                    }
                    if (!isExist)
                        receiver.info.buffs.Add(buff);
                }
            }
        }
    }
}

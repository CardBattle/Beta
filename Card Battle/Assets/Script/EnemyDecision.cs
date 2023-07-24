using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class EnemyDecision : MonoBehaviour
{
    // 콜라이더에 부딪힌 카드 정보
    private Card Importedcard;
    // 배틀매니저에서 넘기기위한 카드 정보
    public Card card;
    // 전에 쓴 카드를 삭제하기 위해 있는 변수
    public bool cardCheck;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Importedcard = collision.gameObject.GetComponent<Card>();

        DecisionButtom();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Importedcard = null;
    }

    public void DecisionButtom()
    {
        if (Importedcard == null)
            return;

        card = Importedcard;
        card.defenseCheck = false;
        cardCheck = true;

        BattleManager.Bm.enemyCards.Remove(Importedcard);
        BattleManager.Bm.enemyDeleteCards.Add(Importedcard);

        BattleManager.Bm.CardAlignment(false);

        BattleManager.Bm.enemyDice = card.info.RandomDice;
        Importedcard.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
    }
}


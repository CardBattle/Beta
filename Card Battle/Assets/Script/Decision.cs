using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Decision : MonoBehaviour
{
    // 콜라이더에 부딪힌 카드 정보
    public Card Importedcard;
    // 배틀매니저에서 넘기기위한 카드 정보
    public Card card;
    // 전에 쓴 카드를 삭제하기 위해 있는 변수
    public bool cardCheck;
    // 내가 카드를 냈는지 안냈는지 체크 하는 변수
    public bool cardPresence;

    // 누르면 바로 타이머가 0초가 되기위해 있는 클래스
    [SerializeField]
    private EnemyDecision enemyDecision;

    // 결정 버튼
    [SerializeField]
    private Button button;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        Importedcard = collision.gameObject.GetComponent<Card>();
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
        card.defenseCheck = true;
        cardPresence = true;
        cardCheck = true;

        BattleManager.Bm.playerCards.Remove(Importedcard);
        BattleManager.Bm.playerDeleteCards.Add(Importedcard);


        BattleManager.Bm.CardAlignment(true);

        BattleManager.Bm.playerDice = card.info.RandomDice;
        BattleManager.Bm.state = BattleManager.State.CardDecision;

        if ((BattleManager.Bm.enemyCards.Count == 0) || Importedcard != null
            && enemyDecision.card != null) { BattleManager.Bm.timer = 0; }

        Importedcard.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
    }
}


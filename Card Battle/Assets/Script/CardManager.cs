using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;

    //���� �÷��̾��� ī�� ������
    List<GameObject> playerCardPrefabs;
    List<GameObject> enemyCardPrefabs;
    //Instantiate �� ī�� ���ӿ�����Ʈ ����Ʈ
    public List<GameObject> playerCardObjs;
    public List<GameObject> enemyCardObjs;

    public void Init()
    {
        playerCardPrefabs = BattleManager.Bm.player.cards;
        enemyCardPrefabs = BattleManager.Bm.enemy.cards;

        player = BattleManager.Bm.player.gameObject;
        enemy = BattleManager.Bm.enemy.gameObject;


        Load(playerCardPrefabs, playerCardObjs);
        Load(enemyCardPrefabs, enemyCardObjs);
    }

    public void DeckCardInit()
    {
        foreach (var DeckList in playerCardPrefabs)
        {
            UIManager.Um.deckList.Add(Instantiate(DeckList));
        }
        UIManager.Um.deckList.Sort((x, y) => x.GetComponent<Card>().id.CompareTo(y.GetComponent<Card>().id));
        UIManager.Um.DeckCardTransform();
    }

    void Load(List<GameObject> cardPrefabs, List<GameObject> cardObjs)
    {
        foreach (var cardPrefab in cardPrefabs)
        {
            cardObjs.Add(cardPrefab);
        }
    }
}

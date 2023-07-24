using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;

    //적과 플레이어의 카드 프리팹
    List<GameObject> playerCardPrefabs;
    List<GameObject> enemyCardPrefabs;
    //Instantiate 된 카드 게임오브젝트 리스트
    public List<GameObject> playerCardObjs;
    public List<GameObject> enemyCardObjs;
    //선택된 카드들(드로우된 카드들)
    /*public List<GameObject> playerSelectedCards;
    public List<GameObject> enemySelectedCards;*/

    public void Init()
    {
        playerCardPrefabs = player.GetComponent<Character>().cards;
        enemyCardPrefabs = enemy.GetComponent<Character>().cards;

        Load(playerCardPrefabs, playerCardObjs);
        Load(enemyCardPrefabs, enemyCardObjs);
    }

    void Load(List<GameObject> cardPrefabs, List<GameObject> cardObjs)
    {
        foreach (var cardPrefab in cardPrefabs)
        {
            cardObjs.Add(cardPrefab);        
        }
    }

    /*void Test()
    {
        foreach (var card in playerCards)
            card.info.use(player.GetComponent<Character>(), enemy.GetComponent<Character>());
    }*/
}

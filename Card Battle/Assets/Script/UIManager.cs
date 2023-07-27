using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager Um;  
    public Text result;
    [SerializeField]
    private GameObject battleUI;
    [SerializeField]
    private GameObject menuUI;
    [SerializeField]
    private AudioSource bgm;

    public Sprite[] playerDiceSprite;
    public Sprite[] enemyDiceSprite;

    public GameObject playerDice;
    public GameObject enemyDice;

    public GameObject canvasUI;
    public GameObject defeatUI;
    public GameObject victoryUI;

    public Slider bgmVol;
    public Slider soundEffVol;

    public void Awake()
    {
        if (Um == null)
        {
            Um = this;
        }

        bgmVol.value = PlayerPrefs.GetFloat("BGMVol");
        bgm.volume = PlayerPrefs.GetFloat("BGMVol");
    }
    public void BgmVol()
    {
        bgm.volume = bgmVol.value;
        PlayerPrefs.SetFloat("BGMVol",bgmVol.value);
        PlayerPrefs.Save();
    }
    public void ReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Menu()
    {
        Transform canvasUITransform = canvasUI.transform;
        Time.timeScale = 0;
        battleUI.SetActive(false);
        menuUI.SetActive(true);
        canvasUITransform.transform.GetChild(2).gameObject.SetActive(true);
        canvasUITransform.transform.GetChild(4).gameObject.SetActive(true);
        canvasUITransform.transform.GetChild(5).gameObject.SetActive(true);
        canvasUITransform.transform.GetChild(6).gameObject.SetActive(true);
        canvasUITransform.transform.GetChild(7).gameObject.SetActive(true);


    }
    public void Return()
    {
        Transform canvasUITransform = canvasUI.transform;
        Time.timeScale = 1;
        menuUI.SetActive(false);
        canvasUITransform.transform.GetChild(2).gameObject.SetActive(false);
        canvasUITransform.transform.GetChild(4).gameObject.SetActive(false);
        canvasUITransform.transform.GetChild(5).gameObject.SetActive(false);
        canvasUITransform.transform.GetChild(6).gameObject.SetActive(false);
        canvasUITransform.transform.GetChild(7).gameObject.SetActive(false);
        battleUI.SetActive(true);
        
    }

    public void NextStage()
    {
        BattleManager.Bm.stage += 1;
        PlayerPrefs.SetInt("Stage", BattleManager.Bm.stage);
        PlayerPrefs.Save();
        print(BattleManager.Bm.stage);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void SelectionScene()
    {
        PlayerPrefs.DeleteKey("Stage");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void TitleScene()
    {
        PlayerPrefs.DeleteKey("Stage");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Victory()
    {
        Transform canvasUITransform = canvasUI.transform;
        Transform victoryUITransform = victoryUI.transform;
        Text victoryText = victoryUITransform.GetChild(2).GetComponent<Text>();
        Text turnCountText = victoryUITransform.GetChild(3).GetComponent<Text>();


        if (BattleManager.Bm.stage == 2)
        {
            battleUI.transform.GetChild(3).gameObject.SetActive(false);
            victoryText.text = "모든 스테이지를 클리어 하신걸 축하합니다.";        
            canvasUITransform.GetChild(0).gameObject.SetActive(false);
            canvasUITransform.GetChild(1).gameObject.SetActive(false);
            victoryUITransform.GetChild(4).gameObject.SetActive(false);
            victoryUITransform.GetChild(5).gameObject.SetActive(true);
        }
        else
        {
            canvasUITransform.GetChild(0).gameObject.SetActive(false);
            battleUI.transform.GetChild(3).gameObject.SetActive(false);
            menuUI.SetActive(true);
        }

        turnCountText.text = $"지나간 턴 수: {BattleManager.Bm.turnCount}";
        ClearRank(BattleManager.Bm.turnCount);
        victoryUI.SetActive(true);
    }
    public void Defeat()
    {
        battleUI.transform.GetChild(3).gameObject.SetActive(false);
        menuUI.SetActive(true);
        defeatUI.transform.GetChild(1).
            gameObject.GetComponent<Text>().text = $"지나간 턴 수: {BattleManager.Bm.turnCount}";  
        defeatUI.SetActive(true);
    }

    public void ClearRank(int count)
    {
      
        if (count <= 5)     
        {
            victoryUI.transform.GetChild(1).
            gameObject.GetComponent<Text>().text = "S";     
        }    
        else if (count <= 10)    
        {
            victoryUI.transform.GetChild(1).
            gameObject.GetComponent<Text>().text = "A";
        }
        else if (count <= 15)
        {
            victoryUI.transform.GetChild(1).
            gameObject.GetComponent<Text>().text = "B";
        }
        else if (count <= 20)
        {
            victoryUI.transform.GetChild(1).
            gameObject.GetComponent<Text>().text = "C";
        }
        else if (count <= 25)
        {
            victoryUI.transform.GetChild(1).
            gameObject.GetComponent<Text>().text = "D";
        }
        else
        {
            victoryUI.transform.GetChild(1).
            gameObject.GetComponent<Text>().text = "F";
        }
    }

    public IEnumerator Dice(int dice, bool player)
    {
        if (player)
        {
            SpriteRenderer diceSprite = playerDice.GetComponent<SpriteRenderer>();
            switch (dice)
            {
                case 1:
                    diceSprite.sprite = playerDiceSprite[0];
                    break;
                case 2:
                    diceSprite.sprite = playerDiceSprite[1];
                    break;
                case 3:
                    diceSprite.sprite = playerDiceSprite[2];
                    break;
                case 4:
                    diceSprite.sprite = playerDiceSprite[3];
                    break;
                case 5:
                    diceSprite.sprite = playerDiceSprite[4];
                    break;
                case 6:
                    diceSprite.sprite = playerDiceSprite[5];
                    break;
                default:
                    break;
            }
        }
        else if (!player)
        {
            SpriteRenderer diceSprite = enemyDice.GetComponent<SpriteRenderer>();
            switch (dice)
            {
                case 1:
                    diceSprite.sprite = enemyDiceSprite[0];
                    break;
                case 2:
                    diceSprite.sprite = enemyDiceSprite[1];
                    break;
                case 3:
                    diceSprite.sprite = enemyDiceSprite[2];
                    break;
                case 4:
                    diceSprite.sprite = enemyDiceSprite[3];
                    break;
                case 5:
                    diceSprite.sprite = enemyDiceSprite[4];
                    break;
                case 6:
                    diceSprite.sprite = enemyDiceSprite[5];
                    break;
                default:
                    break;
            }
        }

        playerDice.SetActive(true);
        enemyDice.SetActive(true);
       
        yield return new WaitForSeconds(1f);
       
        playerDice.SetActive(false);
        enemyDice.SetActive(false);
    }
}

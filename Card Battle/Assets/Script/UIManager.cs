using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
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

    //public CardManager uicardManager;

    public Sprite[] playerDiceSprite;
    public Sprite[] enemyDiceSprite;

    public List<GameObject> deckList;
    //  public List<Card> DeckListData;

    public GameObject playerDice;
    public GameObject enemyDice;
    
    public GameObject tutorial;
    public GameObject tutorialImage;

    public GameObject deckListUI;

    
    public GameObject canvasUI;
    public GameObject defeatUI;
    public GameObject victoryUI;

    public Slider bgmVol;
    public Slider sfxVol;
    public Button battleSceneReturnBtn;

    public AudioMixer battleMixer;

    private int row = 7;
    private float xDistance = 2.05f;

    public void Awake()
    {
        if (Um == null)
        {
            Um = this;
        }

        bgmVol.value = PlayerPrefs.GetFloat("BGMVol");
        sfxVol.value = PlayerPrefs.GetFloat("SFXVol");
        
        bgm.volume = bgmVol.value;
        
    }
    public void Start()
    {
        SFXVol();
    }
    public void BGMVol()
    {
        bgm.volume = bgmVol.value;
        PlayerPrefs.SetFloat("BGMVol", bgmVol.value);
        PlayerPrefs.Save();
    }
    public void SFXVol()
    {
        float sound = sfxVol.value;
        
        if (sound == -40f)
            battleMixer.SetFloat("SFX", -80f);       
        else 
            battleMixer.SetFloat("SFX", sound);

        PlayerPrefs.SetFloat("SFXVol", sfxVol.value);
        PlayerPrefs.Save();

    }
    public void ReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //PlayerPrefs.DeleteKey("Tutorial");
    }
    public void Menu()
    {
        if (BattleManager.Bm.state == BattleManager.State.WaitState)
            BattleManager.Bm.state = BattleManager.State.Menu;
        else if (BattleManager.Bm.state == BattleManager.State.SelectCard)
            BattleManager.Bm.state = BattleManager.State.SelectMenu;


        Transform canvasUITransform = canvasUI.transform;
        Time.timeScale = 0;
        battleUI.SetActive(false);
        menuUI.SetActive(true);
        canvasUITransform.transform.GetChild(2).gameObject.SetActive(true);
        canvasUITransform.transform.GetChild(4).gameObject.SetActive(true);
        canvasUITransform.transform.GetChild(5).gameObject.SetActive(true);
        canvasUITransform.transform.GetChild(6).gameObject.SetActive(true);
        canvasUITransform.transform.GetChild(7).gameObject.SetActive(true);
        canvasUITransform.transform.GetChild(8).gameObject.SetActive(true);
    }
    public void DeckCardTransform()
    {
        for (int i = 0; i < deckList.Count; i++)
        {
            deckList[i].GetComponent<Collider2D>().enabled = false;
            deckList[i].GetComponent<Card>().originPRS = DeckCardSort(i, row);
            deckList[i].GetComponent<Card>().MoveTransform(deckList[i].GetComponent<Card>().originPRS, false);
            deckList[i].GetComponent<Card>().CardFront(true);
            deckList[i].GetComponent<Order>().DeckSettingOrder(0);
            deckList[i].SetActive(false);
        }
    }
    public void DeckList()
    {
        if (BattleManager.Bm.state == BattleManager.State.WaitState)
            BattleManager.Bm.state = BattleManager.State.Menu;
        else if (BattleManager.Bm.state == BattleManager.State.SelectCard)
            BattleManager.Bm.state = BattleManager.State.SelectMenu;


        Time.timeScale = 0;
        battleUI.SetActive(false);
        deckListUI.SetActive(true);

        for (int i = 0; i < deckList.Count; i++)
        {
            deckList[i].SetActive(true);
        }
    }
    public void DeckListReturn()
    {
        if (BattleManager.Bm.state == BattleManager.State.Menu)
            BattleManager.Bm.state = BattleManager.State.WaitState;
        else if (BattleManager.Bm.state == BattleManager.State.SelectMenu)
            BattleManager.Bm.state = BattleManager.State.SelectCard;

        Time.timeScale = 1;
        deckListUI.SetActive(false);
        battleUI.SetActive(true);
        for (int i = 0; i < deckList.Count; i++)
        {
            deckList[i].SetActive(false);
        }
    }
    public void Return()
    {
        if (BattleManager.Bm.state == BattleManager.State.Menu)
            BattleManager.Bm.state = BattleManager.State.WaitState;
        else if (BattleManager.Bm.state == BattleManager.State.SelectMenu)
            BattleManager.Bm.state = BattleManager.State.SelectCard;

        Transform canvasUITransform = canvasUI.transform;
        Time.timeScale = 1;
        menuUI.SetActive(false);
        canvasUITransform.transform.GetChild(2).gameObject.SetActive(false);
        canvasUITransform.transform.GetChild(4).gameObject.SetActive(false);
        canvasUITransform.transform.GetChild(5).gameObject.SetActive(false);
        canvasUITransform.transform.GetChild(6).gameObject.SetActive(false);
        canvasUITransform.transform.GetChild(7).gameObject.SetActive(false);
        canvasUITransform.transform.GetChild(8).gameObject.SetActive(false);
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
            battleUI.transform.GetChild(4).gameObject.SetActive(false);

            victoryText.text = $"모든 스테이지를 클리어 하신걸\n 축하합니다.";
            canvasUITransform.GetChild(0).gameObject.SetActive(false);
            canvasUITransform.GetChild(1).gameObject.SetActive(false);
            victoryUITransform.GetChild(4).gameObject.SetActive(false);
            victoryUITransform.GetChild(5).gameObject.SetActive(true);
        }
        else
        {
            canvasUITransform.GetChild(0).gameObject.SetActive(false);
            battleUI.transform.GetChild(3).gameObject.SetActive(false);
            battleUI.transform.GetChild(4).gameObject.SetActive(false);
            menuUI.SetActive(true);
        }

        turnCountText.text = $"지나간 턴 수: {BattleManager.Bm.turnCount}";
        ClearRank(BattleManager.Bm.turnCount);
        victoryUI.SetActive(true);
    }
    public void Defeat()
    {
        battleUI.transform.GetChild(3).gameObject.SetActive(false);
        battleUI.transform.GetChild(4).gameObject.SetActive(false);
        menuUI.SetActive(true);
        defeatUI.transform.GetChild(1).
            gameObject.GetComponent<Text>().text = $"지나간 턴 수: {BattleManager.Bm.turnCount}";
        defeatUI.SetActive(true);
    }

    public void Tutorial()
    {
        battleUI.SetActive(false);
        tutorial.SetActive(true);
    }

    public void YesTutorial()
    {
        tutorial.SetActive(false);
        tutorialImage.SetActive(true);
    }

    public void ReTutorial()
    {
        Transform canvasUITransform = canvasUI.transform;      
        menuUI.SetActive(false);
        canvasUITransform.transform.GetChild(2).gameObject.SetActive(false);
        canvasUITransform.transform.GetChild(4).gameObject.SetActive(false);
        canvasUITransform.transform.GetChild(5).gameObject.SetActive(false);
        canvasUITransform.transform.GetChild(6).gameObject.SetActive(false);
        canvasUITransform.transform.GetChild(7).gameObject.SetActive(false);
        canvasUITransform.transform.GetChild(8).gameObject.SetActive(false);



        battleUI.SetActive(false);
        tutorialImage.SetActive(true);
    }

    public void NoTutorial()
    {
        tutorial.SetActive(false);
        battleUI.SetActive(true);
        PlayerPrefs.SetInt("Tutorial", 1);
        PlayerPrefs.Save();

        Time.timeScale = 1;
    }

    public void TutorialBattleSceneReturn()
    {
        tutorialImage.SetActive(false);
        battleUI.SetActive(true);
        battleSceneReturnBtn.gameObject.SetActive(false);

        PlayerPrefs.SetInt("Tutorial", 1);
        PlayerPrefs.Save();
        Time.timeScale = 1;
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

    public IEnumerator ShufflingDice()
    {
        playerDice.SetActive(true);
        enemyDice.SetActive(true);

        SpriteRenderer playerDiceSprite = playerDice.GetComponent<SpriteRenderer>();
        SpriteRenderer enemyDiceSprite = enemyDice.GetComponent<SpriteRenderer>();

        for (int i = 0; i < 25; i++)
        {
            playerDiceSprite.sprite = this.playerDiceSprite[Random.Range(0, 6)];
            enemyDiceSprite.sprite = this.enemyDiceSprite[Random.Range(0, 6)];
            yield return new WaitForSeconds(0.1f);
        }

        BattleManager.Bm.DiceResult();
    }

    public IEnumerator Dice(int dice, bool player)
    {

        if (player)
        {
            print($"플레이어 주사위 {dice}");
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
            print($"적 주사위 {dice}");
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

            yield return new WaitForSeconds(1.5f);

            BattleManager.Bm.BattleTurn();

            playerDice.SetActive(false);
            enemyDice.SetActive(false);
        }
    }

    PRS DeckCardSort(int cardIndex, int row)
    {
        PRS result;
        if (cardIndex < row)
        {
            if (cardIndex == 0)
            {
                result = new PRS(new Vector2(-6.2f, 3.2f), Utlis.Qi, Vector3.one);
            }
            else
            {
                result = new PRS(new Vector2(deckList[cardIndex - 1]
                    .transform.position.x + xDistance, 3.2f), Utlis.Qi, Vector3.one);
            }
        }
        else if (cardIndex < (row * 2))
        {
            if (cardIndex == 7)
            {
                result = new PRS(new Vector2(-6.2f, 0.5f), Utlis.Qi, Vector3.one);
            }
            else
            {
                result = new PRS(new Vector2(deckList[cardIndex - 1]
                    .transform.position.x + xDistance, 0.5f), Utlis.Qi, Vector3.one);
            }
        }
        else
        {
            if (cardIndex == 14)
            {
                result = new PRS(new Vector2(-6.2f, -2.2f), Utlis.Qi, Vector3.one);
            }
            else
            {
                result = new PRS(new Vector2(deckList[cardIndex - 1]
                    .transform.position.x + xDistance, -2.2f), Utlis.Qi, Vector3.one);
            }
        }
        return result;
    }
}

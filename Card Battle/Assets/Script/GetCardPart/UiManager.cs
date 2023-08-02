using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class UiManager : MonoBehaviour
{
    enum StudyState
    {
        first,
        second,
        third,
    }
    enum CardState
    {
        get,
        upgrade
    }
    enum UpgradeState
    {
        firstSword,
        secondSword,
        thirdSword,
        firstBow,
        secondBow,
        thirdBow,
        firstMagic,
        secondMagic,
        thirdMagic,
    }
    private StudyState studyState;
    private CardState cardState;
    private UpgradeState upgradeState;

    //스터디씬 좌표 0,0
    //검술씬 0,10
    //궁술씬 20,10
    //마술씬 20,00
    //스타트씬 -20, -10
    public Image[] loadViewImage;
    public Image[] selectStudyImage;
    public Image[] upgradeBoxImage;
    public Sprite[] cardSprite;

    public Image studyViewImage;

    public Sprite[] studyViewResources;
    public Sprite[] selectStudyResources;
    

    public Image[] cardResources;
    public Image[] cardPlusResources;



    public Camera mainCamera;

    public Text[] saveInfo;

    public Text[] studyInfo;
    public Text[] studingResult;
    public Text[] studyResult;
    public Text cardBordeName;
    public Text messageText;
    public Text uiText;

    
    public GameObject startUi;
    public GameObject dataUi;
    public GameObject studyUi;
    public GameObject studingUi;
    public GameObject statUi;
    public GameObject selectCardUi;
    public GameObject startButton;
    public GameObject message;
    public GameObject cardUi;
    public GameObject getCardUi;
    public GameObject upgradeCardUi;
    public GameObject fstCard1;
    public GameObject fstCard2;
    public GameObject endStudy;
    public GameObject uiMessage;
    public GameObject upgradeBox;
    public Transform player;

    private List<int> selectStudy;
    public List<Image> getCardList;
    private List<int> cardList;
    private List<GameObject> cardListData;

    private int playerHp;
    private int playerAttack;
    private int playerDefense;
    private int DefaultPlayerHp;
    private int DefaultPlayerAttack;
    private int DefaultPlayerDefense;
    private int studyViewNum;
    private int order;
    private int saveSlot;
    private int searchCount;


    private CharacterData data;
    private DefaultCharacterData saveData;


    public void Awake()
    {
        DataInit();
        SaveSlot();
        studyReset();
    }
    public void DataInit()
    {
        GameObject dataObject = GameObject.FindGameObjectWithTag("PlayerData");
        Debug.Log(dataObject);
        data = dataObject.GetComponent<CharacterData>();
    }

    public void SaveSlot()
    {
        string directoryPath = Path.Combine(Application.persistentDataPath);//검사할 경로
        int binaryFileCount = CountBinaryFilesInDirectory(directoryPath);
        Debug.Log("바이너리로 저장된 파일 개수: " + binaryFileCount);
        saveSlot = binaryFileCount;
    }
    int CountBinaryFilesInDirectory(string path)
    {
        int binaryFileCount = 0;

        if (Directory.Exists(path))
        {
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                if (IsBinaryFile(file))
                {
                    binaryFileCount++;
                }
            }
        }
        else
        {
            Debug.LogWarning("디렉토리가 존재하지 않습니다: " + path);
        }

        return binaryFileCount;
    }

    bool IsBinaryFile(string filePath)
    {
        // 파일 확장자가 바이너리 파일에 해당하는지 여부를 체크합니다.
        // 예를 들어, 여기에서는 ".dat" 파일을 바이너리 파일로 간주합니다.
        string extension = Path.GetExtension(filePath).ToLower();
        return extension == ".dat"; // 원하는 확장자로 변경해주세요.
    }


    public void studyReset()
    {
        studyViewNum = 0;
        studyViewImage.sprite = studyViewResources[studyViewNum];
        selectStudy = new List<int>();
        cardListData = new List<GameObject>();
        player.position = new Vector3(-0.86f, 0.56f, 0.0f);
        mainCamera.transform.position = new Vector3(-20.0f, 10.0f, -100.0f);
        startUi.SetActive(true);
        dataUi.SetActive(false);
        studyUi.SetActive(false);
        studingUi.SetActive(false);
        selectCardUi.SetActive(false);
        statUi.SetActive(false);
        startButton.SetActive(false); ;
        message.SetActive(false);
        cardUi.SetActive(false);
        getCardUi.SetActive(false);
        upgradeCardUi.SetActive(false);
        endStudy.SetActive(false);
        studingResult[0].text = "1회차";
        studingResult[1].text = null;
        studingResult[2].text = null;
        studyState = StudyState.first;
        cardList = new List<int>();
        order = 0;
        searchCount = 0;
    }
    public void startData()
    {

        cardList = data.chrCardnum;
        playerHp = data.chrMaxHp;
        playerAttack = data.chrAttackDmg;
        playerDefense = data.chrDefense;
        DefaultPlayerHp = data.chrMaxHp;
        DefaultPlayerAttack = data.chrAttackDmg;
        DefaultPlayerDefense = data.chrDefense;
        addNomalCard();
    }
    public void addNomalCard()
    {
        if (cardList.Count == 0)
        {
            for (int i = 0; i < 6; i++)
            {
                cardList.Add(i);
                Debug.Log(cardList[0]);
            }
        }
    }
    public void StartBtn()
    {
        startUi.SetActive(false);
        dataUi.SetActive(true);
        mainCamera.transform.position = new Vector3(-20.0f, 0.0f, -100.0f);

        if (saveSlot == 0)
        {
            LoadCharacterData0();
            LoadBtn0();
        }
        else if (saveSlot == 1)
        {
            LoadCharacterData0();
            loadViewImage[0].sprite = data.img;
            saveInfo[0].text = data.chrCardnum.Count.ToString();
            loadViewImage[1].sprite = null;
            saveInfo[1].text = "데이터가 없습니다";
            loadViewImage[2].sprite = null;
            saveInfo[2].text = "데이터가 없습니다";

        }
        else if (saveSlot == 2)
        {
            LoadCharacterData0();
            loadViewImage[0].sprite = data.img;
            saveInfo[0].text = data.chrCardnum.Count.ToString(); LoadCharacterData1();
            loadViewImage[1].sprite = data.img;
            saveInfo[1].text = data.chrCardnum.Count.ToString();
            loadViewImage[2].sprite = null;
            saveInfo[2].text = "데이터가 없습니다";
        }
        else if (saveSlot == 3)
        {
            LoadCharacterData0();
            loadViewImage[0].sprite = data.img;
            saveInfo[0].text = data.chrCardnum[8].ToString();
            LoadCharacterData1();
            loadViewImage[1].sprite = data.img;
            saveInfo[1].text = data.chrCardnum.Count.ToString(); LoadCharacterData2();
            loadViewImage[2].sprite = data.img;
            saveInfo[2].text = data.chrCardnum.Count.ToString();
        }
    }

    public void LoadBtn0()
    {
        dataUi.SetActive(false);
        studyUi.SetActive(true);
        LoadCharacterData0();
        mainCamera.transform.position = new Vector3(0.0f, 0.0f, -100.0f);
        saveSlot = 0;
        startData();

    }
    public void LoadBtn1()
    {
        if (saveSlot == 1)
        {
            data.DataInit(0, "test", 1, 1, 1, 1
                        , WeaponType.SWORD, 0, cardList);
            saveSlot = 1;
            dataUi.SetActive(false);
            studyUi.SetActive(true);
            mainCamera.transform.position = new Vector3(0.0f, 0.0f, -100.0f);
        }
        else if (saveSlot > 1)
        {
            dataUi.SetActive(false);
            studyUi.SetActive(true);
            LoadCharacterData1();
            mainCamera.transform.position = new Vector3(0.0f, 0.0f, -100.0f);
            saveSlot = 1;
        }
        startData();
    }
    public void LoadBtn2()
    {
        if (saveSlot == 2)
        {
            data.DataInit(0, "test", 1, 1, 1, 1
                        , WeaponType.SWORD, 0, cardList);
            saveSlot = 2;
            dataUi.SetActive(false);
            studyUi.SetActive(true);
            mainCamera.transform.position = new Vector3(0.0f, 0.0f, -100.0f);
        }
        else if (saveSlot == 3)
        {
            dataUi.SetActive(false);
            studyUi.SetActive(true);
            LoadCharacterData2();
            mainCamera.transform.position = new Vector3(0.0f, 0.0f, -100.0f);
            saveSlot = 2;
        }
        startData();
    }


    public void ChangeStudyImageBtn()
    {
        if (studyViewNum > 1)
        {
            studyViewNum = 0;
            studyViewImage.sprite = studyViewResources[studyViewNum];
            studyInfo[0].text = "SWORD";
            studyInfo[1].text = "공격과 방어의 밸런스가 특징이다.";
            studyInfo[2].text = "모든 스텟 ↑";

        }
        else
        {
            studyViewNum++;
            studyViewImage.sprite = studyViewResources[studyViewNum];
            if (studyViewNum == 0)
            {
                studyInfo[0].text = "SWORD";
                studyInfo[1].text = "공격과 방어의 밸런스가 특징이다.";
                studyInfo[2].text = "모든 스텟 ↑";
            }
            else if (studyViewNum == 1)
            {
                studyInfo[0].text = "BOW";
                studyInfo[1].text = "강력한 공격에 특화되어있다.";
                studyInfo[2].text = "체력↑ 공격↑";


            }
            else if (studyViewNum == 2)
            {
                studyInfo[0].text = "MAGIC";
                studyInfo[1].text = "다섯 가지의 기술을 배울 수 있다.";
                studyInfo[2].text = "공격↑";

            }
        }
    }
    public void SlectStudyBtn()
    {
        if (selectStudyImage[0].sprite == null)
        {
            selectStudyImage[0].sprite = selectStudyResources[studyViewNum];
            selectStudyImage[0].color = new Color(255, 255, 255, 255);
            selectStudy.Add(studyViewNum);
        }
        else if (selectStudyImage[1].sprite == null)
        {
            selectStudyImage[1].sprite = selectStudyResources[studyViewNum];
            selectStudyImage[1].color = new Color(255, 255, 255, 255);
            selectStudy.Add(studyViewNum);
        }
        else if (selectStudyImage[2].sprite == null)
        {
            selectStudyImage[2].sprite = selectStudyResources[studyViewNum];
            selectStudyImage[2].color = new Color(255, 255, 255, 255);
            selectStudy.Add(studyViewNum);
        }
    }

    public void ResetStudyBtn()
    {
        selectStudyImage[0].sprite = null;
        selectStudyImage[0].color = new Color(255, 255, 255, 0);
        selectStudyImage[1].sprite = null;
        selectStudyImage[1].color = new Color(255, 255, 255, 0);
        selectStudyImage[2].sprite = null;
        selectStudyImage[2].color = new Color(255, 255, 255, 0);
        selectStudy.Clear();
    }
    public void NextSceneBtn()
    {
        if (selectStudy.Count == 3)
        {
            if (selectStudy[0] == 0)
            {
                studyUi.SetActive(false);
                studingUi.SetActive(true);
                statUi.SetActive(true);
                startButton.SetActive(true); ;

                mainCamera.transform.position = new Vector3(0.0f, 10.0f, -100.0f);
                player.position = new Vector3(4.0f, 12.0f, 0.0f);
            }
            else if (selectStudy[0] == 1)
            {
                studyUi.SetActive(false);
                studingUi.SetActive(true);
                statUi.SetActive(true);
                startButton.SetActive(true); ;
                mainCamera.transform.position = new Vector3(0.0f, 10.0f, -100.0f);
                player.position = new Vector3(4.0f, 12.0f, 0.0f);
            }
            else if (selectStudy[0] == 2)
            {
                studyUi.SetActive(false);
                studingUi.SetActive(true);
                statUi.SetActive(true);
                startButton.SetActive(true); ;
                mainCamera.transform.position = new Vector3(0.0f, 10.0f, -100.0f);
                player.position = new Vector3(4.0f, 12.0f, 0.0f);
            }
        }
    }
    public void StudyStartBtn()
    {
        startButton.SetActive(false);
        if (studyState == StudyState.first)
        {
            StartStudy(0);
        }
        else if (studyState == StudyState.second)
        {
            StartStudy(1);
        }
        else if (studyState == StudyState.third)
        {
            StartStudy(2);
        }
    }

    private void StartStudy(int num)
    {
        if (selectStudy[num] == 0)
        {
            StartCoroutine(SwordStudy(5));
            StopCoroutine(SwordStudy(5));
        }
        else if (selectStudy[num] == 1)
        {
            StartCoroutine(BowStudy(5));
            StopCoroutine(BowStudy(5));
        }
        else if (selectStudy[num] == 2)
        {
            StartCoroutine(MagicStudy(5));
            StopCoroutine(MagicStudy(5));
        }

    }
    public void GetStudyResult()
    {
        statUi.SetActive(false);
        selectCardUi.SetActive(true);
        studyResult[0].text = "결과";
        studyResult[1].text = "체력" + (playerHp - DefaultPlayerHp).ToString() + "↑ "
            + "공격" + (playerAttack - DefaultPlayerAttack).ToString() + "↑ "
            + "방어" + (playerDefense - DefaultPlayerDefense).ToString() + "↑";
    }

    IEnumerator SwordStudy(int num)
    {
        for (int i = 0; i < num; i++)
        {
            SwordStudyResult();
            yield return new WaitForSeconds(1.0f);
        }
        GetStudyResult();
    }

    public void SwordStudyResult()
    {

        studingResult[0].text = "SWORD";
        int randomNum = Random.Range(0, 7);
        if (randomNum == 0)
        {
            playerHp++;
            studingResult[1].text = "체력↑";
            studingResult[2].text = "체력이 붙었다";
        }
        else if (randomNum == 1)
        {
            playerAttack++;
            studingResult[1].text = "공격↑";
            studingResult[2].text = "공격을 잘하게 된 것 같다.";
        }
        else if (randomNum == 2)
        {
            playerDefense++;
            studingResult[1].text = "방어↑";
            studingResult[2].text = "방어를 잘하게 된 것 같다.";
        }
        else if (randomNum == 3)
        {
            playerHp++;
            playerAttack++;
            studingResult[1].text = "체력↑ 공격↑";
            studingResult[2].text = "공격을 열심히 연습했다.";

        }
        else if (randomNum == 4)
        {
            playerHp++;
            playerDefense++;
            studingResult[1].text = "체력↑ 방어↑";
            studingResult[2].text = "방어를 열심히 연습했다.";

        }
        else if (randomNum == 5)
        {
            playerDefense++;
            playerAttack++;
            playerHp++;
            studingResult[1].text = "체력↑ 공격↑ 방어↑";
            studingResult[2].text = "완벽하게 해냈다.";
        }
        else if (randomNum == 6)
        {
            studingResult[1].text = " - ";
            studingResult[2].text = "힘들어서 그냥 쉬었다.";
        }
    }
    IEnumerator BowStudy(int num)
    {
        cardResources[0].sprite = cardSprite[3];//활1,2,3
        cardResources[1].sprite = cardSprite[4];
        cardResources[2].sprite = cardSprite[5];
        for (int i = 0; i < num; i++)
        {
            BowStudyResult();
            yield return new WaitForSeconds(1.0f);
        }
        GetStudyResult();
    }
    public void BowStudyResult()
    {
        studingResult[0].text = "BOW";
        int randomNum = Random.Range(0, 7);
        if (randomNum == 0)
        {
            playerHp++;
            studingResult[1].text = "체력↑";
            studingResult[2].text = "체력이 붙었다";
        }
        else if (randomNum == 1 || randomNum == 4)
        {
            playerAttack++;
            studingResult[1].text = "공격↑";
            studingResult[2].text = "공격을 잘하게 된 것 같다.";
        }
        else if (randomNum == 2 || randomNum == 5 || randomNum == 6)
        {
            studingResult[1].text = " - ";
            studingResult[2].text = "힘들어서 그냥 쉬었다.";
        }
        else if (randomNum == 3)
        {
            playerHp++;
            playerAttack++;
            studingResult[1].text = "체력↑ 공격↑";
            studingResult[2].text = "공격을 열심히 연습했다.";
        }
    }
    IEnumerator MagicStudy(int num)
    {
        cardResources[0].sprite = cardSprite[6];//마법1,2,3
        cardResources[1].sprite = cardSprite[7];
        cardResources[2].sprite = cardSprite[8];
        cardPlusResources[0].sprite = cardSprite[8];
        cardPlusResources[1].sprite = cardSprite[8];
        for (int i = 0; i < num; i++)
        {
            MagicStudyResult();
            yield return new WaitForSeconds(1.0f);
        }
        GetStudyResult();
    }
    public void MagicStudyResult()
    {
        studingResult[0].text = "Magic";
        int randomNum = Random.Range(0, 7);
        if (randomNum == 1 || randomNum == 4 || randomNum == 0)
        {
            playerAttack++;
            studingResult[1].text = "공격↑";
            studingResult[2].text = "그럭저럭 배운것 같다.";
        }
        else if (randomNum == 2 || randomNum == 5 || randomNum == 6)
        {
            studingResult[1].text = " - ";
            studingResult[2].text = "힘들어서 그냥 쉬었다.";
        }
        else if (randomNum == 3)
        {
            playerAttack++;
            playerAttack++;
            studingResult[1].text = "공격↑↑";
            studingResult[2].text = "완벽하게 이해했다.";
        }
    }

    public void FirstCardBtn()
    {
        Debug.Log(cardState);

        if (cardState == CardState.get)
        {
            if (studyState == StudyState.first)
            {
                GetFirstCard(0);
            }
            else if (studyState == StudyState.second)
            {
                GetFirstCard(1);
            }
            else if (studyState == StudyState.third)
            {
                GetFirstCard(2);
            }
        }
        else
        {
            if (studyState == StudyState.first)
            {
                if (selectStudy[0] == 0)
                {
                    upgradeState = UpgradeState.firstSword;
                    UpgradeFirstCard(0);
                }
                else if (selectStudy[0] == 1)
                {
                    upgradeState = UpgradeState.firstBow;
                    UpgradeFirstCard(1);
                }
                else if (selectStudy[0] == 2)
                {
                    upgradeState = UpgradeState.firstMagic;
                    UpgradeFirstCard(2);
                }
            }
            else if (studyState == StudyState.second)
            {
                if (selectStudy[1] == 0)
                {
                    upgradeState = UpgradeState.firstSword;
                    UpgradeFirstCard(0);
                }
                else if (selectStudy[1] == 1)
                {
                    upgradeState = UpgradeState.firstBow;
                    UpgradeFirstCard(1);
                }
                else if (selectStudy[1] == 2)
                {
                    upgradeState = UpgradeState.firstMagic;
                    UpgradeFirstCard(2);
                }
            }
            else if (studyState == StudyState.third)
            {
                if (selectStudy[2] == 0)
                {
                    upgradeState = UpgradeState.firstSword;
                    UpgradeFirstCard(0);
                }
                else if (selectStudy[2] == 1)
                {
                    upgradeState = UpgradeState.firstBow;
                    UpgradeFirstCard(1);
                }
                else if (selectStudy[2] == 2)
                {
                    upgradeState = UpgradeState.firstMagic;
                    UpgradeFirstCard(2);
                }
            }
        }
    }
    public void UpgradeFirstCard(int num)
    {
        if (selectStudy[num] == 0)
        {
            upgradeBox.SetActive(true);
            upgradeBoxImage[0].sprite = cardSprite[4];
            upgradeBoxImage[1].sprite = cardSprite[11];
        }
        else if (selectStudy[num] == 1)
        {
            upgradeBox.SetActive(true);
            upgradeBoxImage[0].sprite = cardSprite[16];
            upgradeBoxImage[1].sprite = cardSprite[19];
        }
        else if (selectStudy[num] == 2)
        {
            upgradeBox.SetActive(true);
            upgradeBoxImage[0].sprite = cardSprite[26];
            upgradeBoxImage[1].sprite = cardSprite[31];
        }
    }
    public void UpgradeSecondCard(int num)
    {
        if (selectStudy[num] == 0)
        {
            upgradeBox.SetActive(true);
            upgradeBoxImage[0].sprite = cardSprite[9];
            upgradeBoxImage[1].sprite = cardSprite[12];
        }
        else if (selectStudy[num] == 1)
        {
            upgradeBox.SetActive(true);
            upgradeBoxImage[0].sprite = cardSprite[17];
            upgradeBoxImage[1].sprite = cardSprite[20];
        }
        else if (selectStudy[num] == 2)
        {
            upgradeBox.SetActive(true);
            upgradeBoxImage[0].sprite = cardSprite[27];
            upgradeBoxImage[1].sprite = cardSprite[32];
        }
    }

    public void UpgradeThirdCard(int num)
    {
        if (selectStudy[num] == 0)
        {
            upgradeBox.SetActive(true);
            upgradeBoxImage[0].sprite = cardSprite[10];
            upgradeBoxImage[1].sprite = cardSprite[13];
        }
        else if (selectStudy[num] == 1)
        {
            upgradeBox.SetActive(true);
            upgradeBoxImage[0].sprite = cardSprite[18];
            upgradeBoxImage[1].sprite = cardSprite[21];
        }
        else if (selectStudy[num] == 2)
        {
            upgradeBox.SetActive(true);
            upgradeBoxImage[0].sprite = cardSprite[28];
            upgradeBoxImage[1].sprite = cardSprite[33];
        }
    }
    public void UpgradeFirstCardBtn(int num)
    {
        Debug.Log(upgradeState);
        if (upgradeState == UpgradeState.firstSword)
        {
            foreach (int a in cardList)
            {
                if (a == 3)
                {
                    cardList.Remove(3);
                    cardList.Add(8);
                    message.SetActive(true);
                    messageText.text = "Cut+카드를 얻었다.";
                    upgradeBox.SetActive(false);

                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Cut카드가 없습니다.";
                        upgradeBox.SetActive(false);
                        break;
                    }
                }
            }
            searchCount = 0;
        }
        else if (upgradeState == UpgradeState.firstBow)
        {
            Debug.Log("들어온거확인");
            foreach (int a in cardList)
            {
                if (a == 4)
                {
                    cardList.Remove(4);
                    cardList.Add(16);
                    message.SetActive(true);
                    messageText.text = "Arrowshot+카드를 얻었다.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Arrowshot카드가 없습니다.";
                        upgradeBox.SetActive(false);
                        break;
                    }
                }
            }
            searchCount = 0;
        }
        else if (upgradeState == UpgradeState.firstMagic)
        {
            foreach (int a in cardList)
            {
                if (a == 5)
                {
                    cardList.Remove(5);
                    cardList.Add(26);
                    message.SetActive(true);
                    messageText.text = "Fireball+카드를 얻었다.";
                    upgradeBox.SetActive(false);
                    break;
                }

                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Fireball카드가 없습니다.";
                        upgradeBox.SetActive(false);
                        break;
                    }
                }
            }
            searchCount = 0;
        }
        else if (upgradeState == UpgradeState.secondSword)
        {
            foreach (int a in cardList)
            {
                if (a == 6)
                {
                    cardList.Remove(6);
                    cardList.Add(9);
                    message.SetActive(true);
                    messageText.text = "Guard+카드를 얻었다.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Guard카드가 없습니다.";
                        upgradeBox.SetActive(false);
                        break;
                    }
                }
            }
            searchCount = 0;

        }
        else if (upgradeState == UpgradeState.secondBow)
        {
            foreach (int a in cardList)
            {
                if (a == 14)
                {
                    cardList.Remove(14);
                    cardList.Add(17);
                    message.SetActive(true);
                    messageText.text = "Hawkeye+카드를 얻었다.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Hawkeye카드가 없습니다.";
                        upgradeBox.SetActive(false);
                        break;
                    }
                }
            }
            searchCount = 0;
        }
        else if (upgradeState == UpgradeState.secondMagic)
        {
            foreach (int a in cardList)
            {
                if (a == 22)
                {
                    cardList.Remove(22);
                    cardList.Add(27);
                    message.SetActive(true);
                    messageText.text = "Electric shock+카드를 얻었다.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Electric shock카드가 없습니다.";
                        upgradeBox.SetActive(false);
                        break;
                    }
                }
            }
            searchCount = 0;

        }
        else if (upgradeState == UpgradeState.thirdSword)
        {
            foreach (int a in cardList)
            {
                if (a == 7)
                {
                    cardList.Remove(7);
                    cardList.Add(10);
                    message.SetActive(true);
                    messageText.text = "ShildStrike+카드를 얻었다.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "ShildStrike카드가 없습니다.";
                        upgradeBox.SetActive(false);
                        break;
                    }
                }
            }
            searchCount = 0;

        }

        else if (upgradeState == UpgradeState.thirdBow)
        {
            foreach (int a in cardList)
            {
                if (a == 15)
                {
                    cardList.Remove(15);
                    cardList.Add(18);
                    message.SetActive(true);
                    messageText.text = "Snapshot+카드를 얻었다.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Snapshot카드가 없습니다.";
                        upgradeBox.SetActive(false);
                        break;
                    }
                }
            }
            searchCount=0;
        }
        else if (upgradeState == UpgradeState.thirdMagic)
        {
            foreach (int a in cardList)
            {
                if (a == 23)
                {
                    cardList.Remove(23);
                    cardList.Add(28);
                    message.SetActive(true);
                    messageText.text = "Ice bolt+카드를 얻었다.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Ice bolt카드가 없습니다.";
                        upgradeBox.SetActive(false);
                        break;
                    }
                }
            }
            searchCount=0;
        }
    }
    public void UpgradeSecondCardBtn(int num)
    {
        if (upgradeState == UpgradeState.firstSword)
        {
            foreach (int a in cardList)
            {
                if (a == 8)
                {
                    cardList.Remove(8);
                    cardList.Add(11);
                    message.SetActive(true);
                    messageText.text = "Cut++카드를 얻었다.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Cut+카드가 없습니다.";
                        upgradeBox.SetActive(false);
                    }
                }
            }
            searchCount=0;
        }
        else if (upgradeState == UpgradeState.firstBow)
        {
            foreach (int a in cardList)
            {
                if (a == 16)
                {
                    cardList.Remove(16);
                    cardList.Add(19);
                    message.SetActive(true);
                    messageText.text = "Arrowshot++카드를 얻었다.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Arrowshot+카드가 없습니다.";
                        upgradeBox.SetActive(false);
                    }
                }
            }
            searchCount = 0;

        }
        else if (upgradeState == UpgradeState.firstMagic)
        {
            foreach (int a in cardList)
            {
                if (a == 26)
                {
                    cardList.Remove(26);
                    cardList.Add(31);
                    message.SetActive(true);
                    messageText.text = "Fireball++카드를 얻었다.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Fireball+카드가 없습니다.";
                        upgradeBox.SetActive(false);
                    }
                }
            }
            searchCount = 0;
        }
        else if (upgradeState == UpgradeState.secondSword)
        {
            foreach (int a in cardList)
            {
                if (a == 9)
                {
                    cardList.Remove(9);
                    cardList.Add(12);
                    message.SetActive(true);
                    messageText.text = "Guard++카드를 얻었다.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Guard+카드가 없습니다.";
                        upgradeBox.SetActive(false);
                    }
                }
            }
            searchCount = 0;

        }
        else if (upgradeState == UpgradeState.secondBow)
        {
            foreach (int a in cardList)
            {
                if (a == 17)
                {
                    cardList.Remove(17);
                    cardList.Add(20);
                    message.SetActive(true);
                    messageText.text = "Hawkeye++카드를 얻었다.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Hawkeye+카드가 없습니다.";
                        upgradeBox.SetActive(false);
                    }
                }
            }
            searchCount = 0;

        }
        else if (upgradeState == UpgradeState.secondMagic)
        {
            foreach (int a in cardList)
            {
                if (a == 27)
                {
                    cardList.Remove(27);
                    cardList.Add(32);
                    message.SetActive(true);
                    messageText.text = "Electric shock++카드를 얻었다.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Electric shock+카드가 없습니다.";
                        upgradeBox.SetActive(false);
                    }
                }
            }
            searchCount = 0;

        }
        else if (upgradeState == UpgradeState.thirdSword)
        {
            foreach (int a in cardList)
            {
                if (a == 10)
                {
                    cardList.Remove(10);
                    cardList.Add(13);
                    message.SetActive(true);
                    messageText.text = "ShildStrike++카드를 얻었다.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "ShildStrike+카드가 없습니다.";
                        upgradeBox.SetActive(false);
                    }
                }
            }
            searchCount=0;

        }
        else if (upgradeState == UpgradeState.thirdBow)
        {
            foreach (int a in cardList)
            {
                if (a == 18)
                {
                    cardList.Remove(18);
                    cardList.Add(21);
                    message.SetActive(true);
                    messageText.text = "Snapshot++카드를 얻었다.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Snapshot+카드가 없습니다.";
                        upgradeBox.SetActive(false);
                    }
                }
            }
            searchCount++;

        }
        else if (upgradeState == UpgradeState.thirdMagic)
        {
            foreach (int a in cardList)
            {
                if (a == 28)
                {
                    cardList.Remove(28);
                    cardList.Add(33);
                    message.SetActive(true);
                    messageText.text = "Ice bolt++카드를 얻었다.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Ice bolt+카드가 없습니다.";
                        upgradeBox.SetActive(false);
                    }
                }
            }
            searchCount++;

        }
    }

    public void GetFirstCard(int num)
    {
        if (selectStudy[num] == 0)
        {
            cardList.Add(3);
            message.SetActive(true);
            messageText.text = "Cut카드를 얻었다.";
        }
        else if (selectStudy[num] == 1)
        {
            cardList.Add(4);
            message.SetActive(true);
            messageText.text = "Arrowshot카드를 얻었다.";
        }
        else if (selectStudy[num] == 2)
        {
            cardList.Add(5);
            message.SetActive(true);
            messageText.text = "Firdball카드를 얻었다.";
        }
    }

    public void SecondCardBtn()
    {
        if (cardState == CardState.get)
        {
            if (studyState == StudyState.first)
            {
                GetSecondCard(0);
            }
            else if (studyState == StudyState.second)
            {
                GetSecondCard(1);
            }
            else if (studyState == StudyState.third)
            {
                GetSecondCard(2);
            }
        }
        else
        {
            if (studyState == StudyState.first)
            {
                if (selectStudy[0] == 0)
                {
                    upgradeState = UpgradeState.secondSword;
                    UpgradeFirstCard(0);
                }
                else if (selectStudy[0] == 1)
                {
                    upgradeState = UpgradeState.secondBow;
                    UpgradeFirstCard(1);
                }
                else if (selectStudy[0] == 2)
                {
                    upgradeState = UpgradeState.secondMagic;
                    UpgradeFirstCard(2);
                }
            }
            else if (studyState == StudyState.second)
            {
                if (selectStudy[1] == 0)
                {
                    upgradeState = UpgradeState.secondSword;
                    UpgradeFirstCard(0);
                }
                else if (selectStudy[1] == 1)
                {
                    upgradeState = UpgradeState.secondBow;
                    UpgradeFirstCard(1);
                }
                else if (selectStudy[1] == 2)
                {
                    upgradeState = UpgradeState.secondMagic;
                    UpgradeFirstCard(2);
                }
            }
            else if (studyState == StudyState.third)
            {
                if (selectStudy[2] == 0)
                {
                    upgradeState = UpgradeState.secondSword;
                    UpgradeFirstCard(0);
                }
                else if (selectStudy[2] == 1)
                {
                    upgradeState = UpgradeState.secondBow;
                    UpgradeFirstCard(1);
                }
                else if (selectStudy[2] == 2)
                {
                    upgradeState = UpgradeState.secondMagic;
                    UpgradeFirstCard(2);
                }
            }
        }
    }
    public void GetSecondCard(int num)
    {
        if (selectStudy[num] == 0)
        {
            cardList.Add(6);
            message.SetActive(true);
            messageText.text = "Guard카드를 얻었다.";
        }
        else if (selectStudy[num] == 1)
        {
            cardList.Add(14);
            message.SetActive(true);
            messageText.text = "Hawkeye카드를 얻었다.";
        }
        else if (selectStudy[num] == 2)
        {
            cardList.Add(22);
            message.SetActive(true);
            messageText.text = "Electric shock카드를 얻었다.";
        }
    }


    public void ThirdCardBtn()
    {
        if (cardState == CardState.get)
        {
            if (studyState == StudyState.first)
            {
                GetThirdCard(0);
                upgradeState = UpgradeState.thirdSword;

            }
            else if (studyState == StudyState.second)
            {
                GetThirdCard(1);
                upgradeState = UpgradeState.thirdBow;
            }
            else if (studyState == StudyState.third)
            {
                GetThirdCard(2);
                upgradeState = UpgradeState.thirdMagic;
            }
        }
        else
        {
            if (studyState == StudyState.first)
            {
                if (selectStudy[0] == 0)
                {
                    upgradeState = UpgradeState.thirdSword;
                    UpgradeFirstCard(0);
                }
                else if (selectStudy[0] == 1)
                {
                    upgradeState = UpgradeState.thirdBow;
                    UpgradeFirstCard(1);
                }
                else if (selectStudy[0] == 2)
                {
                    upgradeState = UpgradeState.thirdMagic;
                    UpgradeFirstCard(2);
                }
            }
            else if (studyState == StudyState.second)
            {
                if (selectStudy[1] == 0)
                {
                    upgradeState = UpgradeState.thirdSword;
                    UpgradeFirstCard(0);
                }
                else if (selectStudy[1] == 1)
                {
                    upgradeState = UpgradeState.thirdBow;
                    UpgradeFirstCard(1);
                }
                else if (selectStudy[1] == 2)
                {
                    upgradeState = UpgradeState.thirdMagic;
                    UpgradeFirstCard(2);
                }
            }
            else if (studyState == StudyState.third)
            {
                if (selectStudy[2] == 0)
                {
                    upgradeState = UpgradeState.thirdSword;
                    UpgradeFirstCard(0);
                }
                else if (selectStudy[2] == 1)
                {
                    upgradeState = UpgradeState.thirdBow;
                    UpgradeFirstCard(1);
                }
                else if (selectStudy[2] == 2)
                {
                    upgradeState = UpgradeState.thirdMagic;
                    UpgradeFirstCard(2);
                }
            }
        }
    }

    public void FourthCardBtn()
    {
        GetFourthCard();
    }
    public void FifthCardBtn()
    {
        GetFifthCard();
    }

    public void GetFourthCard()
    {
        cardList.Add(24);
        message.SetActive(true);
        messageText.text = "Concentration카드를 얻었다.";
    }

    public void GetFifthCard()
    {
        cardList.Add(25);
        message.SetActive(true);
        messageText.text = "Heal카드를 얻었다.";
    }

    public void GetThirdCard(int num)
    {
        if (selectStudy[num] == 0)
        {
            cardList.Add(7);
            message.SetActive(true);
            messageText.text = "ShieldStrike 카드를 얻었다.";
        }
        else if (selectStudy[num] == 1)
        {
            cardList.Add(15);
            message.SetActive(true);
            messageText.text = "Snapshot카드를 얻었다.";
        }
        else if (selectStudy[num] == 2)
        {
            cardList.Add(23);
            message.SetActive(true);
            messageText.text = "ice bolt카드를 얻었다.";
        }
    }
    public void CloseUiMassageBtn()
    {
        uiMessage.SetActive(false);
    }
    public void OpenUpgradeCardBtn()
    {
        if (studyState == StudyState.first)
        {
            if (selectStudy[0] == 0)
            {
                foreach (int a in cardList)
                {
                    if (a == 3 || a == 6 || a == 7 || a == 8 || a == 9 || a == 10)
                    {
                        cardUi.SetActive(true);
                        selectCardUi.SetActive(false);
                        getCardUi.SetActive(true);
                        fstCard1.SetActive(true);
                        cardBordeName.text = "카 드 강 화";
                        ChangeSprite();
                        cardState = CardState.upgrade;
                        break;
                    }
                    else
                    {
                        searchCount++;
                        if (searchCount == cardList.Count)
                        {
                            uiMessage.SetActive(true);
                            uiText.text = "강화할 카드가 없습니다.";
                            break;
                        }
                    }
                }
                searchCount = 0;
            }
            else if (selectStudy[0] == 1)
            {
                foreach (int a in cardList)
                {
                    if (a == 4 || a == 14 || a == 15 || a == 16 || a == 17 || a == 18)
                    {
                        cardUi.SetActive(true);
                        selectCardUi.SetActive(false);
                        getCardUi.SetActive(true);
                        fstCard1.SetActive(true);
                        cardBordeName.text = "카 드 강 화";
                        ChangeSprite();
                        cardState = CardState.upgrade;
                        break;

                    }
                    else
                    {
                        searchCount++;
                        if (searchCount == cardList.Count)
                        {
                            uiMessage.SetActive(true);
                            uiText.text = "강화할 카드가 없습니다.";
                            break;
                        }
                    }
                }
                searchCount = 0;

            }
            else if (selectStudy[0] == 2)
            {
                foreach (int a in cardList)
                {
                    if (a == 5 || a == 22 || a == 23 || a == 24 || a == 25 || a == 26 || a == 27 || a == 28 || a == 29 || a == 30)
                    {
                        cardUi.SetActive(true);
                        selectCardUi.SetActive(false);
                        getCardUi.SetActive(true);
                        fstCard1.SetActive(true);
                        cardBordeName.text = "카 드 강 화";
                        ChangeSprite();
                        cardState = CardState.upgrade;
                        Debug.Log(cardState);
                        break;
                    }
                    else
                    {
                        searchCount++;
                        if (searchCount == cardList.Count)
                        {
                            uiMessage.SetActive(true);
                            uiText.text = "강화할 카드가 없습니다.";
                            break;
                        }
                    }
                }
                searchCount = 0;

            }
        }
        else if (studyState == StudyState.second)
        {
            if (selectStudy[1] == 0)
            {
                foreach (int a in cardList)
                {
                    if (a == 3 || a == 6 || a == 7 || a == 8 || a == 9 || a == 10)
                    {
                        cardUi.SetActive(true);
                        selectCardUi.SetActive(false);
                        getCardUi.SetActive(true);
                        fstCard1.SetActive(true);
                        cardBordeName.text = "카 드 강 화";
                        ChangeSprite();
                        cardState = CardState.upgrade;
                        break;

                    }
                    else
                    {
                        searchCount++;
                        if (searchCount == cardList.Count)
                        {
                            uiMessage.SetActive(true);
                            uiText.text = "강화할 카드가 없습니다.";
                            break;
                        }
                    }
                }
                searchCount = 0;

            }
            else if (selectStudy[1] == 1)
            {
                foreach (int a in cardList)
                {
                    if (a == 4 || a == 14 || a == 15 || a == 16 || a == 17 || a == 18)
                    {
                        cardUi.SetActive(true);
                        selectCardUi.SetActive(false);
                        getCardUi.SetActive(true);
                        fstCard1.SetActive(true);
                        cardBordeName.text = "카 드 강 화";
                        ChangeSprite();
                        cardState = CardState.upgrade;
                        break;

                    }
                    else
                    {
                        searchCount++;
                        if (searchCount == cardList.Count)
                        {
                            uiMessage.SetActive(true);
                            uiText.text = "강화할 카드가 없습니다.";
                            break;
                        }
                    }
                }
                searchCount = 0;

            }
            else if (selectStudy[1] == 2)
            {
                foreach (int a in cardList)
                {
                    if (a == 5 || a == 22 || a == 23 || a == 24 || a == 25 || a == 26 || a == 27 || a == 28 || a == 29 || a == 30)
                    {
                        cardUi.SetActive(true);
                        selectCardUi.SetActive(false);
                        getCardUi.SetActive(true);
                        fstCard1.SetActive(true);
                        cardBordeName.text = "카 드 강 화";
                        ChangeSprite();
                        cardState = CardState.upgrade;
                        break;

                    }
                    else
                    {
                        searchCount++;
                        if (searchCount == cardList.Count)
                        {
                            uiMessage.SetActive(true);
                            uiText.text = "강화할 카드가 없습니다.";
                            break;
                        }
                    }
                }
                searchCount = 0;

            }

        }
        else if (studyState == StudyState.third)
        {
            if (selectStudy[2] == 0)
            {
                foreach (int a in cardList)
                {
                    if (a == 3 || a == 6 || a == 7 || a == 8 || a == 9 || a == 10)
                    {
                        cardUi.SetActive(true);
                        selectCardUi.SetActive(false);
                        getCardUi.SetActive(true);
                        fstCard1.SetActive(true);
                        cardBordeName.text = "카 드 강 화";
                        ChangeSprite();
                        cardState = CardState.upgrade;
                        break;

                    }
                    else
                    {
                        searchCount++;
                        if (searchCount == cardList.Count)
                        {
                            uiMessage.SetActive(true);
                            uiText.text = "강화할 카드가 없습니다.";
                            break;
                        }
                    }
                }
                searchCount = 0;

            }
            else if (selectStudy[2] == 1)
            {
                foreach (int a in cardList)
                {
                    if (a == 4 || a == 14 || a == 15 || a == 16 || a == 17 || a == 18)
                    {
                        cardUi.SetActive(true);
                        selectCardUi.SetActive(false);
                        getCardUi.SetActive(true);
                        fstCard1.SetActive(true);
                        cardBordeName.text = "카 드 강 화";
                        ChangeSprite();
                        cardState = CardState.upgrade;
                        break;

                    }
                    else
                    {
                        searchCount++;
                        if (searchCount == cardList.Count)
                        {
                            uiMessage.SetActive(true);
                            uiText.text = "강화할 카드가 없습니다.";
                            break;
                        }
                    }
                }
                searchCount=0;

            }
            else if (selectStudy[2] == 2)
            {
                foreach (int a in cardList)
                {
                    if (a == 5 || a == 22 || a == 23 || a == 24 || a == 25 || a == 26 || a == 27 || a == 28 || a == 29 || a == 30)
                    {
                        cardUi.SetActive(true);
                        selectCardUi.SetActive(false);
                        getCardUi.SetActive(true);
                        fstCard1.SetActive(true);
                        cardBordeName.text = "카 드 강 화";
                        ChangeSprite();
                        cardState = CardState.upgrade;
                        break;
                    }
                    else
                    {
                        searchCount++;
                        if (searchCount == cardList.Count)
                        {
                            uiMessage.SetActive(true);
                            uiText.text = "강화할 카드가 없습니다.";
                            break;
                        }
                    }
                }
                searchCount=0;

            }

        }
    }

    public void OpenGetCardBtn()
    {
        cardUi.SetActive(true);
        selectCardUi.SetActive(false);
        getCardUi.SetActive(true);
        fstCard1.SetActive(true);
        cardBordeName.text = "카 드 획 득";
        ChangeSprite();
        cardState = CardState.get;
    }
    public void ChangeSprite()
    {
        if(studyState==StudyState.first)
        {
            if(selectStudy[0]==0)
            {
                cardResources[0].sprite = cardSprite[0];//검0,1,2
                cardResources[1].sprite = cardSprite[1];
                cardResources[2].sprite = cardSprite[2];
            }
            else if (selectStudy[0] == 1)
            {
                cardResources[0].sprite = cardSprite[3];//활3,4,5
                cardResources[1].sprite = cardSprite[4];
                cardResources[2].sprite = cardSprite[5];
            }
            else if (selectStudy[0] == 2)
            {
                cardResources[0].sprite = cardSprite[6];//마법6,7,8.9,10
                cardResources[1].sprite = cardSprite[7];
                cardResources[2].sprite = cardSprite[8];
                cardResources[3].sprite = cardSprite[9];
                cardResources[4].sprite = cardSprite[10];
            }
        }
        else if(studyState == StudyState.second)
        {
            if (selectStudy[1] == 0)
            {
                cardResources[0].sprite = cardSprite[0];//검0,1,2
                cardResources[1].sprite = cardSprite[1];
                cardResources[2].sprite = cardSprite[2];
            }
            else if (selectStudy[1] == 1)
            {
                cardResources[0].sprite = cardSprite[3];//활3,4,5
                cardResources[1].sprite = cardSprite[4];
                cardResources[2].sprite = cardSprite[5];
            }
            else if (selectStudy[1] == 2)
            {
                cardResources[0].sprite = cardSprite[6];//마법6,7,8.9,10
                cardResources[1].sprite = cardSprite[7];
                cardResources[2].sprite = cardSprite[8];
                cardResources[3].sprite = cardSprite[9];
                cardResources[4].sprite = cardSprite[10];
            }
        }
        else if (studyState == StudyState.third)
        {
            if (selectStudy[2] == 0)
            {
                cardResources[0].sprite = cardSprite[0];//검0,1,2
                cardResources[1].sprite = cardSprite[1];
                cardResources[2].sprite = cardSprite[2];
            }
            else if (selectStudy[2] == 1)
            {
                cardResources[0].sprite = cardSprite[3];//활3,4,5
                cardResources[1].sprite = cardSprite[4];
                cardResources[2].sprite = cardSprite[5];
            }
            else if (selectStudy[2] == 2)
            {
                cardResources[0].sprite = cardSprite[6];//마법6,7,8.9,10
                cardResources[1].sprite = cardSprite[7];
                cardResources[2].sprite = cardSprite[8];
                cardResources[3].sprite = cardSprite[9];
                cardResources[4].sprite = cardSprite[10];
            }
        }
    }

    public void CloseMessageBtn()
    {
        message.SetActive(false);
        cardUi.SetActive(false);
        getCardUi.SetActive(false);
        selectCardUi.SetActive(false);
        startButton.SetActive(true);
        statUi.SetActive(true);
        fstCard1.SetActive(false);
        fstCard2.SetActive(false);

        studingResult[1].text = null;
        studingResult[2].text = null;
        if (studyState == StudyState.first)
        {
            studingResult[0].text = "2회차";
            studyState = StudyState.second;
        }
        else if(studyState== StudyState.second)
        {
            studingResult[0].text = "3회차";
            studyState = StudyState.third;
        }
        else if (studyState == StudyState.third)
        {
            startButton.SetActive(false);
            statUi.SetActive(false);
            studingUi.SetActive(false);
            mainCamera.transform.position = new Vector3(0.0f, -10.0f, -100.0f);
            DeckList();
        }
    }
    public void RightCardBtn()
    {
        if (studyState == StudyState.first)
        {
            if (selectStudy[0] == 2)
            {
                fstCard2.SetActive(true);
                fstCard1.SetActive(false);
            }
        }
        else if (studyState == StudyState.second)
        {
            if (selectStudy[1] == 2)
            {
                fstCard2.SetActive(true);
                fstCard1.SetActive(false);
            }
        }
        else if (studyState == StudyState.third)
        {
            if (selectStudy[2] == 2)
            {
                fstCard2.SetActive(true);
                fstCard1.SetActive(false);
            }
        }
    }

    public void LeftCardBtn()
    {
        if (studyState == StudyState.first)
        {
            if (selectStudy[0] == 2)
            {
                fstCard2.SetActive(false);
                fstCard1.SetActive(true);
            }
        }
        else if (studyState == StudyState.second)
        {
            if (selectStudy[1] == 2)
            {
                fstCard2.SetActive(false);
                fstCard1.SetActive(true);
            }
        }
        else if (studyState == StudyState.third)
        {
            if (selectStudy[2] == 2)
            {
                fstCard2.SetActive(false);
                fstCard1.SetActive(true);
            }
        }
    }

    public void DeckList()
    {
        endStudy.SetActive(true);

        int count = 0;
        foreach (int targetId in cardList)
        {
            GameObject getCard = Instantiate(FindPrefabById(targetId));
            Vector3 cardPosition = PositionCheck(count);
            getCard.transform.position = cardPosition;
            var card = getCard.GetComponent<Card>();
            card.CardFront(true);
            card.GetComponent<Order>().SetOriginOrder(order++);
            count++;
        }
    }

    public Vector3 PositionCheck(int count)
    {
        float positionX = -5.0f;
        float positionY = -9.2f;
        if (count < 7)
        {
            positionX = -5.0f + (count * 1.6f);
            positionY = -9.2f;
        }
        else
        {
            positionX = -5.0f + ((count-7)  * 1.6f);
            positionY = -11.2f;
        }

        Vector3 position = new Vector3(positionX, positionY, 0.0f);
        return position;
    }

    public GameObject FindPrefabById(int targetId)
    {
        string searchFolderPath = "Assets/Prefabs/Cards";
        string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new string[] { searchFolderPath });

        foreach (string prefabGUID in prefabGUIDs)
        {
            // 프리팹 경로를 실제 프리팹으로 변환
            string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGUID);

            // 프리팹을 가져오기
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            cardListData.Add(prefab);
            // 프리팹이 유효하고, 해당 프리팹이 해당 조건을 만족하는 경우 반환
            if (prefab != null)
            {
                Card myScriptComponent = prefab.GetComponent<Card>();
                myScriptComponent.DataInit();

                // 스크립트 컴포넌트가 존재하고, targetId와 일치하는 경우 해당 프리팹 반환
                if (myScriptComponent != null && myScriptComponent.info.Id == targetId)
                {
                    return prefab;
                }
            }
        }
        return null;
    }

    public void EndStudy()
    {
        GameObject dataObject = GameObject.FindGameObjectWithTag("PlayerData");

        Debug.Log(dataObject);
        data = dataObject.GetComponent<CharacterData>();
        data.DataInit(0, "test", 1, DefaultPlayerHp + playerHp, DefaultPlayerAttack + playerAttack, DefaultPlayerDefense + playerDefense, SelectWeaponType(), SelectSprite(), cardList);
        data.Init();
        data.ChangeData();
        SaveData();
        SceneManager.LoadScene("TestScene2");
    }
    public WeaponType SelectWeaponType()
    {
        int swordNum = 0;
        int arrowNum = 0;
        int magicNum = 0;

        foreach (int a in selectStudy)
        {
            if (a == 0)
            {
                swordNum++;
            }
            else if (a == 1)
            {
                arrowNum++;
            }
            else
            {
                magicNum++;
            }
        }
        if (swordNum > arrowNum && swordNum > magicNum)
        {
            return WeaponType.SWORD;
        }
        else if (arrowNum > swordNum && arrowNum > magicNum)
        {
            return WeaponType.BOW;
        }
        else if (magicNum > swordNum && magicNum > arrowNum)
        {
            return WeaponType.WAND;
        }
        else
        {
            return WeaponType.SWORD;
        }
    }


    public int SelectSprite()
    {
        int swordNum = 0;
        int arrowNum = 0;
        int magicNum = 0;

        foreach(int a in selectStudy)
        {
            if(a == 0)
            {
                swordNum++;
            }
            else if (a == 1)
            {
                arrowNum++;
            }
            else 
            {
                magicNum++;
            }
        }
        if (swordNum > arrowNum && swordNum > magicNum)
        {
            return 0;
        }
        else if (arrowNum > swordNum && arrowNum > magicNum)
        {
            return 1;
        }
        else if (magicNum > swordNum && magicNum > arrowNum)
        {
            return 2;
        }
        else
        {
            return 0;
        }
    }



    public void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string filePath = Path.Combine(Application.persistentDataPath, saveSlot.ToString()+".dat");
        FileStream fileStream = File.Create(filePath);
        saveData = data.info;
        Debug.Log(saveData.chrId);
        Debug.Log(saveData.chrCardIds[0]);
        Debug.Log(saveData.chrCardIds[1]);
        Debug.Log(data.info.chrMaxHp);
        Debug.Log(data.info.chrDefense);


        // 이제 SaveData() 메서드 내에서 직렬화할 데이터는 DefaultCharacterData 인스턴스인 info입니다.
        formatter.Serialize(fileStream, saveData);
        fileStream.Close();
        string dataPath = Application.persistentDataPath;
        Debug.Log("Persistent Data Path: " + dataPath);
        Debug.Log("캐릭터 데이터를 바이너리로 저장했습니다.");
    }

    private int LoadCharacterData0()
    {
        string filePath = Path.Combine(Application.persistentDataPath, 0.ToString()+".dat");

        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = File.Open(filePath, FileMode.Open);
            DefaultCharacterData dataLoad;
            dataLoad = (DefaultCharacterData)formatter.Deserialize(fileStream);
            fileStream.Close();

            data.DataInit(dataLoad.chrId, dataLoad.chrName, dataLoad.chrLv, dataLoad.chrMaxHp, dataLoad.chrAttackDmg, dataLoad.chrDefense, dataLoad.weapon, dataLoad.imgName, dataLoad.chrCardIds);//캐릭터 저장용)
            Debug.Log("캐릭터 데이터를 바이너리에서 로드했습니다.");
            return 0;
        }
        else
        {
            // 바이너리 파일이 없으면 새로운 CharacterData 클래스 생성
            // 예제용 초기화 데이터 적용

            data.DataInit(0, "test", 1, 1, 1, 1 
                        , WeaponType.SWORD, 0, cardList);
            

            Debug.Log("저장된 캐릭터 데이터가 없어서 새로 생성했습니다.");
            return 1;
        }
    }
    private int LoadCharacterData1()
    {
        string filePath = Path.Combine(Application.persistentDataPath, 1.ToString() + ".dat");

        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = File.Open(filePath, FileMode.Open);
            DefaultCharacterData dataLoad;
            dataLoad = (DefaultCharacterData)formatter.Deserialize(fileStream);
            fileStream.Close();

            data.DataInit(dataLoad.chrId, dataLoad.chrName, dataLoad.chrLv, dataLoad.chrMaxHp, dataLoad.chrAttackDmg, dataLoad.chrDefense, dataLoad.weapon, dataLoad.imgName, dataLoad.chrCardIds);//캐릭터 저장용)
            Debug.Log("캐릭터 데이터를 바이너리에서 로드했습니다.");
            return 0;
        }
        else
        {
            return 1;
        }
    }
    private int LoadCharacterData2()
    {
        string filePath = Path.Combine(Application.persistentDataPath, 2.ToString() + ".dat");

        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = File.Open(filePath, FileMode.Open);
            DefaultCharacterData dataLoad;
            dataLoad = (DefaultCharacterData)formatter.Deserialize(fileStream);
            fileStream.Close();
            data.DataInit(dataLoad.chrId, dataLoad.chrName, dataLoad.chrLv, dataLoad.chrMaxHp, dataLoad.chrAttackDmg, dataLoad.chrDefense, dataLoad.weapon, dataLoad.imgName, dataLoad.chrCardIds);//캐릭터 저장용)

            Debug.Log("캐릭터 데이터를 바이너리에서 로드했습니다.");
            return 0;
        }
        else
        {
            return 1;
        }
    }

    // CharacterData 클래스의 데이터를 설정
    public void SetCharacterData(int id, string name, int lv, int maxHp, int attackDmg, int defense, WeaponType weaponName, int img, List<int> chrCardnum1)//캐릭터 저장용
    {
        data.DataInit(id, name, lv, maxHp, attackDmg, defense, weaponName, img, chrCardnum1);
    }

    // CharacterData 클래스 반환
    public CharacterData GetCharacterData()
    {
        return data;
    }
}


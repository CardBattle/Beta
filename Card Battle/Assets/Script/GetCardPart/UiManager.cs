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
        third
    }
    private StudyState studyState;
    
    //���͵�� ��ǥ 0,0
    //�˼��� 0,10
    //�ü��� 20,10
    //������ 20,00
    public Image[] selectStudyImage;
    public Image studyViewImage;
    public Sprite[] studyViewResources;
    public Sprite[] selectStudyResources;

    public Camera mainCamera;

    public Text[] studyInfo;
    public Text[] studingResult;
    public Text[] studyResult;
    public Text cardBordeName;
    public Text messageText;

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
    private CharacterData data;
    private DefaultCharacterData saveData;


    public void Awake()
    {
        studyReset();
    }

    public void studyReset()
    {
        studyViewNum = 0;
        studyViewImage.sprite = studyViewResources[studyViewNum];
        selectStudy = new List<int>();
        cardList = new List<int> { 0, 1, 2, 3, 4, 5 };
        cardListData = new List<GameObject>();
        player.position = new Vector3(-0.86f, 0.56f, 0.0f);
        mainCamera.transform.position = new Vector3(0.0f, 0.0f, -100.0f);
        studyUi.SetActive(true);
        studingUi.SetActive(false);
        selectCardUi.SetActive(false);
        statUi.SetActive(false);
        startButton.SetActive(false);;
        message.SetActive(false);
        cardUi.SetActive(false);
        getCardUi.SetActive(false);
        upgradeCardUi.SetActive(false);
        endStudy.SetActive(false);
        /*if(GameObject.FindGameObjectWithTag("PlayerData")!=null)
        {
            GameObject dataObject = GameObject.FindGameObjectWithTag("PlayerData");
            data = dataObject.GetComponent<CharacterData>();
            playerHp = data.chrMaxHp;
            playerAttack = data.chrAttackDmg;
            playerDefense = data.chrDefense;
            DefaultPlayerHp = data.chrMaxHp;
            DefaultPlayerAttack = data.chrAttackDmg;
            DefaultPlayerDefense = data.chrDefense;
            cardListData = data.chrCard;
        }
        else*/
  //      {
            playerHp = 20;
            playerAttack = 1;
            playerDefense = 1;
            DefaultPlayerHp = 20;
            DefaultPlayerAttack = 1;
            DefaultPlayerDefense = 1;
//        }
        
        studingResult[0].text = "1ȸ��";
        studingResult[1].text = null;
        studingResult[2].text = null;
        studyState = StudyState.first;
        order = 0;
    }
    public void ChangeStudyImageBtn()
    {
        if (studyViewNum > 1)
        {
            studyViewNum = 0;
            studyViewImage.sprite = studyViewResources[studyViewNum];
            studyInfo[0].text = "SWORD";
            studyInfo[1].text = "���ݰ� ����� �뷱���� Ư¡�̴�.";
            studyInfo[2].text = "��� ���� ��";

        }
        else
        {
            studyViewNum++;
            studyViewImage.sprite = studyViewResources[studyViewNum];
            if (studyViewNum == 0)
            {
                studyInfo[0].text = "SWORD";
                studyInfo[1].text = "���ݰ� ����� �뷱���� Ư¡�̴�.";
                studyInfo[2].text = "��� ���� ��";
            }
            else if (studyViewNum == 1)
            {
                studyInfo[0].text = "BOW";
                studyInfo[1].text = "������ ���ݿ� Ưȭ�Ǿ��ִ�.";
                studyInfo[2].text = "ü�¡� ���ݡ�";


            }
            else if (studyViewNum == 2)
            {
                studyInfo[0].text = "MAGIC";
                studyInfo[1].text = "�ټ� ������ ����� ��� �� �ִ�.";
                studyInfo[2].text = "���ݡ�";

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
        if (selectStudy.Count==3)
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
        studyResult[0].text = "���";
        studyResult[1].text = "ü��"+(playerHp- DefaultPlayerHp).ToString()+"�� "
            + "����" + (playerAttack - DefaultPlayerAttack).ToString() + "�� "
            + "���" + (playerDefense - DefaultPlayerDefense).ToString() + "��";
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
            studingResult[1].text = "ü�¡�";
            studingResult[2].text = "ü���� �پ���";
        }
        else if (randomNum == 1)
        {
            playerAttack++;
            studingResult[1].text = "���ݡ�";
            studingResult[2].text = "������ ���ϰ� �� �� ����.";
        }
        else if (randomNum == 2)
        {
            playerDefense++;
            studingResult[1].text = "����";
            studingResult[2].text = "�� ���ϰ� �� �� ����.";
        }
        else if (randomNum == 3)
        {
            playerHp++;
            playerAttack++;
            studingResult[1].text = "ü�¡� ���ݡ�";
            studingResult[2].text = "������ ������ �����ߴ�.";

        }
        else if (randomNum == 4)
        {
            playerHp++;
            playerDefense++;
            studingResult[1].text = "ü�¡� ����";
            studingResult[2].text = "�� ������ �����ߴ�.";

        }
        else if (randomNum == 5)
        {
            playerDefense++;
            playerAttack++;
            playerHp++;
            studingResult[1].text = "ü�¡� ���ݡ� ����";
            studingResult[2].text = "�Ϻ��ϰ� �س´�.";
        }
        else if (randomNum == 6)
        {
            studingResult[1].text = " - ";
            studingResult[2].text = "���� �׳� ������.";
        }
    }
    IEnumerator BowStudy(int num)
    {
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
            studingResult[1].text = "ü�¡�";
            studingResult[2].text = "ü���� �پ���";
        }
        else if (randomNum == 1 || randomNum == 4)
        {
            playerAttack++;
            studingResult[1].text = "���ݡ�";
            studingResult[2].text = "������ ���ϰ� �� �� ����.";
        }
        else if (randomNum == 2 || randomNum == 5 || randomNum == 6)
        {
            studingResult[1].text = " - ";
            studingResult[2].text = "���� �׳� ������.";
        }
        else if (randomNum == 3)
        {
            playerHp++;
            playerAttack++;
            studingResult[1].text = "ü�¡� ���ݡ�";
            studingResult[2].text = "������ ������ �����ߴ�.";
        }
    }
    IEnumerator MagicStudy(int num)
    {
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
            studingResult[1].text = "���ݡ�";
            studingResult[2].text = "�׷����� ���� ����.";
        }
        else if (randomNum == 2 || randomNum == 5 ||  randomNum == 6)
        {
            studingResult[1].text = " - ";
            studingResult[2].text = "���� �׳� ������.";
        }
        else if (randomNum == 3)
        {
            playerAttack++;
            playerAttack++;
            studingResult[1].text = "���ݡ��";
            studingResult[2].text = "�Ϻ��ϰ� �����ߴ�.";
        }
    }

    public void FirstCardBtn()
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

    public void GetFirstCard(int num)
    {
        if (selectStudy[num] == 0)
        {
            cardList.Add(3);
            message.SetActive(true);
            messageText.text = "Cutī�带 �����.";
        }
        else if (selectStudy[num] == 1)
        {
            cardList.Add(4);
            message.SetActive(true);
            messageText.text = "Arrowshotī�带 �����.";
        }
        else if (selectStudy[num] == 2)
        {
            cardList.Add(5);
            message.SetActive(true);
            messageText.text = "Firdballī�带 �����.";
        }
    }

    public void SecondCardBtn()
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
    public void GetSecondCard(int num)
    {
        if (selectStudy[num] == 0)
        {
            cardList.Add(6);
            message.SetActive(true);
            messageText.text = "Guardī�带 �����.";
        }
        else if (selectStudy[num] == 1)
        {
            cardList.Add(14);
            message.SetActive(true);
            messageText.text = "Hawkeyeī�带 �����.";
        }
        else if (selectStudy[num] == 2)
        {
            cardList.Add(22);
            message.SetActive(true);
            messageText.text = "Electric shockī�带 �����.";
        }
    }


    public void ThirdCardBtn()
    {
        if (studyState == StudyState.first)
        {
            GetThirdCard(0);
        }
        else if (studyState == StudyState.second)
        {
            GetThirdCard(1);
        }
        else if (studyState == StudyState.third)
        {
            GetThirdCard(2);
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
        messageText.text = "Concentrationī�带 �����.";
    }

    public void GetFifthCard()
    {
        cardList.Add(25);
        message.SetActive(true);
        messageText.text = "Healī�带 �����.";
    }

    public void GetThirdCard(int num)
    {
        if (selectStudy[num] == 0)
        {
            cardList.Add(7);
            message.SetActive(true);
            messageText.text = "ShieldStrike ī�带 �����.";
        }
        else if (selectStudy[num] == 1)
        {
            cardList.Add(15);
            message.SetActive(true);
            messageText.text = "Snapshotī�带 �����.";
        }
        else if (selectStudy[num] == 2)
        {
            cardList.Add(23);
            message.SetActive(true);
            messageText.text = "ice boltī�带 �����.";
        }
    }

    public void UpgradeCardBtn()
    {

    }

    public void OpenGetCardBtn()
    {
        cardUi.SetActive(true);
        selectCardUi.SetActive(false);
        getCardUi.SetActive(true);
        fstCard1.SetActive(true);
        cardBordeName.text = "ī �� ȹ ��";
    }

    public void OpenUpgradeCardBtn()
    {

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
            studingResult[0].text = "2ȸ��";
            studyState = StudyState.second;
        }
        else if(studyState== StudyState.second)
        {
            studingResult[0].text = "3ȸ��";
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
            // ������ ��θ� ���� ���������� ��ȯ
            string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGUID);

            // �������� ��������
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            cardListData.Add(prefab);
            // �������� ��ȿ�ϰ�, �ش� �������� �ش� ������ �����ϴ� ��� ��ȯ
            if (prefab != null)
            {
                Card myScriptComponent = prefab.GetComponent<Card>();
                myScriptComponent.Init();

                // ��ũ��Ʈ ������Ʈ�� �����ϰ�, targetId�� ��ġ�ϴ� ��� �ش� ������ ��ȯ
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
        data.DataInit(0, "test", 1, DefaultPlayerHp + playerHp, DefaultPlayerAttack + playerAttack, DefaultPlayerDefense + playerDefense, cardListData
                        ,WeaponType.SWORD, null, cardList);
        data.Init();
        SaveData();
        SceneManager.LoadScene("TestScene2");
    }


    public void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string filePath = Path.Combine(Application.persistentDataPath, "Save");
        FileStream fileStream = File.Create(filePath);
        saveData = data.info;
        Debug.Log(saveData.chrId);
        Debug.Log(data.info.chrMaxHp);
        Debug.Log(data.info.chrDefense);


        // ���� SaveData() �޼��� ������ ����ȭ�� �����ʹ� DefaultCharacterData �ν��Ͻ��� info�Դϴ�.
        formatter.Serialize(fileStream, saveData);
        fileStream.Close();
        string dataPath = Application.persistentDataPath;
        Debug.Log("Persistent Data Path: " + dataPath);
        Debug.Log("ĳ���� �����͸� ���̳ʸ��� �����߽��ϴ�.");
    }

    private void LoadCharacterData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "Save");

        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = File.Open(filePath, FileMode.Open);

            data = (CharacterData)formatter.Deserialize(fileStream);
            fileStream.Close();

            Debug.Log("ĳ���� �����͸� ���̳ʸ����� �ε��߽��ϴ�.");
        }
        else
        {
            // ���̳ʸ� ������ ������ ���ο� CharacterData Ŭ���� ����
            data = new CharacterData();

            // ������ �ʱ�ȭ ������ ����
            data.DataInit(0, "test", 1, 1, 1, 1, cardListData
                        , WeaponType.SWORD, null, cardList);
            

            Debug.Log("����� ĳ���� �����Ͱ� ��� ���� �����߽��ϴ�.");
        }
    }

    // CharacterData Ŭ������ �����͸� ����
    public void SetCharacterData(int id, string name, int lv, int maxHp, int attackDmg, int defense, List<GameObject> cardList, WeaponType weaponName, Sprite imgNum, List<int> chrCardnum)
    {
        data.DataInit(id, name, lv, maxHp, attackDmg, defense, cardList, weaponName, imgNum, chrCardnum);
    }

    // CharacterData Ŭ���� ��ȯ
    public CharacterData GetCharacterData()
    {
        return data;
    }
}


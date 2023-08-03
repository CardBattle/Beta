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

    //���͵�� ��ǥ 0,0
    //�˼��� 0,10
    //�ü��� 20,10
    //������ 20,00
    //��ŸƮ�� -20, -10
    public Image[] loadViewImage;
    public Image[] selectStudyImage;
    public Image[] upgradeBoxImage;
    public Sprite[] cardSprite;
    public SpriteRenderer weaponSprite;

    public Image studyViewImage;

    public Sprite[] studyViewResources;
    public Sprite[] selectStudyResources;
    

    public Image[] cardImage;

    public Camera mainCamera;

    public Text[] saveInfo;

    public Text[] studyInfo;
    public Text[] studingResult;
    public Text[] studyResult;
    public Text cardBordeName;
    public Text messageText;
    public Text uiText;
    public AudioClip[] audioClips; // �ν����Ϳ��� ����� Ŭ���� �Ҵ�
    private AudioSource audioSource;


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
    public GameObject[] vfx;


    public Animator babyAni;
    public GameObject baby;
    private Character babydata;


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
    private int effect;

    private CharacterData data;
    private DefaultCharacterData saveData;
    Transform warriorTransform;

    public void Awake()
    {
        DataInit();
        SaveSlot();
        studyReset();
        audioSource = GetComponent<AudioSource>();
    }
    public void DataInit()
    {
        GameObject dataObject = GameObject.FindGameObjectWithTag("PlayerData");
        Debug.Log(dataObject);
        data = dataObject.GetComponent<CharacterData>();
    }

    public void SaveSlot()
    {
        string directoryPath = Path.Combine(Application.persistentDataPath);//�˻��� ���
        int binaryFileCount = CountBinaryFilesInDirectory(directoryPath);
        Debug.Log("���̳ʸ��� ����� ���� ����: " + binaryFileCount);
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
            Debug.LogWarning("���丮�� �������� �ʽ��ϴ�: " + path);
        }

        return binaryFileCount;
    }

    bool IsBinaryFile(string filePath)
    {
        // ���� Ȯ���ڰ� ���̳ʸ� ���Ͽ� �ش��ϴ��� ���θ� üũ�մϴ�.
        // ���� ���, ���⿡���� ".dat" ������ ���̳ʸ� ���Ϸ� �����մϴ�.
        string extension = Path.GetExtension(filePath).ToLower();
        return extension == ".dat"; // ���ϴ� Ȯ���ڷ� �������ּ���.
    }


    public void studyReset()
    {
        studyViewNum = 0;
        studyViewImage.sprite = studyViewResources[studyViewNum];
        selectStudy = new List<int>();
        cardListData = new List<GameObject>();
        baby.transform.position = new Vector3(-20f, 12.0f, 0.0f);
        baby.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        weaponSprite.sprite = null;
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
        studingResult[0].text = "1ȸ��";
        studingResult[1].text = null;
        studingResult[2].text = null;
        studyState = StudyState.first;
        cardList = new List<int>();
        order = 0;
        searchCount = 0;
        warriorTransform = baby.transform.Find("Warrior");
        babydata = warriorTransform.GetComponent<Character>();
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
        baby.transform.position = new Vector3(-1f, 0.0f, 0.0f);
        baby.transform.localScale = new Vector3(1f, 1f, 1f);
        //weaponSprite.sprite = Resources.Load<Sprite>("Sprites/Weapon-Sword");
        babydata.AniInit(0);

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
            loadViewImage[0].sprite = selectStudyResources[data.imgNum];
            saveInfo[0].text = "�̸� " + data.chrName + "\n" + "���� " + data.chrLv.ToString() + "\n" + "ī�� ���� " + data.chrCardnum.Count.ToString() + "\n" + "ü�� " + data.chrMaxHp.ToString() + "\n" + "���ݷ� "
                + data.chrAttackDmg.ToString() + "\n" + "���� " + data.chrDefense.ToString();
            //loadViewImage[1].sprite = null;
            saveInfo[1].text = "�����Ͱ� �����ϴ�";
            //loadViewImage[2].sprite = null;
            saveInfo[2].text = "�����Ͱ� �����ϴ�";

        }
        else if (saveSlot == 2)
        {
            LoadCharacterData0();
            loadViewImage[0].sprite = selectStudyResources[data.imgNum];
            saveInfo[0].text = "�̸� " + data.chrName + "\n" + "���� " + data.chrLv.ToString() + "\n" + "ī�� ���� " + data.chrCardnum.Count.ToString() + "\n" + "ü�� " + data.chrMaxHp.ToString() + "\n" + "���ݷ� "
                + data.chrAttackDmg.ToString() + "\n" + "���� " + data.chrDefense.ToString();
            LoadCharacterData1();
            loadViewImage[1].sprite = selectStudyResources[data.imgNum];
            saveInfo[1].text = "�̸� " + data.chrName + "\n" + "���� " + data.chrLv.ToString() + "\n" + "ī�� ���� " + data.chrCardnum.Count.ToString() + "\n" + "ü�� " + data.chrMaxHp.ToString() + "\n" + "���ݷ� "
                + data.chrAttackDmg.ToString() + "\n" + "���� " + data.chrDefense.ToString();
            //loadViewImage[2].sprite = null;
            saveInfo[2].text = "�����Ͱ� �����ϴ�";
        }
        else if (saveSlot == 3)
        {
            LoadCharacterData0();
            loadViewImage[0].sprite = selectStudyResources[data.imgNum];
            saveInfo[0].text = "�̸� "+data.chrName +"\n" + "���� " + data.chrLv.ToString() + "\n" +  "ī�� ���� " + data.chrCardnum.Count.ToString() + "\n" + "ü�� " + data.chrMaxHp.ToString() + "\n" + "���ݷ� "
                + data.chrAttackDmg.ToString() + "\n" + "���� " + data.chrDefense.ToString();
            LoadCharacterData1();
            loadViewImage[1].sprite = selectStudyResources[data.imgNum];
            saveInfo[1].text = "�̸� " + data.chrName + "\n" + "���� " + data.chrLv.ToString() + "\n" + "ī�� ���� " + data.chrCardnum.Count.ToString() + "\n" + "ü�� " + data.chrMaxHp.ToString() + "\n" + "���ݷ� "
                + data.chrAttackDmg.ToString() + "\n" + "���� " + data.chrDefense.ToString();
            LoadCharacterData2();
            loadViewImage[2].sprite = selectStudyResources[data.imgNum];
            saveInfo[2].text = "�̸� " + data.chrName + "\n" + "���� " + data.chrLv.ToString() + "\n" + "ī�� ���� " + data.chrCardnum.Count.ToString() + "\n" + "ü�� " + data.chrMaxHp.ToString() + "\n" + "���ݷ� "
                + data.chrAttackDmg.ToString() + "\n" + "���� " + data.chrDefense.ToString();
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
            data.DataInit(0, "test", 1, 20, 1, 1
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
            data.DataInit(0, "test", 1, 20, 1, 1
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
            studyInfo[1].text = "���ݰ� ����� �뷱���� Ư¡�̴�.";
            studyInfo[2].text = "��� ���� ��";
            //weaponSprite.sprite = Resources.Load<Sprite>("Sprites/Weapon-Sword");
            babydata.AniInit(0);
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
                //weaponSprite.sprite = Resources.Load<Sprite>("Sprites/Weapon-Sword");
                babydata.AniInit(0);


            }
            else if (studyViewNum == 1)
            {
                studyInfo[0].text = "BOW";
                studyInfo[1].text = "������ ���ݿ� Ưȭ�Ǿ��ִ�.";
                studyInfo[2].text = "ü�¡� ���ݡ�";
                //weaponSprite.sprite = Resources.Load<Sprite>("Sprites/Weapon-Bow");
                babydata.AniInit(1);

            }
            else if (studyViewNum == 2)
            {
                studyInfo[0].text = "MAGIC";
                studyInfo[1].text = "�ټ� ������ ����� ��� �� �ִ�.";
                studyInfo[2].text = "���ݡ�";
                //weaponSprite.sprite = Resources.Load<Sprite>("Sprites/Weapon-Wand");
                babydata.AniInit(2);


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
        Debug.Log(selectStudy[0]);
        Debug.Log(selectStudy[1]);
        Debug.Log(selectStudy[2]);

        if (selectStudy.Count == 3)
        {
            if (selectStudy[0] == 0)
            {
                studyUi.SetActive(false);
                studingUi.SetActive(true);
                statUi.SetActive(true);
                startButton.SetActive(true); ;

                mainCamera.transform.position = new Vector3(0.0f, 10.0f, -100.0f);
                baby.transform.position = new Vector3(4.0f, 12.0f, 0.0f);
            }
            else if (selectStudy[0] == 1)
            {
                studyUi.SetActive(false);
                studingUi.SetActive(true);
                statUi.SetActive(true);
                startButton.SetActive(true); ;
                mainCamera.transform.position = new Vector3(0.0f, 10.0f, -100.0f);
                baby.transform.position = new Vector3(4.0f, 12.0f, 0.0f);
            }
            else if (selectStudy[0] == 2)
            {
                studyUi.SetActive(false);
                studingUi.SetActive(true);
                statUi.SetActive(true);
                startButton.SetActive(true); ;
                mainCamera.transform.position = new Vector3(0.0f, 10.0f, -100.0f);
                baby.transform.position = new Vector3(4.0f, 12.0f, 0.0f);
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
    protected void PlaySFX()
    {
            audioSource.clip = audioClips[effect];
            audioSource.Play();
    }
    public void GetStudyResult()
    {
        statUi.SetActive(false);
        selectCardUi.SetActive(true);
        studyResult[0].text = "���";
        studyResult[1].text = "ü��" + (playerHp - DefaultPlayerHp).ToString() + "�� "
            + "����" + (playerAttack - DefaultPlayerAttack).ToString() + "�� "
            + "���" + (playerDefense - DefaultPlayerDefense).ToString() + "��";
    }
    public void GoodStudy()
    {
        effect = 1;
        babydata.GetComponent<Animator>().SetTrigger("Attack");
        babydata.GetComponent<SFXVFX>().play += PlaySFX;
        babydata.GetComponent<SFXVFX>().play += delegate () { Instantiate(vfx[0], warriorTransform.position, Quaternion.identity); };

    }
    public void NomalStudy()
    {
        effect = 2;
        babydata.GetComponent<Animator>().SetTrigger("Attack");
        babydata.GetComponent<SFXVFX>().play += PlaySFX;
        babydata.GetComponent<SFXVFX>().play += delegate () { Instantiate(vfx[0], warriorTransform.position, Quaternion.identity); };
    }
    public void BadStudy()
    {
        effect = 3;
        babydata.GetComponent<Animator>().SetTrigger("Hurt");
        babydata.GetComponent<SFXVFX>().play += PlaySFX;
        babydata.GetComponent<SFXVFX>().play += delegate () { Instantiate(vfx[0], warriorTransform.position, Quaternion.identity); };
    }

    IEnumerator SwordStudy(int num)
    {
        cardImage[0].sprite = cardSprite[3];//Ȱ1,2,3
        cardImage[1].sprite = cardSprite[6];
        cardImage[2].sprite = cardSprite[7];
        for (int i = 0; i < num; i++)
        {
            SwordStudyResult();
            yield return new WaitForSeconds(1.0f);
        }
        GetStudyResult();
    }

    public void SwordStudyResult()
    {
        //weaponSprite.sprite = Resources.Load<Sprite>("Sprites/Weapon-Sword");
        babydata.AniInit(0);

        studingResult[0].text = "SWORD";
        int randomNum = Random.Range(0, 7);
        if (randomNum == 0)
        {
            playerHp++;
            studingResult[1].text = "ü�¡�";
            studingResult[2].text = "ü���� �پ���";
            NomalStudy();
        }
        else if (randomNum == 1)
        {
            playerAttack++;
            studingResult[1].text = "���ݡ�";
            studingResult[2].text = "������ ���ϰ� �� �� ����.";
            NomalStudy();
        }
        else if (randomNum == 2)
        {
            playerDefense++;
            studingResult[1].text = "����";
            studingResult[2].text = "�� ���ϰ� �� �� ����.";
            NomalStudy();
        }
        else if (randomNum == 3)
        {
            playerHp++;
            playerAttack++;
            studingResult[1].text = "ü�¡� ���ݡ�";
            studingResult[2].text = "������ ������ �����ߴ�.";
            NomalStudy();
        }
        else if (randomNum == 4)
        {
            playerHp++;
            playerDefense++;
            studingResult[1].text = "ü�¡� ����";
            studingResult[2].text = "�� ������ �����ߴ�.";
            NomalStudy();

        }
        else if (randomNum == 5)
        {
            playerDefense++;
            playerAttack++;
            playerHp++;
            studingResult[1].text = "ü�¡� ���ݡ� ����";
            studingResult[2].text = "�Ϻ��ϰ� �س´�.";
            GoodStudy();
        }
        else if (randomNum == 6)
        {
            studingResult[1].text = " - ";
            studingResult[2].text = "���� �׳� ������.";
            BadStudy();
        }
    }
    IEnumerator BowStudy(int num)
    {
        cardImage[0].sprite = cardSprite[4];//Ȱ1,2,3
        cardImage[1].sprite = cardSprite[14];
        cardImage[2].sprite = cardSprite[15];
        for (int i = 0; i < num; i++)
        {
            BowStudyResult();
            yield return new WaitForSeconds(1.0f);
        }
        GetStudyResult();
    }
    public void BowStudyResult()
    {
        //weaponSprite.sprite = Resources.Load<Sprite>("Sprites/Weapon-Bow");
        babydata.AniInit(1);

        studingResult[0].text = "BOW";
        int randomNum = Random.Range(0, 7);
        if (randomNum == 0)
        {
            playerHp++;
            studingResult[1].text = "ü�¡�";
            studingResult[2].text = "ü���� �پ���";
            NomalStudy();

        }
        else if (randomNum == 1 || randomNum == 4)
        {
            playerAttack++;
            studingResult[1].text = "���ݡ�";
            studingResult[2].text = "������ ���ϰ� �� �� ����.";
            NomalStudy();

        }
        else if (randomNum == 2 || randomNum == 5 || randomNum == 6)
        {
            studingResult[1].text = " - ";
            studingResult[2].text = "���� �׳� ������.";
            BadStudy();
        }
        else if (randomNum == 3)
        {
            playerHp++;
            playerAttack++;
            studingResult[1].text = "ü�¡� ���ݡ�";
            studingResult[2].text = "������ ������ �����ߴ�.";
            GoodStudy();

        }
    }
    IEnumerator MagicStudy(int num)
    {

        cardImage[0].sprite = cardSprite[5];//����1,2,3
        cardImage[1].sprite = cardSprite[22];
        cardImage[2].sprite = cardSprite[23];
        cardImage[3].sprite = cardSprite[24];
        cardImage[4].sprite = cardSprite[25];


        for (int i = 0; i < num; i++)
        {
            MagicStudyResult();
            yield return new WaitForSeconds(1.0f);
        }
        GetStudyResult();
    }
    public void MagicStudyResult()
    {
        //weaponSprite.sprite = Resources.Load<Sprite>("Sprites/Weapon-Wand");
        babydata.AniInit(2);
        studingResult[0].text = "Magic";
        int randomNum = Random.Range(0, 7);
        if (randomNum == 1 || randomNum == 4 || randomNum == 0)
        {
            playerAttack++;
            studingResult[1].text = "���ݡ�";
            studingResult[2].text = "�׷����� ���� ����.";
            NomalStudy();
        }
        else if (randomNum == 2 || randomNum == 5 || randomNum == 6)
        {
            studingResult[1].text = " - ";
            studingResult[2].text = "���� �׳� ������.";
            BadStudy();
        }
        else if (randomNum == 3)
        {
            playerAttack++;
            playerAttack++;
            studingResult[1].text = "���ݡ��";
            studingResult[2].text = "�Ϻ��ϰ� �����ߴ�.";
            GoodStudy();

        }
    }

    public void FirstCardBtn()
    {
        
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
        Debug.Log(cardState);
        Debug.Log(upgradeState);
    }
    public void UpgradeFirstCard(int num)
    {
        if (upgradeState == UpgradeState.firstSword)
        {
            upgradeBox.SetActive(true);
            upgradeBoxImage[0].sprite = cardSprite[8];
            upgradeBoxImage[1].sprite = cardSprite[11];
        }
        else if (upgradeState == UpgradeState.firstBow)
        {
            upgradeBox.SetActive(true);
            upgradeBoxImage[0].sprite = cardSprite[16];
            upgradeBoxImage[1].sprite = cardSprite[19];
        }
        else if (upgradeState == UpgradeState.firstMagic)
        {
            upgradeBox.SetActive(true);
            upgradeBoxImage[0].sprite = cardSprite[26];
            upgradeBoxImage[1].sprite = cardSprite[31];
        }
    }
    public void UpgradeSecondCard(int num)
    {
        if (upgradeState == UpgradeState.secondSword)
        {
            upgradeBox.SetActive(true);
            upgradeBoxImage[0].sprite = cardSprite[9];
            upgradeBoxImage[1].sprite = cardSprite[12];
        }
        else if (upgradeState == UpgradeState.secondBow)
        {
            upgradeBox.SetActive(true);
            upgradeBoxImage[0].sprite = cardSprite[17];
            upgradeBoxImage[1].sprite = cardSprite[20];
        }
        else if(upgradeState == UpgradeState.secondMagic)
        {
            upgradeBox.SetActive(true);
            upgradeBoxImage[0].sprite = cardSprite[27];
            upgradeBoxImage[1].sprite = cardSprite[32];
        }
    }

    public void UpgradeThirdCard(int num)
    {
        if (upgradeState == UpgradeState.thirdSword)
        {
            upgradeBox.SetActive(true);
            upgradeBoxImage[0].sprite = cardSprite[10];
            upgradeBoxImage[1].sprite = cardSprite[13];
        }
        else if (upgradeState == UpgradeState.thirdBow)
        {
            upgradeBox.SetActive(true);
            upgradeBoxImage[0].sprite = cardSprite[18];
            upgradeBoxImage[1].sprite = cardSprite[21];
        }
        else if (upgradeState == UpgradeState.thirdMagic)
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
                    messageText.text = "Cut+ī�带 �����.";
                    upgradeBox.SetActive(false);

                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Cutī�尡 �����ϴ�.";
                        upgradeBox.SetActive(false);
                        break;
                    }
                }
            }
            searchCount = 0;
        }
        else if (upgradeState == UpgradeState.firstBow)
        {
            Debug.Log("���°�Ȯ��");
            foreach (int a in cardList)
            {
                if (a == 4)
                {
                    cardList.Remove(4);
                    cardList.Add(16);
                    message.SetActive(true);
                    messageText.text = "Arrowshot+ī�带 �����.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Arrowshotī�尡 �����ϴ�.";
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
                    messageText.text = "Fireball+ī�带 �����.";
                    upgradeBox.SetActive(false);
                    break;
                }

                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Fireballī�尡 �����ϴ�.";
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
                    messageText.text = "Guard+ī�带 �����.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Guardī�尡 �����ϴ�.";
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
                    messageText.text = "Hawkeye+ī�带 �����.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Hawkeyeī�尡 �����ϴ�.";
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
                    messageText.text = "Electric shock+ī�带 �����.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Electric shockī�尡 �����ϴ�.";
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
                    messageText.text = "ShildStrike+ī�带 �����.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "ShildStrikeī�尡 �����ϴ�.";
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
                    messageText.text = "Snapshot+ī�带 �����.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Snapshotī�尡 �����ϴ�.";
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
                    messageText.text = "Ice bolt+ī�带 �����.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Ice boltī�尡 �����ϴ�.";
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
                    messageText.text = "Cut++ī�带 �����.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Cut+ī�尡 �����ϴ�.";
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
                    messageText.text = "Arrowshot++ī�带 �����.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Arrowshot+ī�尡 �����ϴ�.";
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
                    messageText.text = "Fireball++ī�带 �����.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Fireball+ī�尡 �����ϴ�.";
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
                    messageText.text = "Guard++ī�带 �����.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Guard+ī�尡 �����ϴ�.";
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
                    messageText.text = "Hawkeye++ī�带 �����.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Hawkeye+ī�尡 �����ϴ�.";
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
                    messageText.text = "Electric shock++ī�带 �����.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Electric shock+ī�尡 �����ϴ�.";
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
                    messageText.text = "ShildStrike++ī�带 �����.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "ShildStrike+ī�尡 �����ϴ�.";
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
                    messageText.text = "Snapshot++ī�带 �����.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Snapshot+ī�尡 �����ϴ�.";
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
                    messageText.text = "Ice bolt++ī�带 �����.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Ice bolt+ī�尡 �����ϴ�.";
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
                    UpgradeSecondCard(0);
                }
                else if (selectStudy[0] == 1)
                {
                    upgradeState = UpgradeState.secondBow;
                    UpgradeSecondCard(1);
                }
                else if (selectStudy[0] == 2)
                {
                    upgradeState = UpgradeState.secondMagic;
                    UpgradeSecondCard(2);
                }
            }
            else if (studyState == StudyState.second)
            {
                if (selectStudy[1] == 0)
                {
                    upgradeState = UpgradeState.secondSword;
                    UpgradeSecondCard(0);
                }
                else if (selectStudy[1] == 1)
                {
                    upgradeState = UpgradeState.secondBow;
                    UpgradeSecondCard(1);
                }
                else if (selectStudy[1] == 2)
                {
                    upgradeState = UpgradeState.secondMagic;
                    UpgradeSecondCard(2);
                }
            }
            else if (studyState == StudyState.third)
            {
                if (selectStudy[2] == 0)
                {
                    upgradeState = UpgradeState.secondSword;
                    UpgradeSecondCard(0);
                }
                else if (selectStudy[2] == 1)
                {
                    upgradeState = UpgradeState.secondBow;
                    UpgradeSecondCard(1);
                }
                else if (selectStudy[2] == 2)
                {
                    upgradeState = UpgradeState.secondMagic;
                    UpgradeSecondCard(2);
                }
            }
        }
        Debug.Log(studyState);
        Debug.Log(upgradeState);
        Debug.Log(selectStudy[0]);
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
        if (cardState == CardState.get)
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
        else
        {
            if (studyState == StudyState.first)
            {
                if (selectStudy[0] == 0)
                {
                    upgradeState = UpgradeState.thirdSword;
                    UpgradeThirdCard(0);
                }
                else if (selectStudy[0] == 1)
                {
                    upgradeState = UpgradeState.thirdBow;
                    UpgradeThirdCard(1);
                }
                else if (selectStudy[0] == 2)
                {
                    upgradeState = UpgradeState.thirdMagic;
                    UpgradeThirdCard(2);
                }
            }
            else if (studyState == StudyState.second)
            {
                if (selectStudy[1] == 0)
                {
                    upgradeState = UpgradeState.thirdSword;
                    UpgradeThirdCard(0);
                }
                else if (selectStudy[1] == 1)
                {
                    upgradeState = UpgradeState.thirdBow;
                    UpgradeThirdCard(1);
                }
                else if (selectStudy[1] == 2)
                {
                    upgradeState = UpgradeState.thirdMagic;
                    UpgradeThirdCard(2);
                }
            }
            else if (studyState == StudyState.third)
            {
                if (selectStudy[2] == 0)
                {
                    upgradeState = UpgradeState.thirdSword;
                    UpgradeThirdCard(0);
                }
                else if (selectStudy[2] == 1)
                {
                    upgradeState = UpgradeState.thirdBow;
                    UpgradeThirdCard(1);
                }
                else if (selectStudy[2] == 2)
                {
                    upgradeState = UpgradeState.thirdMagic;
                    UpgradeThirdCard(2);
                }
            }
        }
        Debug.Log(cardState);
        Debug.Log(upgradeState);
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
                        cardBordeName.text = "ī �� �� ȭ";
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
                            uiText.text = "��ȭ�� ī�尡 �����ϴ�.";
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
                        cardBordeName.text = "ī �� �� ȭ";
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
                            uiText.text = "��ȭ�� ī�尡 �����ϴ�.";
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
                        cardBordeName.text = "ī �� �� ȭ";
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
                            uiText.text = "��ȭ�� ī�尡 �����ϴ�.";
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
                        cardBordeName.text = "ī �� �� ȭ";
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
                            uiText.text = "��ȭ�� ī�尡 �����ϴ�.";
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
                        cardBordeName.text = "ī �� �� ȭ";
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
                            uiText.text = "��ȭ�� ī�尡 �����ϴ�.";
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
                        cardBordeName.text = "ī �� �� ȭ";
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
                            uiText.text = "��ȭ�� ī�尡 �����ϴ�.";
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
                        cardBordeName.text = "ī �� �� ȭ";
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
                            uiText.text = "��ȭ�� ī�尡 �����ϴ�.";
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
                        cardBordeName.text = "ī �� �� ȭ";
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
                            uiText.text = "��ȭ�� ī�尡 �����ϴ�.";
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
                        cardBordeName.text = "ī �� �� ȭ";
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
                            uiText.text = "��ȭ�� ī�尡 �����ϴ�.";
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
        cardBordeName.text = "ī �� ȹ ��";
        ChangeSprite();
        cardState = CardState.get;
    }
    public void ChangeSprite()
    {
        if(studyState==StudyState.first)
        {
            if(selectStudy[0]==0)
            {
                cardImage[0].sprite = cardSprite[3];//��0,1,2
                cardImage[1].sprite = cardSprite[6];
                cardImage[2].sprite = cardSprite[7];
            }
            else if (selectStudy[0] == 1)
            {
                cardImage[0].sprite = cardSprite[4];//Ȱ3,4,5
                cardImage[1].sprite = cardSprite[14];
                cardImage[2].sprite = cardSprite[15];
            }
            else if (selectStudy[0] == 2)
            {
                cardImage[0].sprite = cardSprite[5];//����6,7,8.9,10
                cardImage[1].sprite = cardSprite[22];
                cardImage[2].sprite = cardSprite[23];
                cardImage[3].sprite = cardSprite[24];
                cardImage[4].sprite = cardSprite[25];
            }
        }
        else if(studyState == StudyState.second)
        {
            if (selectStudy[1] == 0)
            {
                cardImage[0].sprite = cardSprite[3];//��0,1,2
                cardImage[1].sprite = cardSprite[6];
                cardImage[2].sprite = cardSprite[7];
            }
            else if (selectStudy[1] == 1)
            {
                cardImage[0].sprite = cardSprite[4];//Ȱ3,4,5
                cardImage[1].sprite = cardSprite[14];
                cardImage[2].sprite = cardSprite[15];
            }
            else if (selectStudy[1] == 2)
            {
                cardImage[0].sprite = cardSprite[5];//����6,7,8.9,10
                cardImage[1].sprite = cardSprite[22];
                cardImage[2].sprite = cardSprite[23];
                cardImage[3].sprite = cardSprite[24];
                cardImage[4].sprite = cardSprite[25];
            }
        }
        else if (studyState == StudyState.third)
        {
            if (selectStudy[2] == 0)
            {
                cardImage[0].sprite = cardSprite[3];//��0,1,2
                cardImage[1].sprite = cardSprite[6];
                cardImage[2].sprite = cardSprite[7];
            }
            else if (selectStudy[2] == 1)
            {
                cardImage[0].sprite = cardSprite[4];//Ȱ3,4,5
                cardImage[1].sprite = cardSprite[14];
                cardImage[2].sprite = cardSprite[15];
            }
            else if (selectStudy[2] == 2)
            {
                cardImage[0].sprite = cardSprite[5];//����6,7,8.9,10
                cardImage[1].sprite = cardSprite[22];
                cardImage[2].sprite = cardSprite[23];
                cardImage[3].sprite = cardSprite[24];
                cardImage[4].sprite = cardSprite[25];
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
                myScriptComponent.DataInit();

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


        // ���� SaveData() �޼��� ������ ����ȭ�� �����ʹ� DefaultCharacterData �ν��Ͻ��� info�Դϴ�.
        formatter.Serialize(fileStream, saveData);
        fileStream.Close();
        string dataPath = Application.persistentDataPath;
        Debug.Log("Persistent Data Path: " + dataPath);
        Debug.Log("ĳ���� �����͸� ���̳ʸ��� �����߽��ϴ�.");
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

            data.DataInit(dataLoad.chrId, dataLoad.chrName, dataLoad.chrLv, dataLoad.chrMaxHp, dataLoad.chrAttackDmg, dataLoad.chrDefense, dataLoad.weapon, dataLoad.imgName, dataLoad.chrCardIds);//ĳ���� �����)
            Debug.Log("ĳ���� �����͸� ���̳ʸ����� �ε��߽��ϴ�.");
            return 0;
        }
        else
        {
            // ���̳ʸ� ������ ������ ���ο� CharacterData Ŭ���� ����
            // ������ �ʱ�ȭ ������ ����

            data.DataInit(0, "test", 1, 20, 1, 1 
                        , WeaponType.SWORD, 0, cardList);
            

            Debug.Log("����� ĳ���� �����Ͱ� ��� ���� �����߽��ϴ�.");
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

            data.DataInit(dataLoad.chrId, dataLoad.chrName, dataLoad.chrLv, dataLoad.chrMaxHp, dataLoad.chrAttackDmg, dataLoad.chrDefense, dataLoad.weapon, dataLoad.imgName, dataLoad.chrCardIds);//ĳ���� �����)
            Debug.Log("ĳ���� �����͸� ���̳ʸ����� �ε��߽��ϴ�.");
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
            data.DataInit(dataLoad.chrId, dataLoad.chrName, dataLoad.chrLv, dataLoad.chrMaxHp, dataLoad.chrAttackDmg, dataLoad.chrDefense, dataLoad.weapon, dataLoad.imgName, dataLoad.chrCardIds);//ĳ���� �����)

            Debug.Log("ĳ���� �����͸� ���̳ʸ����� �ε��߽��ϴ�.");
            return 0;
        }
        else
        {
            return 1;
        }
    }

    // CharacterData Ŭ������ �����͸� ����
    public void SetCharacterData(int id, string name, int lv, int maxHp, int attackDmg, int defense, WeaponType weaponName, int img, List<int> chrCardnum1)//ĳ���� �����
    {
        data.DataInit(id, name, lv, maxHp, attackDmg, defense, weaponName, img, chrCardnum1);
    }

    // CharacterData Ŭ���� ��ȯ
    public CharacterData GetCharacterData()
    {
        return data;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class StudyUIManager : MonoBehaviour
{
    //몇번째 수업인지 확인용
    enum StudyState
    {
        first,
        second,
        third,
    }
    //카드획득인지 강화인지 확인용
    enum CardState
    {
        get,
        upgrade
    }
    //어떤 카드를 강화할 것인지 확인용
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
        fourthMagic,
        fifthMaigic
    }
    private StudyState studyState;
    private CardState cardState;
    private UpgradeState upgradeState;

    //캔버스에서 사용할 이미지 오브젝트
    public Image[] loadViewImage;
    public Image[] selectStudyImage;
    public Image[] upgradeBoxImage;
    public Image studyViewImage;
    public Image[] cardImage;

    //이미지 으브젝트에서 사용할 스프라이트 리소스
    public Sprite[] cardSprite;
    public Sprite[] studyViewResources;
    public Sprite[] selectStudyResources;
    
    //캐릭터가 사용할 무기 스프라이트
    public SpriteRenderer weaponSprite;

    //카메라 위치 이동을 위한 변수
    public Camera mainCamera;

    //캔버스에서 사용할 텍스트 오브젝트
    public Text[] saveInfo;
    public Text[] studyInfo;
    public Text[] studingResult;
    public Text[] studyResult;
    public Text cardBordeName;
    public Text messageText;
    public Text uiText;

    //육성중 재생할 효과음
    public AudioClip[] audioClips; 
    private AudioSource audioSource;

    //UI사용을 구현할 게임 오브젝트, SetActive를 이용해서 활성화, 비활성화한다.
    public GameObject startUI;
    public GameObject dataUI;
    public GameObject studyUI;
    public GameObject studingUI;
    public GameObject statUI;
    public GameObject selectCardUI;
    public GameObject startButton;
    public GameObject message;
    public GameObject cardUI;
    public GameObject getCardUI;
    public GameObject upgradeCardUI;
    public GameObject fstCard1;
    public GameObject fstCard2;
    public GameObject endStudy;
    public GameObject uiMessage;
    public GameObject upgradeBox;
    public GameObject[] vfx;

    //플레이어 오브젝트
    public GameObject baby;
    private Character babydata;

    //과목 선택 확인용 리스트
    private List<int> selectStudy;
    public List<Image> getCardList;

    //카드저장 확인 및 저장용 리스트
    private List<int> cardList;
    private List<GameObject> cardListData;

    //스텟 구현용 int 변수
    private int playerHp;
    private int playerAttack;
    private int playerDefense;
    private int DefaultPlayerHp;
    private int DefaultPlayerAttack;
    private int DefaultPlayerDefense;

    //메소드 구현용 변수
    private int studyViewNum;
    private int order;
    private int saveSlot;
    private int searchCount;
    private int effect;

    //데이터 저장 및 불러오기 용 변수
    private CharacterData data;
    private DefaultCharacterData saveData;

    //애니메이션 적용에 사용할 캐릭터 위치 변수
    Transform warriorTransform;

    public void Start()
    {
        DataInit();
        SaveSlot();
        studyReset();
        audioSource = GetComponent<AudioSource>();
    }

    //데이터 참조
    public void DataInit()
    {
        GameObject dataObject = GameObject.FindGameObjectWithTag("PlayerData");
        Debug.Log(dataObject);
        data = dataObject.GetComponent<CharacterData>();
    }

    //몇번 파일에 저장할지 저장하는 메소드
    public void SaveSlot()
    {
        string directoryPath = Path.Combine(Application.persistentDataPath);//검사할 경로
        int binaryFileCount = CountBinaryFilesInDirectory(directoryPath);
        Debug.Log("바이너리로 저장된 파일 개수: " + binaryFileCount);
        saveSlot = binaryFileCount;
    }
    //저장된 세이브파일이 몇개 있는지 카운트하는 메소드
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
    //바이너리 파일인지 판단하는 메소드
    bool IsBinaryFile(string filePath)
    {
        // 파일 확장자가 바이너리 파일에 해당하는지 여부를 체크합니다.
        // 예를 들어, 여기에서는 ".dat" 파일을 바이너리 파일로 간주합니다.
        string extension = Path.GetExtension(filePath).ToLower();
        return extension == ".dat"; // 원하는 확장자로 변경해주세요.
    }

    //변수 초기화
    public void studyReset()
    {
        studyViewNum = 0;
        studyViewImage.sprite = studyViewResources[studyViewNum];
        selectStudy = new List<int>();
        cardListData = new List<GameObject>();
        baby.transform.position = new Vector3(-19f, 14.0f, 0.0f);
        baby.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
        weaponSprite.sprite = null;
        mainCamera.transform.position = new Vector3(-20.0f, 10.0f, -100.0f);
        startUI.SetActive(true);
        dataUI.SetActive(false);
        studyUI.SetActive(false);
        studingUI.SetActive(false);
        selectCardUI.SetActive(false);
        statUI.SetActive(false);
        startButton.SetActive(false); ;
        message.SetActive(false);
        cardUI.SetActive(false);
        getCardUI.SetActive(false);
        upgradeCardUI.SetActive(false);
        endStudy.SetActive(false);
        studingResult[0].text = "1회차";
        studingResult[1].text = null;
        studingResult[2].text = null;
        studyState = StudyState.first;
        cardList = new List<int>();
        order = 0;
        searchCount = 0;
        warriorTransform = baby.transform.Find("Warrior");
        babydata = warriorTransform.GetComponent<Character>();
    }

    //스텟 초기화
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
    //캐릭터 최초생성시 기본카드 추가하는 메소드
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
    //시작버튼을 누르면 바이너리 파일을 돌면서 어떤것이 저장되어있는지 텍스트로 보여주는 메소드
    public void StartBtn()
    {
        baby.transform.position = new Vector3(-1f, 0.0f, 0.0f);
        baby.transform.localScale = new Vector3(1f, 1f, 1f);
        //weaponSprite.sprite = Resources.Load<Sprite>("Sprites/Weapon-Sword");
        babydata.AniInit(0);

        startUI.SetActive(false);
        dataUI.SetActive(true);
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
            saveInfo[0].text = "이름 " + data.chrName + "\n" + "레벨 " + data.chrLv.ToString() + "\n" + "카드 개수 " + data.chrCardnum.Count.ToString() + "\n" + "체력 " + data.chrMaxHp.ToString() + "\n" + "공격력 "
                + data.chrAttackDmg.ToString() + "\n" + "방어력 " + data.chrDefense.ToString();
            //loadViewImage[1].sprite = null;
            saveInfo[1].text = "데이터가 없습니다";
            //loadViewImage[2].sprite = null;
            saveInfo[2].text = "데이터가 없습니다";

        }
        else if (saveSlot == 2)
        {
            LoadCharacterData0();
            loadViewImage[0].sprite = selectStudyResources[data.imgNum];
            saveInfo[0].text = "이름 " + data.chrName + "\n" + "레벨 " + data.chrLv.ToString() + "\n" + "카드 개수 " + data.chrCardnum.Count.ToString() + "\n" + "체력 " + data.chrMaxHp.ToString() + "\n" + "공격력 "
                + data.chrAttackDmg.ToString() + "\n" + "방어력 " + data.chrDefense.ToString();
            LoadCharacterData1();
            loadViewImage[1].sprite = selectStudyResources[data.imgNum];
            saveInfo[1].text = "이름 " + data.chrName + "\n" + "레벨 " + data.chrLv.ToString() + "\n" + "카드 개수 " + data.chrCardnum.Count.ToString() + "\n" + "체력 " + data.chrMaxHp.ToString() + "\n" + "공격력 "
                + data.chrAttackDmg.ToString() + "\n" + "방어력 " + data.chrDefense.ToString();
            //loadViewImage[2].sprite = null;
            saveInfo[2].text = "데이터가 없습니다";
        }
        else if (saveSlot == 3)
        {
            LoadCharacterData0();
            loadViewImage[0].sprite = selectStudyResources[data.imgNum];
            saveInfo[0].text = "이름 "+data.chrName +"\n" + "레벨 " + data.chrLv.ToString() + "\n" +  "카드 개수 " + data.chrCardnum.Count.ToString() + "\n" + "체력 " + data.chrMaxHp.ToString() + "\n" + "공격력 "
                + data.chrAttackDmg.ToString() + "\n" + "방어력 " + data.chrDefense.ToString();
            LoadCharacterData1();
            loadViewImage[1].sprite = selectStudyResources[data.imgNum];
            saveInfo[1].text = "이름 " + data.chrName + "\n" + "레벨 " + data.chrLv.ToString() + "\n" + "카드 개수 " + data.chrCardnum.Count.ToString() + "\n" + "체력 " + data.chrMaxHp.ToString() + "\n" + "공격력 "
                + data.chrAttackDmg.ToString() + "\n" + "방어력 " + data.chrDefense.ToString();
            LoadCharacterData2();
            loadViewImage[2].sprite = selectStudyResources[data.imgNum];
            saveInfo[2].text = "이름 " + data.chrName + "\n" + "레벨 " + data.chrLv.ToString() + "\n" + "카드 개수 " + data.chrCardnum.Count.ToString() + "\n" + "체력 " + data.chrMaxHp.ToString() + "\n" + "공격력 "
                + data.chrAttackDmg.ToString() + "\n" + "방어력 " + data.chrDefense.ToString();
        }
    }
    //첫번쨰 파일을 불러오는 메소드
    public void LoadBtn0()
    {
        dataUI.SetActive(false);
        studyUI.SetActive(true);
        LoadCharacterData0();
        mainCamera.transform.position = new Vector3(0.0f, 0.0f, -100.0f);
        saveSlot = 0;
        startData();
    }
    //비어있는 두번째 슬롯을 누른다면 새로운 캐릭터를 생성하는 메소드

    public void LoadBtn1()
    {
        if (saveSlot == 1)
        {
            data.DataInit(0, "test", 1, 20, 1, 1
                        , WeaponType.SWORD, 0, cardList);
            saveSlot = 1;
            dataUI.SetActive(false);
            studyUI.SetActive(true);
            mainCamera.transform.position = new Vector3(0.0f, 0.0f, -100.0f);
        }
        else if (saveSlot > 1)
        {
            dataUI.SetActive(false);
            studyUI.SetActive(true);
            LoadCharacterData1();
            mainCamera.transform.position = new Vector3(0.0f, 0.0f, -100.0f);
            saveSlot = 1;
        }
        startData();
    }
    //비어있는 세번째 슬롯을 누른다면 새로운 캐릭터를 생성하는 메소드
    public void LoadBtn2()
    {
        if (saveSlot == 2)
        {
            data.DataInit(0, "test", 1, 20, 1, 1
                        , WeaponType.SWORD, 0, cardList);
            saveSlot = 2;
            dataUI.SetActive(false);
            studyUI.SetActive(true);
            mainCamera.transform.position = new Vector3(0.0f, 0.0f, -100.0f);
        }
        else if (saveSlot == 3)
        {
            dataUI.SetActive(false);
            studyUI.SetActive(true);
            LoadCharacterData2();
            mainCamera.transform.position = new Vector3(0.0f, 0.0f, -100.0f);
            saveSlot = 2;
        }
        startData();
    }

    //과목을 선택할때 이미지를 바꿔주는 메소드
    public void ChangeStudyImageBtn()
    {
        if (studyViewNum > 1)
        {
            studyViewNum = 0;
            studyViewImage.sprite = studyViewResources[studyViewNum];
            studyInfo[0].text = "SWORD";
            studyInfo[1].text = "공격과 방어의 밸런스가 특징이다.";
            studyInfo[2].text = "모든 스텟 ↑";
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
                studyInfo[1].text = "공격과 방어의 밸런스가 특징이다.";
                studyInfo[2].text = "모든 스텟 ↑";
                //weaponSprite.sprite = Resources.Load<Sprite>("Sprites/Weapon-Sword");
                babydata.AniInit(0);


            }
            else if (studyViewNum == 1)
            {
                studyInfo[0].text = "BOW";
                studyInfo[1].text = "강력한 공격에 특화되어있다.";
                studyInfo[2].text = "체력↑ 공격↑";
                //weaponSprite.sprite = Resources.Load<Sprite>("Sprites/Weapon-Bow");
                babydata.AniInit(1);

            }
            else if (studyViewNum == 2)
            {
                studyInfo[0].text = "MAGIC";
                studyInfo[1].text = "다섯 가지의 기술을 배울 수 있다.";
                studyInfo[2].text = "공격↑";
                //weaponSprite.sprite = Resources.Load<Sprite>("Sprites/Weapon-Wand");
                babydata.AniInit(2);


            }
        }
    }
    //과목을 선택했을때 무엇을 골랐는지 보여주는 메소드
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
    //선택한 과목을 지우는 메소드
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
    //3가지 과목을 선택했을때 다음 상황으로 넘어가는 메소드
    public void NextSceneBtn()
    {
        if (selectStudy.Count == 3)
        {
            if (selectStudy[0] == 0)
            {
                studyUI.SetActive(false);
                studingUI.SetActive(true);
                statUI.SetActive(true);
                startButton.SetActive(true); ;

                mainCamera.transform.position = new Vector3(0.0f, 10.0f, -100.0f);
                baby.transform.position = new Vector3(3.5f, 11.0f, 0.0f);
            }
            else if (selectStudy[0] == 1)
            {
                studyUI.SetActive(false);
                studingUI.SetActive(true);
                statUI.SetActive(true);
                startButton.SetActive(true); ;
                mainCamera.transform.position = new Vector3(0.0f, 10.0f, -100.0f);
                baby.transform.position = new Vector3(3.5f, 11.0f, 0.0f);
            }
            else if (selectStudy[0] == 2)
            {
                studyUI.SetActive(false);
                studingUI.SetActive(true);
                statUI.SetActive(true);
                startButton.SetActive(true); ;
                mainCamera.transform.position = new Vector3(0.0f, 10.0f, -100.0f);
                baby.transform.position = new Vector3(3.5f, 11.0f, 0.0f);
            }
        }
    }
    //어떤과목을 선택했는지 확인하고 시작하는 메소드

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
    //수업 코루틴을 실행하는 메소드

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
    //캐릭터 애니메이션 용 특수효과를 넣는 메소드
    protected void PlaySFX()
    {
            audioSource.clip = audioClips[effect];
            audioSource.Play();
    }
    //수업 종료 후 어떤 스텟이 올랐는지 보여주는 메소드
    public void GetStudyResult()
    {
        statUI.SetActive(false);
        selectCardUI.SetActive(true);
        studyResult[0].text = "결과";
        studyResult[1].text = "체력" + (playerHp - DefaultPlayerHp).ToString() + "↑ "
            + "공격" + (playerAttack - DefaultPlayerAttack).ToString() + "↑ "
            + "방어" + (playerDefense - DefaultPlayerDefense).ToString() + "↑";
    }
    //수업 결과에 따라 보여줄 애니메이션을 재생하는 메소드
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
    //수업간 스텟을 오르게하는 메소드
    IEnumerator SwordStudy(int num)
    {
        cardImage[0].sprite = cardSprite[3];//활1,2,3
        cardImage[1].sprite = cardSprite[6];
        cardImage[2].sprite = cardSprite[7];
        for (int i = 0; i < num; i++)
        {
            SwordStudyResult();
            yield return new WaitForSeconds(1.0f);
        }
        GetStudyResult();
    }
    //수업간 오른스텟을 확인하는 메소드
    public void SwordStudyResult()
    {
        //weaponSprite.sprite = Resources.Load<Sprite>("Sprites/Weapon-Sword");
        babydata.AniInit(0);

        studingResult[0].text = "SWORD";
        int randomNum = Random.Range(0, 7);
        if (randomNum == 0)
        {
            playerHp++;
            studingResult[1].text = "체력↑";
            studingResult[2].text = "체력이 붙었다";
            NomalStudy();
        }
        else if (randomNum == 1)
        {
            playerAttack++;
            studingResult[1].text = "공격↑";
            studingResult[2].text = "공격을 잘하게 된 것 같다.";
            NomalStudy();
        }
        else if (randomNum == 2)
        {
            playerDefense++;
            studingResult[1].text = "방어↑";
            studingResult[2].text = "방어를 잘하게 된 것 같다.";
            NomalStudy();
        }
        else if (randomNum == 3)
        {
            playerHp++;
            playerAttack++;
            studingResult[1].text = "체력↑ 공격↑";
            studingResult[2].text = "공격을 열심히 연습했다.";
            NomalStudy();
        }
        else if (randomNum == 4)
        {
            playerHp++;
            playerDefense++;
            studingResult[1].text = "체력↑ 방어↑";
            studingResult[2].text = "방어를 열심히 연습했다.";
            NomalStudy();

        }
        else if (randomNum == 5)
        {
            playerDefense++;
            playerAttack++;
            playerHp++;
            studingResult[1].text = "체력↑ 공격↑ 방어↑";
            studingResult[2].text = "완벽하게 해냈다.";
            GoodStudy();
        }
        else if (randomNum == 6)
        {
            studingResult[1].text = " - ";
            studingResult[2].text = "힘들어서 그냥 쉬었다.";
            BadStudy();
        }
    }
    IEnumerator BowStudy(int num)
    {
        cardImage[0].sprite = cardSprite[4];//활1,2,3
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
            studingResult[1].text = "체력↑";
            studingResult[2].text = "체력이 붙었다";
            NomalStudy();

        }
        else if (randomNum == 1 || randomNum == 4)
        {
            playerAttack++;
            studingResult[1].text = "공격↑";
            studingResult[2].text = "공격을 잘하게 된 것 같다.";
            NomalStudy();

        }
        else if (randomNum == 2 || randomNum == 5 || randomNum == 6)
        {
            studingResult[1].text = " - ";
            studingResult[2].text = "힘들어서 그냥 쉬었다.";
            BadStudy();
        }
        else if (randomNum == 3)
        {
            playerHp++;
            playerAttack++;
            studingResult[1].text = "체력↑ 공격↑";
            studingResult[2].text = "공격을 열심히 연습했다.";
            GoodStudy();

        }
    }
    IEnumerator MagicStudy(int num)
    {

        cardImage[0].sprite = cardSprite[5];//마법1,2,3
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
            studingResult[1].text = "공격↑";
            studingResult[2].text = "그럭저럭 배운것 같다.";
            NomalStudy();
        }
        else if (randomNum == 2 || randomNum == 5 || randomNum == 6)
        {
            studingResult[1].text = " - ";
            studingResult[2].text = "힘들어서 그냥 쉬었다.";
            BadStudy();
        }
        else if (randomNum == 3)
        {
            playerAttack++;
            playerAttack++;
            studingResult[1].text = "공격↑↑";
            studingResult[2].text = "완벽하게 이해했다.";
            GoodStudy();

        }
    }
    //카드를 얻을지 강화할지 판단하여 호출하는 메소드
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
    }

    //카드를 업그레이드할때 어떤 수업을 골랐는지 확인하고 그에 맞는 카드를 불러오는 메소드
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
    //업그레이드 할 카드가 있는지 판단하고 없으면 전화면으로, 있으면 그 카드를 리스트에서 지우고 업그레이드한 카드를 추가하는 메소드
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
        else if (upgradeState == UpgradeState.fourthMagic)
        {
            foreach (int a in cardList)
            {
                if (a == 24)
                {
                    cardList.Remove(24);
                    cardList.Add(29);
                    message.SetActive(true);
                    messageText.text = "Concentration+카드를 얻었다.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Concentration카드가 없습니다.";
                        upgradeBox.SetActive(false);
                        break;
                    }
                }
            }
            searchCount = 0;

        }
        else if (upgradeState == UpgradeState.fifthMaigic)
        {
            foreach (int a in cardList)
            {
                if (a == 25)
                {
                    cardList.Remove(25);
                    cardList.Add(30);
                    message.SetActive(true);
                    messageText.text = "Heal+카드를 얻었다.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Heal카드가 없습니다.";
                        upgradeBox.SetActive(false);
                        break;
                    }
                }
            }
            searchCount = 0;
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
        else if (upgradeState == UpgradeState.fourthMagic)
        {
            foreach (int a in cardList)
            {
                if (a == 29)
                {
                    cardList.Remove(29);
                    cardList.Add(34);
                    message.SetActive(true);
                    messageText.text = "Concentration++카드를 얻었다.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Concentration+카드가 없습니다.";
                        upgradeBox.SetActive(false);
                        break;
                    }
                }
            }
            searchCount = 0;

        }
        else if (upgradeState == UpgradeState.fifthMaigic)
        {
            foreach (int a in cardList)
            {
                if (a == 30)
                {
                    cardList.Remove(30);
                    cardList.Add(35);
                    message.SetActive(true);
                    messageText.text = "Heal++카드를 얻었다.";
                    upgradeBox.SetActive(false);
                    break;
                }
                else
                {
                    searchCount++;
                    if (searchCount == cardList.Count)
                    {
                        uiMessage.SetActive(true);
                        uiText.text = "Heal+카드가 없습니다.";
                        upgradeBox.SetActive(false);
                        break;
                    }
                }
            }
            searchCount = 0;
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
    }

    public void FourthCardBtn()
    {
        if (cardState == CardState.get)
        {
            GetFourthCard();
        }
        else
        {
            UpgradeFourthCard();
        }
    }
    public void FifthCardBtn()
    {
        if (cardState == CardState.get)
        {
            GetFifthCard();
        }
        else
        {
            UpgradeFifthCard();
        }
    }

    public void UpgradeFourthCard()
    {
        upgradeBox.SetActive(true);
        upgradeBoxImage[0].sprite = cardSprite[29];
        upgradeBoxImage[1].sprite = cardSprite[34];
        upgradeState = UpgradeState.fourthMagic;
    }
    public void UpgradeFifthCard()
    {
        upgradeBox.SetActive(true);
        upgradeBoxImage[0].sprite = cardSprite[30];
        upgradeBoxImage[1].sprite = cardSprite[35];
        upgradeState = UpgradeState.fifthMaigic;
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
    
    //처음 카드강화를 눌렀을때 강화 가능한 카드가 있는지 판단하는 메소드
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
                        cardUI.SetActive(true);
                        selectCardUI.SetActive(false);
                        getCardUI.SetActive(true);
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
                        cardUI.SetActive(true);
                        selectCardUI.SetActive(false);
                        getCardUI.SetActive(true);
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
                        cardUI.SetActive(true);
                        selectCardUI.SetActive(false);
                        getCardUI.SetActive(true);
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
                        cardUI.SetActive(true);
                        selectCardUI.SetActive(false);
                        getCardUI.SetActive(true);
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
                        cardUI.SetActive(true);
                        selectCardUI.SetActive(false);
                        getCardUI.SetActive(true);
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
                        cardUI.SetActive(true);
                        selectCardUI.SetActive(false);
                        getCardUI.SetActive(true);
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
                        cardUI.SetActive(true);
                        selectCardUI.SetActive(false);
                        getCardUI.SetActive(true);
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
                        cardUI.SetActive(true);
                        selectCardUI.SetActive(false);
                        getCardUI.SetActive(true);
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
                        cardUI.SetActive(true);
                        selectCardUI.SetActive(false);
                        getCardUI.SetActive(true);
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
    //CardUI를 활성화하는 메소드
    public void OpenGetCardBtn()
    {
        cardUI.SetActive(true);
        selectCardUI.SetActive(false);
        getCardUI.SetActive(true);
        fstCard1.SetActive(true);
        cardBordeName.text = "카 드 획 득";
        ChangeSprite();
        cardState = CardState.get;
    }
    //선택한 수업에 맞게 얻을 수 있는 카드 이미지를 변경하는 메소드
    public void ChangeSprite()
    {
        if(studyState==StudyState.first)
        {
            if(selectStudy[0]==0)
            {
                cardImage[0].sprite = cardSprite[3];//검0,1,2
                cardImage[1].sprite = cardSprite[6];
                cardImage[2].sprite = cardSprite[7];
            }
            else if (selectStudy[0] == 1)
            {
                cardImage[0].sprite = cardSprite[4];//활3,4,5
                cardImage[1].sprite = cardSprite[14];
                cardImage[2].sprite = cardSprite[15];
            }
            else if (selectStudy[0] == 2)
            {
                cardImage[0].sprite = cardSprite[5];//마법6,7,8.9,10
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
                cardImage[0].sprite = cardSprite[3];//검0,1,2
                cardImage[1].sprite = cardSprite[6];
                cardImage[2].sprite = cardSprite[7];
            }
            else if (selectStudy[1] == 1)
            {
                cardImage[0].sprite = cardSprite[4];//활3,4,5
                cardImage[1].sprite = cardSprite[14];
                cardImage[2].sprite = cardSprite[15];
            }
            else if (selectStudy[1] == 2)
            {
                cardImage[0].sprite = cardSprite[5];//마법6,7,8.9,10
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
                cardImage[0].sprite = cardSprite[3];//검0,1,2
                cardImage[1].sprite = cardSprite[6];
                cardImage[2].sprite = cardSprite[7];
            }
            else if (selectStudy[2] == 1)
            {
                cardImage[0].sprite = cardSprite[4];//활3,4,5
                cardImage[1].sprite = cardSprite[14];
                cardImage[2].sprite = cardSprite[15];
            }
            else if (selectStudy[2] == 2)
            {
                cardImage[0].sprite = cardSprite[5];//마법6,7,8.9,10
                cardImage[1].sprite = cardSprite[22];
                cardImage[2].sprite = cardSprite[23];
                cardImage[3].sprite = cardSprite[24];
                cardImage[4].sprite = cardSprite[25];
            }
        }
    }
    //획득,강화를 끝냈을때 UI를 정리하고 다음 상황을 준비하는 메소드
    public void CloseMessageBtn()
    {
        message.SetActive(false);
        cardUI.SetActive(false);
        getCardUI.SetActive(false);
        selectCardUI.SetActive(false);
        startButton.SetActive(true);
        statUI.SetActive(true);
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
            statUI.SetActive(false);
            studingUI.SetActive(false);
            mainCamera.transform.position = new Vector3(0.0f, -10.0f, -100.0f);
            DeckList();
        }
    }
    //강화확인 버튼을 눌렀을때 UI를 실행시키는 메소드
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
    //모든수업이 끝나고 생성한 카드를 생성하여 카드를 확인하는 메소드
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
            PolygonCollider2D colliderToRemove = getCard.GetComponent<PolygonCollider2D>();
            if (colliderToRemove != null)
            {
                Destroy(colliderToRemove);
            }
            else
            {
                Debug.Log("카드가 없습니다");
            }
            count++;
        }
    }

    //카드확인시 생성한 카드의 위치를 잡아주는 메소드
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

    //카드 생성시 List<int>인 카드리스트에서 ID를 불러와 리소스 폴더에 있는 프리팹들의 ID와 비교해서 카드리스트에 저장하는 메소드
    public GameObject FindPrefabById(int targetId)
    {
        string[] searchFolderPaths = new string[]
         {
            "Prefabs/Cards/Sword",
            "Prefabs/Cards/Bow",
            "Prefabs/Cards/Magic",
            "Prefabs/Cards/Default"
         };

        foreach (string searchFolderPath in searchFolderPaths)
        {
            GameObject[] prefabArray = Resources.LoadAll<GameObject>(searchFolderPath);
            foreach (GameObject prefab in prefabArray)
            {
                Card myScriptComponent = prefab.GetComponent<Card>();
                myScriptComponent.Init();
                if (myScriptComponent != null && myScriptComponent.info.Id == targetId)
                {
                    cardListData.Add(prefab);
                    myScriptComponent.DataInit();
                    return prefab;
                }
            }
        }

        return null;
    }
    //모든 작업이 긑나고 데이터를 저장한뒤 배틀씬으로 넘어가는 메소드
    public void EndStudy()
    {
        GameObject dataObject = GameObject.FindGameObjectWithTag("PlayerData");

        Debug.Log(dataObject);
        data = dataObject.GetComponent<CharacterData>();
        data.DataInit(0, "test", 1, playerHp, playerAttack, playerDefense, SelectWeaponType(), SelectSprite(), cardList);
        data.Init();
        data.ChangeData();
        SaveData();
        SceneManager.LoadScene("TestScene2");
    }

    //가지고있는 카드의 종류에 따라 캐릭터가 어떤 무기를 들어야 하는지 결정하는 메소드
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
    //캐릭터가 가진 카드에 따라 무기스프라이트를 변경하는 메소드
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

    //CaracterData에 저장한 변수를 직렬화하여 바이너리로 저장하는 메소드
    public void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string filePath = Path.Combine(Application.persistentDataPath, saveSlot.ToString()+".dat");
        FileStream fileStream = File.Create(filePath);
        // 이제 SaveData() 메서드 내에서 직렬화할 데이터는 DefaultCharacterData 인스턴스인 info입니다.
        saveData = data.info;
        formatter.Serialize(fileStream, saveData);
        fileStream.Close();
        string dataPath = Application.persistentDataPath;
    }
    
    //1번 데이터 파일을 불러오는 메소드
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

            data.DataInit(0, "test", 1, 20, 1, 1 
                        , WeaponType.SWORD, 0, cardList);
            

            Debug.Log("저장된 캐릭터 데이터가 없어서 새로 생성했습니다.");
            return 1;
        }
    }
    //2번 데이터 파일을 불러오는 메소드
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
    //3번 데이터 파일을 불러오는 메소드
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

    // CharacterData 클래스의 데이터를 입력하는 메소드
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


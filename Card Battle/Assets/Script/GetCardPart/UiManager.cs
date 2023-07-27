using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    //스터디씬 좌표 0,0
    //검술씬 0,10
    //궁술씬 20,10
    //마술씬 20,00
    public Image[] selectStudyImage;
    public Image studyViewImage;
    public Sprite[] studyViewResources;
    public Sprite[] selectStudyResources;
    private int studyViewNum;
    public Camera mainCamera;
    public Text[] studyInfo;
    public Text[] studyResult;
    public GameObject StudyUi;
    public GameObject StudingUi;
    public GameObject StudyResultUi;
    public GameObject StartButton;
    public Transform player;
    private List<int> slectStudy;
    private int playerHp;
    private int playerAttack;
    private int playerDepense;
    private List<int> cardList;
    private int DefaultPlayerHp;
    private int DefaultPlayerAttack;
    private int DefaultPlayerDepense;


    public void Awake()
    {
        studyReset();
    }

    public void studyReset()
    {
        studyViewNum = 0;
        studyViewImage.sprite = studyViewResources[studyViewNum];
        slectStudy = new List<int>();
        cardList = new List<int>();
        player.position = new Vector3(-0.86f, 0.56f, 0.0f);
        mainCamera.transform.position = new Vector3(0.0f, 0.0f, -100.0f);
        StudyUi.SetActive(true);
        StudingUi.SetActive(false);
        StudyResultUi.SetActive(false);
        playerHp = 20;
        playerAttack = 1;
        playerDepense = 1;
        DefaultPlayerHp = 20;
        DefaultPlayerAttack = 1;
        DefaultPlayerDepense = 1;
        studyResult[0].text = null;
        studyResult[1].text = null;
        studyResult[2].text = null;
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
                studyInfo[1].text = "다양한 종류의 마법을 배울 수 있다.";
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
            slectStudy.Add(studyViewNum);
        }
        else if (selectStudyImage[1].sprite == null)
        {
            selectStudyImage[1].sprite = selectStudyResources[studyViewNum];
            selectStudyImage[1].color = new Color(255, 255, 255, 255);
            slectStudy.Add(studyViewNum);
        }
        else if (selectStudyImage[2].sprite == null)
        {
            selectStudyImage[2].sprite = selectStudyResources[studyViewNum];
            selectStudyImage[2].color = new Color(255, 255, 255, 255);
            slectStudy.Add(studyViewNum);
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
        slectStudy.Clear();
    }
    public void NextSceneBtn()
    {
        if (slectStudy.Count==3)
        {
            if (slectStudy[0] == 0)
            {
                StudyUi.SetActive(false);
                StudingUi.SetActive(true);
                mainCamera.transform.position = new Vector3(0.0f, 10.0f, -100.0f);
                player.position = new Vector3(4.0f, 12.0f, 0.0f);
            }
            else if (slectStudy[0] == 1)
            {
                mainCamera.transform.position = new Vector3(0.0f, 10.0f, -100.0f);
            }
            else if (slectStudy[0] == 2)
            {
                mainCamera.transform.position = new Vector3(0.0f, 10.0f, -100.0f);
            }
        }
    }
    public void StudyStartBtn()
    {
        StartButton.SetActive(false);
        StartCoroutine(SwordStudy(5));
        StopCoroutine(SwordStudy(5));
    }
    public void GetStudyResult()
    {
        studyResult[1].text = "체력"+(playerHp- DefaultPlayerHp).ToString()+"증가했다"
            + "공격" + (playerAttack - DefaultPlayerAttack).ToString() + "증가했다"
            + "방어" + (playerDepense - DefaultPlayerDepense).ToString() + "증가했다";
        studyResult[2].text = null;
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
        Debug.Log("Result");
        int randomNum = Random.Range(0, 7);
        if (randomNum == 0)
        {
            playerHp++;
            studyResult[0].text = "SWORD";
            studyResult[1].text = "체력↑";
            studyResult[2].text = "체력이 붙었다";
        }
        else if (randomNum == 1)
        {
            playerAttack++;
            studyResult[0].text = "SWORD";
            studyResult[1].text = "공격↑";
            studyResult[2].text = "공격을 잘하게 된 것 같다.";
        }
        else if (randomNum == 2)
        {
            playerDepense++;
            studyResult[0].text = "SWORD";
            studyResult[1].text = "방어↑";
            studyResult[2].text = "방어를 잘하게 된 것 같다.";
        }
        else if (randomNum == 3)
        {
            playerHp++;
            playerAttack++;
            studyResult[0].text = "SWORD";
            studyResult[1].text = "체력↑ 공격↑";
            studyResult[2].text = "공격을 열심히 연습했다.";

        }
        else if (randomNum == 4)
        {
            playerHp++;
            playerDepense++;
            studyResult[0].text = "SWORD";
            studyResult[1].text = "체력↑ 방어↑";
            studyResult[2].text = "방어를 열심히 연습했다.";

        }
        else if (randomNum == 5)
        {
            playerDepense++;
            playerAttack++;
            playerHp++;
            studyResult[0].text = "SWORD";
            studyResult[1].text = "체력↑ 공격↑ 방어↑";
            studyResult[2].text = "완벽하게 해냈다.";
        }
        else if (randomNum == 6)
        {
            studyResult[0].text = "SWORD";
            studyResult[1].text = " - ";
            studyResult[2].text = "힘들어서 그냥 쉬었다.";
        }
    }
}

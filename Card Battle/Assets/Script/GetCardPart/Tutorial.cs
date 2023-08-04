using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject TutorialObject;
    public GameObject TutorialImageObject;
    public Image TutorialImage;
    public Sprite[] tutoSprite;
    private int page;


    public void CloseTuto()
    {
        TutorialObject.SetActive(false);
    }
    public void OpenTuto()
    {
        page = 0;
        TutorialImageObject.SetActive(true);
        TutorialImage.sprite = tutoSprite[page];

    }
    public void NextTuto()
    {
        if (page < 9)
        {
            page++;
            TutorialImage.sprite = tutoSprite[page];
        }
        else
        {
            TutorialObject.SetActive(false);
        }
    }
    public void PreviousTuto()
    {
        if (page > 0 && page < 9)
        {
            page--;
            TutorialImage.sprite = tutoSprite[page];
        }
    }
}

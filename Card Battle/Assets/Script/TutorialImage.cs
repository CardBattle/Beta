using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialImage : MonoBehaviour
{
    public Sprite[] tutorialImages;
    public Button battleSceneReturnBtn;
    public int sprite = 0;

    private void OnMouseDown()
    {
        ++sprite;

        if (sprite == 8)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = tutorialImages[sprite];          
            battleSceneReturnBtn.gameObject.SetActive(true);
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
        else
        {          
            gameObject.GetComponent<SpriteRenderer>().sprite = tutorialImages[sprite];
        }       
    }

    private void OnEnable()
    {
        sprite = 0;
        gameObject.GetComponent<SpriteRenderer>().sprite = tutorialImages[sprite];
        gameObject.GetComponent<Collider2D>().enabled = true;
    }
}

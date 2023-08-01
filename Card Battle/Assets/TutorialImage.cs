using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialImage : MonoBehaviour
{
    public Sprite[] tutorialImages;
    private int sprite = 0;

    private void OnMouseDown()
    {
        if (sprite == 6)
        {
            gameObject.SetActive(false);

        }
        else
        {
            ++sprite;

            gameObject.GetComponent<SpriteRenderer>().sprite = tutorialImages[sprite];
        }       
    }
}

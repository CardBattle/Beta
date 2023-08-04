using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer main;
    [SerializeField]
    private SpriteRenderer object1;
    [SerializeField]
    private SpriteRenderer object2;

    [SerializeField]
    private List<Sprite> imageList;

    public void ChangeMap1()
    {
        main.sprite = imageList[0];
        object1.sprite = imageList[1];
        object2.sprite = imageList[2];
    }
    public void ChangeMap2()
    {
        main.sprite = imageList[3];
        object1.sprite = imageList[4];
        object2.sprite = imageList[5];
    }
    public void ChangeMap3()
    {
        main.sprite = imageList[6];
        object1.sprite = imageList[7];
        object2.sprite = imageList[8];
    }
    public void ChangeMap4()
    {
        main.sprite = imageList[9];
        object1.sprite = imageList[10];
        object2.sprite = imageList[11];
    }
    private void Start()
    {
        ChangeMap();
    }
    public void ChangeMap()
    {
        
        int a = Random.Range(0, 3);
        Debug.Log(a);
        switch (a)
        {
            case 0:
                ChangeMap1();
                break;
            case 1:
                ChangeMap2();
                break;
            case 2:
                ChangeMap3();
                break;
            default:
                ChangeMap4();
                break;
        }
    }
}


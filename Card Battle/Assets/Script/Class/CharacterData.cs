using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    public DefaultCharacterData info;
    public StudyUIManager ui;

    public int chrId;
    public string chrName;
    public int chrLv;
    public int chrMaxHp;
    public int chrAttackDmg;
    public int chrDefense;
    public List<GameObject> chrCard;
    public List<int> chrCardnum;
    public WeaponType weapon;
    public Sprite img;
    public int imgNum;


    private void Start()
    {
        int count = FindObjectsOfType<CharacterData>().Length;
        if (count > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void DataInit(int id, string name, int lv, int maxHp, int attackDmg, int defense, WeaponType weaponName, int img, List<int> chrCardnum1)//캐릭터 저장용
    {
        chrId = id;
        chrName = name;
        chrLv = lv;
        chrMaxHp = maxHp;
        chrAttackDmg = attackDmg;
        chrDefense = defense;
        chrCardnum = chrCardnum1;
        weapon = weaponName;
        imgNum = img;
    }
    public void LoadDataInit(int id, string name, int lv, int maxHp, int attackDmg, int defense, WeaponType weaponName, int img, List<int> chrCardnum1)//캐릭터 저장용
    {
        chrId = id;
        chrName = name;
        chrLv = lv;
        chrMaxHp = maxHp;
        chrAttackDmg = attackDmg;
        chrDefense = defense;
        chrCardnum = chrCardnum1;
        weapon = weaponName;
        imgNum = img;
    }

    public void ChangeData()
    {
        GameObject dataObject = GameObject.FindGameObjectWithTag("StudyUIManager");
        StudyUIManager data = dataObject.GetComponent<StudyUIManager>();
        img = data.studyViewResources[imgNum];
        foreach(int a in chrCardnum)
        {
            chrCard.Add(data.FindPrefabById(a));
        }
    }

    public void Init()//바이너리 저장용
    {
        info = new(chrId, chrName, chrLv, chrMaxHp, chrAttackDmg, chrDefense, chrCardnum, weapon, imgNum);
        Debug.Log(info.chrId);
        Debug.Log(info.chrMaxHp);
    }
}

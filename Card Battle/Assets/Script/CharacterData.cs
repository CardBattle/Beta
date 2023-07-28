using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    public DefaultCharacterData info;
    public int chrId = 1;
    public string chrName = "test1";
    public int chrLv = 1;
    public int chrMaxHp = 20;
    public int chrAttackDmg = 1;
    public int chrDefense = 1;
    public List<GameObject> chrCard;
    public List<int> chrCardnum;

    public WeaponType weapon = WeaponType.SWORD;
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

    public void DataInit(int id, string name, int lv, int maxHp, int attackDmg, int defense, List<GameObject> cardList, WeaponType weaponName, Sprite imgNum, List<int> chrCardnum)
    {
        chrId = id;
        chrName = name;
        chrLv = lv;
        chrMaxHp = maxHp;
        chrAttackDmg = attackDmg;
        chrDefense = defense;
        chrCard = cardList;
        weapon = weaponName;
        img = imgNum;
    }

    public void Init()
    {
        info = new(chrId, chrName, chrLv, chrMaxHp, chrAttackDmg, chrDefense, chrCardnum, weapon, imgNum);
        Debug.Log(info.chrId);
        Debug.Log(info.chrMaxHp);
    }
}

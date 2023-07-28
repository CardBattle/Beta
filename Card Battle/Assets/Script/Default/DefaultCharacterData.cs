using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DefaultCharacterData
{
    public int chrId;
    public string chrName;
    public int chrLv;
    public int chrMaxHp;
    public int chrAttackDmg;
    public int chrDefense;
    public List<int> chrCardIds;
    public WeaponType weapon;
    public int imgName;

    public void DataInit(int id, string name, int lv, int maxHp, int attackDmg, int defense, List<int> cardList, WeaponType weaponName, int imgNum)
    {
        chrId = id;
        chrName = name;
        chrLv = lv;
        chrMaxHp = maxHp;
        chrAttackDmg = attackDmg;
        chrDefense = defense;
        chrCardIds = cardList;
        weapon = weaponName;
        imgName = imgNum;
    }

    public DefaultCharacterData(int id, string name, int level, int maxHp, int attackDmg, int defense, List<int> cards, WeaponType weapon, int img)
    {
        this.chrId = id;
        this.chrName = name;
        this.chrLv = level;
        this.chrMaxHp = maxHp;
        this.chrAttackDmg = attackDmg;
        this.chrDefense = defense;
        this.chrCardIds = cards;
        this.weapon = weapon;
        this.imgName = img;
    }
}

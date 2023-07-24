using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    public int chrId = 1;
    public string chrName = "test1";
    public int chrLv = 1;
    public int chrMaxHp = 20;
    public int chrAttackDmg = 1;
    public int chrDefense = 1;
    public List<int> chrCard = new List<int> { 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3 };
    public string WeaponName = "SWORD";
    public int imgNum = 1;
}

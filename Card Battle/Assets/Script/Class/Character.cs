using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public DefaultCharacter info;

    [SerializeField]
    private string _name;
    [SerializeField]
    private int id;
    [SerializeField]
    private int level;
    [SerializeField]
    private int defense;
    [SerializeField]
    private int hp;
    [SerializeField]
    private int attackDmg;
    [SerializeField]
    private WeaponType weapon;
    [SerializeField]
    private Sprite img;
    
    public List<Buff> buffs;

    public Data cardDATA;
    public CharacterData data;

    

    public List<GameObject> cards; //카드 매니저에서 캐릭터가 소유한 카드 프리팹을 접근해야하기 때문에 public


    Animator anim;

    public void CharDATA()
    {

        cards.Clear();
        _name = data.chrName;
        id = data.chrId;
        hp = data.chrMaxHp;
        level = data.chrLv;
        defense = data.chrDefense;
        attackDmg = data.chrAttackDmg;
        
        foreach(var card in data.chrCard)
        {
            cards.Add(cardDATA.cardPrefabs[card]);
        }
    }

    public void Init()
    {
        buffs = new List<Buff>();
        info = new(id, _name, level, hp, attackDmg, defense, cards, buffs, weapon, img);
        anim = GetComponent<Animator>();
    }

    public void DieAnim()
    {
        anim.SetBool("Die", true);
    }

    public void DrawAnim()
    {
        anim.SetTrigger("Draw");
    }

}

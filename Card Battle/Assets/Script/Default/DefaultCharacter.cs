using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum WeaponType
{
    DEFAULT,
    SWORD,
    BOW,
    WAND,
    BOSS
}
public class DefaultCharacter
{
    //열거형을 이용해서 직업이랑 무기 구분


    int id; // 플레이어 직업 구분
    public int Id { get => id; }

    WeaponType weapon; // 무기 타입
    public WeaponType Weapon { get => weapon; set => weapon = value; }

    string name; // 플레이어 닉네임
    public string Name { get => name; set => name = value; }

    int level; //플레이어의 레벨
    public int Level { get => level; set => level = value; }

    public int defense;

    public int Defense { get => defense; set => defense = value; } //플레이어 방어력

    int maxHp;
    public int MaxHp { get => hp; set => hp = value; }

    int hp; //캐릭터 체력 

    public int Hp
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value <= 0 ? 0 : (value > MaxHp ? hp = MaxHp : hp = value);
        }
    }

    int attackDmg; // 캐릭터 공격력

    public int AttackDmg { get => attackDmg; set => value = attackDmg; }//공력력도 다시 확인

    public List<GameObject> cards; //플레이어가 가지고있는 덱 확인

    public List<Buff> buffs;

    Sprite img;
    public Sprite Img { get => img; set => img = value; }

    public delegate void Use();

    public DefaultCharacter(int id, string name, int level, int maxHp, int attackDmg, int defense, List<GameObject> cards, List<Buff> buffs, WeaponType weapon, Sprite img)
    {
        this.id = id;
        this.name = name;
        this.level = level;
        this.maxHp = maxHp;
        hp = this.maxHp;
        this.attackDmg = attackDmg;
        this.defense = defense;
        this.cards = cards;
        this.buffs = buffs;
        this.weapon = weapon;
        this.img = img;
    }
}

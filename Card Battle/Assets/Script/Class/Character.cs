using System.Collections.Generic;
using UnityEditor.Animations;
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
    private SpriteRenderer weaponSprite;
    [SerializeField]
    private Sprite img;

    public List<Buff> buffs;

    public Data cardDATA;
    public CharacterData data;



    public List<GameObject> cards; //카드 매니저에서 캐릭터가 소유한 카드 프리팹을 접근해야하기 때문에 public


    Animator anim;

    public void CharDATA()
    {
        _name = data.chrName;
        id = data.chrId;
        hp = data.chrMaxHp;
        level = data.chrLv;
        defense = data.chrDefense;
        attackDmg = data.chrAttackDmg;

        foreach (var card in data.chrCard)
        {
            cards.Add(cardDATA.cardPrefabs[card]);
        }
    }

    public void Init()
    {
        buffs = new List<Buff>();
        info = new(id, _name, level, hp, attackDmg, defense, cards, buffs, weapon, img);
        anim = GetComponent<Animator>();
        
        switch (weapon)
        {
            case WeaponType.BOW:
                weaponSprite.sprite = Resources.Load<Sprite>("Sprites/Weapon-Bow");
                anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Anim/Bow");
                break;
            case WeaponType.SWORD:
                weaponSprite.sprite = Resources.Load<Sprite>("Sprites/Weapon-Sword");
                anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Anim/Sword");
                break;
            case WeaponType.WAND:
                weaponSprite.sprite = Resources.Load<Sprite>("Sprites/Weapon-Wand");
                anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Anim/Wand");
                break;
            default:
                return;
        }
        ;
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

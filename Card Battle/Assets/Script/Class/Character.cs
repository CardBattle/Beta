using System.Collections;
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
        if (GameObject.FindGameObjectWithTag("PlayerData") != null)//플레이어데이터 테스트용 if문 빌드시 else를 삭제
        {
            cards.Clear();
            GameObject dataObject = GameObject.FindGameObjectWithTag("PlayerData");
            data = dataObject.GetComponent<CharacterData>();
            _name = data.chrName;
            id = data.chrId;
            hp = data.chrMaxHp;
            level = data.chrLv;
            defense = data.chrDefense;
            attackDmg = data.chrAttackDmg;
            cards = data.chrCard;
            weapon = data.weapon;
        }
        else
        {
            _name = "test";
            id = 1;
            hp = 20;
            level = 1;
            defense = 1;
            attackDmg = 1;
            weapon = WeaponType.SWORD;
        }
        /*
        cards.Clear();

        foreach (var card in data.chrCard)
        {
            cards.Add(cardDATA.cardPrefabs[card]);
        }*/
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

    public IEnumerator HurtAnim()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<Animator>().SetTrigger("Hurt");
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

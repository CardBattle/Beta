using System.Collections;
using UnityEngine;
public class CardUse : MonoBehaviour
{
    [HideInInspector]
    public Card card;
    [HideInInspector]
    public AudioSource sfx;
    public GameObject vfx;

    protected Card senderCard;
    protected Card receiverCard;

    public void Init()
    {
        card = GetComponent<Card>();

        sfx = GetComponent<AudioSource>();
        if (card.info.Property == PropertyType.ATTACK)
            card.info.use += AttackAnim;
        else card.info.use += DefenseAnim;
        
        card.info.use += Use;
    }

    public virtual void Use(Character sender, Character receiver)
    {
        BattleManager bm = BattleManager.Bm;
        if (sender.tag == "Player")
        {
            senderCard = bm.playerDecision.card;
            receiverCard = bm.enemyDecision.card;
        }
        else
        {
            senderCard = bm.enemyDecision.card;
            receiverCard = bm.playerDecision.card;
        }

        if (card.info.buffs.Count > 0)
        {
            bool isExist = false;
            foreach (var buff in card.info.buffs)
            {
                Character c;
                if (buff.info.Type == BuffType.BUFF)
                    c = sender;
                else
                    c = receiver;
                foreach (var exist in c.info.buffs)
                {
                    if (exist.info.Id == buff.info.Id)
                    {
                        isExist = true;
                        exist.info.CurrentTurn += buff.info.Turns;
                        break;
                    }
                }
                if (!isExist)
                    c.info.buffs.Add(buff);
            }
        }
        sender.GetComponent<SFXVFX>().play += PlaySFX;
        if (card.info.Property == PropertyType.ATTACK)
            sender.GetComponent<SFXVFX>().play += delegate () { Instantiate(vfx, receiver.transform.position, Quaternion.identity); };
        else
            sender.GetComponent<SFXVFX>().play += delegate () { Instantiate(vfx, sender.transform.position, Quaternion.identity); };
    }

    protected void PlaySFX()
    {
        if(sfx != null)
            sfx.Play();
    }

    protected void AttackAnim(Character sender, Character receiver)
    {    
        sender.GetComponent<Animator>().SetTrigger("Attack");

        StartCoroutine(receiver.HurtAnim());
    }

    protected void DefenseAnim(Character sender, Character receiver)
    {
        sender.GetComponent<Animator>().SetTrigger("Buff");
    }

    protected int CalculateDmg(int attackDmg, int dice, int effVal, float effectiveness)
    {
        return (int)((attackDmg + dice + effVal) * effectiveness);
    }

    /// <summary>
    /// type1에는 사용되는 카드의 타입을, type2에는 공격받는 캐릭터의 무기 타입을 넣는다.
    /// </summary>
    /// <param name="type1">사용한 카드의 타입</param>
    /// <param name="type2">공격받는 캐릭터의 타입</param>
    /// <returns>상성이 계산된 float값 리턴</returns>
    protected float CalculateEffect(WeaponType type1, WeaponType type2)
    {
        if (type2 == WeaponType.BOSS) return 0.5f;

        if (type1 == WeaponType.DEFAULT || type2 == WeaponType.DEFAULT) return 1f;
        if (type1 == WeaponType.SWORD)
        {
            if (type2 == WeaponType.WAND) return 0.5f;
            if (type2 == WeaponType.BOW) return 2f;
        }
        if (type1 == WeaponType.BOW)
        {
            if (type2 == WeaponType.SWORD) return 0.5f;
            if (type2 == WeaponType.WAND) return 2f;
        }
        if (type1 == WeaponType.WAND)
        {
            if (type2 == WeaponType.BOW) return 0.5f;
            if (type2 == WeaponType.SWORD) return 2f;
        }

        return 1f;
    }

}

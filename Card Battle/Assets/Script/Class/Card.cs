using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;
using TMPro;
using DG.Tweening;

public class Card : MonoBehaviour
{
    public DefaultCard info;

    [SerializeField]
    private int id;
    [SerializeField]
    private PropertyType property;
    [SerializeField]
    private WeaponType type;
    [SerializeField]
    private string _name;
    [SerializeField]
    private string _buff;
    [SerializeField]
    private int effVal;
    [SerializeField]
    private int level;
    [SerializeField]
    private Sprite cardSpaceimg;
    [SerializeField]
    private Sprite cardimg;
    [SerializeField]
    private Sprite cardTypeSpaceimg;
    [SerializeField]
    private Sprite cardNameSpaceimg;
    [SerializeField]
    private Sprite cardBuffimg;
    [SerializeField]
    private Sprite cardLevelimg;
    [SerializeField]
    private Sprite cardTypeimg;
    [SerializeField]
    private Sprite cardEffValimg;

    [SerializeField]
    private Sprite frontCard;
    [SerializeField]
    private Sprite backCard;
    [SerializeField]
    private List<Buff> buffs;

    [SerializeField]
    private SpriteRenderer CardSpaceimg;
    [SerializeField]
    private SpriteRenderer Cardimg;
    [SerializeField]
    private SpriteRenderer CardLevelimg;
    [SerializeField]
    private SpriteRenderer CardNameSpaceimg;
    [SerializeField]
    private SpriteRenderer CardTypeSpaceimg;
    [SerializeField]
    private SpriteRenderer CardTypeimg;
    [SerializeField]
    private SpriteRenderer CardBuffimg;
    [SerializeField]
    private SpriteRenderer CardEffValimg;
    [SerializeField]
    private TextMeshPro _Name;
    [SerializeField]
    private TextMeshPro EffVal;
    [SerializeField]
    private TextMeshPro Buff;
    [SerializeField]
    private TextMeshPro Level;
    // 내 카드인지 아닌지 체크하는 변수
    private bool myCard;

    // 카드위치를 바꾸기 위한 변수
    public bool cardSelect;
    // 카드 윈래 위치를 저장하는 클래스
    public PRS originPRS;
    public void Init()
    {

        buffs = GetComponents<Buff>().ToList();
        foreach (Buff buff in buffs)
        {
            if (buff != null)
                buff.Init();
        }
        info = new(id, property, type, _name, buffs, effVal, cardimg);

        GetComponent<CardUse>().Init();
    }

    private void OnMouseEnter()
    {
        if (myCard && BattleManager.Bm.state != BattleManager.State.CardDecision)
            BattleManager.Bm.CardMouseEnter();
    }
    private void OnMouseOver()
    {
        if (myCard && BattleManager.Bm.state != BattleManager.State.CardDecision)
            BattleManager.Bm.CardMouseOver(this);
        else
            BattleManager.Bm.CardMouseExit(this);
    }
    private void OnMouseExit()
    {
        if (myCard && BattleManager.Bm.state != BattleManager.State.CardDecision)
            BattleManager.Bm.CardMouseExit(this);
    }
    private void OnMouseDown()
    {
        if (myCard)
        {
            if (cardSelect)
            {
                BattleManager.Bm.CardSelectDown(this);
            }
            else
            {
                BattleManager.Bm.CardMoustDown(this);
            }
        }
    }

    public void CardFront(bool myCard)
    {
        this.myCard = myCard;

        if (myCard)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = frontCard;
            CardSpaceimg.sprite = cardSpaceimg;
            Cardimg.sprite = cardimg;
            CardNameSpaceimg.sprite = cardNameSpaceimg;
            CardBuffimg.sprite = cardBuffimg;
            CardLevelimg.sprite = cardLevelimg;
            CardEffValimg.sprite = cardEffValimg;
            CardTypeSpaceimg.sprite = cardTypeSpaceimg;
            CardTypeimg.sprite = cardTypeimg;
            
           
            _Name.text = _name;
            Buff.text = _buff;
            Level.text = level.ToString();
            EffVal.text = effVal.ToString();        
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = backCard;
            _Name.text = " ";
            EffVal.text = " ";
            Buff.text = " ";
            Level.text = " ";         
        }
    }

    public void EnemyCardFront()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = frontCard;
        CardSpaceimg.sprite = cardSpaceimg;
        Cardimg.sprite = cardimg;
        CardNameSpaceimg.sprite = cardNameSpaceimg;
        CardBuffimg.sprite = cardBuffimg;
        CardLevelimg.sprite = cardLevelimg;
        CardEffValimg.sprite = cardEffValimg;
        CardTypeSpaceimg.sprite = cardTypeSpaceimg;
        CardTypeimg.sprite = cardTypeimg;


        _Name.text = _name;
        Buff.text = _buff;
        Level.text = level.ToString();
        EffVal.text = effVal.ToString();
    }
    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if (useDotween)
        {
            transform.DOMove(prs.pos, dotweenTime);
            transform.DORotateQuaternion(prs.rot, dotweenTime);
            transform.DOScale(prs.scale, dotweenTime);
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }
    public void Test()
    {
        Debug.Log($"id: {info.Id}\ntype:{info.Type}\nname: {info.Name}\n" +
        $"level: {info.Level}\nimg: {info.Img}");
        if (buffs.Count > 0)
            foreach (Buff buff in buffs)
            {
                Debug.Log($"buff : {buff.info.Name}");
            }
        else
            Debug.Log("No buffs");
    }
}

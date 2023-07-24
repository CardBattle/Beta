using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public DefaultBuff info;

    [SerializeField]
    private int id;
    [SerializeField]
    private BuffType type;
    [SerializeField]
    private string _name;
    [SerializeField]
    private int turns;
    public BuffUse buffUse;

    public void Init()
    {
        info = new(id, type, _name, turns);

        buffUse.Init(this);
    }

    public void BuffCheck(Character character)
    {
        if (info.CurrentTurn <= 0)
        {
            character.info.buffs.Remove(this);
        }
    }
}

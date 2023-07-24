using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BuffUse : MonoBehaviour
{
    Buff buff;
    public void Init(Buff buff)
    {
        this.buff = buff;
        this.buff.info.use = Use;
    }

    public virtual void Use(Character character)
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnPlus : BuffUse
{
    public override void Use(Character character)
    {
        character.info.Hp -= 2;
        Debug.Log("BurnPlus");
    }
}

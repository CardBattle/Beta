using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnPlusPlus : BuffUse
{
    public override void Use(Character character)
    {
        character.info.Hp -= 4;

        Debug.Log("Burn");
    }
}

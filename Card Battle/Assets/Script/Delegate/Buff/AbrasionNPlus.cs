using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbrasionNPlus : BuffUse
{
    public override void Use(Character character)
    {
        character.info.Hp -= 2;

        Debug.Log("AbrasionN");
    }
}

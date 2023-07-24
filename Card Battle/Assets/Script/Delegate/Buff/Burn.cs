using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : BuffUse
{
    public override void Use(Character character)
    {
        character.info.Hp -= 1;

        Debug.Log("Burn");
    }
}

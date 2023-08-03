using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuts : BuffUse
{
    public override void Use(Character character)
    {
        character.info.Hp -= 3;
        Debug.Log("Cuts");
    }
}

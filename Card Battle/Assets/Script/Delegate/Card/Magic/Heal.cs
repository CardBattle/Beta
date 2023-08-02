using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : CardUse
{
    public override void Use(Character sender, Character receiver)
    {
        base.Use(sender, receiver);
        
       /* sender.cardDATA.

        print(sender.info.Hp);*/
    }

}

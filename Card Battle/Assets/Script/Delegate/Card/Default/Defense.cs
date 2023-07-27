using UnityEngine;

public class Defense : CardUse
{
    public override void Use(Character sender, Character receiver)
    {
        base.Use(sender, receiver);

        if (senderCard.info.Dice < receiverCard.info.Dice)
            receiverCard.info.EffVal -= 1;
        else receiverCard.info.EffVal -= 2;
    }

}

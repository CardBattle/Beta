using UnityEngine;

public class Cut : CardUse
{
    public override void Use(Character sender, Character receiver)
    {
        base.Use(sender, receiver);

        receiver.info.Hp -= CalculateDmg(sender.info.AttackDmg, card.info.RandomDice, card.info.EffVal,
        CalculateEffect(card.info.Type, receiver.info.Weapon));

        Debug.Log("Cut");
    }

}

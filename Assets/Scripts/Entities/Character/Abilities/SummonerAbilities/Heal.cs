using UnityEngine;

public class Heal : SelfTargeted, SummonerAbility
{
    protected Heal()
    {
        cooldown = 10;

        startCooldownOnStartAbilityCast = true;

        CanBeCastAtAnytime = true;
    }

    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        GetComponent<CharacterStats>().Health.Restore(150);

        FinishAbilityCast();
    }
}

using UnityEngine;

public class Heal : SelfTargeted, SummonerAbility
{
    protected Heal()
    {
        cooldown = 10;

        startCooldownOnAbilityCast = true;

        CanBeCastAtAnytime = true;
    }

    protected override void SetAbilitySpritePath()
    {
        abilitySpritePath = "Sprites/SummonerAbilities/Heal";
    }

    public override void UseAbility(Vector3 destination)
    {
        //StartAbilityCast();

        GetComponent<CharacterStats>().Health.Restore(150);

        //FinishAbilityCast();
    }
}

using System.Collections.Generic;
using UnityEngine;

public class CC_W : SelfTargeted
{
    protected CC_W()
    {
        abilityName = "CC BTW HAHA";

        abilityType = AbilityType.PASSIVE;
        effectType = AbilityEffectType.SINGLE_TARGET;

        MaxLevel = 1;

        baseCooldown = 4;

        affectedByCooldownReduction = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/CC/CCW";
    }

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = new List<Team>();
    }

    protected override void Start()
    {
        base.Start();

        AbilityDebuffs = new AbilityBuff[] { gameObject.AddComponent<CC_W_Debuff>() };
    }

    public override void UseAbility()
    {
        StartAbilityCast();

        AbilityDebuffs[0].AddNewBuffToAffectedUnit(champion);

        FinishAbilityCast();
    }
}

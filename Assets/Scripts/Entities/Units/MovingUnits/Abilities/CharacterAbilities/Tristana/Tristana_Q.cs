using System.Collections.Generic;
using UnityEngine;

public class Tristana_Q : AutoTargeted
{
    protected Tristana_Q()
    {
        abilityName = "Rapid Fire";

        abilityType = AbilityType.PASSIVE;

        MaxLevel = 5;

        baseCooldown = 20;// 20/19/18/17/16
        baseCooldownPerLevel = -1;

        affectedByCooldownReduction = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Tristana/TristanaQ";
    }

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = new List<Team>();
    }

    protected override void Start()
    {
        base.Start();

        AbilityBuffs = new AbilityBuff[] { gameObject.AddComponent<Tristana_Q_Buff>() };
    }

    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        AbilityBuffs[0].AddNewBuffToAffectedUnit(champion);

        FinishAbilityCast();
    }
}

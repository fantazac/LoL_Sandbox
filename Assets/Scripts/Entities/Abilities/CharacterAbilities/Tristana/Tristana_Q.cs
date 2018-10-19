using UnityEngine;

public class Tristana_Q : SelfTargeted
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

    protected override void Start()
    {
        base.Start();

        AbilityBuffs = new AbilityBuff[] { gameObject.AddComponent<Tristana_Q_Buff>()};
    }

    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        AbilityBuffs[0].AddNewBuffToAffectedEntity(character);

        FinishAbilityCast();
    }
}

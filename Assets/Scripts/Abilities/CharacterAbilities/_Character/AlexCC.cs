public class AlexCC : AutoTargeted
{
    protected AlexCC()
    {
        abilityName = "AlexCC";

        abilityType = AbilityType.PASSIVE;

        AbilityLevel = 1;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/_Character/AlexCC";
    }

    protected override void Start()
    {
        base.Start();

        AbilityDebuffs = new AbilityBuff[] { gameObject.AddComponent<AlexCC_Debuff>() };
    }

    public override void UseAbility()
    {
        StartAbilityCast();

        AbilityDebuffs[0].AddNewBuffToAffectedUnit(champion);

        FinishAbilityCast();
    }
}

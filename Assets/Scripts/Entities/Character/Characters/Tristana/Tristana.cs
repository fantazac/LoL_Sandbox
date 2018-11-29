public class Tristana : Character
{
    protected Tristana()
    {
        characterPortraitPath = "Sprites/Characters/CharacterPortraits/Tristana";
    }

    protected override void SetCharacterSpecificScripts()
    {
        BasicAttackManager = gameObject.AddComponent<TristanaBasicAttack>();
        StatsManager = gameObject.AddComponent<TristanaStatsManager>();

        CharacterAbilityManager = gameObject.AddComponent<TristanaAbilityManager>();
    }
}

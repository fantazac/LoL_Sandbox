public class Tristana : Character
{
    protected Tristana()
    {
        characterPortraitPath = "Sprites/Characters/CharacterPortraits/Tristana";
    }

    protected override void SetCharacterSpecificScripts()
    {
        BasicAttack = gameObject.AddComponent<TristanaBasicAttack>();
        StatsManager = gameObject.AddComponent<TristanaStatsManager>();

        AbilityManager = gameObject.AddComponent<TristanaAbilityManager>();
    }
}

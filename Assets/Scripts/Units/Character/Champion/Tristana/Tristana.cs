public class Tristana : Champion
{
    protected Tristana()
    {
        championPortraitPath = "Sprites/Characters/CharacterPortraits/Tristana";
    }

    protected override void InitCharacterProperties()
    {
        BasicAttack = gameObject.AddComponent<TristanaBasicAttack>();
        StatsManager = gameObject.AddComponent<TristanaStatsManager>();

        AbilityManager = gameObject.AddComponent<TristanaAbilityManager>();
    }
}

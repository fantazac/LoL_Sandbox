public class Lucian : Champion
{
    protected Lucian()
    {
        championPortraitPath = "Sprites/Characters/CharacterPortraits/Lucian";
    }

    protected override void InitCharacterProperties()
    {
        BasicAttack = gameObject.AddComponent<LucianBasicAttack>();
        StatsManager = gameObject.AddComponent<LucianStatsManager>();

        AbilityManager = gameObject.AddComponent<LucianAbilityManager>();
    }
}

public class CC : Champion
{
    protected CC()
    {
        championPortraitPath = "Sprites/Characters/CharacterPortraits/CC";

        Name = "CC";
    }

    protected override void InitCharacterProperties()
    {
        BasicAttack = gameObject.AddComponent<CCBasicAttack>();
        StatsManager = gameObject.AddComponent<CCStatsManager>();

        AbilityManager = gameObject.AddComponent<CCAbilityManager>();
    }
}

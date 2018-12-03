public class Ezreal : Champion
{
    protected Ezreal()
    {
        championPortraitPath = "Sprites/Characters/CharacterPortraits/Ezreal";
    }

    protected override void InitCharacterProperties()
    {
        BasicAttack = gameObject.AddComponent<EzrealBasicAttack>();
        StatsManager = gameObject.AddComponent<EzrealStatsManager>();

        AbilityManager = gameObject.AddComponent<EzrealAbilityManager>();
    }
}

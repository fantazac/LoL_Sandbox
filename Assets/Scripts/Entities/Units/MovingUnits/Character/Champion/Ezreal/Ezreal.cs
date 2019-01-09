public class Ezreal : Champion
{
    protected Ezreal()
    {
        Name = "Ezreal";
    }

    protected override void SetPortraitSpritePath()
    {
        portraitSpritePath = "Sprites/Portraits/Character/Champion/Ezreal";
    }

    protected override void InitCharacterProperties()
    {
        BasicAttack = gameObject.AddComponent<EzrealBasicAttack>();
        StatsManager = gameObject.AddComponent<EzrealStatsManager>();

        AbilityManager = gameObject.AddComponent<EzrealAbilityManager>();
    }
}

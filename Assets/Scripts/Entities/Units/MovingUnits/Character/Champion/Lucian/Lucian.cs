public class Lucian : Champion
{
    protected Lucian()
    {
        Name = "Lucian";
    }

    protected override void SetPortraitSpritePath()
    {
        portraitSpritePath = "Sprites/Portraits/Character/Champion/Lucian";
    }

    protected override void InitCharacterProperties()
    {
        BasicAttack = gameObject.AddComponent<LucianBasicAttack>();
        StatsManager = gameObject.AddComponent<LucianStatsManager>();

        AbilityManager = gameObject.AddComponent<LucianAbilityManager>();
    }
}

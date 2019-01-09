public class CC : Champion
{
    protected CC()
    {
        Name = "CC";
    }

    protected override void SetPortraitSpritePath()
    {
        portraitSpritePath = "Sprites/Portraits/Character/Champion/CC";
    }

    protected override void InitCharacterProperties()
    {
        BasicAttack = gameObject.AddComponent<CCBasicAttack>();
        StatsManager = gameObject.AddComponent<CCStatsManager>();

        AbilityManager = gameObject.AddComponent<CCAbilityManager>();
    }
}

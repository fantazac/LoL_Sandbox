public class Tristana : Champion
{
    protected Tristana()
    {
        Name = "Tristana";
    }

    protected override void SetPortraitSpritePath()
    {
        portraitSpritePath = "Sprites/Portraits/Character/Champion/Tristana";
    }

    protected override void InitCharacterProperties()
    {
        BasicAttack = gameObject.AddComponent<TristanaBasicAttack>();
        StatsManager = gameObject.AddComponent<TristanaStatsManager>();

        AbilityManager = gameObject.AddComponent<TristanaAbilityManager>();
    }
}

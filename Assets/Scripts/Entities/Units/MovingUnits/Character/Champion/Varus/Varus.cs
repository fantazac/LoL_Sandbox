public class Varus : Champion
{
    protected Varus()
    {
        Name = "Varus";
    }

    protected override void SetPortraitSpritePath()
    {
        portraitSpritePath = "Sprites/Portraits/Character/Champion/Varus";
    }

    protected override void InitCharacterProperties()
    {
        BasicAttack = gameObject.AddComponent<VarusBasicAttack>();
        StatsManager = gameObject.AddComponent<VarusStatsManager>();

        AbilityManager = gameObject.AddComponent<VarusAbilityManager>();
    }
}

public class Varus : Champion
{
    protected Varus()
    {
        championPortraitPath = "Sprites/Characters/CharacterPortraits/Varus";
    }

    protected override void InitCharacterProperties()
    {
        BasicAttack = gameObject.AddComponent<VarusBasicAttack>();
        StatsManager = gameObject.AddComponent<VarusStatsManager>();

        AbilityManager = gameObject.AddComponent<VarusAbilityManager>();
    }
}

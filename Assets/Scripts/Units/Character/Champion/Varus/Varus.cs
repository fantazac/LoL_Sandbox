public class Varus : Character
{
    protected Varus()
    {
        characterPortraitPath = "Sprites/Characters/CharacterPortraits/Varus";
    }

    protected override void SetCharacterSpecificScripts()
    {
        BasicAttack = gameObject.AddComponent<VarusBasicAttack>();
        StatsManager = gameObject.AddComponent<VarusStatsManager>();

        AbilityManager = gameObject.AddComponent<VarusAbilityManager>();
    }
}

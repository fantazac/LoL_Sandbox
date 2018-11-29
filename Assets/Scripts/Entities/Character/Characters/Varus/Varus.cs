public class Varus : Character
{
    protected Varus()
    {
        characterPortraitPath = "Sprites/Characters/CharacterPortraits/Varus";
    }

    protected override void SetCharacterSpecificScripts()
    {
        BasicAttackManager = gameObject.AddComponent<VarusBasicAttack>();
        StatsManager = gameObject.AddComponent<VarusStatsManager>();

        CharacterAbilityManager = gameObject.AddComponent<VarusAbilityManager>();
    }
}

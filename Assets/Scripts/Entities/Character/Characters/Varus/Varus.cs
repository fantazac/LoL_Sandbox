public class Varus : Character
{
    protected Varus()
    {
        characterPortraitPath = "Sprites/Characters/CharacterPortraits/Varus";
    }

    protected override void SetCharacterSpecificScripts()
    {
        EntityBasicAttack = gameObject.AddComponent<VarusBasicAttack>();
        EntityStatsManager = gameObject.AddComponent<VarusStatsManager>();

        CharacterAbilityManager = gameObject.AddComponent<VarusAbilityManager>();
    }
}

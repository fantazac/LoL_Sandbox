public class Tristana : Character
{
    protected Tristana()
    {
        characterPortraitPath = "Sprites/Characters/CharacterPortraits/Tristana";
    }

    protected override void SetCharacterSpecificScripts()
    {
        EntityBasicAttack = gameObject.AddComponent<TristanaBasicAttack>();
        EntityStatsManager = gameObject.AddComponent<TristanaStatsManager>();

        CharacterAbilityManager = gameObject.AddComponent<TristanaAbilityManager>();
    }
}

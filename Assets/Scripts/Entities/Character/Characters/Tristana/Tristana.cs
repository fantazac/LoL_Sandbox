public class Tristana : Character
{
    protected Tristana()
    {
        characterPortraitPath = "Sprites/Characters/CharacterPortraits/Tristana";
    }

    protected override void SetCharacterSpecificScripts()
    {
        EntityBasicAttack = gameObject.AddComponent<TristanaBasicAttack>();
        EntityStats = gameObject.AddComponent<TristanaStats>();

        CharacterAbilityManager = gameObject.AddComponent<TristanaAbilityManager>();
    }
}

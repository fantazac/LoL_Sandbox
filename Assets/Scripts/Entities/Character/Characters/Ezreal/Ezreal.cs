public class Ezreal : Character
{
    protected Ezreal()
    {
        characterPortraitPath = "Sprites/Characters/CharacterPortraits/Ezreal";
    }

    protected override void SetCharacterSpecificScripts()
    {
        EntityBasicAttack = gameObject.AddComponent<EzrealBasicAttack>();
        EntityStatsManager = gameObject.AddComponent<EzrealStatsManager>();

        CharacterAbilityManager = gameObject.AddComponent<EzrealAbilityManager>();
    }
}

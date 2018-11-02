public class CC : Character
{
    protected CC()
    {
        characterPortraitPath = "Sprites/Characters/CharacterPortraits/CC";
    }

    protected override void SetCharacterSpecificScripts()
    {
        EntityBasicAttack = gameObject.AddComponent<CCBasicAttack>();
        EntityStatsManager = gameObject.AddComponent<CCStatsManager>();

        CharacterAbilityManager = gameObject.AddComponent<CCAbilityManager>();
    }
}

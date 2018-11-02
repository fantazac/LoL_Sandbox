public class Lucian : Character
{
    protected Lucian()
    {
        characterPortraitPath = "Sprites/Characters/CharacterPortraits/Lucian";
    }

    protected override void SetCharacterSpecificScripts()
    {
        EntityBasicAttack = gameObject.AddComponent<LucianBasicAttack>();
        EntityStatsManager = gameObject.AddComponent<LucianStatsManager>();

        CharacterAbilityManager = gameObject.AddComponent<LucianAbilityManager>();
    }
}

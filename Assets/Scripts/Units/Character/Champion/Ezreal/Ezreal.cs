public class Ezreal : Character
{
    protected Ezreal()
    {
        characterPortraitPath = "Sprites/Characters/CharacterPortraits/Ezreal";
    }

    protected override void SetCharacterSpecificScripts()
    {
        BasicAttack = gameObject.AddComponent<EzrealBasicAttack>();
        StatsManager = gameObject.AddComponent<EzrealStatsManager>();

        AbilityManager = gameObject.AddComponent<EzrealAbilityManager>();
    }
}

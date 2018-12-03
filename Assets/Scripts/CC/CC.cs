public class CC : Character
{
    protected CC()
    {
        characterPortraitPath = "Sprites/Characters/CharacterPortraits/CC";
    }

    protected override void SetCharacterSpecificScripts()
    {
        BasicAttack = gameObject.AddComponent<CCBasicAttack>();
        StatsManager = gameObject.AddComponent<CCStatsManager>();

        AbilityManager = gameObject.AddComponent<CCAbilityManager>();
    }
}

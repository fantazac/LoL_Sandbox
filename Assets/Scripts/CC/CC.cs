public class CC : Character
{
    protected CC()
    {
        characterPortraitPath = "Sprites/Characters/CharacterPortraits/CC";
    }

    protected override void SetCharacterSpecificScripts()
    {
        BasicAttackManager = gameObject.AddComponent<CCBasicAttack>();
        StatsManager = gameObject.AddComponent<CCStatsManager>();

        CharacterAbilityManager = gameObject.AddComponent<CCAbilityManager>();
    }
}

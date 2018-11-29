public class Lucian : Character
{
    protected Lucian()
    {
        characterPortraitPath = "Sprites/Characters/CharacterPortraits/Lucian";
    }

    protected override void SetCharacterSpecificScripts()
    {
        BasicAttackManager = gameObject.AddComponent<LucianBasicAttack>();
        StatsManager = gameObject.AddComponent<LucianStatsManager>();

        CharacterAbilityManager = gameObject.AddComponent<LucianAbilityManager>();
    }
}

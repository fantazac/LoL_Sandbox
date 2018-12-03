public class Lucian : Character
{
    protected Lucian()
    {
        characterPortraitPath = "Sprites/Characters/CharacterPortraits/Lucian";
    }

    protected override void SetCharacterSpecificScripts()
    {
        BasicAttack = gameObject.AddComponent<LucianBasicAttack>();
        StatsManager = gameObject.AddComponent<LucianStatsManager>();

        AbilityManager = gameObject.AddComponent<LucianAbilityManager>();
    }
}

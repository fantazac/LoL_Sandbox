public class MissFortune : Character
{
    protected MissFortune()
    {
        characterPortraitPath = "Sprites/Characters/CharacterPortraits/MissFortune";
    }

    protected override void SetCharacterSpecificScripts()
    {
        BasicAttack = gameObject.AddComponent<MissFortuneBasicAttack>();
        StatsManager = gameObject.AddComponent<MissFortuneStatsManager>();

        AbilityManager = gameObject.AddComponent<MissFortuneAbilityManager>();
    }
}

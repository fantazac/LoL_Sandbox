public class MissFortune : Character
{
    protected MissFortune()
    {
        characterPortraitPath = "Sprites/Characters/CharacterPortraits/MissFortune";
    }

    protected override void SetCharacterSpecificScripts()
    {
        BasicAttackManager = gameObject.AddComponent<MissFortuneBasicAttack>();
        StatsManager = gameObject.AddComponent<MissFortuneStatsManager>();

        CharacterAbilityManager = gameObject.AddComponent<MissFortuneAbilityManager>();
    }
}

public class MissFortune : Character
{
    protected MissFortune()
    {
        characterPortraitPath = "Sprites/Characters/CharacterPortraits/MissFortune";
    }

    protected override void SetCharacterSpecificScripts()
    {
        EntityBasicAttack = gameObject.AddComponent<MissFortuneBasicAttack>();
        EntityStatsManager = gameObject.AddComponent<MissFortuneStatsManager>();

        CharacterAbilityManager = gameObject.AddComponent<MissFortuneAbilityManager>();
    }
}

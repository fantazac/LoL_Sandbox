public class MissFortune : Character
{
    protected MissFortune()
    {
        characterPortraitPath = "Sprites/Characters/CharacterPortraits/MissFortune";
    }

    protected override void SetCharacterSpecificScripts()
    {
        EntityBasicAttack = gameObject.AddComponent<MissFortuneBasicAttack>();
        EntityStats = gameObject.AddComponent<MissFortuneStats>();

        CharacterAbilityManager = gameObject.AddComponent<MissFortuneAbilityManager>();
    }
}

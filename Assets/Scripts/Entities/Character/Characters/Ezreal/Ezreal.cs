﻿public class Ezreal : Character
{
    protected Ezreal()
    {
        characterPortraitPath = "Sprites/Characters/CharacterPortraits/Ezreal";
    }

    protected override void SetCharacterSpecificScripts()
    {
        BasicAttackManager = gameObject.AddComponent<EzrealBasicAttack>();
        StatsManager = gameObject.AddComponent<EzrealStatsManager>();

        CharacterAbilityManager = gameObject.AddComponent<EzrealAbilityManager>();
    }
}

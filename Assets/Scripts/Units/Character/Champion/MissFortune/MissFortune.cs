public class MissFortune : Champion
{
    protected MissFortune()
    {
        championPortraitPath = "Sprites/Characters/CharacterPortraits/MissFortune";

        Name = "Miss Fortune";
    }

    protected override void InitCharacterProperties()
    {
        BasicAttack = gameObject.AddComponent<MissFortuneBasicAttack>();
        StatsManager = gameObject.AddComponent<MissFortuneStatsManager>();

        AbilityManager = gameObject.AddComponent<MissFortuneAbilityManager>();
    }
}

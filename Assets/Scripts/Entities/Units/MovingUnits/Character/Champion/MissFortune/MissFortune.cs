public class MissFortune : Champion
{
    protected MissFortune()
    {
        Name = "Miss Fortune";
    }

    protected override void SetPortraitSpritePath()
    {
        portraitSpritePath = "Sprites/Portraits/Character/Champion/MissFortune";
    }

    protected override void InitCharacterProperties()
    {
        BasicAttack = gameObject.AddComponent<MissFortuneBasicAttack>();
        StatsManager = gameObject.AddComponent<MissFortuneStatsManager>();

        AbilityManager = gameObject.AddComponent<MissFortuneAbilityManager>();
    }
}

public class Ezreal_P : PassiveTargeted
{
    protected Ezreal_P()
    {
        abilityName = "Rising Spell Force";

        abilityType = AbilityType.PASSIVE;

        //MaxLevel = 3;
        AbilityLevel = 1;

        IsEnabled = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Ezreal/EzrealP";
    }

    protected override void Start()
    {
        base.Start();

        AbilityBuffs = new AbilityBuff[] { gameObject.AddComponent<Ezreal_P_Buff>() };

        foreach (Ability ability in champion.AbilityManager.CharacterAbilities)
        {
            ability.OnAbilityHit += PassiveEffect;
        }

        champion.LevelManager.OnLevelUp += OnCharacterLevelUp;
    }

    /*public override void OnCharacterLevelUp(int level)
    {
        if (level == 7 || level == 13)
        {
            LevelUp();
        }
    }*/
}

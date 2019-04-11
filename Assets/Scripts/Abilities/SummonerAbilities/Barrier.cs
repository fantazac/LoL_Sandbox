using System.Collections.Generic;

public class Barrier : SelfTargeted
{
    protected Barrier()
    {
        abilityName = "Barrier";

        abilityType = AbilityType.SHIELD;
        effectType = AbilityEffectType.SHIELD;

        baseCooldown = 180;
        
        CanBeCastDuringOtherAbilityCastTimes = true;
        
        IsEnabled = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/SummonerAbilities/Barrier";
    }

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = new List<Team>();
    }

    protected override void Start()
    {
        base.Start();

        AbilityBuffs = new AbilityBuff[] { gameObject.AddComponent<Barrier_Buff>() };
        
        champion.LevelManager.OnLevelUp += OnCharacterLevelUp;
    }
    
    private void OnCharacterLevelUp(int level)
    {
        LevelUpBuffsAndDebuffs();
    }

    public override void UseAbility()
    {
        StartAbilityCast();

        AbilityBuffs[0].AddNewBuffToAffectedUnit(champion);

        FinishAbilityCast();
    }
}

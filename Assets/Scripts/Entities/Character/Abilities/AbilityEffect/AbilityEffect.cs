using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityEffect : MonoBehaviour
{
    protected EntityTeam teamOfShooter;
    protected AbilityAffectedUnitType affectedUnitType;

    public List<Entity> UnitsAlreadyHit { get; protected set; }

    public delegate void OnAbilityEffectHitHandler(AbilityEffect abilityEffect, Entity entityHit);
    public event OnAbilityEffectHitHandler OnAbilityEffectHit;

    protected AbilityEffect()
    {
        UnitsAlreadyHit = new List<Entity>();
    }

    protected abstract IEnumerator ActivateAbilityEffect();

    protected virtual void OnTriggerEnter(Collider collider) { }

    protected virtual bool CanAffectTarget(Entity entityHit)
    {
        if (TargetIsValid(entityHit))
        {
            foreach (Entity entity in UnitsAlreadyHit)
            {
                if (entityHit == entity)
                {
                    return false;
                }
            }

            return true;
        }

        return false;
    }

    protected void OnAbilityEffectHitTarget(Entity entityHit)
    {
        if (OnAbilityEffectHit != null)
        {
            OnAbilityEffectHit(this, entityHit);
        }
    }

    protected bool TargetIsValid(Entity entityHit)
    {
        switch (affectedUnitType)
        {
            case AbilityAffectedUnitType.UNITS:
                return EntityIsAUnit(entityHit.EntityType);

            case AbilityAffectedUnitType.ALLIES:
                return EntityIsAnAlly(entityHit.Team) && EntityIsAUnit(entityHit.EntityType);
            case AbilityAffectedUnitType.ENEMIES:
                return !EntityIsAnAlly(entityHit.Team) && EntityIsAUnit(entityHit.EntityType);

            case AbilityAffectedUnitType.CHARACTERS:
                return EntityIsACharacter(entityHit.EntityType);
            case AbilityAffectedUnitType.ALLY_CHARACTERS:
                return EntityIsAnAllyCharacter(entityHit.Team, entityHit.EntityType);
            case AbilityAffectedUnitType.ENEMY_CHARACTERS:
                return EntityIsAnEnemyCharacter(entityHit.Team, entityHit.EntityType);

            case AbilityAffectedUnitType.MINIONS:
                return EntityIsAMinion(entityHit.EntityType);
            case AbilityAffectedUnitType.ALLY_MINIONS:
                return EntityIsAnAllyMinion(entityHit.Team, entityHit.EntityType);
            case AbilityAffectedUnitType.ENEMY_MINIONS:
                return EntityIsAnEnemyMinion(entityHit.Team, entityHit.EntityType);

            case AbilityAffectedUnitType.PETS:
                return EntityIsAPet(entityHit.EntityType);
            case AbilityAffectedUnitType.ALLY_PETS:
                return EntityIsAnAllyPet(entityHit.Team, entityHit.EntityType);
            case AbilityAffectedUnitType.ENEMY_PETS:
                return EntityIsAnEnemyPet(entityHit.Team, entityHit.EntityType);

            case AbilityAffectedUnitType.MONSTERS:
                return EntityIsAMonster(entityHit.EntityType);
            case AbilityAffectedUnitType.LARGE_MONSTERS:
                return EntityIsALargeMonster(entityHit.EntityType);
            case AbilityAffectedUnitType.EPIC_MONSTERS:
                return EntityIsAnEpicMonster(entityHit.EntityType);

            case AbilityAffectedUnitType.STRUCTURES:
                return EntityIsAStructure(entityHit.EntityType);
            case AbilityAffectedUnitType.ALLY_STRUCTURES:
                return EntityIsAnAllyStructure(entityHit.Team, entityHit.EntityType);
            case AbilityAffectedUnitType.ENEMY_STRUCTURES:
                return EntityIsAnEnemyStructure(entityHit.Team, entityHit.EntityType);

            case AbilityAffectedUnitType.TURRETS:
                return EntityIsATurret(entityHit.EntityType);
            case AbilityAffectedUnitType.ALLY_TURRETS:
                return EntityIsAnAllyTurret(entityHit.Team, entityHit.EntityType);
            case AbilityAffectedUnitType.ENEMY_TURRETS:
                return EntityIsAnEnemyTurret(entityHit.Team, entityHit.EntityType);

            case AbilityAffectedUnitType.INHIBITORS:
                return EntityIsAnInhibitor(entityHit.EntityType);
            case AbilityAffectedUnitType.ALLY_INHIBITORS:
                return EntityIsAnAllyInhibitor(entityHit.Team, entityHit.EntityType);
            case AbilityAffectedUnitType.ENEMY_INHIBITORS:
                return EntityIsAnEnemyInhibitor(entityHit.Team, entityHit.EntityType);

            case AbilityAffectedUnitType.NEXUS:
                return EntityIsANexus(entityHit.EntityType);
            case AbilityAffectedUnitType.ALLY_NEXUS:
                return EntityIsAnAllyNexus(entityHit.Team, entityHit.EntityType);
            case AbilityAffectedUnitType.ENEMY_NEXUS:
                return EntityIsAnEnemyNexus(entityHit.Team, entityHit.EntityType);

            case AbilityAffectedUnitType.TERRAIN:
                return EntityIsTerrain(entityHit.EntityType);
        }

        return false;
    }

    protected bool EntityIsACharacter(EntityType entityType)
    {
        return entityType == EntityType.CHARACTER;
    }

    protected bool EntityIsALargeMonster(EntityType entityType)
    {
        return entityType == EntityType.MONSTER || EntityIsAnEpicMonster(entityType); // will change
    }

    protected bool EntityIsAMinion(EntityType entityType)
    {
        return entityType == EntityType.MINION;
    }

    protected bool EntityIsAMonster(EntityType entityType)
    {
        return entityType == EntityType.MONSTER || EntityIsALargeMonster(entityType) || EntityIsAnEpicMonster(entityType);
    }

    protected bool EntityIsAnAlly(EntityTeam entityTeam)
    {
        return entityTeam == teamOfShooter;
    }

    protected bool EntityIsAnAllyCharacter(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsACharacter(entityType) && EntityIsAnAlly(entityTeam);
    }

    protected bool EntityIsAnEnemyCharacter(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsACharacter(entityType) && !EntityIsAnAlly(entityTeam);
    }

    protected bool EntityIsAnAllyInhibitor(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsAnInhibitor(entityType) && EntityIsAnAlly(entityTeam);
    }

    protected bool EntityIsAnEnemyInhibitor(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsAnInhibitor(entityType) && !EntityIsAnAlly(entityTeam);
    }

    protected bool EntityIsAnAllyMinion(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsAMinion(entityType) && EntityIsAnAlly(entityTeam);
    }

    protected bool EntityIsAnEnemyMinion(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsAMinion(entityType) && !EntityIsAnAlly(entityTeam);
    }

    protected bool EntityIsAnAllyNexus(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsANexus(entityType) && EntityIsAnAlly(entityTeam);
    }

    protected bool EntityIsAnEnemyNexus(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsANexus(entityType) && !EntityIsAnAlly(entityTeam);
    }

    protected bool EntityIsAnAllyPet(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsAPet(entityType) && EntityIsAnAlly(entityTeam);
    }

    protected bool EntityIsAnEnemyPet(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsAPet(entityType) && !EntityIsAnAlly(entityTeam);
    }

    protected bool EntityIsAnAllyStructure(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsAStructure(entityType) && EntityIsAnAlly(entityTeam);
    }

    protected bool EntityIsAnEnemyStructure(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsAStructure(entityType) && !EntityIsAnAlly(entityTeam);
    }

    protected bool EntityIsAnAllyTurret(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsATurret(entityType) && EntityIsAnAlly(entityTeam);
    }

    protected bool EntityIsAnEnemyTurret(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsATurret(entityType) && !EntityIsAnAlly(entityTeam);
    }

    protected bool EntityIsAnEpicMonster(EntityType entityType)
    {
        return entityType == EntityType.MONSTER; // will change
    }

    protected bool EntityIsANexus(EntityType entityType)
    {
        return entityType == EntityType.NEXUS;
    }

    protected bool EntityIsAnInhibitor(EntityType entityType)
    {
        return entityType == EntityType.INHIBITOR;
    }

    protected bool EntityIsAPet(EntityType entityType)
    {
        return entityType == EntityType.PET;
    }

    protected bool EntityIsAStructure(EntityType entityType)
    {
        return EntityIsAnInhibitor(entityType) || EntityIsANexus(entityType) || EntityIsATurret(entityType);
    }

    protected bool EntityIsATurret(EntityType entityType)
    {
        return entityType == EntityType.TURRET;
    }

    protected bool EntityIsAUnit(EntityType entityType)
    {
        return !(EntityIsAStructure(entityType) || EntityIsTerrain(entityType));
    }

    protected bool EntityIsTerrain(EntityType entityType)
    {
        return entityType == EntityType.TERRAIN;
    }
}

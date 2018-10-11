public static class TargetIsValid
{
    private static EntityTeam localTeamOfCallingEntity;

    public static bool CheckIfTargetIsValid(Entity entityHit, AbilityAffectedUnitType affectedUnitType, EntityTeam teamOfCallingEntity)
    {
        localTeamOfCallingEntity = teamOfCallingEntity;

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

            case AbilityAffectedUnitType.OBJECTIVES:
                return EntityIsAnObjective(entityHit.Team, entityHit.EntityType);
            case AbilityAffectedUnitType.OBJECTIVES_AND_ENEMY_CHARACTERS:
                return EntityIsAnObjectiveOrAnEnemyCharacter(entityHit.Team, entityHit.EntityType);

            case AbilityAffectedUnitType.TERRAIN:
                return EntityIsTerrain(entityHit.EntityType);
        }

        return false;
    }

    private static bool EntityIsACharacter(EntityType entityType)
    {
        return entityType == EntityType.CHARACTER;
    }

    private static bool EntityIsALargeMonster(EntityType entityType)
    {
        return entityType == EntityType.MONSTER || EntityIsAnEpicMonster(entityType); // will change
    }

    private static bool EntityIsAMinion(EntityType entityType)
    {
        return entityType == EntityType.MINION;
    }

    private static bool EntityIsAMonster(EntityType entityType)
    {
        return entityType == EntityType.MONSTER || EntityIsALargeMonster(entityType) || EntityIsAnEpicMonster(entityType);
    }

    private static bool EntityIsAnAlly(EntityTeam entityTeam)
    {
        return entityTeam == localTeamOfCallingEntity;
    }

    private static bool EntityIsAnAllyCharacter(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsACharacter(entityType) && EntityIsAnAlly(entityTeam);
    }

    private static bool EntityIsAnEnemyCharacter(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsACharacter(entityType) && !EntityIsAnAlly(entityTeam);
    }

    private static bool EntityIsAnAllyInhibitor(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsAnInhibitor(entityType) && EntityIsAnAlly(entityTeam);
    }

    private static bool EntityIsAnEnemyInhibitor(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsAnInhibitor(entityType) && !EntityIsAnAlly(entityTeam);
    }

    private static bool EntityIsAnAllyMinion(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsAMinion(entityType) && EntityIsAnAlly(entityTeam);
    }

    private static bool EntityIsAnEnemyMinion(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsAMinion(entityType) && !EntityIsAnAlly(entityTeam);
    }

    private static bool EntityIsAnAllyNexus(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsANexus(entityType) && EntityIsAnAlly(entityTeam);
    }

    private static bool EntityIsAnEnemyNexus(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsANexus(entityType) && !EntityIsAnAlly(entityTeam);
    }

    private static bool EntityIsAnAllyPet(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsAPet(entityType) && EntityIsAnAlly(entityTeam);
    }

    private static bool EntityIsAnEnemyPet(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsAPet(entityType) && !EntityIsAnAlly(entityTeam);
    }

    private static bool EntityIsAnAllyStructure(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsAStructure(entityType) && EntityIsAnAlly(entityTeam);
    }

    private static bool EntityIsAnEnemyStructure(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsAStructure(entityType) && !EntityIsAnAlly(entityTeam);
    }

    private static bool EntityIsAnAllyTurret(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsATurret(entityType) && EntityIsAnAlly(entityTeam);
    }

    private static bool EntityIsAnEnemyTurret(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsATurret(entityType) && !EntityIsAnAlly(entityTeam);
    }

    private static bool EntityIsAnEpicMonster(EntityType entityType)
    {
        return entityType == EntityType.MONSTER; // will change
    }

    private static bool EntityIsANexus(EntityType entityType)
    {
        return entityType == EntityType.NEXUS;
    }

    private static bool EntityIsAnInhibitor(EntityType entityType)
    {
        return entityType == EntityType.INHIBITOR;
    }

    private static bool EntityIsAnObjective(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsAnEnemyStructure(entityTeam, entityType) || EntityIsAnEpicMonster(entityType);
    }

    private static bool EntityIsAnObjectiveOrAnEnemyCharacter(EntityTeam entityTeam, EntityType entityType)
    {
        return EntityIsAnObjective(entityTeam, entityType) || EntityIsAnEnemyCharacter(entityTeam, entityType);
    }

    private static bool EntityIsAPet(EntityType entityType)
    {
        return entityType == EntityType.PET;
    }

    private static bool EntityIsAStructure(EntityType entityType)
    {
        return EntityIsAnInhibitor(entityType) || EntityIsANexus(entityType) || EntityIsATurret(entityType);
    }

    private static bool EntityIsATurret(EntityType entityType)
    {
        return entityType == EntityType.TURRET;
    }

    private static bool EntityIsAUnit(EntityType entityType)
    {
        return !(EntityIsAStructure(entityType) || EntityIsTerrain(entityType));
    }

    private static bool EntityIsTerrain(EntityType entityType)
    {
        return entityType == EntityType.TERRAIN;
    }
}

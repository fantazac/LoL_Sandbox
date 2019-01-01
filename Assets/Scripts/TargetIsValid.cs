public static class TargetIsValid
{
    private static Team castingUnitTeam;

    public static bool CheckIfTargetIsValid(Unit unitHit, AbilityAffectedUnitType affectedUnitType, Team castingUnitTeam)
    {
        TargetIsValid.castingUnitTeam = castingUnitTeam;

        switch (affectedUnitType)
        {
            case AbilityAffectedUnitType.ALLIES:
                return UnitIsAnAlly(unitHit.Team);
            case AbilityAffectedUnitType.ENEMIES:
                return !UnitIsAnAlly(unitHit.Team);

            case AbilityAffectedUnitType.CHARACTERS:
                return UnitIsACharacter(unitHit.UnitType);
            case AbilityAffectedUnitType.ALLY_CHARACTERS:
                return UnitIsAnAllyCharacter(unitHit.Team, unitHit.UnitType);
            case AbilityAffectedUnitType.ENEMY_CHARACTERS:
                return UnitIsAnEnemyCharacter(unitHit.Team, unitHit.UnitType);

            case AbilityAffectedUnitType.MINIONS:
                return UnitIsAMinion(unitHit.UnitType);
            case AbilityAffectedUnitType.ALLY_MINIONS:
                return UnitIsAnAllyMinion(unitHit.Team, unitHit.UnitType);
            case AbilityAffectedUnitType.ENEMY_MINIONS:
                return UnitIsAnEnemyMinion(unitHit.Team, unitHit.UnitType);

            case AbilityAffectedUnitType.PETS:
                return UnitIsAPet(unitHit.UnitType);
            case AbilityAffectedUnitType.ALLY_PETS:
                return UnitIsAnAllyPet(unitHit.Team, unitHit.UnitType);
            case AbilityAffectedUnitType.ENEMY_PETS:
                return UnitIsAnEnemyPet(unitHit.Team, unitHit.UnitType);

            case AbilityAffectedUnitType.MONSTERS:
                return UnitIsAMonster(unitHit.UnitType);
            case AbilityAffectedUnitType.LARGE_MONSTERS:
                return UnitIsALargeMonster(unitHit.UnitType);
            case AbilityAffectedUnitType.EPIC_MONSTERS:
                return UnitIsAnEpicMonster(unitHit.UnitType);

            case AbilityAffectedUnitType.STRUCTURES:
                return UnitIsAStructure(unitHit.UnitType);
            case AbilityAffectedUnitType.ALLY_STRUCTURES:
                return UnitIsAnAllyStructure(unitHit.Team, unitHit.UnitType);
            case AbilityAffectedUnitType.ENEMY_STRUCTURES:
                return UnitIsAnEnemyStructure(unitHit.Team, unitHit.UnitType);

            case AbilityAffectedUnitType.TURRETS:
                return UnitIsATurret(unitHit.UnitType);
            case AbilityAffectedUnitType.ALLY_TURRETS:
                return UnitIsAnAllyTurret(unitHit.Team, unitHit.UnitType);
            case AbilityAffectedUnitType.ENEMY_TURRETS:
                return UnitIsAnEnemyTurret(unitHit.Team, unitHit.UnitType);

            case AbilityAffectedUnitType.INHIBITORS:
                return UnitIsAnInhibitor(unitHit.UnitType);
            case AbilityAffectedUnitType.ALLY_INHIBITORS:
                return UnitIsAnAllyInhibitor(unitHit.Team, unitHit.UnitType);
            case AbilityAffectedUnitType.ENEMY_INHIBITORS:
                return UnitIsAnEnemyInhibitor(unitHit.Team, unitHit.UnitType);

            case AbilityAffectedUnitType.NEXUS:
                return UnitIsANexus(unitHit.UnitType);
            case AbilityAffectedUnitType.ALLY_NEXUS:
                return UnitIsAnAllyNexus(unitHit.Team, unitHit.UnitType);
            case AbilityAffectedUnitType.ENEMY_NEXUS:
                return UnitIsAnEnemyNexus(unitHit.Team, unitHit.UnitType);

            case AbilityAffectedUnitType.OBJECTIVES:
                return UnitIsAnObjective(unitHit.Team, unitHit.UnitType);
            case AbilityAffectedUnitType.OBJECTIVES_AND_ENEMY_CHARACTERS:
                return UnitIsAnObjectiveOrAnEnemyCharacter(unitHit.Team, unitHit.UnitType);

            case AbilityAffectedUnitType.TERRAIN:
                return UnitIsTerrain(unitHit.UnitType);
        }

        return false;
    }

    private static bool UnitIsACharacter(UnitType unitType)
    {
        return unitType == UnitType.CHARACTER;
    }

    private static bool UnitIsALargeMonster(UnitType unitType)
    {
        return unitType == UnitType.MONSTER || UnitIsAnEpicMonster(unitType); // will change
    }

    private static bool UnitIsAMinion(UnitType unitType)
    {
        return unitType == UnitType.MINION;
    }

    private static bool UnitIsAMonster(UnitType unitType)
    {
        return unitType == UnitType.MONSTER || UnitIsALargeMonster(unitType) || UnitIsAnEpicMonster(unitType);
    }

    private static bool UnitIsAnAlly(Team unitTeam)
    {
        return unitTeam == castingUnitTeam;
    }

    private static bool UnitIsAnAllyCharacter(Team unitTeam, UnitType unitType)
    {
        return UnitIsACharacter(unitType) && UnitIsAnAlly(unitTeam);
    }

    private static bool UnitIsAnEnemyCharacter(Team unitTeam, UnitType unitType)
    {
        return UnitIsACharacter(unitType) && !UnitIsAnAlly(unitTeam);
    }

    private static bool UnitIsAnAllyInhibitor(Team unitTeam, UnitType unitType)
    {
        return UnitIsAnInhibitor(unitType) && UnitIsAnAlly(unitTeam);
    }

    private static bool UnitIsAnEnemyInhibitor(Team unitTeam, UnitType unitType)
    {
        return UnitIsAnInhibitor(unitType) && !UnitIsAnAlly(unitTeam);
    }

    private static bool UnitIsAnAllyMinion(Team unitTeam, UnitType unitType)
    {
        return UnitIsAMinion(unitType) && UnitIsAnAlly(unitTeam);
    }

    private static bool UnitIsAnEnemyMinion(Team unitTeam, UnitType unitType)
    {
        return UnitIsAMinion(unitType) && !UnitIsAnAlly(unitTeam);
    }

    private static bool UnitIsAnAllyNexus(Team unitTeam, UnitType unitType)
    {
        return UnitIsANexus(unitType) && UnitIsAnAlly(unitTeam);
    }

    private static bool UnitIsAnEnemyNexus(Team unitTeam, UnitType unitType)
    {
        return UnitIsANexus(unitType) && !UnitIsAnAlly(unitTeam);
    }

    private static bool UnitIsAnAllyPet(Team unitTeam, UnitType unitType)
    {
        return UnitIsAPet(unitType) && UnitIsAnAlly(unitTeam);
    }

    private static bool UnitIsAnEnemyPet(Team unitTeam, UnitType unitType)
    {
        return UnitIsAPet(unitType) && !UnitIsAnAlly(unitTeam);
    }

    private static bool UnitIsAnAllyStructure(Team unitTeam, UnitType unitType)
    {
        return UnitIsAStructure(unitType) && UnitIsAnAlly(unitTeam);
    }

    private static bool UnitIsAnEnemyStructure(Team unitTeam, UnitType unitType)
    {
        return UnitIsAStructure(unitType) && !UnitIsAnAlly(unitTeam);
    }

    private static bool UnitIsAnAllyTurret(Team unitTeam, UnitType unitType)
    {
        return UnitIsATurret(unitType) && UnitIsAnAlly(unitTeam);
    }

    private static bool UnitIsAnEnemyTurret(Team unitTeam, UnitType unitType)
    {
        return UnitIsATurret(unitType) && !UnitIsAnAlly(unitTeam);
    }

    private static bool UnitIsAnEpicMonster(UnitType unitType)
    {
        return unitType == UnitType.MONSTER; // will change
    }

    private static bool UnitIsANexus(UnitType unitType)
    {
        return unitType == UnitType.NEXUS;
    }

    private static bool UnitIsAnInhibitor(UnitType unitType)
    {
        return unitType == UnitType.INHIBITOR;
    }

    private static bool UnitIsAnObjective(Team unitTeam, UnitType unitType)
    {
        return UnitIsAnEnemyStructure(unitTeam, unitType) || UnitIsAnEpicMonster(unitType);
    }

    private static bool UnitIsAnObjectiveOrAnEnemyCharacter(Team unitTeam, UnitType unitType)
    {
        return UnitIsAnObjective(unitTeam, unitType) || UnitIsAnEnemyCharacter(unitTeam, unitType);
    }

    private static bool UnitIsAPet(UnitType unitType)
    {
        return unitType == UnitType.PET;
    }

    private static bool UnitIsAStructure(UnitType unitType)
    {
        return UnitIsAnInhibitor(unitType) || UnitIsANexus(unitType) || UnitIsATurret(unitType);
    }

    private static bool UnitIsATurret(UnitType unitType)
    {
        return unitType == UnitType.TURRET;
    }

    private static bool UnitIsTerrain(UnitType unitType)
    {
        return unitType == UnitType.TERRAIN;
    }
}

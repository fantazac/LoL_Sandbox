public enum AbilityAffectedUnitType
{
    //Nothing for abilities that don't have affected units (ex. Lucian E), will need to see if it's a problem or not

    UNITS,

    ALLIES,
    ENEMIES,

    CHARACTERS,
    ALLY_CHARACTERS,
    ENEMY_CHARACTERS,

    MINIONS,
    ALLY_MINIONS,
    ENEMY_MINIONS,

    PETS,
    ALLY_PETS,
    ENEMY_PETS,

    MONSTERS,
    LARGE_MONSTERS,
    EPIC_MONSTERS,

    STRUCTURES,
    ALLY_STRUCTURES,
    ENEMY_STRUCTURES,

    TURRETS,
    ALLY_TURRETS,
    ENEMY_TURRETS,

    INHIBITORS,
    ALLY_INHIBITORS,
    ENEMY_INHIBITORS,

    NEXUS,
    ALLY_NEXUS,
    ENEMY_NEXUS,

    OBJECTIVES,
    OBJECTIVES_AND_ENEMY_CHARACTERS,

    TERRAIN,
}

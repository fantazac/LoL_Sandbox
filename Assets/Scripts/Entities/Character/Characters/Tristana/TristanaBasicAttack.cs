public class TristanaBasicAttack : CharacterBasicAttack, DamageSourceOnEntityKill
{
    private Tristana_E tristanaE;

    protected TristanaBasicAttack()
    {
        delayPercentBeforeAttack = 0.1666f;
        speed = 1900;

        basicAttackPrefabPath = "BasicAttacksPrefabs/Characters/Tristana/TristanaBA";
    }

    protected override void Start()
    {
        base.Start();

        tristanaE = GetComponent<Tristana_E>();
    }

    public void KilledEntity(Entity killedEntity)
    {
        if (tristanaE && tristanaE.AbilityLevel > 0)
        {
            tristanaE.DamageAllEnemiesInPassiveExplosionRadius(killedEntity);
        }
    }
}

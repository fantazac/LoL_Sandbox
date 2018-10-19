public class TristanaBasicAttack : CharacterBasicAttack
{
    public delegate void OnBasicAttackKilledEntityHandler(Entity killedEntity);
    public event OnBasicAttackKilledEntityHandler OnBasicAttackKilledEntity;

    protected TristanaBasicAttack()
    {
        delayPercentBeforeAttack = 0.1666f;
        speed = 1900;

        basicAttackPrefabPath = "BasicAttacksPrefabs/Characters/Tristana/TristanaBA";
    }

    protected override void ApplyDamageToEntityHit(Entity entityHit, bool isACriticalStrike)
    {
        bool entityHitWasDeadBeforeTheBasicAttackHit = entityHit.EntityStats.Health.IsDead();

        base.ApplyDamageToEntityHit(entityHit, isACriticalStrike);

        if (entityHit.EntityStats.Health.IsDead() && !entityHitWasDeadBeforeTheBasicAttackHit && OnBasicAttackKilledEntity != null)
        {
            OnBasicAttackKilledEntity(entityHit);
        }
    }
}

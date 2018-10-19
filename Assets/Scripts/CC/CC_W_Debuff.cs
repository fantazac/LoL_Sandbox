public class CC_W_Debuff : AbilityBuff
{
    private float knockupSpeed;

    protected CC_W_Debuff()
    {
        buffName = "CC BTW HAHA";

        isADebuff = true;

        buffDuration = 2;
        buffCrowdControlEffect = CrowdControlEffects.KNOCKUP;

        knockupSpeed = 2;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/CC/CCW_Debuff";
    }

    protected override void ApplyBuffEffect(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStatusManager.AddCrowdControlEffectOnEntity(buffCrowdControlEffect);
        affectedEntity.EntityStatusMovementManager.SetupMovementBlock(buffCrowdControlEffect, this, character, transform.position, buffDuration, knockupSpeed);
    }

    protected override void RemoveBuffEffect(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStatusManager.RemoveCrowdControlEffectFromEntity(buffCrowdControlEffect);
        affectedEntity.EntityStatusMovementManager.EndMovementBlock(buffCrowdControlEffect, this);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, 0, buffDuration);
    }
}
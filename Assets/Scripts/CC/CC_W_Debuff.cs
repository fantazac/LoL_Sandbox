using UnityEngine;

public class CC_W_Debuff : AbilityBuff
{
    //private float knockupSpeed;
    //private Vector3 knockupDestination;

    protected CC_W_Debuff()
    {
        buffName = "CC BTW HAHA";

        isADebuff = true;

        buffDuration = 2;

        //knockupSpeed = 3;
        //knockupDestination = Vector3.up * knockupSpeed * buffDuration * 0.5f;

        buffCrowdControlEffect = CrowdControlEffects.CHARM;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/CC/CCW_Debuff";
    }

    protected override void ApplyBuffEffect(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStatusManager.AddCrowdControlEffectOnEntity(buffCrowdControlEffect);
        //affectedEntity.EntityForcedActionManager.SetupForcedAction(buffCrowdControlEffect, this, character);
        //affectedEntity.EntityDisplacementManager.SetupDisplacement(knockupDestination, knockupSpeed, this, true);
    }

    protected override void RemoveBuffEffect(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStatusManager.RemoveCrowdControlEffectFromEntity(buffCrowdControlEffect);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, 0, GetBuffDuration(affectedEntity));
    }
}
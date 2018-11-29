using UnityEngine;

public class CC_Q_Debuff : AbilityBuff
{
    //private float knockupSpeed;
    //private Vector3 knockupDestination;

    protected CC_Q_Debuff()
    {
        buffName = "CC BTW";

        isADebuff = true;

        buffDuration = 2;

        //knockupSpeed = 3;
        //knockupDestination = Vector3.up * knockupSpeed * buffDuration * 0.5f;

        buffCrowdControlEffect = CrowdControlEffect.TAUNT;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/CC/CCQ_Debuff";
    }

    protected override void ApplyBuffEffect(Entity affectedEntity, Buff buff)
    {
        affectedEntity.EntityStatusManager.AddCrowdControlEffect(buffCrowdControlEffect);
        affectedEntity.EntityForcedActionManager.SetupForcedAction(buffCrowdControlEffect, this, character);
        //affectedEntity.EntityDisplacementManager.SetupDisplacement(knockupDestination, knockupSpeed, this, true);
    }

    protected override void RemoveBuffEffect(Entity affectedEntity, Buff buff)
    {
        affectedEntity.EntityStatusManager.RemoveCrowdControlEffect(buffCrowdControlEffect);
        affectedEntity.EntityForcedActionManager.StopCurrentForcedAction(this);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, 0, GetBuffDuration(affectedEntity));
    }
}
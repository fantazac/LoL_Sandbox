using UnityEngine;

public class Heal_Debuff : AbilityBuff
{
    protected Heal_Debuff()
    {
        buffName = "Heal";

        isADebuff = true;

        buffDuration = 35;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/SummonerAbilities/Heal";
    }

    public override void AddNewBuffToAffectedEntity(Entity affectedEntity)
    {
        SetupBuff(affectedEntity.BuffManager.GetDebuffOfSameType(this), affectedEntity);
    }

    protected override void SetupBuff(Buff buff, Entity affectedEntity)
    {
        if (buff != null)
        {
            Consume(affectedEntity, buff);
        }
        affectedEntity.BuffManager.ApplyBuff(CreateNewBuff(affectedEntity), buffSprite, isADebuff);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, 0, GetBuffDuration(affectedEntity));
    }
}

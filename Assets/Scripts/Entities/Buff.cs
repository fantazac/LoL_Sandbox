public class Buff
{
    private Ability sourceAbility;
    private Entity entityHit;

    private float duration;
    private float durationRemaining;

    public Buff(Ability sourceAbility, Entity entityHit, float duration)
    {
        this.sourceAbility = sourceAbility;
        this.entityHit = entityHit;
        this.duration = duration;

        durationRemaining = duration;

        sourceAbility.ApplyBuffToEntityHit(entityHit);
    }

    public Buff(Ability sourceAbility, Entity entityHit) : this(sourceAbility, entityHit, 0) { }

    public void RemoveBuff()
    {
        sourceAbility.RemoveBuffFromEntityHit(entityHit);
    }

    public void ReduceDurationRemaining(float frameDuration)
    {
        durationRemaining -= frameDuration;
    }

    public bool HasExpired()
    {
        return durationRemaining <= 0;
    }

    public bool HasDuration()
    {
        return duration > 0;
    }
}

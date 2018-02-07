public class Buff
{
    private Ability sourceAbility;
    private Entity entityHit;

    public float Duration { get; private set; }
    public float DurationRemaining { get; private set; }
    public int Stacks { get; private set; }

    public bool HasDuration { get; private set; }
    public bool HasStacks { get; private set; }

    public Buff(Ability sourceAbility, Entity entityHit, float duration, int stacks)
    {
        this.sourceAbility = sourceAbility;
        this.entityHit = entityHit;
        Duration = duration;
        DurationRemaining = duration;
        Stacks = stacks;

        HasDuration = duration > 0;
        HasStacks = stacks > 0;

        sourceAbility.ApplyBuffToEntityHit(entityHit);
    }

    public Buff(Ability sourceAbility, Entity entityHit) : this(sourceAbility, entityHit, 0, 0) { }
    public Buff(Ability sourceAbility, Entity entityHit, float duration) : this(sourceAbility, entityHit, duration, 0) { }

    public void RemoveBuff()
    {
        sourceAbility.RemoveBuffFromEntityHit(entityHit);
    }

    public void ReduceDurationRemaining(float frameDuration)
    {
        DurationRemaining -= frameDuration;
    }

    public bool HasExpired()
    {
        return DurationRemaining <= 0;
    }
}

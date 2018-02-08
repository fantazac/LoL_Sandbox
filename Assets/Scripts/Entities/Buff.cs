public class Buff
{
    private Ability sourceAbility;
    private Entity entityHit;

    public float Duration { get; private set; }
    public float DurationForUI { get; private set; }
    public float DurationRemaining { get; private set; }

    public int CurrentStacks { get; private set; }
    public int MaximumStacks { get; private set; }
    public float StackDecayingDuration { get; private set; }

    public bool HasDuration { get; private set; }
    public bool HasStacks { get; private set; }

    public Buff(Ability sourceAbility, Entity entityHit, float buffDuration, int maximumStacks, float stackDecayingDuration)
    {
        this.sourceAbility = sourceAbility;
        this.entityHit = entityHit;
        Duration = buffDuration;
        DurationForUI = buffDuration;
        DurationRemaining = buffDuration;
        MaximumStacks = maximumStacks;
        StackDecayingDuration = stackDecayingDuration;
        if (maximumStacks > 0)
        {
            CurrentStacks = 1;
        }

        HasDuration = buffDuration > 0;
        HasStacks = maximumStacks > 0;
    }

    public Buff(Ability sourceAbility, Entity entityHit) : this(sourceAbility, entityHit, 0, 0, 0) { }
    public Buff(Ability sourceAbility, Entity entityHit, float duration) : this(sourceAbility, entityHit, duration, 0, 0) { }
    public Buff(Ability sourceAbility, Entity entityHit, float duration, int maximumStacks) : this(sourceAbility, entityHit, duration, maximumStacks, 0) { }

    public void ApplyBuff()
    {
        sourceAbility.ApplyBuffToEntityHit(entityHit);
    }

    public void RemoveBuff()
    {
        sourceAbility.RemoveBuffFromEntityHit(entityHit);
    }

    public void ReduceDurationRemaining(float frameDuration)
    {
        DurationRemaining -= frameDuration;
        if (DurationRemaining <= 0 && !HasExpired())
        {
            sourceAbility.RemoveBuffFromEntityHit(entityHit);
            if(StackDecayingDuration > 0)
            {
                CurrentStacks--;
                sourceAbility.ApplyBuffToEntityHit(entityHit);
                DurationRemaining = StackDecayingDuration;
                DurationForUI = StackDecayingDuration;
            }
            else
            {
                CurrentStacks = 0;
            }
        }
    }

    public void ResetDurationRemaining()
    {
        DurationForUI = Duration;
        DurationRemaining = Duration;
    }

    public void IncreaseCurrentStacks()
    {
        if (CurrentStacks < MaximumStacks)
        {
            sourceAbility.RemoveBuffFromEntityHit(entityHit);
            CurrentStacks++;
            sourceAbility.ApplyBuffToEntityHit(entityHit);
        }
    }

    public bool HasExpired()
    {
        if (MaximumStacks == 0)
        {
            return DurationRemaining <= 0;
        }
        else
        {
            return CurrentStacks <= 0;
        }
    }
}

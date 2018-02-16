public class Buff
{
    private bool isADebuff;

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

    public Buff(Ability sourceAbility, Entity entityHit, bool isADebuff, float buffDuration, int maximumStacks, float stackDecayingDuration)
    {
        this.sourceAbility = sourceAbility;
        this.entityHit = entityHit;
        this.isADebuff = isADebuff;
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

    public Buff(Ability sourceAbility, Entity entityHit, bool isADebuff) : this(sourceAbility, entityHit, isADebuff, 0, 0, 0) { }
    public Buff(Ability sourceAbility, Entity entityHit, bool isADebuff, float duration) : this(sourceAbility, entityHit, isADebuff, duration, 0, 0) { }
    public Buff(Ability sourceAbility, Entity entityHit, bool isADebuff, float duration, int maximumStacks) : this(sourceAbility, entityHit, isADebuff, duration, maximumStacks, 0) { }

    public void ApplyBuff()
    {
        if (isADebuff)
        {
            sourceAbility.ApplyDebuffToEntityHit(entityHit);
        }
        else
        {
            sourceAbility.ApplyBuffToEntityHit(entityHit);
        }
    }

    public void RemoveBuff()
    {
        if (isADebuff)
        {
            sourceAbility.RemoveDebuffFromEntityHit(entityHit);
        }
        else
        {
            sourceAbility.RemoveBuffFromEntityHit(entityHit);
        }
    }

    public void ConsumeBuff()
    {
        CurrentStacks = 0;
        DurationRemaining = 0;
    }

    public void ReduceDurationRemaining(float frameDuration)
    {
        DurationRemaining -= frameDuration;
        if (DurationRemaining <= 0 && !HasExpired())
        {
            sourceAbility.RemoveBuffFromEntityHit(entityHit);
            if (StackDecayingDuration > 0)
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
            return CurrentStacks == 0;
        }
    }
}

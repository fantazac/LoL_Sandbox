public class Buff
{
    private bool isADebuff;

    public Ability SourceAbility { get; private set; }
    private Entity entityHit;

    public float Duration { get; private set; }
    public float DurationForUI { get; private set; }
    public float DurationRemaining { get; private set; }

    public int CurrentStacks { get; private set; }
    public int MaximumStacks { get; private set; }
    public float StackDecayingDuration { get; private set; }

    public bool HasBeenConsumed { get; private set; }
    public bool HasDuration { get; private set; }
    public bool HasStacks { get; private set; }

    public Buff(Ability sourceAbility, Entity entityHit, bool isADebuff, float buffDuration, int maximumStacks, float stackDecayingDuration)
    {
        SourceAbility = sourceAbility;
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
            SourceAbility.ApplyDebuffToEntityHit(entityHit, CurrentStacks);
        }
        else
        {
            SourceAbility.ApplyBuffToEntityHit(entityHit, CurrentStacks);
        }
    }

    public void RemoveBuff()
    {
        if (isADebuff)
        {
            SourceAbility.RemoveDebuffFromEntityHit(entityHit, CurrentStacks);
        }
        else
        {
            SourceAbility.RemoveBuffFromEntityHit(entityHit, CurrentStacks);
        }
    }

    public void ConsumeBuff()
    {
        CurrentStacks = 0;
        DurationRemaining = 0;
        HasBeenConsumed = true;
    }

    public void ReduceDurationRemaining(float frameDuration)
    {
        DurationRemaining -= frameDuration;
        if (DurationRemaining <= 0)
        {
            if (!HasExpired())
            {
                SourceAbility.RemoveBuffFromEntityHit(entityHit, CurrentStacks);
                if (StackDecayingDuration > 0)
                {
                    CurrentStacks--;
                    SourceAbility.ApplyBuffToEntityHit(entityHit, CurrentStacks);
                    DurationRemaining = StackDecayingDuration;
                    DurationForUI = StackDecayingDuration;
                }
                else
                {
                    ConsumeBuff();
                }
            }
            else
            {
                ConsumeBuff();
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
            SourceAbility.RemoveBuffFromEntityHit(entityHit, CurrentStacks);
            SourceAbility.ApplyBuffToEntityHit(entityHit, ++CurrentStacks);
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

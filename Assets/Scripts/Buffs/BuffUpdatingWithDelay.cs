public class BuffUpdatingWithDelay : Buff // TODO: currently only works if buffDurationPostDelay = 0
{
    private float buffDurationPostDelay;
    private float buffValuePostDelay;

    public BuffUpdatingWithDelay(AbilityBuff sourceAbilityBuff, Unit affectedUnit, float buffValue, float buffDuration, float buffValuePostDelay,
        float buffDurationPostDelay = 0, int maximumStacks = 0, float stackDecayingDelay = 0)
        : base(sourceAbilityBuff, affectedUnit, buffValue, buffDuration, maximumStacks, stackDecayingDelay)
    {
        this.buffDurationPostDelay = buffDurationPostDelay;
        this.buffValuePostDelay = buffValuePostDelay;
    }

    private void BuffPostDelay()
    {
        float oldBuffValue = BuffValue;
        BuffValue = buffValuePostDelay;
        Duration = buffDurationPostDelay;
        DurationForUI = buffDurationPostDelay;
        DurationRemaining = buffDurationPostDelay;

        InitProperties();
        SourceAbilityBuff.UpdateBuffOnAffectedUnit(affectedUnit, oldBuffValue, BuffValue);

        SetBuffValueOnUI();
    }

    public void SetBuffValuePostDelay(float newBuffValuePostDelay)
    {
        buffValuePostDelay = newBuffValuePostDelay;
    }

    public void SetBuffValueAndBuffDurationPostDelay(float newBuffValuePostDelay, float newBuffDurationPostDelay)
    {
        buffValuePostDelay = newBuffValuePostDelay;
        buffDurationPostDelay = newBuffDurationPostDelay;
    }

    public override void ConsumeBuff()
    {
        BuffPostDelay();
    }
}

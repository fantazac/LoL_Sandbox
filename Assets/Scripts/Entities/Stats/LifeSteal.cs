public class LifeSteal : Stat
{
    public LifeSteal() : base() { }

    public override void UpdateTotal()
    {
        total = currentBaseValue + percentBonus;
    }
}

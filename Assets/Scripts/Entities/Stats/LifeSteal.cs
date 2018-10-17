public class LifeSteal : Stat
{
    public LifeSteal() : base() { }

    public override void UpdateTotal()
    {
        total = percentBonus * 0.01f;
    }
}

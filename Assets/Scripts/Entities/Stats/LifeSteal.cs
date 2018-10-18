public class LifeSteal : Stat
{
    public override void UpdateTotal()
    {
        total = percentBonus * 0.01f;
    }
}

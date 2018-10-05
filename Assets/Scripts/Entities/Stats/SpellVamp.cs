public class SpellVamp : Stat
{
    public SpellVamp() : base() { }

    public override void UpdateTotal()
    {
        total = currentBaseValue + percentBonus;
    }
}

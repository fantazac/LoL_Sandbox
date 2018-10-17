public class SpellVamp : Stat
{
    public SpellVamp() : base() { }

    public override void UpdateTotal()
    {
        total = percentBonus * 0.01f;
    }
}

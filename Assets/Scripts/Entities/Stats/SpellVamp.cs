public class SpellVamp : Stat
{
    public override void UpdateTotal()
    {
        total = percentBonus * 0.01f;
    }
}

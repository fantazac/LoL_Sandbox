public class Lethality : Stat
{
    private int characterLevel;

    public Lethality()
    {
        characterLevel = 1;
    }

    public override void OnLevelUp(int level)
    {
        characterLevel = level;

        base.OnLevelUp(level);
    }

    public float GetCurrentValue()
    {
        return total * (0.6f + ((0.4f * characterLevel) / 18));
    }

    public override void UpdateTotal()
    {
        total = baseValue + flatBonus - flatMalus;
    }
}

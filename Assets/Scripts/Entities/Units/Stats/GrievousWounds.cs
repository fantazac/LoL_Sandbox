public class GrievousWounds : Stat
{
    private int grievousWoundsSourcesCount;

    private static float HEALING_REDUCTION = 0.4f;

    public override void UpdateTotal()
    {
        total = grievousWoundsSourcesCount > 0 ? (1 - HEALING_REDUCTION) : 1;
    }

    public void AddGrievousWoundsSource()
    {
        grievousWoundsSourcesCount++;
        UpdateTotal();
    }

    public void RemoveGrievousWoundsSource()
    {
        grievousWoundsSourcesCount--;
        UpdateTotal();
    }
}

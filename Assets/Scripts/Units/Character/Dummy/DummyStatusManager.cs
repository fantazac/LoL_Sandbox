public class DummyStatusManager : StatusManager
{
    private Dummy dummy;

    private void Start()
    {
        dummy = GetComponent<Dummy>();
    }

    protected override void OnDisarm() { }
    protected override void OnDisrupt() { }
    protected override void OnEntangle() { }
    protected override void OnForcedAction() { }
    protected override void OnKnockUp() { }
    protected override void OnRoot() { }
    protected override void OnSilence() { }
    protected override void OnStun() { }
    protected override void OnSuspension() { }

    protected override void OnKnockDown()
    {
        dummy.DisplacementManager.StopCurrentDisplacement();
    }
}

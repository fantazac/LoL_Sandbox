public abstract class MovingUnit : Unit
{
    public MovementManager MovementManager { get { return GetMovementManager(); } }

    protected abstract MovementManager GetMovementManager();
}

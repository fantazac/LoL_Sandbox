public abstract class MovingUnit : Unit
{
    public MovementManager MovementManager => GetMovementManager();

    protected abstract MovementManager GetMovementManager();
}

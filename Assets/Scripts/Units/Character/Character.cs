public abstract class Character : Unit
{
    protected override void InitUnitProperties()
    {
        base.InitUnitProperties();

        InitCharacterProperties();
    }

    protected abstract void InitCharacterProperties();

    protected override void Start()
    {
        if (StaticObjects.Champion && StaticObjects.Champion.HealthBarManager)
        {
            StaticObjects.Champion.HealthBarManager.SetupHealthBarForCharacter(this);
        }

        UnitType = UnitType.CHARACTER;

        base.Start();
    }

    protected void OnDestroy()
    {
        RemoveHealthBar();
    }

    public void RemoveHealthBar()
    {
        if (StaticObjects.Champion && StaticObjects.Champion.HealthBarManager)
        {
            StaticObjects.Champion.HealthBarManager.RemoveHealthBarOfDeletedCharacter(this);
        }
    }
}

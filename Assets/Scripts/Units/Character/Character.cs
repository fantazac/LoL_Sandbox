using UnityEngine;

public abstract class Character : Unit
{
    public Vector3 CharacterHeightOffset { get; private set; }

    protected override void InitUnitProperties()
    {
        base.InitUnitProperties();

        InitCharacterProperties();
    }

    protected abstract void InitCharacterProperties();

    protected override void Start()
    {
        CharacterHeightOffset = Vector3.up * transform.position.y;

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

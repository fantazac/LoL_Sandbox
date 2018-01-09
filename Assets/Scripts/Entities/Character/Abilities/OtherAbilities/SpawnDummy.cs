using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnDummy : Ability, OtherAbility
{
    protected const int MAXIMUM_DUMMY_AMOUNT = 4;

    protected string dummyResourceName;

    protected List<GameObject> dummies;

    protected SpawnDummy()
    {
        dummies = new List<GameObject>();
        OfflineOnly = true;
    }

    protected override void Start()
    {
        if (!StaticObjects.OnlineMode)
        {
            base.Start();
        }
    }

    public override void OnPressedInput(Vector3 mousePosition)
    {
        if (CanUseSkill(mousePosition))
        {
            UseAbility(hit.point + character.CharacterMovement.CharacterHeightOffset);
        }
    }

    public void RemoveAllDummies()
    {
        while (dummies.Count > 0)//more efficient way to do this?
        {
            Destroy(dummies[0]);
            dummies.RemoveAt(0);
        }
    }

    public override void UseAbility(Vector3 destination)
    {
        if (dummies.Count == MAXIMUM_DUMMY_AMOUNT)
        {
            Destroy(dummies[0]);
            dummies.RemoveAt(0);
        }
        GameObject dummy = (GameObject)Instantiate(Resources.Load(dummyResourceName), destination, Quaternion.identity);
        dummy.transform.rotation = Quaternion.LookRotation((transform.position - dummy.transform.position).normalized);
        dummies.Add(dummy);
    }
}

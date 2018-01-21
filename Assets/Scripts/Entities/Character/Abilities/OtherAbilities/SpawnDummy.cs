using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnDummy : GroundTargeted, OtherAbility
{
    protected const int MAXIMUM_DUMMY_AMOUNT = 4;

    protected string dummyResourceName;
    protected int dummyId;
    protected EntityTeam team;

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

    public override bool CanBeCast(Vector3 mousePosition, CharacterAbilityManager characterAbilityManager)
    {
        return  (!StaticObjects.OnlineMode || !OfflineOnly) && base.CanBeCast(mousePosition, characterAbilityManager);
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
        if (dummyId == 4 || dummyId == 8)
        {
            dummyId -= 4;
        }
        Dummy dummy = ((GameObject)Instantiate(Resources.Load(dummyResourceName), destination, Quaternion.identity)).GetComponent<Dummy>();
        dummy.transform.rotation = Quaternion.LookRotation((transform.position - dummy.transform.position).normalized);
        dummy.team = team;
        dummy.characterId = ++dummyId;
        dummies.Add(dummy.gameObject);
    }
}

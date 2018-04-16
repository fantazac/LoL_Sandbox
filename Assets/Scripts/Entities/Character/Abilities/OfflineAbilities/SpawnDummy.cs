using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnDummy : GroundTargeted, OfflineAbility
{
    protected const int MAXIMUM_DUMMY_AMOUNT = 4;

    protected string dummyResourcePath;
    protected int dummyId;
    protected EntityTeam team;

    protected List<Dummy> dummies;

    protected SpawnDummy()
    {
        dummies = new List<Dummy>();
        OfflineOnly = true;

        IsEnabled = true;
    }

    protected override void Start()
    {
        if (!StaticObjects.OnlineMode)
        {
            base.Start();
        }
    }

    public override bool CanBeCast(Vector3 mousePosition)
    {
        return (!StaticObjects.OnlineMode || !OfflineOnly) && base.CanBeCast(mousePosition);
    }

    public void RemoveAllDummies()
    {
        StopAllCoroutines();
        while (dummies.Count > 0)//more efficient way to do this?
        {
            if (dummies[0])
            {
                dummies[0].RemoveHealthBar();
            }
            Destroy(dummies[0].gameObject);
            dummies.RemoveAt(0);
        }
    }

    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        if (dummies.Count == MAXIMUM_DUMMY_AMOUNT)
        {
            Destroy(dummies[0].gameObject);
            dummies.RemoveAt(0);
        }
        if (dummyId == 4 || dummyId == 8)
        {
            dummyId -= 4;
        }
        Dummy dummy = ((GameObject)Instantiate(Resources.Load(dummyResourcePath), destination, Quaternion.identity)).GetComponent<Dummy>();
        dummy.transform.rotation = Quaternion.LookRotation((transform.position - dummy.transform.position).normalized);
        dummy.SetDummyTeamAndID(team, ++dummyId);
        dummies.Add(dummy);
        
        FinishAbilityCast();
    }

    protected override void SetResourcePaths() { }
}

﻿using System.Collections;
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
        while (dummies.Count > 0)//more efficient way to do this?
        {
            if (dummies[0])
            {
                dummies[0].GetComponent<Character>().RemoveHealthBar();
            }
            Destroy(dummies[0]);
            dummies.RemoveAt(0);
        }
    }

    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

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
        dummy.SetDummyTeamAndID(team, ++dummyId);
        dummies.Add(dummy.gameObject);
        StaticObjects.Character.HealthBarManager.SetupHealthBarForCharacter(dummy);

        FinishAbilityCast();
    }

    protected override void SetSpritePaths() { }
}

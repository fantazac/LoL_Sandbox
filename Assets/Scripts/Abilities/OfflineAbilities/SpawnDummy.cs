using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnDummy : GroundTargeted //GroundTargetedAoE
{
    private const int MAXIMUM_DUMMY_AMOUNT = 4;
    protected const int MAXIMUM_DUMMY_ID = MAXIMUM_DUMMY_AMOUNT + 1;

    protected int maxDummyId;
    protected int minDummyId;

    protected int dummyId;
    protected string dummyName;
    protected string dummyResourcePath;
    private GameObject dummyPrefab;
    protected Team team;

    private readonly List<Dummy> dummies;

    protected SpawnDummy()
    {
        dummies = new List<Dummy>();

        AbilityLevel = 1;
    }

    protected override void LoadPrefabs()
    {
        base.LoadPrefabs();

        dummyPrefab = Resources.Load<GameObject>(dummyResourcePath);
    }

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = new List<Team>();
    }

    public void RemoveAllDummies()
    {
        while (dummies.Count > 0) //TODO: more efficient way to do this?
        {
            if (dummies[0])
            {
                dummies[0].RemoveHealthBar();
                Destroy(dummies[0].gameObject);
            }

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

        if (dummyId == maxDummyId)
        {
            dummyId -= MAXIMUM_DUMMY_ID;
        }

        Dummy dummy = Instantiate(dummyPrefab, destination, Quaternion.identity).GetComponent<Dummy>();
        dummy.transform.rotation = Quaternion.LookRotation((transform.position - dummy.transform.position).normalized);
        dummy.SetDummyNameAndTeamAndID(dummyName, team, ++dummyId);
        dummies.Add(dummy);

        FinishAbilityCast();
    }

    protected override void SetResourcePaths() { }
}

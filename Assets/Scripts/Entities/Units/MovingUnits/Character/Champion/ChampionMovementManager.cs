﻿using UnityEngine;
using System.Collections;

public class ChampionMovementManager : MovementManager
{
    private readonly string movementCapsulePrefabPath;
    private GameObject movementCapsulePrefab;

    private Vector3 currentlySelectedDestination;
    private float destinationRange;

    private Unit currentlySelectedTarget;
    private Unit currentlySelectedBasicAttackTarget;
    private float targetRange;

    private IEnumerator currentMovementCoroutine;

    private Champion champion;

    public delegate void OnMovementInputReceivedHandler();
    public event OnMovementInputReceivedHandler OnMovementInputReceived;
    
    public delegate void OnChampionIsInDestinationRangeHandler(Vector3 destination);
    public event OnChampionIsInDestinationRangeHandler OnChampionIsInDestinationRange;

    public delegate void OnChampionIsInTargetRangeHandler(Unit target);
    public event OnChampionIsInTargetRangeHandler OnChampionIsInTargetRange;

    public delegate void OnChampionMovedHandler();
    public event OnChampionMovedHandler OnChampionMoved;

    private ChampionMovementManager()
    {
        currentlySelectedDestination = Vector3.down;

        movementCapsulePrefabPath = "MovementCapsulePrefab/MovementCapsule";
    }

    protected override void Awake()
    {
        base.Awake();

        champion = GetComponent<Champion>();
    }

    protected override void Start()
    {
        base.Start();

        if (!champion.IsLocalChampion()) return;

        champion.InputManager.OnPressedS += StopMovementOnInput;

        LoadPrefabs();
    }

    private void LoadPrefabs()
    {
        movementCapsulePrefab = Resources.Load<GameObject>(movementCapsulePrefabPath);
    }

    public void UnsubscribeCameraEvent()
    {
        OnChampionMoved = null;
    }

    public void PrepareMovementTowardsPoint(Vector3 pointOnMap)
    {
        if (StaticObjects.OnlineMode)
        {
            SendToServer_Movement_Point(pointOnMap + champion.CharacterHeightOffset);
        }
        else
        {
            SetMoveTowardsPoint(pointOnMap + champion.CharacterHeightOffset);
        }

        Instantiate(movementCapsulePrefab, pointOnMap, Quaternion.identity);
    }

    public void PrepareMovementTowardsTarget(Unit target)
    {
        if (StaticObjects.OnlineMode)
        {
            SendToServer_Movement_Target(target, champion.StatsManager.AttackRange.GetTotal(), true);
        }
        else
        {
            SetMoveTowardsTarget(target, champion.StatsManager.AttackRange.GetTotal(), true);
        }
    }

    private void SendToServer_Movement_Point(Vector3 destination)
    {
        PhotonNetwork.RemoveRPCs(champion.PhotonView); //if using AllBufferedViaServer somewhere else, this needs to change
        champion.PhotonView.RPC(nameof(ReceiveFromServer_Movement_Point), PhotonTargets.AllBufferedViaServer, destination);
    }

    [PunRPC]
    private void ReceiveFromServer_Movement_Point(Vector3 destination)
    {
        SetMoveTowardsPoint(destination);
    }

    private void SendToServer_Movement_Target(Unit target, float range, bool isBasicAttack)
    {
        PhotonNetwork.RemoveRPCs(champion.PhotonView); //if using AllBufferedViaServer somewhere else, this needs to change
        champion.PhotonView.RPC(nameof(ReceiveFromServer_Movement_Target), PhotonTargets.AllBufferedViaServer, target.ID, range, isBasicAttack);
    }

    [PunRPC]
    private void ReceiveFromServer_Movement_Target(int unitId, float range, bool isBasicAttack)
    {
        SetMoveTowardsTarget(champion.AbilityManager.FindTarget(unitId), range, isBasicAttack);
    }

    public void SetMoveTowardsPoint(Vector3 destination, float range = 0)
    {
        OnMovementInputReceived?.Invoke();
        
        if (currentlySelectedDestination == destination) return;

        if (currentlySelectedDestination != Vector3.down && (OnChampionIsInDestinationRange == null || range > 0) && (OnChampionIsInDestinationRange != null || range == 0))
        {
            currentlySelectedDestination = destination;
            champion.OrientationManager.RotateCharacter(destination);
        }
        else
        {
            StopMovement();
            currentlySelectedDestination = destination;
            destinationRange = range;
            currentMovementCoroutine = range > 0 ? MoveTowardsPointWithRange() : MoveTowardsPoint();
            StartCoroutine(currentMovementCoroutine);
            champion.OrientationManager.RotateCharacter(destination);
        }
    }

    private IEnumerator MoveTowardsPoint()
    {
        champion.AutoAttackManager.EnableAutoAttack();

        while (transform.position != currentlySelectedDestination)
        {
            if (CanUseMovement())
            {
                transform.position = Vector3.MoveTowards(transform.position, currentlySelectedDestination, Time.deltaTime * champion.StatsManager.MovementSpeed.GetTotal());

                NotifyChampionMoved();
            }

            yield return null;
        }

        currentlySelectedDestination = Vector3.down;
        currentMovementCoroutine = null;
    }

    private IEnumerator MoveTowardsPointWithRange()
    {
        champion.AutoAttackManager.StopAutoAttack();

        float distance = Vector3.Distance(currentlySelectedDestination, transform.position);
        while (distance > destinationRange || (distance <= destinationRange && champion.DisplacementManager.IsBeingDisplaced))
        {
            if (CanUseMovement())
            {
                transform.position = Vector3.MoveTowards(transform.position, currentlySelectedDestination, Time.deltaTime * champion.StatsManager.MovementSpeed.GetTotal());

                NotifyChampionMoved();
            }

            yield return null;

            distance = Vector3.Distance(currentlySelectedDestination, transform.position);
        }

        if (OnChampionIsInDestinationRange != null)
        {
            OnChampionIsInDestinationRange(currentlySelectedDestination);
            OnChampionIsInDestinationRange = null;
        }

        champion.AutoAttackManager.EnableAutoAttack();
        currentlySelectedDestination = Vector3.down;
        currentMovementCoroutine = null;
    }

    public void SetMoveTowardsHalfDistanceOfAbilityCastRange()
    {
        if (!IsMovingTowardsPositionForAnEvent()) return;
        
        OnChampionIsInDestinationRange = null;
        destinationRange *= 0.5f;
    }

    public void SetCharacterIsInTargetRangeEventForBasicAttack() //example: Lucian Q out of range -> Lucian W while walking: This cancels the Q cast but continues movement post-W to auto the same target
    {
        if (!currentlySelectedTarget) return;
        
        OnChampionIsInDestinationRange = null;
        OnChampionIsInTargetRange = null;
        champion.BasicAttack.SetupBasicAttack(currentlySelectedTarget, false);
    }

    public void SetMoveTowardsTarget(Unit target, float range, bool isBasicAttack, bool forceNewCoroutine = false)
    {
        OnMovementInputReceived?.Invoke();
        
        if (OnChampionIsInTargetRange == null || forceNewCoroutine)
        {
            StopMovement();
            SetupCorrectTarget(target, isBasicAttack);
            targetRange = isBasicAttack ? champion.StatsManager.AttackRange.GetTotal() : range;
            currentMovementCoroutine = MoveTowardsTarget(isBasicAttack);
            StartCoroutine(currentMovementCoroutine);
        }
        else
        {
            OnChampionIsInTargetRange = null;
            SetupCorrectTarget(target, isBasicAttack);
            if (Vector3.Distance(target.transform.position, transform.position) > range)
            {
                champion.OrientationManager.RotateCharacterUntilReachedTarget(target.transform, isBasicAttack);
            }
        }
    }

    private void SetupCorrectTarget(Unit target, bool isBasicAttack)
    {
        currentlySelectedTarget = !isBasicAttack ? target : null;
        currentlySelectedBasicAttackTarget = isBasicAttack ? target : null;
        if (isBasicAttack)
        {
            champion.BasicAttack.SetupBasicAttack(target, true);
        }
    }

    private IEnumerator MoveTowardsTarget(bool isBasicAttack)
    {
        Unit target = currentlySelectedBasicAttackTarget ? currentlySelectedBasicAttackTarget : currentlySelectedTarget;

        if (target && Vector3.Distance(target.transform.position, transform.position) > targetRange)
        {
            champion.AutoAttackManager.StopAutoAttack();

            Transform targetTransform = target.transform;

            champion.OrientationManager.RotateCharacterUntilReachedTarget(targetTransform, currentlySelectedBasicAttackTarget != null);

            float distance = Vector3.Distance(targetTransform.position, transform.position);
            while (distance > targetRange || (distance <= targetRange && champion.DisplacementManager.IsBeingDisplaced))
            {
                if (CanUseMovement())
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, Time.deltaTime * champion.StatsManager.MovementSpeed.GetTotal());

                    NotifyChampionMoved();
                }

                yield return null;

                target = currentlySelectedBasicAttackTarget ? currentlySelectedBasicAttackTarget : currentlySelectedTarget;
                targetTransform = target ? target.transform : null;

                if (!targetTransform)
                {
                    break;
                }

                distance = Vector3.Distance(targetTransform.position, transform.position);

                if (isBasicAttack)
                {
                    targetRange = champion.StatsManager.AttackRange.GetTotal();
                }
            }

            champion.AutoAttackManager.EnableAutoAttack();
            champion.OrientationManager.StopRotation();
        }

        if (target)
        {
            if (target == currentlySelectedBasicAttackTarget && (!champion.AbilityManager.CanUseBasicAttacks() || !champion.StatusManager.CanUseBasicAttacks() ||
                                                                 champion.DisplacementManager.IsBeingDisplaced)) //Checks if disarmed
            {
                while (!champion.AbilityManager.CanUseBasicAttacks() || !champion.StatusManager.CanUseBasicAttacks() || champion.DisplacementManager.IsBeingDisplaced)
                {
                    yield return null;
                }

                SetMoveTowardsTarget(currentlySelectedBasicAttackTarget, targetRange, true);
            }

            currentlySelectedTarget = null;
            currentlySelectedBasicAttackTarget = null;

            if (OnChampionIsInTargetRange != null)
            {
                OnChampionIsInTargetRange(target);
                OnChampionIsInTargetRange = null;
            }
        }
        else if (!IsMoving())
        {
            StopMovement();
            champion.AutoAttackManager.EnableAutoAttack();
        }

        currentMovementCoroutine = null;
    }

    public void RotateCharacterIfMoving()
    {
        if (currentlySelectedDestination != Vector3.down)
        {
            champion.OrientationManager.RotateCharacter(currentlySelectedDestination);
        }
        else if (currentlySelectedTarget)
        {
            champion.OrientationManager.RotateCharacterUntilReachedTarget(currentlySelectedTarget.transform, true);
        }
        else if (currentlySelectedBasicAttackTarget)
        {
            champion.OrientationManager.RotateCharacterUntilReachedTarget(currentlySelectedBasicAttackTarget.transform, true);
        }
    }

    private void StopMovementOnInput()
    {
        if (StaticObjects.OnlineMode)
        {
            SendToServer_StopMovement();
        }
        else
        {
            champion.AutoAttackManager.StopAutoAttack();
            StopMovement();
        }
    }

    private void SendToServer_StopMovement()
    {
        champion.PhotonView.RPC(nameof(ReceiveFromServer_StopMovement), PhotonTargets.AllViaServer);
    }

    [PunRPC]
    private void ReceiveFromServer_StopMovement()
    {
        champion.AutoAttackManager.StopAutoAttack();
        StopMovement();
    }

    public override void StopMovement(bool resetBufferedAbility = true)
    {
        champion.BasicAttack.StopBasicAttack();
        if (resetBufferedAbility)
        {
            champion.BufferedAbilityManager.ResetBufferedAbility();
        }

        //TODO: Sometimes, it does not enter the if even when there is a coroutine running, so 2+ are alive at once. Try to find why and remove StopAllCoroutines() if possible!
        //if (currentMovementCoroutine != null)
        //{
        //    StopCoroutine(currentMovementCoroutine);
        //    currentMovementCoroutine = null;
        //}
        StopAllCoroutines();
        currentMovementCoroutine = null;

        champion.OrientationManager.StopRotation();
        OnChampionIsInTargetRange = null;
        OnChampionIsInDestinationRange = null;
        currentlySelectedDestination = Vector3.down;
        destinationRange = 0;
        currentlySelectedTarget = null;
        currentlySelectedBasicAttackTarget = null;
        targetRange = 0;
    }

    private bool CanUseMovement()
    {
        return champion.AbilityManager.CanUseMovement() && champion.StatusManager.CanUseMovement() && !champion.DisplacementManager.IsBeingDisplaced;
    }

    public void StopMovementTowardsPoint()
    {
        if (currentlySelectedDestination != Vector3.down)
        {
            StopMovement();
        }
    }

    public void StopMovementTowardsTarget()
    {
        if (IsMovingTowardsTarget())
        {
            StopMovement();
        }
    }

    public void StopMovementTowardsPointIfHasEvent()
    {
        if (IsMovingTowardsPositionForAnEvent())
        {
            StopMovement();
        }
    }

    public void StopMovementTowardsTargetIfHasEvent(bool stopBasicAttack = false)
    {
        if ((currentlySelectedTarget || (stopBasicAttack && currentlySelectedBasicAttackTarget)) && OnChampionIsInTargetRange != null)
        {
            StopMovement();
        }
    }

    public bool IsMoving()
    {
        return IsMovingTowardsPosition() || IsMovingTowardsTarget();
    }

    public bool IsMovingTowardsPosition()
    {
        return currentlySelectedDestination != Vector3.down;
    }

    public bool IsMovingTowardsPositionForAnEvent()
    {
        return IsMovingTowardsPosition() && OnChampionIsInDestinationRange != null;
    }

    public bool IsMovingTowardsTarget()
    {
        return currentlySelectedTarget || currentlySelectedBasicAttackTarget;
    }

    public Unit GetBasicAttackTarget()
    {
        return currentlySelectedBasicAttackTarget;
    }

    public void NotifyChampionMoved()
    {
        OnChampionMoved?.Invoke();
    }
}

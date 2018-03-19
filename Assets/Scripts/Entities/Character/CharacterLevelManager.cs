using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLevelManager : MonoBehaviour
{
    private Character character;

    public int AbilityPoints { get; private set; }
    public int Level { get; private set; }

    private const int MAX_LEVEL = 18;

    private int pointsAvailableForQ;
    private int pointsAvailableForW;
    private int pointsAvailableForE;
    private int pointsAvailableForR;

    public delegate void OnLevelUpHandler(int level);
    public event OnLevelUpHandler OnLevelUp;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    private void Start()
    {
        if (character.AbilityLevelUpUIManager)
        {
            character.AbilityLevelUpUIManager.OnAbilityLevelUp += OnAbilityLevelUp;
            PrepareLevelUp();
        }
    }

    public void PrepareLevelUp()
    {
        if (Level < MAX_LEVEL)
        {
            if (StaticObjects.OnlineMode)
            {
                SendToServer_LevelUp();
            }
            else
            {
                LevelUp();
            }
        }
    }

    public void SetLevelFromLoad(int characterLevel)
    {
        for (int i = 0; i < characterLevel - 1; i++)
        {
            LevelUp();
        }
    }

    private void SendToServer_LevelUp()
    {
        character.PhotonView.RPC("ReceiveFromServer_Level", PhotonTargets.AllViaServer);
    }

    [PunRPC]
    private void ReceiveFromServer_Level()
    {
        LevelUp();
    }

    private void LevelUp()
    {
        ++Level;
        if (!StaticObjects.OnlineMode || character.PhotonView.isMine)
        {
            SetPointsAvaiableForAbilities();
            character.AbilityLevelUpUIManager.gameObject.SetActive(true);
            character.AbilityLevelUpUIManager.SetAbilityPoints(++AbilityPoints, pointsAvailableForQ, pointsAvailableForW, pointsAvailableForE, pointsAvailableForR);
            character.LevelUIManager.SetLevel(Level);
        }
        if (OnLevelUp != null)
        {
            OnLevelUp(Level);
        }
    }

    private void SetPointsAvaiableForAbilities()
    {
        if (Level == 1 || Level == 3 || Level == 5 || Level == 7 || Level == 9)
        {
            pointsAvailableForQ++;
            pointsAvailableForW++;
            pointsAvailableForE++;
        }
        else if (Level == 6 || Level == 11 || Level == 16)
        {
            pointsAvailableForR++;
        }
    }

    private void OnAbilityLevelUp(int abilityId)
    {
        if (abilityId == 0)
        {
            AbilityPoints--;
            pointsAvailableForQ--;
            character.CharacterAbilityManager.LevelUpAbility(AbilityInput.Q);
        }
        else if (abilityId == 1)
        {
            AbilityPoints--;
            pointsAvailableForW--;
            character.CharacterAbilityManager.LevelUpAbility(AbilityInput.W);
        }
        else if (abilityId == 2)
        {
            AbilityPoints--;
            pointsAvailableForE--;
            character.CharacterAbilityManager.LevelUpAbility(AbilityInput.E);
        }
        else if (abilityId == 3)
        {
            AbilityPoints--;
            pointsAvailableForR--;
            character.CharacterAbilityManager.LevelUpAbility(AbilityInput.R);
        }
        character.AbilityLevelUpUIManager.SetAbilityPoints(AbilityPoints, pointsAvailableForQ, pointsAvailableForW, pointsAvailableForE, pointsAvailableForR);
    }
}

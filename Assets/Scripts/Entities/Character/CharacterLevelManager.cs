using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLevelManager : MonoBehaviour
{
    private Character character;

    public int Level { get; private set; }

    private const int MAX_LEVEL = 18;

    public delegate void OnLevelUpHandler(int level);
    public event OnLevelUpHandler OnLevelUp;

    private CharacterLevelManager()
    {
        Level = 1;
    }

    private void Awake()
    {
        character = GetComponent<Character>();
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
        if (character.LevelUIManager != null)
        {
            character.LevelUIManager.SetLevel(Level);
        }
        if (OnLevelUp != null)
        {
            OnLevelUp(Level);
        }
    }
}

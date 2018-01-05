using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDummy : Ability
{
    protected override void Start()
    {
        if (!StaticObjects.OnlineMode)
        {
            base.Start();

            character.CharacterInput.OnPressedN += PressedInput;
        }
        else
        {
            enabled = false;
        }
    }

    protected override void UseAbility()
    {
        GameObject dummy = (GameObject)Instantiate(Resources.Load("Dummy"), transform.position, transform.rotation);
    }
}

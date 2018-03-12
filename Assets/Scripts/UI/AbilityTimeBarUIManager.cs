using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityTimeBarUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ui;
    [SerializeField]
    private Image abilityTimeBar;
    [SerializeField]
    private Text abilityName;
    [SerializeField]
    private Text abilityTime;

    private void Start()
    {
        abilityTimeBar.fillAmount = 0;
        ui.SetActive(false);
    }


}

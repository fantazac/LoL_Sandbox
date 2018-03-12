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
    private Text abilityChannelTime;

    private void Start()
    {
        ResetUI();
    }

    private void ResetUI()
    {
        SetupUI(0, "", "", false);
    }

    private void SetupUI(int fillAmount, string name, string channelTime, bool activateUI)
    {
        StopAllCoroutines();
        if (activateUI)
        {
            abilityTimeBar.fillAmount = fillAmount;
            abilityName.text = name;
            abilityChannelTime.text = channelTime;
            ui.SetActive(activateUI);
        }
        else
        {
            ui.SetActive(activateUI);
            abilityTimeBar.fillAmount = fillAmount;
            abilityName.text = name;
            abilityChannelTime.text = channelTime;
        }
    }

    public void StopCastTimeAndChannelTime()
    {
        ResetUI();
    }

    public void SetCastTime(float castTime, string name)
    {
        SetupUI(0, name, "", true);
        StartCoroutine(CastTime(castTime));
    }

    public void SetCastTimeAndChannelTime(float castTime, float channelTime, string name)
    {
        SetupUI(0, name, "", true);
        StartCoroutine(CastTimeAndChannelTime(castTime, channelTime));
    }

    public void SetChannelTime(float channelTime, string name)
    {
        SetupUI(1, name, channelTime.ToString("0.0"), true);
        StartCoroutine(ChannelTime(channelTime));
    }

    private IEnumerator CastTime(float castTime)
    {
        float pastCastTime = 0;

        while(pastCastTime < castTime)
        {
            yield return null;

            pastCastTime += Time.deltaTime;
            abilityTimeBar.fillAmount = pastCastTime / castTime;
        }

        ResetUI();
    }

    private IEnumerator CastTimeAndChannelTime(float castTime, float channelTime)
    {
        float pastCastTimeOrRemainingChannelTime = 0;

        while (pastCastTimeOrRemainingChannelTime < castTime)
        {
            yield return null;

            pastCastTimeOrRemainingChannelTime += Time.deltaTime;
            abilityTimeBar.fillAmount = pastCastTimeOrRemainingChannelTime / castTime;
        }

        pastCastTimeOrRemainingChannelTime = channelTime;

        while (pastCastTimeOrRemainingChannelTime > 0)
        {
            yield return null;

            pastCastTimeOrRemainingChannelTime -= Time.deltaTime;
            abilityTimeBar.fillAmount = pastCastTimeOrRemainingChannelTime / channelTime;
            abilityChannelTime.text = pastCastTimeOrRemainingChannelTime.ToString("0.0");
        }

        ResetUI();
    }

    private IEnumerator ChannelTime(float channelTime)
    {
        float remainingChannelTime = channelTime;

        while (remainingChannelTime > 0)
        {
            yield return null;

            remainingChannelTime -= Time.deltaTime;
            abilityTimeBar.fillAmount = remainingChannelTime / channelTime;
            abilityChannelTime.text = remainingChannelTime.ToString("0.0");
        }

        ResetUI();
    }
}

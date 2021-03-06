﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject buffOutlinePrefab;

    private Dictionary<Buff, GameObject> buffOutlines;
    private Dictionary<Buff, Image> darkImages;
    private Dictionary<Buff, Text> buffStacksTexts;

    private BuffUIManager()
    {
        buffOutlines = new Dictionary<Buff, GameObject>();
        darkImages = new Dictionary<Buff, Image>();
        buffStacksTexts = new Dictionary<Buff, Text>();
    }

    public void SetNewBuff(Buff buff, Sprite buffSprite)
    {
        GameObject buffOutline = Instantiate(buffOutlinePrefab, new Vector3(), new Quaternion());
        buffOutline.transform.SetParent(transform, false);
        buffOutlines.Add(buff, buffOutline);

        buffOutline.transform.GetChild(0).GetComponent<Image>().sprite = buffSprite;
        darkImages.Add(buff, buffOutline.transform.GetChild(0).GetChild(0).GetComponent<Image>());
        buffStacksTexts.Add(buff, buffOutline.transform.GetComponentInChildren<Text>());

        if (!buff.HasDuration)
        {
            darkImages[buff].fillAmount = 0;
        }
    }

    public void UpdateBuffDuration(Buff buff, float duration, float durationRemaining)
    {
        darkImages[buff].fillAmount = durationRemaining / duration;
    }

    public void UpdateBuffStacks(Buff buff, int buffStacks)
    {
        buffStacksTexts[buff].text = buffStacks > 0 ? "" + buffStacks : "";
    }

    public void UpdateBuffValue(Buff buff, int buffValue)
    {
        buffStacksTexts[buff].text = buffValue > 0 ? "" + buffValue : "";
    }

    public void UpdateBuff(Buff buff)
    {
        if (!buff.HasDuration)
        {
            darkImages[buff].fillAmount = 0;
        }
    }

    public void RemoveExpiredBuff(Buff buff)
    {
        darkImages.Remove(buff);
        buffStacksTexts.Remove(buff);

        GameObject expiredBuffOutline = buffOutlines[buff];
        buffOutlines.Remove(buff);
        Destroy(expiredBuffOutline);
    }
}

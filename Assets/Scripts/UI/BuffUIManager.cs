using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject buffOutlinePrefab;

    private Dictionary<Buff, GameObject> buffOutlines;
    private Dictionary<Buff, Image> antiClockwiseImages;
    private Dictionary<Buff, Image> clockwiseImages;
    private Dictionary<Buff, Text> buffStacksTexts;

    private const float IMAGE_FILL_RATIO = 0.98f;

    private BuffUIManager()
    {
        buffOutlines = new Dictionary<Buff, GameObject>();
        antiClockwiseImages = new Dictionary<Buff, Image>();
        clockwiseImages = new Dictionary<Buff, Image>();
        buffStacksTexts = new Dictionary<Buff, Text>();
    }

    public void SetNewBuff(Buff buff, Sprite buffSprite)
    {
        GameObject buffOutline = Instantiate(buffOutlinePrefab, new Vector3(), new Quaternion());
        buffOutline.transform.SetParent(transform, false);
        buffOutlines.Add(buff, buffOutline);

        Image[] buffImages = buffOutline.transform.GetChild(0).GetComponentsInChildren<Image>();
        antiClockwiseImages.Add(buff, buffImages[1]);
        clockwiseImages.Add(buff, buffImages[2]);
        buffStacksTexts.Add(buff, buffOutline.transform.GetComponentInChildren<Text>());

        if (!buff.HasDuration)
        {
            buffImages[0].fillAmount = 1;
        }
    }

    public void UpdateBuffDuration(Buff buff, float duration, float durationRemaining)
    {
        UpdateBuffDuration(buff, duration, durationRemaining, 0);
    }

    public void UpdateBuffDuration(Buff buff, float duration, float durationRemaining, int buffStacks)
    {
        antiClockwiseImages[buff].fillAmount = (durationRemaining / duration) * IMAGE_FILL_RATIO;
        clockwiseImages[buff].fillAmount = (1 - (durationRemaining / duration)) * IMAGE_FILL_RATIO;
        if(buffStacks > 0)
        {
            buffStacksTexts[buff].text = "" + buffStacks;
        }
    }

    public void RemoveExpiredBuff(Buff buff)
    {
        antiClockwiseImages.Remove(buff);
        clockwiseImages.Remove(buff);
        buffStacksTexts.Remove(buff);

        GameObject expiredBuffOutline = buffOutlines[buff];
        buffOutlines.Remove(buff);
        Destroy(expiredBuffOutline);
    }
}

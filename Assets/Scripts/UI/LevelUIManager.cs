using UnityEngine;
using UnityEngine.UI;

public class LevelUIManager : MonoBehaviour
{
    [SerializeField] private Image characterPortraitImage;
    [SerializeField] private Text levelText;

    public void SetPortraitSprite(Sprite portraitSprite)
    {
        characterPortraitImage.sprite = portraitSprite;
    }

    public void SetLevel(int level)
    {
        levelText.text = level + "";
    }
}

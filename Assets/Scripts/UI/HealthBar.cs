using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image healthImage;
    [SerializeField]
    private Image resourceImage;
    [SerializeField]
    private Text levelText;

    private Transform characterToFollow;

    private Vector2 healthBarOffset;
    private Vector2 characterYOffset;

    private Camera characterCamera;
    private Rect canvas;

    private void Start()
    {
        characterCamera = StaticObjects.CharacterCamera;
        healthBarOffset = Vector2.right * Screen.width * -0.5f + Vector2.up * Screen.height * -0.5f;
        characterYOffset = Vector2.up * 120;
    }

    public void SetupHealthBar(Character character)
    {
        characterToFollow = character.transform;
    }

    private void LateUpdate()
    {
        Vector3 position = characterCamera.WorldToScreenPoint(characterToFollow.position);
        GetComponent<RectTransform>().anchoredPosition =
            Vector2.right * (position.x + healthBarOffset.x) +
            Vector2.up * (position.y + healthBarOffset.y) +
            characterYOffset;
    }
}

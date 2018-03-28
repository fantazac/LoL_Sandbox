using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Transform characterToFollow;

    private Vector2 healthBarOffset;
    private Vector2 characterXOffset;
    private Vector2 characterYOffset;
    private Vector2 characterYOffset2;

    private Camera characterCamera;
    private Rect canvas;

    private void Start()
    {
        characterCamera = StaticObjects.CharacterCamera;
        canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect;
        healthBarOffset = Vector2.right * canvas.width * -0.5f + Vector2.up * canvas.height * -0.5f;
        characterXOffset = Vector2.right * GetComponent<RectTransform>().rect.width * -0.25f;
        characterYOffset = Vector2.up * 120;
        characterYOffset2 = Vector2.up * -40;
    }

    public void SetupHealthBar(Character character)
    {
        characterToFollow = character.transform;
    }

    private void LateUpdate()
    {
        Vector3 position = characterCamera.WorldToScreenPoint(characterToFollow.position);
        GetComponent<RectTransform>().anchoredPosition =
            Vector2.right * (position.x * 2 + healthBarOffset.x) +
            Vector2.up * (position.y * 2 + healthBarOffset.y) +
            characterXOffset +
            characterYOffset +
            characterYOffset2 * ((position.y * 2) / canvas.height);
    }
}

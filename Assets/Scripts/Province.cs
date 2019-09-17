using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Province : MonoBehaviour, I_SmartwallInteractable
{
    private RectTransform textRectTransform;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private Vector3 targetScale;

    private Vector2 targetSize;

    private PolygonCollider2D collider;


    [Header("Settings")]
    [SerializeField]
    private Color startColor;

    private Color targetColor;


    [Header("References")]
    [SerializeField]
    private RectTransform StartTransform;

    private SpriteRenderer spriteRenderer;
    private TextMeshPro text;

    [SerializeField]
    private TopoGame topoGame;


    private void Awake()
    {
        text = GetComponentInChildren<TextMeshPro>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        textRectTransform = text.rectTransform;

        // setup
        collider = gameObject.AddComponent<PolygonCollider2D>();

        text.text = name;
        text.sortingOrder = 2;
        textRectTransform.sizeDelta = new Vector2(collider.bounds.size.x, .3f);
        textRectTransform.position = collider.bounds.center;

        // save target values
        targetPosition = textRectTransform.position;
        targetRotation = textRectTransform.rotation;
        targetScale = textRectTransform.localScale;
        targetSize = textRectTransform.sizeDelta;
        targetColor = spriteRenderer.color;

        // set start values
        text.transform.position = StartTransform.position;
        text.transform.rotation = Quaternion.identity;
        text.transform.localScale = Vector3.one;
        textRectTransform.sizeDelta = StartTransform.sizeDelta;
        spriteRenderer.color = startColor;

        // Hide untill needed
        gameObject.SetActive(false);
    }

    private IEnumerator AnimatedMove(float duration)
    {
        float elapsedTime = 0;

        Vector3 startPos = textRectTransform.position + Vector3.back;
        Quaternion startRot = textRectTransform.rotation;
        Vector3 startScale = textRectTransform.localScale;
        Vector2 startSize = textRectTransform.sizeDelta;

        float progress = 0;
        while (progress < 1)
        {
            progress = elapsedTime / duration;

            textRectTransform.position = Vector3.Lerp(startPos, targetPosition, progress);
            textRectTransform.rotation = Quaternion.Lerp(startRot, targetRotation, progress);
            textRectTransform.localScale = Vector3.Lerp(startScale, targetScale, progress);
            textRectTransform.sizeDelta = Vector2.Lerp(startSize, targetSize, progress);
            spriteRenderer.color = Color.Lerp(startColor, targetColor, progress);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final values
        textRectTransform.position = targetPosition;
        textRectTransform.rotation = targetRotation;
        textRectTransform.localScale = targetScale;
        textRectTransform.sizeDelta = targetSize;
        spriteRenderer.color = targetColor;

        topoGame.NextProvince();
    }

    public void Hit(Vector3 hitPosition)
    {
        if (spriteRenderer.color == startColor)
        {
            StartCoroutine(AnimatedMove(0.5f));
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Province : MonoBehaviour, I_SmartwallInteractable
{
    [SerializeField]
    private RectTransform StartTransform;

    [SerializeField]
    private TextMeshProUGUI text;

    private RectTransform textRectTransform;

    [SerializeField]
    private Image image;

    [SerializeField]
    private Color startColor;

    [SerializeField]
    private Color targetColor;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private Vector3 targetScale;

    private Vector2 targetSize;

    private void Awake()
    {
        textRectTransform = (RectTransform) text.transform;
        targetPosition = textRectTransform.position;
        targetRotation = textRectTransform.rotation;
        targetScale = textRectTransform.localScale;
        targetSize = textRectTransform.sizeDelta;

        text.transform.position = StartTransform.position +
                                  new Vector3(StartTransform.sizeDelta.x, -StartTransform.sizeDelta.y, 0);
        text.transform.rotation = Quaternion.identity;
        text.transform.localScale = Vector3.one;
        textRectTransform.sizeDelta = StartTransform.sizeDelta;

        gameObject.SetActive(false);
    }

    public void MoveToMap()
    { }

    private IEnumerator AnimatedMove(float duration)
    {
        float elapsedTime = 0;

        Vector3 startPos = textRectTransform.position;
        Quaternion startRot = textRectTransform.rotation;
        Vector3 startScale = textRectTransform.localScale;
        Vector2 startSize = textRectTransform.sizeDelta;

        float progress = 0;
        while (progress < 1)
        {
            progress = duration / elapsedTime;
            
            textRectTransform.position = Vector3.Lerp(startPos, targetPosition, progress);
            textRectTransform.rotation = Quaternion.Lerp(startRot, targetRotation, progress);
            textRectTransform.localScale = Vector3.Lerp(startScale, targetScale, progress);
            textRectTransform.sizeDelta = Vector2.Lerp(startSize, targetSize, progress);

            elapsedTime++;
            yield return null;
        }
    }

    public void Hit(Vector3 hitPosition)
    {
        Debug.Log("test");
    }
}
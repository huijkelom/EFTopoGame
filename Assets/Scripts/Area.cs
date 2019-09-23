using System.Collections;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class Area : MonoBehaviour, I_SmartwallInteractable
{
	private RectTransform textRectTransform;

	private Vector3 targetPosition;
	private Quaternion targetRotation;
	private Vector3 targetScale;

	private Vector2 targetSize;

	private Collider2D collider;

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

	[SerializeField]
	private AnimationCurve shakePattern;

	private Coroutine runningCoroutine;

	private bool hit;


	private void Awake()
	{
		// References
		text              = GetComponentInChildren<TextMeshPro>();
		spriteRenderer    = GetComponent<SpriteRenderer>();
		collider          = gameObject.GetComponent<Collider2D>();
		textRectTransform = text.rectTransform;

		// setup
		text.text                   = name;
		text.sortingOrder           = 2;
		textRectTransform.sizeDelta = new Vector2(collider.bounds.size.x, .3f);
		textRectTransform.position  = collider.bounds.center;

		// save target values
		targetPosition = textRectTransform.position;
		targetRotation = textRectTransform.rotation;
		targetScale    = textRectTransform.localScale;
		targetSize     = textRectTransform.sizeDelta;
		targetColor    = spriteRenderer.color;

		// set start values
		text.transform.position     = StartTransform.position;
		text.transform.rotation     = Quaternion.identity;
		text.transform.localScale   = Vector3.one;
		text.color                  = startColor;
		textRectTransform.sizeDelta = StartTransform.sizeDelta;
		spriteRenderer.color        = startColor;

		// Hide until needed
		gameObject.SetActive(false);
	}

	public void Hit(Vector3 hitPosition)
	{
		if (!hit)
		{
			hit = true;
			topoGame.previousAreas.Add(this);
			if (topoGame.previousAreas.Count > 1)
				topoGame.previousAreas[topoGame.previousAreas.Count - 2].Leave();
			StartCoroutine(AnimatedMove(0.5f));
		}
		else
		{
			topoGame.WrongAnswerHit();
		}
	}

	private void OnEnable() => Enter();

	public void Enter()
	{
		if (runningCoroutine != null) StopCoroutine(runningCoroutine);
		runningCoroutine = StartCoroutine(TextEnter(.5f));
	}

	public void Leave()
	{
		if (runningCoroutine != null) StopCoroutine(runningCoroutine);
		runningCoroutine = StartCoroutine(TextLeave(.5f));
	}

	public void Shake()
	{
		if (hit) return;
		if (runningCoroutine != null) StopCoroutine(runningCoroutine);
		runningCoroutine = StartCoroutine(TextShake(.5f));
	}

	private IEnumerator AnimatedMove(float duration)
	{
		float elapsedTime = 0;

		Vector3    startPos   = textRectTransform.position + Vector3.back;
		Quaternion startRot   = textRectTransform.rotation;
		Vector3    startScale = textRectTransform.localScale;
		Vector2    startSize  = textRectTransform.sizeDelta;

		float progress = 0;
		while (progress < 1)
		{
			progress = elapsedTime / duration;

			textRectTransform.position   = Vector3.Lerp(startPos, targetPosition, progress);
			textRectTransform.rotation   = Quaternion.Lerp(startRot, targetRotation, progress);
			textRectTransform.localScale = Vector3.Lerp(startScale, targetScale, progress);
			textRectTransform.sizeDelta  = Vector2.Lerp(startSize, targetSize, progress);
			spriteRenderer.color         = Color.Lerp(startColor, targetColor, progress);

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		// Ensure final values
		textRectTransform.position   = targetPosition;
		textRectTransform.rotation   = targetRotation;
		textRectTransform.localScale = targetScale;
		textRectTransform.sizeDelta  = targetSize;
		spriteRenderer.color         = targetColor;

		runningCoroutine = null;

		topoGame.NextArea();
	}

	private IEnumerator TextLeave(float duration)
	{
		float elapsedTime = 0;

		Vector3 startPos = textRectTransform.position;
		Vector3 endPos   = startPos + Vector3.right;

		Color color = text.color;

		float progress = 0;
		while (progress < 1)
		{
			progress = elapsedTime / duration;

			textRectTransform.position = Vector3.Lerp(startPos, endPos, progress);
			text.color                 = Color.Lerp(color, new Color(0, 0, 0, 0), progress);

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		// Ensure final values
		textRectTransform.position = endPos;
		text.color                 = new Color(0, 0, 0, 0);

		runningCoroutine = null;
	}

	private IEnumerator TextShake(float duration)
	{
		float elapsedTime = 0;

		Vector3 startPos = textRectTransform.position;

		float progress = 0;
		while (progress < 1)
		{
			progress = elapsedTime / duration;

			textRectTransform.position = startPos + new Vector3(shakePattern.Evaluate(progress), 0);

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		textRectTransform.position = startPos;

		runningCoroutine = null;
	}

	private IEnumerator TextEnter(float duration)
	{
		float elapsedTime = 0;

		Vector3 startPos = textRectTransform.position + Vector3.right;
		Vector3 endPos   = textRectTransform.position;

		Color color = text.color;

		float progress = 0;
		while (progress < 1)
		{
			progress = elapsedTime / duration;

			textRectTransform.position = Vector3.Lerp(startPos, endPos, progress);
			text.color                 = Color.Lerp(new Color(0, 0, 0, 0), color, progress);

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		// Ensure final values
		textRectTransform.position = endPos;
		text.color                 = color;

		runningCoroutine = null;
	}
}
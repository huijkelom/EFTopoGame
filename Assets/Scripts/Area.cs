using System.Collections;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class Area : MonoBehaviour, I_SmartwallInteractable
{
	private RectTransform _textRectTransform;

	private Vector3 _targetPosition;
	private Quaternion _targetRotation;
	private Vector3 _targetScale;

	private Vector2 _targetSize;

	public Collider2D HitCollider { get; private set; }

	[Header("Settings")]
	[SerializeField]
	private Color startColor;
	private Color _targetColor;

	[Header("References")]
	[SerializeField]
	private RectTransform startTransform;
	private SpriteRenderer _spriteRenderer;
	private TextMeshPro _text;

	[SerializeField]
	private TopoGame topoGame;

	[SerializeField]
	private AnimationCurve shakePattern;

	private Coroutine _runningCoroutine;

	private bool _hit;
	public bool ready = false;

	private void Awake()
	{
		// References
		_text              = GetComponentInChildren<TextMeshPro>();
		_spriteRenderer    = GetComponent<SpriteRenderer>();
		HitCollider        = gameObject.GetComponent<Collider2D>();
		_textRectTransform = _text.rectTransform;

		// setup
		_text.text         = name;
		_text.sortingOrder = 2;
		var bounds = HitCollider.bounds;
		_textRectTransform.sizeDelta = new Vector2(bounds.size.x, .3f);
		_textRectTransform.position  = bounds.center;

		// save target values
		_targetPosition = _textRectTransform.position;
		_targetRotation = _textRectTransform.rotation;
		_targetScale    = _textRectTransform.localScale;
		_targetSize     = _textRectTransform.sizeDelta;
		_targetColor    = _spriteRenderer.color;

		// set start values
		_text.transform.position     = startTransform.position;
		_text.transform.rotation     = Quaternion.identity;
		_text.transform.localScale   = Vector3.one;
		_text.color                  = startColor;
		_textRectTransform.sizeDelta = startTransform.sizeDelta;
		_spriteRenderer.color        = startColor;

		// Hide until needed
		HitCollider.enabled = false;
		_textRectTransform.gameObject.SetActive(false);
	}

	public void Hit(Vector3 hitPosition)
	{
		if (!_hit && ready)
		{
			_hit = true;
			topoGame.previousAreas.Add(this);
			if (topoGame.previousAreas.Count > 1)
				topoGame.previousAreas[topoGame.previousAreas.Count - 2].Leave();

			if (_runningCoroutine != null) StopCoroutine(_runningCoroutine);
			StartCoroutine(AnimatedMove(0.5f));
		}
		else
		{
			topoGame.WrongAnswerHit(hitPosition);
		}
	}

	private void OnEnable() => Enter();

	public void Enter()
	{
		if (_runningCoroutine != null) StopCoroutine(_runningCoroutine);
		_runningCoroutine = StartCoroutine(TextEnter(.5f));
	}

	public void Leave()
	{
		if (_runningCoroutine != null) StopCoroutine(_runningCoroutine);
		_runningCoroutine = StartCoroutine(TextLeave(.5f));
	}

	public void Shake()
	{
		if (_hit || _runningCoroutine != null) return;
		_runningCoroutine = StartCoroutine(TextShake(.5f));
	}

	private IEnumerator AnimatedMove(float duration)
	{
		float elapsedTime = 0;

		Vector3    startPos   = _textRectTransform.position + Vector3.back;
		Quaternion startRot   = _textRectTransform.rotation;
		Vector3    startScale = _textRectTransform.localScale;
		Vector2    startSize  = _textRectTransform.sizeDelta;

		for (float elapsed = 0; elapsed < duration; elapsed += Time.deltaTime)
		{
			float progress = elapsedTime / duration;
			_textRectTransform.position   = Vector3.Lerp(startPos, _targetPosition, progress);
			_textRectTransform.rotation   = Quaternion.Lerp(startRot, _targetRotation, progress);
			_textRectTransform.localScale = Vector3.Lerp(startScale, _targetScale, progress);
			_textRectTransform.sizeDelta  = Vector2.Lerp(startSize, _targetSize, progress);
			_spriteRenderer.color         = Color.Lerp(startColor, _targetColor, progress);

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		// Ensure final values
		_textRectTransform.position   = _targetPosition;
		_textRectTransform.rotation   = _targetRotation;
		_textRectTransform.localScale = _targetScale;
		_textRectTransform.sizeDelta  = _targetSize;
		_spriteRenderer.color         = _targetColor;

		_runningCoroutine = null;

		topoGame.NextArea();
	}

	private IEnumerator TextLeave(float duration)
	{
		float elapsedTime = 0;

		Vector3 startPos = _textRectTransform.position;
		Vector3 endPos   = startPos + Vector3.right;

		Color color = _text.color;

		float progress = 0;
		while (progress < 1)
		{
			progress = elapsedTime / duration;

			_textRectTransform.position = Vector3.Lerp(startPos, endPos, progress);
			_text.color                 = Color.Lerp(color, new Color(0, 0, 0, 0), progress);

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		// Ensure final values
		_textRectTransform.position = endPos;
		_text.color                 = new Color(0, 0, 0, 0);

		_runningCoroutine = null;
	}

	private IEnumerator TextShake(float duration)
	{
		float elapsedTime = 0;

		Vector3 startPos = _textRectTransform.position;

		float progress = 0;
		while (progress < 1)
		{
			progress = elapsedTime / duration;

			_textRectTransform.position = startPos + new Vector3(shakePattern.Evaluate(progress), 0);

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		_textRectTransform.position = startPos;

		_runningCoroutine = null;
	}

	private IEnumerator TextEnter(float duration)
	{
		float elapsedTime = 0;

		var     position = _textRectTransform.position;
		Vector3 startPos = position + Vector3.right;
		Vector3 endPos   = position;

		Color color = startColor;

		float progress = 0;
		while (progress < 1)
		{
			progress = elapsedTime / duration;

			_textRectTransform.position = Vector3.Lerp(startPos, endPos, progress);
			_text.color                 = Color.Lerp(new Color(0, 0, 0, 0), color, progress);

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		// Ensure final values
		_textRectTransform.position = endPos;
		_text.color                 = startColor;

		_runningCoroutine = null;
		ready             = true;
	}

	public void Activate()
	{
		_textRectTransform.gameObject.SetActive(true);
		HitCollider.enabled = true;
	}
}
using System.Collections;
using UnityEngine;

public class HitMarker : MonoBehaviour
{
	private SpriteRenderer _spriteRenderer;

	private void Awake() => _spriteRenderer = GetComponent<SpriteRenderer>();

	private Coroutine running;

	public void Move(Vector3 position)
	{
		transform.position = position;

		if (running != null) StopCoroutine(running);
		running = StartCoroutine(FadeOut(.5f));
	}

	private IEnumerator FadeOut(float duration)
	{
		Color startColor  = new Color(255, 255, 255, 1);
		Color targetColor = new Color(255, 255, 255, 0);
		_spriteRenderer.color = startColor;

		yield return new WaitForSeconds(1f);
		for (float elapsed = 0; elapsed < duration; elapsed += Time.deltaTime)
		{
			_spriteRenderer.color = Color.Lerp(startColor, targetColor, elapsed / duration);
			yield return null;
		}

		_spriteRenderer.color = targetColor;

		running = null;
	}
}
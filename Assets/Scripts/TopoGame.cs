using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TopoGame : MonoBehaviour
{
	[SerializeField]
	private List<Area> areas = default;

	[SerializeField]
	private string nextScene = default;

	[SerializeField]
	private GameTimer timer = default;
	[SerializeField]
	private TextMeshProUGUI scoreText = default;

	[SerializeField]
	private HitMarker hitMarker = default;

	public List<Area> previousAreas = default;

	public Area currentTarget { get; private set; }

	public UnityEvent DoneEvent = new UnityEvent();

	[SerializeField]
	private int score = 0;
	[SerializeField]
	private float totalTime = default;

	private void Start() => NextArea();

	public void WrongAnswerHit(Vector3 position)
	{
		currentTarget.Shake();
		hitMarker.Move(position);
	}

	public void NextArea()
	{
		totalTime      += timer.timeLimit - timer.RemainingTime;
		score          += (int) timer.RemainingTime;
		scoreText.text =  $"{score.ToString()} Pts.";

		List<Area> availableAreas = areas.Where(x => !x.gameObject.activeSelf).ToList();
		if (availableAreas.Count > 0)
		{
			timer.SetTime(15);
			timer.StartTimer();

			Area area = availableAreas[Random.Range(0, availableAreas.Count - 1)];
			area.gameObject.SetActive(true);
			currentTarget = area;
		}
		else
		{
			timer.PauseTimer(true);
			DoneEvent.Invoke();

			ScoreScreenController.MoveToScores(new List<int> {score});
		}
	}
}
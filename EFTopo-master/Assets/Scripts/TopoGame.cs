using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class TopoGame : MonoBehaviour
{
	[SerializeField]
	private List<Area> areas = default;

	[SerializeField]
	private GameTimer timer = default;
	[SerializeField]
	private TextMeshPro scoreText = default;

	[SerializeField]
	private HitMarker hitMarker = default;

	public List<Area> previousAreas = default;

	public Area currentTarget { get; private set; }

	public UnityEvent DoneEvent = new UnityEvent();

	[SerializeField]
	private int score = 0;

    public bool Playing = false;

	public void StartGame()
	{
		NextArea();
        Playing = true;
	}

	public void WrongAnswerHit(Vector3 position)
	{
        if (Playing == true)
        {
            currentTarget.Shake();
            hitMarker.Move(position);
        }
	}

	public void NextArea()
	{
		score += (int) timer.RemainingTime;
		if (currentTarget) currentTarget.AppendText($"+{(int)timer.RemainingTime}");
		scoreText.text = $"{score.ToString()} Pts.";

		List<Area> availableAreas = areas.Where(x => !x.HitCollider.enabled).ToList();
		if (availableAreas.Count > 0)
		{
			timer.SetTime(15);
			timer.StartTimer();

			Area area = availableAreas[Random.Range(0, availableAreas.Count - 1)];
			area.Activate();
			currentTarget = area;
		}
		else
		{
			timer.PauseTimer(true);
			DoneEvent.Invoke();

			ScoreScreenController.MoveToScores(new List<int> {score}, SceneManager.GetActiveScene().buildIndex, SceneManager.GetActiveScene().buildIndex);
		}
	}
}
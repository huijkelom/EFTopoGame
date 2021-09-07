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
        }
	}

	public void NextArea()
	{
		score += (int) timer.TimeRemaining;
		if (currentTarget) currentTarget.AppendText($"+{(int)timer.TimeRemaining}");
		scoreText.text = $"{score.ToString()} Pts.";

		List<Area> availableAreas = areas.Where(x => !x.HitCollider.enabled).ToList();
		if (availableAreas.Count > 0)
		{
			timer.StartTimer(15);

			Area area = availableAreas[Random.Range(0, availableAreas.Count - 1)];
			area.Activate();
			currentTarget = area;
		}
		else
		{
			timer.PauseTimer(true);
			DoneEvent.Invoke();

            Invoke("MoveToScores", 1f);
		}
	}

    private void MoveToScores()
    {
        ScoreScreenController.MoveToScores(new List<int> { score }, SceneManager.GetActiveScene().buildIndex, SceneManager.GetActiveScene().buildIndex);
    }
}
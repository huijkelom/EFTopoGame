using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
	public TextMesh labelOfTimer;
	public Image gage;
	/// <summary>
	/// Time limit can be overwritten by the setting file if it contains a setting from Time.
	/// </summary>
	public float timeLimit;
	public Color colorWhenOutOfTime;
	public float percentageOutOfTime = 15;
	private float _startTime;
	private Color _colourStart;
	private bool _paused = false;
	public UnityEvent timerRanOut = new UnityEvent();
	private Coroutine _runningRoutine;
	
	public float RemainingTime { get; private set; }

	/// <summary>
	/// Start running the set timer.
	/// </summary>
	public void StartTimer()
	{
		_startTime         = Time.time;
		labelOfTimer.color = _colourStart;

		if (_runningRoutine != null) StopCoroutine(_runningRoutine);
		_runningRoutine = StartCoroutine("RunTimer");
	}

	/// <summary>
	/// Pause or unpause the timer.
	/// </summary>
	public void PauseTimer(bool pause)
	{
		_paused = pause;
	}

	void Awake()
	{
		//Check if a Text class has been linked
		if (labelOfTimer == null)
		{
			labelOfTimer = gameObject.GetComponentInChildren<TextMesh>(); //Try to find a Text class
			if (labelOfTimer == null)
			{
				Debug.LogWarning(
					"L_Text | Start | Text changer has no label to change and can't find one on its gameobject: " +
					gameObject.name);
				return;
			}
			else
			{
				Debug.LogWarning(
					"L_Text | Start | Text changer has no label to change but it has found a Text class on its gameobject: " +
					gameObject.name);
			}
		}

		_colourStart = labelOfTimer.color;
	}

//	private void Start()
//	{
//		//load time setting from settings file, if there is not Time setting in the file the inspector value is used.
//		string[] temp = GlobalGameSettings.GetSetting("Playtime").Split(' ');
//		if (temp.Length > 0)
//		{
//			timeLimit = int.Parse(temp[0]);
//		}
//
//		int minutes = (int) (timeLimit / 60);
//		int seconds = (int) (timeLimit % 60);
//		labelOfTimer.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
//	}

	public void SetTime(float time)
	{
		timeLimit = time;
		int minutes = (int) (timeLimit / 60);
		int seconds = (int) (timeLimit % 60);
		labelOfTimer.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
	}

	IEnumerator RunTimer()
	{
		RemainingTime = timeLimit;
		while (RemainingTime > 0)
		{
			if (!_paused)
			{
				RemainingTime -= Time.deltaTime;
			}

			if (RemainingTime <= 0)
			{
				timerRanOut.Invoke();
				RemainingTime = 0;
			}

			int minutes = (int) (RemainingTime / 60);
			int seconds = (int) (RemainingTime % 60);
			gage.fillAmount = RemainingTime / timeLimit;

			labelOfTimer.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
			if (RemainingTime < (timeLimit / percentageOutOfTime))
			{
				float factor = RemainingTime / percentageOutOfTime;
				labelOfTimer.color = Color.Lerp(colorWhenOutOfTime, _colourStart, factor);
			}

			yield return null;
		}
	}
}
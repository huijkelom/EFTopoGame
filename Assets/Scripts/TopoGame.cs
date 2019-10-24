using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TopoGame : MonoBehaviour
{
	[SerializeField]
	private List<Area> areas;

	[SerializeField]
	private string nextScene;

	public List<Area> previousAreas;

	public Area currentTarget { get; private set; }

	public UnityEvent DoneEvent = new UnityEvent();
	

	private void Start() => NextArea();

	public void WrongAnswerHit() => currentTarget.Shake();
	
	public void NextArea()
	{
		List<Area> availableAreas = areas.Where(x => !x.gameObject.activeSelf).ToList();
		if (availableAreas.Count > 0)
		{
			Area area = availableAreas[Random.Range(0, availableAreas.Count - 1)];
			area.gameObject.SetActive(true);
			currentTarget = area;
		}
		else
		{
			DoneEvent.Invoke();
		}
	}
}
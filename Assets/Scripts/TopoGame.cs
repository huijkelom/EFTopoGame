using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopoGame : MonoBehaviour
{
	[SerializeField]
	private List<Area> areas;

	[SerializeField]
	private string nextScene;

	public List<Area> previousAreas;

	private void Start()
	{
		NextArea();
	}

	public void NextArea()
	{
		List<Area> availableAreas = areas.Where(x => !x.gameObject.activeSelf).ToList();
		if (availableAreas.Count > 0)
		{
			Area area = availableAreas[Random.Range(0, availableAreas.Count - 1)];
			area.gameObject.SetActive(true);
		}
		else
		{
			SceneManager.LoadScene(nextScene);
		}
	}
}
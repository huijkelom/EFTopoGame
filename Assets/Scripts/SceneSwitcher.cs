using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
	[SerializeField]
	private bool autoSwitch;
	[SerializeField]
	private List<MapScene> mapScenes;

	public void SwitchScene(string sceneName) => SceneManager.LoadScene(sceneName);

	public void SwitchScene()
	{
		string map = GlobalGameSettings.GetSetting("Kaart");
		SceneManager.LoadScene(mapScenes.First(x => x.map.Equals(map)).scene);
	}

	private void OnEnable()
	{
		if (autoSwitch) SwitchScene();
	}
}
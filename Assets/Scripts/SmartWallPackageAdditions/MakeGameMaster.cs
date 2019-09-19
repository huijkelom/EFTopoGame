using UnityEngine;

public class MakeGameMaster : MonoBehaviour
{
	[SerializeField]
	private GameObject GameMasterPrefab;

	private void Start()
	{
		if (FindObjectOfType<CentralGateway>()) return;
		Instantiate(GameMasterPrefab);
	}
}
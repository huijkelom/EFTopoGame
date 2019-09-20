using UnityEngine;

public class MissCatcher : MonoBehaviour, I_SmartwallInteractable
{
	[SerializeField]
	private TopoGame topoGame;

	public void Hit(Vector3 hitPosition) => topoGame.WrongAnswerHit();
}
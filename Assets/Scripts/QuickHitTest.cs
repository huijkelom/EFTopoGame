using UnityEngine;

public class QuickHitTest : MonoBehaviour, I_SmartwallInteractable
{
    public void Hit(Vector3 hitPosition)
    {
        Debug.Log("hit");
    }
}
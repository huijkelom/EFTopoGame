using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [SerializeField] private Animator Animator;

    private float rnd;

    // Start is called before the first frame update
    void Start()
    {
        rnd = Random.Range(0, 5f);
        Invoke("StartWave", rnd);
    }

    private void StartWave()
    {
        StartCoroutine(_Wave());
    }

    private IEnumerator _Wave()
    {
        while (true)
        {
            Animator.Play("Wave");
            yield return new WaitForSeconds(rnd * 2);
        }
    }
}

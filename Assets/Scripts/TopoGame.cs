using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopoGame : MonoBehaviour
{
    [SerializeField]
    private List<Province> provinces;

    [SerializeField]
    private string nextScene;

    private void Start()
    {
        NextProvince();
    }

    public void NextProvince()
    {
        List<Province> availableProvinces = provinces.Where(x => !x.gameObject.activeSelf).ToList();
        if (availableProvinces.Count > 0)
        {
            Province province = availableProvinces[Random.Range(0, availableProvinces.Count - 1)];
            province.gameObject.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
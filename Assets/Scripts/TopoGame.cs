using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TopoGame : MonoBehaviour
{
    [SerializeField]
    private List<Province> provinces;

    private void Start()
    {
        NextProvince();
    }

    private void NextProvince()
    {
        List<Province> availableProvinces = provinces.Where(x => !x.gameObject.activeSelf).ToList();
        Province province = availableProvinces[Random.Range(0, availableProvinces.Count-1)];
        
        province.gameObject.SetActive(true);
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;

public class MapSelector : MonoBehaviour
{

    private TMP_Dropdown _dropdown;
    private List<string> _mapNames;
    void Start()
    {
        _dropdown = transform.Find("Dropdown").gameObject.GetComponent<TMP_Dropdown>();
        LoadMapNames();
    }

    void LoadMapNames()
    {
        _mapNames = new List<string>(Directory.GetFiles(@"Maps\", "*.dat"));
        for (int index = 0; index < _mapNames.Count; index++)
        {
            _mapNames[index] = _mapNames[index].Replace(@"Maps\", "");
            _mapNames[index] = _mapNames[index].Replace(".dat", "");
        }
        _dropdown.ClearOptions();
        _dropdown.AddOptions(_mapNames);
    }
}

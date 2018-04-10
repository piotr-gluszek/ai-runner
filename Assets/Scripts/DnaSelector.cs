using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;

public class DnaSelector : MonoBehaviour
{

    private TMP_Dropdown _dropdown;
    private List<string> _dnaNames;
    void Start()
    {
        _dropdown = transform.Find("Dropdown").gameObject.GetComponent<TMP_Dropdown>();
        LoadDnaNames();
    }

    void LoadDnaNames()
    {
        _dnaNames = new List<string>(Directory.GetFiles(@"DNA\", "*.txt"));
        for (int index = 0; index < _dnaNames.Count; index++)
        {
            _dnaNames[index] = _dnaNames[index].Replace(@"DNA\", "");
            _dnaNames[index] = _dnaNames[index].Replace(".txt", "");
        }
        _dropdown.ClearOptions();
        _dropdown.AddOptions(_dnaNames);
    }
}

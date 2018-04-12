using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsVisualization : MonoBehaviour
{

    private TMP_Text _settings;
    // Use this for initialization
    void Start()
    {
        _settings = GetComponent<TMP_Text>();
        _settings.SetText("Gen size: " + Settings.AttemptNum
                          + "\nMutate rate: " + Settings.MutationRate
                          + "\nCrossover rate: " + Settings.CrossoverRate
                          + "\nMap: " + Settings.SelectedMapPath
                          + "\nDNA: " + Settings.DnaFilePath);
    }


}

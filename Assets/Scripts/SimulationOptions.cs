using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;


public class SimulationOptions : MonoBehaviour
{
    private GameObject _selectDnaButton;
    private GameObject _mapSelector;
    private GameObject _dnaSelector;

    // Use this for initialization
    void Start()
    {
        _selectDnaButton = GameObject.Find("Options/Canvas/DNA/SelectDNAButton");
        _mapSelector = GameObject.Find("Options/Canvas/MapSelector");
        _dnaSelector = GameObject.Find("Options/Canvas/DnaSelector");
    }

    public void ShowMapSelector()
    {
        _mapSelector.SetActive(true);
    }

    public void ShowDnaSelector()
    {
        _dnaSelector.SetActive(true);
    }
    public void SubmitDna()
    {
        string dnaName = _dnaSelector.transform.Find("Dropdown").gameObject.GetComponent<TMP_Dropdown>().captionText.text;
        Settings.DnaFilePath = @"DNA\" + dnaName + ".dna";
        _dnaSelector.SetActive(false);
    }

    public void SubmitMap()
    {
        string mapName = _mapSelector.transform.Find("Dropdown").gameObject.GetComponent<TMP_Dropdown>().captionText.text;
        Settings.SelectedMapPath=@"Maps\"+mapName+".dat";
        _mapSelector.SetActive(false);
    }
    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void SetDnaRandomization(bool state)
    {
        Settings.DnaRandomization = state;
    }
    public void SetAttemptsNumber(int number)
    {
        Settings.AttemptNum = number;
        Debug.Log(Settings.AttemptNum);
    }

    public void SetMutationRate(float rate)
    {
        Settings.MutationRate = rate;
        Debug.Log(Settings.MutationRate);
    }

    public void SetMapPath(string path)
    {
        Settings.SelectedMapPath = path;
        Debug.Log(Settings.SelectedMapPath);
    }

    public void SetDnaFilePath(string path)
    {
        Settings.DnaFilePath = path;
        Debug.Log(Settings.DnaFilePath);
    }
    public void ToggleSelectDnaButtonVisibility()
    {
        if (_selectDnaButton.active)
            _selectDnaButton.SetActive(false);
        else _selectDnaButton.SetActive(true);
    }
}

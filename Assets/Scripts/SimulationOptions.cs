using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;


public class SimulationOptions : MonoBehaviour
{
    private GameObject _selectDnaButton;

    // Use this for initialization
    void Start()
    {
        _selectDnaButton = GameObject.Find("Canvas/DNA/SelectDNAButton");
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

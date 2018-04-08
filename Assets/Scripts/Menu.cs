using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    public void StartSimulation()
    {
        Debug.Log("Simulation started.");
    }

    public void ShowOptions()
    {
        Debug.Log("Options shown.");
    }

    public void RunMapCreator()
    {
        Debug.Log("Map creator opened.");
    }
    public void Quit()
    {
        Debug.Log("Game closed.");
        Application.Quit();
    }
}

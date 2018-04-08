using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;

public class AttemptsNumberInput : MonoBehaviour
{

    public SimulationOptions options;
    

    public void SendNumber()
    {
        try
        {
            int number = Convert.ToInt32(GetComponent<TMP_InputField>().text);
            options.SetAttemptsNumber(number);
        }
        catch (FormatException fe)
        {
            Debug.Log(fe.Message);
        }
    }
    
    
}

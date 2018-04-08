using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MutationRatioSlider : MonoBehaviour
{

    private Slider _slider;
    public SimulationOptions options;
	// Use this for initialization
	void Start ()
	{
	    _slider = GetComponent<Slider>();
	}

    public void SendMutationRate()
    {
        options.SetMutationRate(_slider.value);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controler : MonoBehaviour {

    public Player child;
    public Transform startPoint;

    bool alive = false;

	// Use this for initialization
	void Start () {

        
        alive = true;
	}
	
	// Update is called once per frame
	void Update () {

        GameObject p = GameObject.FindGameObjectWithTag("Dead");

        if (p != null && alive) {
            
            Instantiate(child);
            Destroy(p);
            alive = false;
        }
        

	}
}

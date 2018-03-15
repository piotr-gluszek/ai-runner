using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// © 2017 TheFlyingKeyboard and released under MIT License
// theflyingkeyboard.net
public class SpawnSquare : MonoBehaviour
{
    public GameObject objectToSpawn;
    
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
          
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawnPosition.z = 0.0f;
            objectToSpawn = GameObject.CreatePrimitive(PrimitiveType.Cube);
            objectToSpawn.transform.position=spawnPosition;
            
        }
    }
}

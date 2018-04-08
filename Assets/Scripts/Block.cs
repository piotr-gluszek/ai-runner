using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class Block : MonoBehaviour
{
    GameObject options;
    bool optionsActive;
    
    private void Start()
    {
        options = GameObject.Find("Options/Canvas/Buttons");
       
    }

    private void OnMouseOver()
    {
        optionsActive = false;
        // Ignore input if Options are displayed.
        if (!optionsActive)
        {


            if (Input.GetMouseButton(0))
                Destroy(this.gameObject);
            if (Input.GetMouseButton(1))
            {
                this.gameObject.tag = "Finish";
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            }
        }
    }


   

}

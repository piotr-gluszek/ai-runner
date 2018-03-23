using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour {

  


    // Game object which will be set active.
    public Player player;
    GameObject buttons;
    bool isActive;

	void Start () {
        buttons = transform.Find("Buttons").gameObject;
        isActive = false;
       
	}
	
	
	void Update () {

        // Show Options when ESC pressed.
        // When ESC pressed while Options are visible set to invisible (inactive).
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isActive)
            {
                buttons.SetActive(true);
                player.Freeze();
   
            }
            else
            {
                buttons.SetActive(false);
                player.Move();
            }
                

            isActive = !isActive;
                
            
        }
		
	}
}

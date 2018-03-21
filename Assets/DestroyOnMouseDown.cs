﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnMouseDown : MonoBehaviour
{

    private void OnMouseOver()
    {
        if(Input.GetMouseButton(0))
        Destroy(this.gameObject);
        if (Input.GetMouseButton(1))
            this.gameObject.tag = "Finish";
    }
   

}

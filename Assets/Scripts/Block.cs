using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class Block : MonoBehaviour,IInitializable
{
    ICommunicutable _parentScript;
    GameObject _parent;
    bool _initialized = false;

 
    public void Initialize(GameObject parent)
    {
        _parent = parent;
        _parentScript = _parent.GetComponent<ICommunicutable>();
        _initialized = true;
    }
    private void OnMouseOver()
    {
        if(_initialized)
        _parentScript.ProceedData(transform.gameObject);
    }
}

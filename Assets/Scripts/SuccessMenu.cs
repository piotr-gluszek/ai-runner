using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SuccessMenu : MonoBehaviour {
public void LoadNextLevel()
    {
        SceneManager.LoadScene("Creator");
    }

}

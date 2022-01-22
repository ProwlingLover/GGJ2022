using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TurnToNextScene : MonoBehaviour {
    public string nextSceneName = "";
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.name == "host")
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
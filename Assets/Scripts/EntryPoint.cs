using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class EntryPoint : MonoBehaviour {
    
    private void Start()
    {
        
    }

    public void OnClickStartGame()
    {
        SceneManager.LoadScene("startChatScene");
    }

    public void ShowDevTeam()
    {
        // var img = GameObject.Find("")
    }
}
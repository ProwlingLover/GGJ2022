using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class EntryPoint : MonoBehaviour {
    
    public Image teamShow;
    public Image opShow;
    private void Start()
    {
        
    }

    public void OnClickStartGame()
    {
        SceneManager.LoadScene("startChatScene");
    }

    public void ShowDevTeam()
    {
        if (teamShow != null)
        {
            teamShow.gameObject.SetActive(true);
        }
    }

    public void ShowOp()
    {
        if (opShow != null)
        {
            opShow.gameObject.SetActive(true);
        }
    }

    public void CloseOp()
    {
        if (opShow != null)
        {
            opShow.gameObject.SetActive(false);
        }
    }

    public void HostQuit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void CloseDevTeam()
    {
        if (teamShow != null)
        {
            teamShow.gameObject.SetActive(false);
        }
    }
}
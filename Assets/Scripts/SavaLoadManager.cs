using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public Vector3 playerPos;
    public Dictionary<string, bool> lightState;
}

public class SavaLoadManager : MonoBehaviour
{

    private GameData data;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        data = new GameData();
        data.lightState = new Dictionary<string, bool>();
    }

    public void Save()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        data.playerPos = player.transform.position;
        //Debug.Log(data.playerPos);
        var lights = GameObject.FindGameObjectsWithTag("memory");
        for (int i=0,j=lights.Length;i<j;i++)
        {
            var v = lights[i].GetComponentInChildren<Light>();
            if (v != null)
            {
                data.lightState.Add(v.transform.position.ToString(), v.enabled);
                //Debug.Log(v.transform.position.ToString()+v.enabled);
            }
        }
    }

    public void Clear()
    {
        data.playerPos = Vector3.zero;
        data.lightState.Clear();
    }

    public GameData Load()
    {
        return data;
    }
}

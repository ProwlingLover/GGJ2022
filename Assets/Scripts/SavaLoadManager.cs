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

    private static GameData data;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (data == null)
        {
            Debug.Log("1");
            data = new GameData();
            data.lightState = new Dictionary<string, bool>();
        }
        else
        {
            Debug.Log("2");
            Load();
        }
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
                var hash = v.transform.position.ToString();
                if (!data.lightState.ContainsKey(hash)) {
                    data.lightState.Add(hash, v.enabled);
                }
                else
                {
                    data.lightState[hash] = v.enabled;
                }
                //Debug.Log(v.transform.position.ToString()+v.enabled);
            }
        }
    }

    public void Clear()
    {
        data.playerPos = Vector3.zero;
        data.lightState.Clear();
    }

    public void Load()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = data.playerPos;
        //Debug.Log(data.playerPos);
        var lights = GameObject.FindGameObjectsWithTag("memory");
        for (int i = 0, j = lights.Length; i < j; i++)
        {
            var v = lights[i].GetComponentInChildren<Light>();
            if (v != null)
            {
                var hash = v.transform.position.ToString();
                if (!data.lightState.ContainsKey(hash))
                {
                    v.enabled = false;
                }
                else
                {
                    v.enabled = data.lightState[hash];
                }
            }
        }
        Debug.Log("Load");
    }
}

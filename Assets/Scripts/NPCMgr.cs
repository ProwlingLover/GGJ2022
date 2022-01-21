using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;


public class NPCMgr : MonoBehaviour {
    private List<ECEnemy> enemyList = new List<ECEnemy>();
    private List<ECNPC> npcList;
    private GameObject npcRoot;
    public List<Vector3> presetNPCList;


    private void Start() 
    {
        InitNPCs();
    }    

    private void InitNPCs()
    {
        npcRoot = GameObject.Find("npcs");
        // if (npcRoot != null) 
        // {
        //     var npcTemplate = GameObject.Find("guardNPC");
        //     int presetNPCListCount = presetNPCList.Count;
        //     if (presetNPCListCount > 0)
        //     {
        //         for (int i = 0; i < presetNPCListCount; i++)
        //         {
        //             GameObject npc = GameObject.Instantiate(npcTemplate, presetNPCList[i], Quaternion.identity) as GameObject;
        //             npc.transform.parent = npcRoot.transform;
        //             npc.SetActive(true);
        //             var ecEnemy = npc.AddComponent<ECEnemy>();
        //             enemyList.Add(ecEnemy);
        //         }
        //     }
        //     foreach (var it in enemyList)
        //     {
        //         it.RemoveTarget();
        //     }
        // }
    }
    
    private void FixedUpdate() 
    {
        if (enemyList != null) 
        {
            foreach(var it in enemyList)
            {
                it.UpdateState();
            }
        }
    }
}
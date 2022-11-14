using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCData : MonoBehaviour
{
    public int id;
    public bool isNPC;
    public TalkManager talkManager;
    public string NPCName;

    public void OnTriggerStay(Collider other)
    {       
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(Input.GetKeyDown(KeyCode.F))
            talkManager.Action(id, isNPC, NPCName);
            
        }
        
    }
}

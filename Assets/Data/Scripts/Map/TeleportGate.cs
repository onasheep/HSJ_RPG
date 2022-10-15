using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class TeleportGate : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform DestGate;
    public Inventory inven;
    public GameObject myPlayer;
    public EventCam eventCam;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


    }
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {              
                inven.MessageLog.enabled = false;
                myPlayer.GetComponent<NavMeshAgent>().enabled = false;
                myPlayer.transform.position = DestGate.position;
                eventCam.BossCamMove();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            inven.MessageLog.enabled = true;
            inven.MessageLog.text = "FŰ�� ���� �ڷ����͸� �⵿��Ű����";
        }
        

    }
    private void OnTriggerExit(Collider other)
    {
        inven.MessageLog.enabled = false;
        myPlayer.GetComponent<NavMeshAgent>().enabled = true;

    }
}

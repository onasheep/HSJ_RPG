using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemData : MonoBehaviour
{
    public ItemData dropItemData;
    public Inventory inven;

    private void Awake()
    {
        inven = GameObject.Find("UIWindow").GetComponent<Inventory>();
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (!inven.CheckInvenFull(inven.itemslots))
                {
                    inven.itemslots[inven.FindEmptySlot(inven.itemslots)].slotItemData = dropItemData;
                }
                if(inven.MessageLog.enabled)
                {
                    inven.MessageLog.enabled = false;

                }
                Destroy(this.gameObject);
            }
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            inven.MessageLog.enabled = true;
            inven.MessageLog.text = "G키를 눌러 아이템을 획득하세요";
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            inven.MessageLog.enabled = false;
        }
    }
}

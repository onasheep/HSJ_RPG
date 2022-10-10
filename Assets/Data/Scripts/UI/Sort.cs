using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Sort : MonoBehaviour, IPointerClickHandler
{
    public GameObject ItemArea;
    [SerializeField]
    public ItemSlot[] itemslots;
    int index;
    ItemSlot[] itemsSaved;
    void Awake()
    {
        itemsSaved = new ItemSlot[25];
    }

    void Update()
    {
        itemslots = ItemArea.GetComponentsInChildren<ItemSlot>();

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        SaveItem();
        itemslots = new ItemSlot[25];
        for (int i = 0; i < itemslots.Length; i++)
        {
            if (itemsSaved[i].slotItemData != null)
            {
                itemslots[i].slotItemData = itemsSaved[i].slotItemData;
            }
            else if (itemsSaved[i].slotItemData == null)
            {
                itemslots[i].slotItemData = null;
            }


        }
        //    for (int j = 0; j < itemsSaved.ToArray().Length; i++)
        //    {
        //        if (itemsSaved[j].slotItemData != null)
        //        {
        //            itemslots[i].slotItemData = itemsSaved[i].slotItemData;
        //        }
        //        if (itemsSaved[j].slotItemData == null)
        //        {
        //            return;
        //        }
        //    }
        //}
        //for (int i = itemslots.Length - 1; i > 0 ; i--)
        //{
        //    for (int j = 0; j < i; j++)
        //    {
        //        if (itemslots[j].slotItemData != null)
        //        {
        //            if (itemslots[j].slotItemData.index < itemslots[j+1].slotItemData.index)
        //            {
        //                temp = itemslots[j].slotItemData;
        //                itemslots[j].slotItemData = itemslots[j+1].slotItemData;
        //                itemslots[j].slotItemData = temp;
        //            }
        //        }
        //        else if (itemslots[j].slotItemData == null)
        //        {
        //            //temp = itemslots[j].slotItemData;
        //            itemslots[j].slotItemData = itemslots[j + 1].slotItemData;

        //        }              
        //    }
        //}
    }

    void SaveItem()
    {
        for (int i = 0; i < itemslots.Length; i++)
        {
            
            if (itemslots[i] != null)
            {
                itemsSaved[i] = itemslots[i];
            }
            if (itemslots[i] == null)
            {
                if (itemslots[i + 1] != null)
                {
                    itemsSaved[i] = itemslots[i + 1];
                    i++;
                }
                else return;
                       
            }
        }
    }

}

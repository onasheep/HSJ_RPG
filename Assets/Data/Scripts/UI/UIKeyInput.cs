using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIKeyInput : MonoBehaviour
{
    public GameObject Filter;
    public GameObject Equip;
    public GameObject Inven;
    private bool OnEquip = false;
    private bool OnInven = false;

    


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {            
            CloseEquipment();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {            
            CloseInventory();
        }

    }

    void CloseEquipment()
    {
        if (OnEquip == true)
        {
            Equip.SetActive(false);
            OnEquip = false;
        }
        else if (OnEquip == false)
        {
            Equip.SetActive(true);
            OnEquip = true;
        }
    }

    void CloseInventory()
    {
        if (OnInven == true)
        {
            OnInven = false;
            Inven.SetActive(false);
        }
        else if (OnInven == false)
        {
            Inven.SetActive(true);
            OnInven = true;
        }
    }

}

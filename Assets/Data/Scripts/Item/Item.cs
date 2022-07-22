using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using Newtonsoft.Json;
using System.IO;


[Serializable]
public class ItemData
{
    public int index;
    public string name;
    public int atk;
    public int def;
    public int recoverHp;
    public int recoverMp;
    public int price;
    public int type;
    public string imagename;
}
[SerializeField]
public class Item : MonoBehaviour
{
    public List<ItemData> Potionlist;
    public List<ItemData> Weaponlist;
    public List<ItemData> Armorlist;
    public List<ItemData> Shieldlist;
    public List<ItemData> Headlist;
    public List<ItemData> Backlist;

    ItemData data;


    public void Awake()
    {
        FromJson();
    }
    public void FromJson()
    {
        string[] list = File.ReadAllLines(Application.dataPath + "/Data/Json/ItemData.json");
        foreach (string s in list)
        {

            data = JsonUtility.FromJson<ItemData>(s);
            int type = data.type;
            switch (type)
            {
                case 1:
                    Headlist.Add(data);
                    break;
                case 2:
                    Backlist.Add(data);
                    break;
                case 3:
                    Armorlist.Add(data);
                    break;
                case 4:
                    Weaponlist.Add(data);
                    break;
                case 5:
                    Shieldlist.Add(data);
                    break;
                case 6:
                    Potionlist.Add(data);
                    break;

            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class Inventory : MonoBehaviour
{
    public EquipmentMeshData EquipMData;
    public TMPro.TMP_Text MessageLog;

    public GameObject ItemArea;
    public GameObject EquipArea;

    [SerializeField]
    public ItemSlot[] itemslots;
    public ItemSlot[] equipslots;
    public ItemSlot[] quickslots;
    // Item 안에 있는 리스트를 가져오기 위해
    [SerializeField]
    Item item;
    public Player myPlayer;

    
    bool isEquip = true;
    bool unEquip = false;

    public int potionCount;
    public TMPro.TMP_Text potionNumtxt;

    public GameObject QuickSlot;

    public Button deletItem;

    TMPro.TextMeshProUGUI amountText;

    public Button sortButton;

    ItemData temp = null;

    /// <summary>
    /// 아이템, 장비창을 배열에 넣고 TYPE을 각각 나눠준다 
    /// 슬롯  0 Head, 1 Back, 2 Armor, 3 Weapon, 4 Shield, 5 Portion
    /// 아이템 타입 0 null, 1 Head, 2 Back, 3 Armor, 4 Weapon, 5 Shield, 6 Portion
    /// </summary>
    void Awake()
    {
        itemslots = ItemArea.GetComponentsInChildren<ItemSlot>();
        for(int i = 0; i < itemslots.Length; i++)
        {
            itemslots[i].sloType = ItemSlot.Type.INVENTORY;
        }
        equipslots = EquipArea.GetComponentsInChildren<ItemSlot>();
        for(int i = 0; i < equipslots.Length; i++)
        {
            equipslots[i].sloType = ItemSlot.Type.EQUIPMENT;

        }
        quickslots = QuickSlot.GetComponentsInChildren<ItemSlot>();


    }
    private void Start()
    {
        EquipBegginerSet();
        AddToStat();



        for (int i = 0; i < 24; i++)
        {

            amountText = itemslots[i].GetComponent<TMPro.TextMeshProUGUI>();

            int itemPercentage = Random.Range(0, 101);
            if (itemPercentage < 10)
            {
                ItemData Back = item.Backlist[Random.Range(0, 3)];
                itemslots[i].slotItemData = Back;

            }
            else if (itemPercentage >= 10 & itemPercentage < 30)
            {
                ItemData Weapon = item.Weaponlist[Random.Range(0, 3)];
                itemslots[i].slotItemData = Weapon;

            }
            else if (itemPercentage >= 30 & itemPercentage < 50)
            {
                ItemData Armor = item.Armorlist[Random.Range(0, 3)];
                itemslots[i].slotItemData = Armor;
            }
            else if (itemPercentage >= 50 & itemPercentage < 70)
            {
                ItemData Shield = item.Shieldlist[Random.Range(0, 3)];
                itemslots[i].slotItemData = Shield;
            }
            else if (itemPercentage >= 70 & itemPercentage < 90)
            {
                ItemData Potion = item.Potionlist[Random.Range(0, 4)];
                    itemslots[i].slotItemData = Potion;
            }
            else
            {
                ItemData Head = item.Headlist[Random.Range(0, 2)];
                itemslots[i].slotItemData = Head;
            }
        }
    }
    void Update()
    {
        AddItem();
        CheckEquipItem();
    }


    /// <summary>
    /// false 값이면 구조체가 null이므로 비어 있고 true면 슬롯이 차있다
    /// </summary>
    public bool CheckInvenFull(ItemSlot[] _itemdatalist)
    {
        bool isFull = true;
        for (int i = 0; i < _itemdatalist.Length; i++)
        {
            // 구조체 null 을 판별할수 없으므로 0 값을 임의로 넣어둠 
            // 나중에 물어볼것.
            if (_itemdatalist[i].slotItemData == null || _itemdatalist[i].slotItemData.type == 0)
            {
                return false;
            }
        }
        return isFull;
    }

    /// <summary>
    /// 인벤토리에서 빈공간 찾아 인덱스 반환
    /// </summary>
    /// <param name="_itemdatalist"></param>
    /// <returns></returns>
    public int FindEmptySlot(ItemSlot[] _itemdatalist)
    {
        int emptySlotIndex = 0;
        for (int i = 0; i < _itemdatalist.Length; i++)
        {

            if (_itemdatalist[i].slotItemData == null || _itemdatalist[i].slotItemData.type == 0)
            {
                emptySlotIndex = i;
                break;
            }
        }
        return emptySlotIndex;

    }
    
    public void SumPotion()
    {
        int count = 1;
        for (int i = 0; i < itemslots.Length; i++)
        {
            if (itemslots[i].slotItemData.type == 6)
            {
                if(itemslots[i].slotItemData.index == itemslots[i].slotItemData.index)
                {
                    count++;
                    amountText.enabled = true;
                    amountText.text = count.ToString();
                }
            }
        }
    }

    public void SortInventory()
    {
        for(int i = 0; i < itemslots.Length; i++)
        {
            if (itemslots[i].slotItemData != null)
            {
                return;
            }
            else if (itemslots[i].slotItemData == null)
            {
               
                temp = itemslots[i].slotItemData;
                itemslots[i].slotItemData = itemslots[i + 1].slotItemData;
                itemslots[i + 1].slotItemData = temp;
            }
        }
    }



    void EquipBegginerSet()
    {
        equipslots[1].slotItemData = item.Backlist[0];
        equipslots[2].slotItemData = item.Armorlist[0];
        equipslots[3].slotItemData = item.Weaponlist[0];
        equipslots[4].slotItemData = item.Shieldlist[0];
    }
    public void AddItem()

    {


    }

    // 포션 들어오면 칸을 차지하지 않고 포션 인덱스를 늘리기 
    void CheckPotion()
    {
        //if (itemslots[i].slotItemData == Potion)
        //{
        //    potionCount++;
        //    AmountText.enabled = true;
        //    potionNumtxt.text = potionCount.ToString();
        //}
    }


    /// <summary>
    /// 아이템 창 데이터 체크해서 장착하고 있는 장비로 변경
    /// </summary>
    public void CheckEquipItem()
    {
        // 헬맷 
        if (equipslots[0].slotItemData == item.Headlist[0])
        {
            EquipMData.Helmet.SetActive(isEquip);
            EquipMData.Hair.SetActive(unEquip);
            EquipMData.Helm.SetActive(unEquip);
        }
        else if (equipslots[0].slotItemData == item.Headlist[1])
        {
            EquipMData.Helmet.SetActive(unEquip);
            EquipMData.Helm.SetActive(isEquip);
            EquipMData.Hair.SetActive(unEquip);
        }
        else
        {
            EquipMData.Helmet.SetActive(unEquip);
            EquipMData.Helm.SetActive(unEquip);
            EquipMData.Hair.SetActive(isEquip);
        }

        // 등
        if (equipslots[1].slotItemData == item.Backlist[0])
        {
            EquipMData.WoodLog.SetActive(isEquip);
            EquipMData.Bag.SetActive(unEquip);
            EquipMData.Wings.SetActive(unEquip);
            EquipMData.Cape.SetActive(unEquip);
        }
        else if (equipslots[1].slotItemData == item.Backlist[1])
        {
            EquipMData.WoodLog.SetActive(unEquip);
            EquipMData.Bag.SetActive(isEquip);
            EquipMData.Wings.SetActive(unEquip);
            EquipMData.Cape.SetActive(unEquip);
        }
        else if(equipslots[1].slotItemData == item.Backlist[2])
        {
            EquipMData.WoodLog.SetActive(unEquip);
            EquipMData.Bag.SetActive(unEquip);
            EquipMData.Wings.SetActive(isEquip);
            EquipMData.Cape.SetActive(isEquip);
        }
        else
        {
            EquipMData.WoodLog.SetActive(unEquip);
            EquipMData.Bag.SetActive(unEquip);
            EquipMData.Wings.SetActive(unEquip);
            EquipMData.Cape.SetActive(unEquip);
        }

        // 갑옷
        if (equipslots[2].slotItemData == item.Armorlist[0])
        {
            
            myPlayer.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = EquipMData.WoodenArm;
            myPlayer.GetComponentInChildren<SkinnedMeshRenderer>().BakeMesh(EquipMData.WoodenArm);
            //EquipMData.WoodenArmor.SetActive(isEquip);
            //EquipMData.LeatherArmor.SetActive(unEquip);
            //EquipMData.IronArmor.SetActive(unEquip);
        }
        else if (equipslots[2].slotItemData == item.Armorlist[1])
        {
 
            myPlayer.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = EquipMData.LeatherArm;
            myPlayer.GetComponentInChildren<SkinnedMeshRenderer>().BakeMesh(EquipMData.LeatherArm);


            //EquipMData.WoodenArmor.SetActive(unEquip);
            //EquipMData.LeatherArmor.SetActive(isEquip);
            //EquipMData.IronArmor.SetActive(unEquip);
        }
        else if (equipslots[2].slotItemData == item.Armorlist[2])
        {

            myPlayer.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = EquipMData.IronArm;
            myPlayer.GetComponentInChildren<SkinnedMeshRenderer>().BakeMesh(EquipMData.IronArm);

            //    EquipMData.WoodenArmor.SetActive(unEquip);
            //    EquipMData.LeatherArmor.SetActive(unEquip);
            //    EquipMData.IronArmor.SetActive(isEquip);
        }
        // 무기
        if (equipslots[3].slotItemData == item.Weaponlist[0])
        {
            EquipMData.IronSword.SetActive(isEquip);
            EquipMData.SilverSword.SetActive(unEquip);
            EquipMData.GoldenSword.SetActive(unEquip);

        }
        else if (equipslots[3].slotItemData == item.Weaponlist[1])
        {
            EquipMData.IronSword.SetActive(unEquip);
            EquipMData.SilverSword.SetActive(isEquip);
            EquipMData.GoldenSword.SetActive(unEquip);

        }
        else if(equipslots[3].slotItemData == item.Weaponlist[2])
        {
            EquipMData.IronSword.SetActive(unEquip);
            EquipMData.SilverSword.SetActive(unEquip);
            EquipMData.GoldenSword.SetActive(isEquip);
        }
        else
        {
            EquipMData.IronSword.SetActive(unEquip);
            EquipMData.SilverSword.SetActive(unEquip);
            EquipMData.GoldenSword.SetActive(unEquip);
        }

        // 방패
        if (equipslots[4].slotItemData == item.Shieldlist[0])
        {
            EquipMData.WoodenPlank.SetActive(isEquip);
            EquipMData.WoodenShield.SetActive(unEquip);
            EquipMData.IronShield.SetActive(unEquip);
        }
        else if (equipslots[4].slotItemData == item.Shieldlist[1])
        {
            EquipMData.WoodenPlank.SetActive(unEquip);
            EquipMData.WoodenShield.SetActive(isEquip);
            EquipMData.IronShield.SetActive(unEquip);
        }
        else if (equipslots[4].slotItemData == item.Shieldlist[2])
        {
            EquipMData.WoodenPlank.SetActive(unEquip);
            EquipMData.WoodenShield.SetActive(unEquip);
            EquipMData.IronShield.SetActive(isEquip);
        }
        else
        {
            EquipMData.WoodenPlank.SetActive(unEquip);
            EquipMData.WoodenShield.SetActive(unEquip);
            EquipMData.IronShield.SetActive(unEquip);
        }
    }

 
    public void AddToStat(ItemData data1 = null, ItemData data2 = null)
    {
        if (data1 == null || data2 == null) return;
        myPlayer.mystat.ATK += data1.atk;
        myPlayer.mystat.DEF += data1.def;

        myPlayer.mystat.ATK -= data2.atk;
        myPlayer.mystat.DEF -= data2.def;
    }

    // 초기 장비 스탯 세팅
    public void AddToStat()
    {
        for (int i = 0; i < equipslots.Length; i++)
        {
            myPlayer.mystat.ATK += equipslots[i].slotItemData.atk;
            myPlayer.mystat.DEF += equipslots[i].slotItemData.def;
        }
    }

    public void PotionToStat(ItemData data1)
    {        // 포션 먹을경우
        myPlayer.mystat.HP += data1.recoverHp;
        myPlayer.mystat.MP += data1.recoverMp;
    }


}

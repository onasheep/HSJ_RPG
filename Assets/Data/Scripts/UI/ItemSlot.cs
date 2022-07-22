using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public enum Type
    {
        INVENTORY, EQUIPMENT
    }
    private Image []image = null;
    private Inventory myInven;
    [SerializeField]
    public ItemData slotItemData;
    [SerializeField]
    public Type sloType;
    int emptySlotIndex;


    #region 슬롯 드래그 파라메터
    Vector2 DragOffset = Vector2.zero;
    #endregion


    void Awake()
    {
        // 자식에 있는 highlight를 GetComponentsInChildren를 통해 배열로 가져옴
        image = gameObject.GetComponentsInChildren<Image>();
        
        myInven = this.GetComponentInParent<Inventory>();

    }
    void Update()
    {
        SetSlotImage();
    }


    //  포션 먹는 경우 아직 구현 X 스탯 완성 후 구현 예정

    // 장비창이 가득 찬 상태에서 헬맷 이외의 아이템을 해제하고 다시 장비하면 머리 창에 장착됨
    /// <summary>
    /// 포션을 사용할떄와 장비를 장착하고 해제할떄랑 조건이 다름/ 포션은 사용하면 사라지므로 Itemdata를 null로 만들어서 비어있는지 없는지 구분하고,
    /// 장비는 비어있는 슬롯과 교환되므로 null로 체크가 되지 않으므로 임의의 null 값인 type 0 값을 이용하여 슬롯이 비워져 있는지 체크함. 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        #region 우클릭을 받았을떄 장비, 아이템, 포션 
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // type 0 은 아이템이 없으므로 type은 1부터 시작함
            // 인벤토리 창을 클릭한 경우
            if (sloType == Type.INVENTORY && this.slotItemData != null)
            {
                if (this.slotItemData.type == 1)
                {
                    InvenToEquip(0);
                }
                else if (this.slotItemData.type == 2)
                {
                    InvenToEquip(1);
                }
                else if (this.slotItemData.type == 3)
                {
                    InvenToEquip(2);
                }
                else if (this.slotItemData.type == 4)
                {
                    InvenToEquip(3);
                }
                else if (this.slotItemData.type == 5)
                {
                    InvenToEquip(4);
                }
                else if (this.slotItemData.type == 6)
                {

                    if (this.slotItemData.recoverHp != 0)
                    {
                        if(myInven.myPlayer.mystat.HP >= myInven.myPlayer.mystat.MaxHP && !myInven.MessageLog.enabled)
                        {
                            StartCoroutine(FadeMessage("HP가 가득 찼습니다."));
                        }
                        else if(myInven.myPlayer.mystat.HP < myInven.myPlayer.mystat.MaxHP)
                        {
                            myInven.PotionToStat(this.slotItemData);
                            this.slotItemData = null;
                        }

                    }
                    else if (this.slotItemData.recoverMp != 0)
                    {
                        if (myInven.myPlayer.mystat.MP >= myInven.myPlayer.mystat.MaxMP && !myInven.MessageLog.enabled)
                        {
                            StartCoroutine(FadeMessage("MP가 가득 찼습니다."));
                        }
                        else if (myInven.myPlayer.mystat.MP < myInven.myPlayer.mystat.MaxMP)
                        {
                            myInven.PotionToStat(this.slotItemData);
                            this.slotItemData = null;
                        }

                    }

                    
                }
            }
            // 장비창을 클릭한 경우
            else if (sloType == Type.EQUIPMENT && this.slotItemData != null)
            {
                EquipToInven();
            }
        }
        #endregion

        #region 좌클릭을 받았을때 
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            if(slotItemData != null)
            {
                myInven.QuickSlot.SetActive(true);
                myInven.QuickSlot.transform.position = this.transform.position;
                
                
                


            }
           
        }
        #endregion 
    }

    // 마우스 올리면 하이라이트 
    public void OnPointerEnter(PointerEventData eventData)
    {
        image[1].color = new Color(1.0f, 200.0f / 255.0f, 100.0f / 255.0f, 1.0f);
    }

    // 마우스 빠져나오면 원래 색으로
    public void OnPointerExit(PointerEventData eventData)
    {
        image[1].color = new Color(1.0f, 1.0f, 1.0f, 150.0f / 255.0f);
        image[2].color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        if(myInven.QuickSlot.activeInHierarchy != true)
        {
            myInven.QuickSlot.SetActive(false);
        }
    }



    void InvenToEquip(int n)
    {
        SwapData(myInven.equipslots[n].slotItemData, this.slotItemData, 0, n);
    }
    void EquipToInven()
    {
        // 인벤토리에 빈 공간이 없는 경우
        if (myInven.CheckInvenFull(myInven.itemslots) && !myInven.MessageLog.enabled)
        {
            if (this.slotItemData.type != 0 && this.slotItemData.name != "WoodenArmor")
            {
                StartCoroutine(FadeMessage("인벤토리가 가득 찼습니다."));

                //ShowMessageLog("인벤토리가 가득 찼습니다.");
                return;
            }
            else
            return;
        }
        // 인벤토리에 빈 공간이 있는 경우 
        else if (!(myInven.CheckInvenFull(myInven.itemslots)))
        {
           
            if (this.slotItemData.name == "WoodenArmor" )
            {
                if(!myInven.MessageLog.enabled)
                {
                    StartCoroutine(FadeMessage("이 아이템은 해제 할 수 없습니다."));

                }
                

            }
            else
            {
                emptySlotIndex = myInven.FindEmptySlot(myInven.itemslots);
                SwapData(myInven.itemslots[emptySlotIndex].slotItemData, this.slotItemData, emptySlotIndex, 0);
            }
        }
    }

    void SwapData(ItemData data1, ItemData data2, int invenSlotNum , int equipSlotNum )
    {
        // 인벤토리와 장비창을 교환하는 경우

        /// <summary>
        /// 아이템 데이터를 슬롯끼리 스왑
        /// </summary>
        /// <param name="data1"> 지금 슬롯에 있는 아이템 데이터</param>
        /// <param name="data2"> 변경될 슬롯에 있는 아이템 데이터</param>
        if (this.sloType == Type.INVENTORY)
        {
            this.slotItemData = data1; // 인벤토리 
            myInven.equipslots[equipSlotNum].slotItemData = data2;  // 장비창 
            // 인벤토리에서 장비창으로 가는 경우이므로 스탯에 this.slotItemData 더해져야하고, equip이 빠져야한다.
            myInven.AddToStat( data2, data1);
        }
        // 장비창과 인벤토리를 교환하는 경우
        if (this.sloType == Type.EQUIPMENT)
        {
            this.slotItemData = data1;  // 장비창 
            myInven.itemslots[invenSlotNum].slotItemData = data2;  // 인벤토리 
            myInven.AddToStat(data1, data2);
        }

    }
 

    /// <summary>
    /// 장비창 또는 인벤토리의 아이템 데이터가 변경되었을떄 이미지 변경
    /// </summary>
    void SetSlotImage()
    {
        if (this.slotItemData != null)
        {
            image[2].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            // 5일 고민한 문제 ㅋㅋ.. as sprite 가 아니고 <Sprite> 형을 로드해야한다...
            this.image[2].sprite = Resources.Load<Sprite>(slotItemData.imagename.ToString());
        }
        if (this.slotItemData == null || this.slotItemData.type == 0 || this.slotItemData == default(ItemData))
        {
            image[2].color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            // 아이템 데이터가 비어져있으면 이미지 삭제
            image[2].sprite = null;
        }
    }
    

    // 메세지 나타나고 사라지게 됨
    IEnumerator FadeMessage(string s)
    {
        myInven.MessageLog.enabled = true;
        myInven.MessageLog.text = s;
        int i = 20;
        while (i >= 0)
        {
            i -= 1;
            float alpha = i / 10.0f;
            myInven.MessageLog.alpha = alpha;
            if ( i == 0)
            {
                myInven.MessageLog.enabled = false;
            }
            yield return new WaitForSeconds(0.03f);
        }
    }


}

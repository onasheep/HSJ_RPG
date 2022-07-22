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


    #region ���� �巡�� �Ķ����
    Vector2 DragOffset = Vector2.zero;
    #endregion


    void Awake()
    {
        // �ڽĿ� �ִ� highlight�� GetComponentsInChildren�� ���� �迭�� ������
        image = gameObject.GetComponentsInChildren<Image>();
        
        myInven = this.GetComponentInParent<Inventory>();

    }
    void Update()
    {
        SetSlotImage();
    }


    //  ���� �Դ� ��� ���� ���� X ���� �ϼ� �� ���� ����

    // ���â�� ���� �� ���¿��� ��� �̿��� �������� �����ϰ� �ٽ� ����ϸ� �Ӹ� â�� ������
    /// <summary>
    /// ������ ����ҋ��� ��� �����ϰ� �����ҋ��� ������ �ٸ�/ ������ ����ϸ� ������Ƿ� Itemdata�� null�� ���� ����ִ��� ������ �����ϰ�,
    /// ���� ����ִ� ���԰� ��ȯ�ǹǷ� null�� üũ�� ���� �����Ƿ� ������ null ���� type 0 ���� �̿��Ͽ� ������ ����� �ִ��� üũ��. 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        #region ��Ŭ���� �޾����� ���, ������, ���� 
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // type 0 �� �������� �����Ƿ� type�� 1���� ������
            // �κ��丮 â�� Ŭ���� ���
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
                            StartCoroutine(FadeMessage("HP�� ���� á���ϴ�."));
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
                            StartCoroutine(FadeMessage("MP�� ���� á���ϴ�."));
                        }
                        else if (myInven.myPlayer.mystat.MP < myInven.myPlayer.mystat.MaxMP)
                        {
                            myInven.PotionToStat(this.slotItemData);
                            this.slotItemData = null;
                        }

                    }

                    
                }
            }
            // ���â�� Ŭ���� ���
            else if (sloType == Type.EQUIPMENT && this.slotItemData != null)
            {
                EquipToInven();
            }
        }
        #endregion

        #region ��Ŭ���� �޾����� 
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

    // ���콺 �ø��� ���̶���Ʈ 
    public void OnPointerEnter(PointerEventData eventData)
    {
        image[1].color = new Color(1.0f, 200.0f / 255.0f, 100.0f / 255.0f, 1.0f);
    }

    // ���콺 ���������� ���� ������
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
        // �κ��丮�� �� ������ ���� ���
        if (myInven.CheckInvenFull(myInven.itemslots) && !myInven.MessageLog.enabled)
        {
            if (this.slotItemData.type != 0 && this.slotItemData.name != "WoodenArmor")
            {
                StartCoroutine(FadeMessage("�κ��丮�� ���� á���ϴ�."));

                //ShowMessageLog("�κ��丮�� ���� á���ϴ�.");
                return;
            }
            else
            return;
        }
        // �κ��丮�� �� ������ �ִ� ��� 
        else if (!(myInven.CheckInvenFull(myInven.itemslots)))
        {
           
            if (this.slotItemData.name == "WoodenArmor" )
            {
                if(!myInven.MessageLog.enabled)
                {
                    StartCoroutine(FadeMessage("�� �������� ���� �� �� �����ϴ�."));

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
        // �κ��丮�� ���â�� ��ȯ�ϴ� ���

        /// <summary>
        /// ������ �����͸� ���Գ��� ����
        /// </summary>
        /// <param name="data1"> ���� ���Կ� �ִ� ������ ������</param>
        /// <param name="data2"> ����� ���Կ� �ִ� ������ ������</param>
        if (this.sloType == Type.INVENTORY)
        {
            this.slotItemData = data1; // �κ��丮 
            myInven.equipslots[equipSlotNum].slotItemData = data2;  // ���â 
            // �κ��丮���� ���â���� ���� ����̹Ƿ� ���ȿ� this.slotItemData ���������ϰ�, equip�� �������Ѵ�.
            myInven.AddToStat( data2, data1);
        }
        // ���â�� �κ��丮�� ��ȯ�ϴ� ���
        if (this.sloType == Type.EQUIPMENT)
        {
            this.slotItemData = data1;  // ���â 
            myInven.itemslots[invenSlotNum].slotItemData = data2;  // �κ��丮 
            myInven.AddToStat(data1, data2);
        }

    }
 

    /// <summary>
    /// ���â �Ǵ� �κ��丮�� ������ �����Ͱ� ����Ǿ����� �̹��� ����
    /// </summary>
    void SetSlotImage()
    {
        if (this.slotItemData != null)
        {
            image[2].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            // 5�� ����� ���� ����.. as sprite �� �ƴϰ� <Sprite> ���� �ε��ؾ��Ѵ�...
            this.image[2].sprite = Resources.Load<Sprite>(slotItemData.imagename.ToString());
        }
        if (this.slotItemData == null || this.slotItemData.type == 0 || this.slotItemData == default(ItemData))
        {
            image[2].color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            // ������ �����Ͱ� ����������� �̹��� ����
            image[2].sprite = null;
        }
    }
    

    // �޼��� ��Ÿ���� ������� ��
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

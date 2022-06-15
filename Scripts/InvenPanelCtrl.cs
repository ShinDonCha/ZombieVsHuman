using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InvenPanelCtrl : MonoBehaviour , IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public GameObject m_DragSlot = null;
    public GameObject m_Slot = null;
    public GameObject m_ChangeSlot = null;

    private DragSlot m_DragSlotCtrl = null;

    public Image itemImage = null;

    public static Vector3 DragSlottr;                       // �巡�׾� ����� ������ ������ġ�� ���ư��� ���� ������ġ Vector ��

    public SlotCtrl m_SlotCtrl = null;
    public SlotCtrl m_ChangeSlotCtrl = null;
    private CanvasCtrl m_CanvasCtrl = new CanvasCtrl();     // �κ��丮�гΰ� ��Ʈ�г��� ���� ������ ������������ ����

    public Sprite Nullmgg = null;                           // �ٲٴ� ChangeSlot ItemType�� Null �϶� SlotItemImg �� �ٲٱ� ���� 

    //public static int itemCount; // ȹ���� �������� ����
    //public int UniqueitemNum;   // �� ���� �������� �ѹ�
    void Awake()
    {
        DragSlottr = m_DragSlot.transform.position;
        m_DragSlotCtrl = m_DragSlot.GetComponent<DragSlot>();
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_Slot = eventData.pointerCurrentRaycast.gameObject;
        m_SlotCtrl = m_Slot.GetComponent<SlotCtrl>();
        itemImage = m_Slot.GetComponent<Image>();        
        m_DragSlotCtrl.DragSetImage(itemImage);
        m_DragSlotCtrl.dragSlot = m_Slot.GetComponent<SlotCtrl>();
        m_DragSlot.transform.position = eventData.position;
        m_DragSlot.GetComponent<Image>().raycastTarget = false;

    }

    // ���콺 �巡�� ���� �� ��� �߻��ϴ� �̺�Ʈ
    public void OnDrag(PointerEventData eventData)
    {
        m_DragSlot.transform.position = eventData.position;
    }

    public void OnDrop(PointerEventData eventData)
    {
        m_ChangeSlot = eventData.pointerCurrentRaycast.gameObject;
        m_ChangeSlotCtrl = m_ChangeSlot.GetComponent<SlotCtrl>();

        if (m_DragSlotCtrl.dragSlot != null)
        {
            ChangeSlot();
        }
    }

    // ���콺 �巡�װ� ������ �� �߻��ϴ� �̺�Ʈ
    public void OnEndDrag(PointerEventData eventData)
    {

        m_DragSlotCtrl.SetColor(0);
        m_DragSlotCtrl.dragSlot = null;
        m_DragSlot.transform.position = DragSlottr;
        m_DragSlot.GetComponent<Image>().raycastTarget = true;
        m_DragSlot.GetComponent<Image>().sprite = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }


    private void ChangeSlot()
    {
        ItemType _tempItem = m_DragSlotCtrl.dragSlot.item;
        int _tempItemCount = m_DragSlotCtrl.dragSlot.UniqueitemNum;
        if (m_SlotCtrl.item == ItemType.Null && m_ChangeSlotCtrl.item == ItemType.Null)
            return;
        else if (m_SlotCtrl.item != ItemType.Null && m_ChangeSlotCtrl.item != ItemType.Null)
        {
            m_SlotCtrl.item = m_ChangeSlotCtrl.item;
            m_SlotCtrl.UniqueitemNum = m_ChangeSlotCtrl.UniqueitemNum;
            m_SlotCtrl.itemImage.sprite = GlobalValue.g_itemDic[m_SlotCtrl.item].m_iconImg;

            m_ChangeSlotCtrl.item = _tempItem;
            m_ChangeSlotCtrl.UniqueitemNum = _tempItemCount;
            m_ChangeSlotCtrl.itemImage.sprite = GlobalValue.g_itemDic[m_ChangeSlotCtrl.item].m_iconImg;
        }
        else if (m_SlotCtrl.item != ItemType.Null &&  m_ChangeSlotCtrl.item == ItemType.Null)
        {
            m_SlotCtrl.item = m_ChangeSlotCtrl.item;
            m_SlotCtrl.UniqueitemNum = m_ChangeSlotCtrl.UniqueitemNum;
            m_SlotCtrl.itemImage.sprite = Nullmgg;

            m_ChangeSlotCtrl.item = _tempItem;
            m_ChangeSlotCtrl.UniqueitemNum = _tempItemCount;
            m_ChangeSlotCtrl.itemImage.sprite = GlobalValue.g_itemDic[m_ChangeSlotCtrl.item].m_iconImg;
        }



        //AddItem(m_DragSlotCtrl.dragSlot.item, m_DragSlotCtrl.dragSlot.UniqueitemNum);

        //if (_tempItemCount > 0 || _tempItemCount <= m_CanvasCtrl.m_rootFullSlotCount)// 28�̶�� ������ �ӽ÷� �־����� 
        //   AddItem(_tempItem, _tempItemCount);
        //else
        //    //m_Slot.ClearSlot();
    }

}

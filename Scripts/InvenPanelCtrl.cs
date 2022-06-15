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

    public static Vector3 DragSlottr;                       // 드래그앤 드랍이 끝난후 본래위치로 돌아가기 위한 본래위치 Vector 값

    public SlotCtrl m_SlotCtrl = null;
    public SlotCtrl m_ChangeSlotCtrl = null;
    private CanvasCtrl m_CanvasCtrl = new CanvasCtrl();     // 인벤토리패널과 루트패널의 슬롯 개수를 가져오기위한 변수

    public Sprite Nullmgg = null;                           // 바꾸는 ChangeSlot ItemType이 Null 일때 SlotItemImg 를 바꾸기 위한 

    //public static int itemCount; // 획득한 아이템의 개수
    //public int UniqueitemNum;   // 이 슬롯 아이템의 넘버
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

    // 마우스 드래그 중일 때 계속 발생하는 이벤트
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

    // 마우스 드래그가 끝났을 때 발생하는 이벤트
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

        //if (_tempItemCount > 0 || _tempItemCount <= m_CanvasCtrl.m_rootFullSlotCount)// 28이라는 정수는 임시로 주어진것 
        //   AddItem(_tempItem, _tempItemCount);
        //else
        //    //m_Slot.ClearSlot();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragDropPanelCtrl : MonoBehaviour ,IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public GameObject m_dragSlot = null;            //드래그 슬롯 게임오브젝트를 담을 변수    
    
    public static DragDropPanelCtrl instance = null;    

    private SlotCtrl m_dragSlotCtrl = null;         //드래그 슬롯의 SlotCtrl 스크립트를 가져올 변수
    private SlotCtrl m_slotCtrl = null;             //OnBeginDrag 되는 대상의 SlotCtrl 스크립트를 가져올 변수
    private SlotCtrl m_targetSlotCtrl = null;       //OnDrop 되는 대상의 SlotCtrl 스크립트를 가져올 변수
    

    void Awake()
    {
        instance = this;
        m_dragSlotCtrl = m_dragSlot.GetComponent<SlotCtrl>();

       
    }


    public void OnBeginDrag(PointerEventData eventData)
    {        
        if (eventData.pointerCurrentRaycast.gameObject.name.Contains("Slot"))        //드래그 시작 시 눌린 게임오브젝트가 일반 슬롯이라면
        {            
            m_slotCtrl = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotCtrl>();       //대상의 SlotCtrl 스크립트 가져오기

            if (m_slotCtrl.m_itemInfo.m_itType != ItemType.Null)            //대상이 무기정보를 지니고 있다면
            {
                m_dragSlot.SetActive(true);                                 //드래그 슬롯 오브젝트를 키기
                m_dragSlotCtrl.m_itemInfo = m_slotCtrl.m_itemInfo;          //드래그 슬롯에 무기정보를 담기
                m_dragSlotCtrl.ChangeImg();
            }                
        }             
    }

    // 마우스 드래그 중일 때 계속 발생하는 이벤트
    public void OnDrag(PointerEventData eventData)
    {
        if (m_dragSlot.activeSelf == false)             //처음 드래그 대상이 일반슬롯이 아니였다면 리턴
            return;

        m_dragSlot.transform.position = eventData.position;             //드래그 슬롯 오브젝트의 위치를 마우스의 위치로 변경
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (m_dragSlot.activeSelf == false)             //처음 드래그 대상이 일반슬롯이 아니였다면 리턴
            return;

        if (eventData.pointerCurrentRaycast.gameObject.CompareTag("UserItem"))       //인벤토리 슬롯에 드랍했으면
        {
            m_targetSlotCtrl = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotCtrl>();     //대상 슬롯의 SlotCtrl 스크립트 가져오기
            ChangeSlot();           //슬롯의 정보 바꾸기 함수
        }
        else if (eventData.pointerCurrentRaycast.gameObject.CompareTag("RootItem"))
        {
            m_targetSlotCtrl = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotCtrl>();
            ChangeSlot();
            
        }
        
    }

    // 마우스 드래그가 끝났을 때 발생하는 이벤트
    public void OnEndDrag(PointerEventData eventData)
    {
        if (m_dragSlot.activeSelf == false)             //처음 드래그 대상이 일반슬롯이 아니였다면 리턴
            return;

        m_dragSlotCtrl.m_itemInfo = null;     //드래그 슬롯의 아이템 정보 초기화
        m_targetSlotCtrl = null;              //담았던 OnDrop 대상의 정보 초기화
        m_slotCtrl = null;                    //담았던 OnBeginDrag 대상의 정보 초기화
        m_dragSlot.SetActive(false);          //드래그 슬롯 오브젝트 끄기        
    }
   

    private void ChangeSlot()           //슬롯의 정보 바꿔주기
    {
        m_slotCtrl.m_itemInfo = m_targetSlotCtrl.m_itemInfo;                
        m_targetSlotCtrl.m_itemInfo = m_dragSlotCtrl.m_itemInfo;
        m_slotCtrl.ChangeImg();
        m_targetSlotCtrl.ChangeImg();
    }
}

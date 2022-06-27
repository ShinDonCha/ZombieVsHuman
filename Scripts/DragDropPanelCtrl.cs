using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragDropPanelCtrl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public GameObject m_dragSlot = null;            //드래그 슬롯 게임오브젝트를 담을 변수
    public GameObject m_slotObj = null;             //slot 프리팹  

    private SlotCtrl m_dragSlotCtrl = null;         //드래그 슬롯의 SlotCtrl 스크립트를 가져올 변수
    private SlotCtrl m_slotCtrl = null;             //OnBeginDrag 되는 대상의 SlotCtrl 스크립트를 가져올 변수
    private SlotCtrl m_targetSlotCtrl = null;       //OnDrop 되는 대상의 SlotCtrl 스크립트를 가져올 변수

    public GameObject m_equipmentPanel = null;      //장비판넬을 담는 변수
    public GameObject m_invenPanel = null;          //인벤판넬을 담는 변수
    public GameObject m_rootPanel = null;           //루트판넬을 담는 변수  

    public GameObject m_worldItem = null;           //인게임 상에서 생성될 아이템          
    private int m_rootFullSlotCount = 28;           //rootPanel에 넣어줄 버튼 개수     

    void Awake()
    {
        m_dragSlotCtrl = m_dragSlot.GetComponent<SlotCtrl>();                
    }    

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.tag.Contains("Slot"))
        {
            m_slotCtrl = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotCtrl>();       //대상의 SlotCtrl 스크립트 가져오기            

            if (m_slotCtrl.m_itemInfo.m_itName != ItemName.Null)            //대상이 무기정보를 지니고 있다면
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

        if (eventData.pointerCurrentRaycast.gameObject.tag.Contains("Slot"))       //슬롯에 드랍했으면
        {
            m_targetSlotCtrl = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotCtrl>();     //대상 슬롯의 SlotCtrl 스크립트 가져오기            

            if (m_targetSlotCtrl.gameObject.tag.Contains("Equip") &&                                 //대상 슬롯이 장비판넬안의 슬롯인데 장착할 아이템과 같은 타입이 아니라면 취소
                m_targetSlotCtrl.gameObject.name != m_dragSlotCtrl.m_itemInfo.m_itType.ToString())
                return;
           
            m_slotCtrl.m_itemInfo = m_targetSlotCtrl.m_itemInfo;                 //장착하려는 아이템과 기존 장비창 아이템의 정보 교환
            m_targetSlotCtrl.m_itemInfo = m_dragSlotCtrl.m_itemInfo;             //장착하려는 아이템과 기존 장비창 아이템의 정보 교환

            m_slotCtrl.ChangeImg();                                              //바뀐 정보대로 이미지 교체
            m_targetSlotCtrl.ChangeImg();                                        //바뀐 정보대로 이미지 교체
            m_slotCtrl.SaveList(m_slotCtrl.transform.parent.gameObject);         //판넬 종류별로 연동된 리스트에 정보 저장
            m_targetSlotCtrl.SaveList(m_targetSlotCtrl.transform.parent.gameObject);    //판넬 종류별로 연동된 리스트에 정보 저장

            //DelItemList(m_slotCtrl);
            //DelItemList(m_targetSlotCtrl);
        }

        // RootPanel 에서 Inventory Panel로 드랍할떄 월드맵에 있는 아이템 삭제
        ItemCtrl[] a_items = FindObjectsOfType<ItemCtrl>();
        for (int i = 0; i < a_items.Length; i++)
        {
            if (a_items[i].m_itemInfo.m_isDropped == false)
            {                
                Destroy(a_items[i].gameObject);
            }
        }

        // Inventory Panel 에서 RootPanel 로 드랍할 때 월드맵에 있는 아이템 생성
        for (int WorlditCount = 0; WorlditCount < PlayerCtrl.inst.m_itemList.Count; WorlditCount++)
        {
            if (PlayerCtrl.inst.m_itemList[WorlditCount].m_itName == ItemName.Null)         //빈 슬롯이면 넘어감
                continue;
            
            if (PlayerCtrl.inst.m_itemList[WorlditCount].m_isDropped == false)              //현재 드랍중이지 않은(리스트에 정보로만 존재하는)슬롯만 아이템의 외형 생성
            {                
                //생성
                PlayerCtrl.inst.m_itemList[WorlditCount].m_isDropped = true;

                int a_rndX = Random.Range(-1, 2);
                int a_rndZ = Random.Range(-1, 2);
                GameObject a_Item = Instantiate(m_worldItem);                                //아이템 모델을 담을 아이템프리팹 생성
                a_Item.transform.position = PlayerCtrl.inst.transform.position + new Vector3(a_rndX, 1, a_rndZ);  //생성위치를 플레이어 주위의 랜덤값 위치로 함
                a_Item.GetComponent<ItemCtrl>().m_itemInfo = PlayerCtrl.inst.m_itemList[WorlditCount];            //생성된 아이템의 정보를 리스트와 일치하게 변경                              
            }             
        }
        // Inventory Panel 에서 RootPanel 로 드랍할 때 월드맵에 있는 아이템 생성
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

    public void SetSlot()
    {                                      
        Cursor.lockState = CursorLockMode.None;                             //줍는동안 마우스 커서 나타나게 하기

        int a_remainCount = m_rootFullSlotCount - PlayerCtrl.inst.m_itemList.Count;      //전체 슬롯개수 - 현재 플레이어 주위의 아이템 개수 = 추가로 생성해줘야할 슬롯 개수
        
        for (int rootadd = 0; rootadd < m_rootFullSlotCount; rootadd++)     //rootPanel의 최대 슬롯 개수만큼 슬롯 생성
        {
            if(rootadd < a_remainCount)             //추가 생성이 필요한 슬롯 개수만큼 리스트 추가
            {
                ItemInfo a_ItemInfo = new ItemInfo();
                PlayerCtrl.inst.m_itemList.Add(a_ItemInfo);
            }
            GameObject a_slotobj = Instantiate(m_slotObj);                   //슬롯 생성
            a_slotobj.transform.SetParent(m_rootPanel.transform, false);    //슬롯을 rootPanel의 차일드로 이동
            SlotCtrl a_slotc = a_slotobj.GetComponent<SlotCtrl>();           //슬롯의 SlotCtrl 스크립트 가져오기
            a_slotc.m_itemInfo = PlayerCtrl.inst.m_itemList[rootadd];           
        }
        
        for (int invenadd = 0; invenadd < GlobalValue.g_userItem.Count; invenadd++)     //인벤토리 패널에 맞는 슬롯 개수만큼 슬롯 생성
        {            
            GameObject a_slotobj = Instantiate(m_slotObj);                    //슬롯 생성
            a_slotobj.transform.SetParent(m_invenPanel.transform, false);   //슬롯을 InvenPanel의 차일드로 이동                                                      
            SlotCtrl a_slotc = a_slotobj.GetComponent<SlotCtrl>();            //슬롯의 SlotCtrl 스크립트 가져오기             
            a_slotc.m_itemInfo = GlobalValue.g_userItem[invenadd];        //유저가 가지고있는 아이템의 정보 가져오기                
        }

        for (int equippedSlot = 0; equippedSlot < GlobalValue.g_equippedItem.Count; equippedSlot++)     //장비 패널에 맞는 슬롯 개수만큼 슬롯 정보 저장
        {
            m_equipmentPanel.transform.GetChild(equippedSlot).GetComponent<SlotCtrl>().m_itemInfo =
                GlobalValue.g_equippedItem[equippedSlot];
        }        
    }

    public void DelSlot()
    {        
        for (int i = 0; i < m_rootFullSlotCount; i++)        //rootPanel에 들은 slot오브젝트 개수만큼 실행                                  
            Destroy(m_rootPanel.transform.GetChild(i).gameObject);        //slot 오브젝트 삭제        
       
        PlayerCtrl.inst.m_itemList.RemoveAll( a =>          //rootPanel의 슬롯중 빈 슬롯의 정보만 리스트에서 삭제
            a.m_itType == ItemType.Null);

        for (int j = 0; j < m_invenPanel.transform.childCount; j++)       //invenPanel에 들은 slot오브젝트 개수만큼 실행
            Destroy(m_invenPanel.transform.GetChild(j).gameObject);      //해당 slot오브젝트 삭제        

        if (Cursor.lockState == CursorLockMode.None)
            Cursor.lockState = CursorLockMode.Locked;    //마우스커서 다시 잠그기
    }    

    void DelItemList(SlotCtrl a_SlotCtrl)
    {
        if (a_SlotCtrl.gameObject.name != "RootPanel")        
            a_SlotCtrl.m_itemInfo.m_isDropped = false;        
    }
}

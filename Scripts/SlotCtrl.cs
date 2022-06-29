using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotCtrl : MonoBehaviour, IPointerClickHandler
{
    public ItemInfo m_itemInfo = new ItemInfo();            //현재 슬롯의 아이템 정보    

    private float m_timer = 0.0f;          //계산용 변수
    private float m_equipTime = 2.0f;      //아이템 장착시 필요한 시간

    public GameObject m_timerImg = null;   //타이머이미지 게임오브젝트를 담을 변수

    void Start()
    {       
        ChangeImg();        //이미지 변경 함수 실행
    }

    void Update()
    {        
        if (this.gameObject.CompareTag("DragSlot"))             //드래그 슬롯일 경우 리턴
            return;
        
        if (m_timerImg.activeSelf == true && m_itemInfo.m_itName != ItemName.Null)      //마우스 우클릭으로 타이머가 켜지고, 빈 슬롯이 아닐 경우..
        {
            m_timer += Time.deltaTime;
            m_timerImg.GetComponent<Image>().fillAmount = m_timer / m_equipTime;
            if(m_equipTime < m_timer)
            {
                m_timerImg.SetActive(false);
                m_timer = 0.0f;

                if(gameObject.name.Contains("Slot"))          //선택된 슬롯이 인벤토리 판넬 또는 루트판넬의 슬롯일 경우
                    Equip();                
                else                                          //장비 판넬의 슬롯일 경우
                    Remove();                                
            }
        }
    }

    public void ChangeImg()
    {
        if (m_itemInfo.m_itName == ItemName.Null)       //해당 슬롯이 비어있는 슬롯이면
        {
            transform.GetChild(0).GetComponent<Image>().sprite = null;      //슬롯의 이미지를 빈 이미지로
        }
        else if (m_itemInfo.m_itName != ItemName.Null)  //해당 슬롯이 무기정보가 들어있는 슬롯이면
        {
            if (transform.childCount != 0)          //일반 슬롯 일경우 차일드의 이미지 변경
                transform.GetChild(0).GetComponent<Image>().sprite = m_itemInfo.m_iconImg;
            else                                   //드래그 슬롯 일경우 본인의 이미지 변경
                this.GetComponent<Image>().sprite = m_itemInfo.m_iconImg;
        }
    }

    void Equip()
    {
        DragDropPanelCtrl a_DDCtrl = FindObjectOfType<DragDropPanelCtrl>();
        GameObject a_GO = a_DDCtrl.m_equipmentPanel;                        //장비판넬 게임오브젝트를 담은 변수 가져오기
        SlotCtrl a_SC = a_GO.transform.GetChild((int)m_itemInfo.m_itType).GetComponent<SlotCtrl>(); //현재 슬롯의 아이템타입과 일치하는 장비판넬 슬롯의 SlotCtrl 가져오기
        a_SC.m_itemInfo = m_itemInfo;           //장비판넬 슬롯의 정보 바꾸기
        a_SC.ChangeImg();                       //장비판넬 슬롯의 이미지 바꾸기
        m_itemInfo = GlobalValue.g_equippedItem[(int)m_itemInfo.m_itType];  //현재 슬롯의 정보 바꾸기
        ChangeImg();                                                        //현재 슬롯의 이미지 바꾸기
        SaveList(a_GO);                                                //장착된 장비의 정보 저장
        SaveList(transform.parent.gameObject);                         //장착해제된 장비의 정보 저장

        ItemCtrl[] a_items = FindObjectsOfType<ItemCtrl>();
        for (int i = 0; i < a_items.Length; i++)
        {
            if (a_items[i].m_itemInfo.m_isDropped == false)
                Destroy(a_items[i].gameObject);
        }

        a_DDCtrl.ItemSetting();
    }

    void Remove()
    {
        DragDropPanelCtrl a_DDCtrl = FindObjectOfType<DragDropPanelCtrl>();
        GameObject a_GO = a_DDCtrl.m_invenPanel;                            //인벤토리 판넬 게임오브젝트를 담은 변수 가져오기

        for (int i = 0; i < GlobalValue.g_invenFullSlotCount; i++)          //인벤토리 슬롯의 최대개수 만큼 실행
        {
            if (GlobalValue.g_userItem[i].m_itType == ItemType.Null)        //인벤토리 슬롯중에 빈 슬롯에만 현재 장착아이템 저장하기
            {
                SlotCtrl a_SC = a_GO.transform.GetChild(i).GetComponent<SlotCtrl>();
                a_SC.m_itemInfo = m_itemInfo;                               //비어있는 인벤토리 슬롯에 현재 슬롯 정보 저장
                a_SC.ChangeImg();                                           //이미지 바꾸기
                m_itemInfo = GlobalValue.g_userItem[i];                     //현재 슬롯의 정보 바꾸기
                ChangeImg();                                                //현재 슬롯의 이미지 바꾸기
                SaveList(a_GO);                                             //장착해제된 장비의 정보 저장
                SaveList(transform.parent.gameObject);                      //장착된 장비의 정보 저장                
                break;                                                      //빈 슬롯을 찾았을 경우 한번만 실행하게 하기 위함
            }
            else
                continue;                                                   //빈 슬롯이 아닐 경우 넘어가기   
        }        
    }

    List<ItemInfo> FindList(GameObject a_Panel)                             //판넬별 정보 저장할 리스트 찾기
    {
        List<ItemInfo> a_List = new List<ItemInfo>();

        switch (a_Panel.name)
        {
            case "EquipmentPanel":
                a_List = GlobalValue.g_equippedItem;
                break;
            case "InventoryPanel":
                a_List = GlobalValue.g_userItem;
                break;
            case "RootPanel":
                a_List = PlayerCtrl.inst.m_itemList;
                break;
        }
        return a_List;
    }

    public void SaveList(GameObject a_Panel)                            //리스트에 정보 저장하는 매서드
    {        
        for (int a_num = 0; a_num < FindList(a_Panel).Count; a_num++)
        {            
            SlotCtrl a_slotCtrl = a_Panel.transform.GetChild(a_num).GetComponent<SlotCtrl>();

            if (a_Panel.name != "RootPanel")            //인벤토리 판넬과, 장비판넬에 있는 아이템일 경우만..            
                a_slotCtrl.m_itemInfo.m_isDropped = false;           

            FindList(a_Panel)[a_num] = a_slotCtrl.m_itemInfo;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)         //현재 슬롯에 마우스 오른쪽 클릭시..
        {
            m_timerImg.SetActive(true);
        }
    }

    
}
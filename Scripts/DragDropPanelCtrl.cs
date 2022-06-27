using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragDropPanelCtrl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public GameObject m_dragSlot = null;            //�巡�� ���� ���ӿ�����Ʈ�� ���� ����
    public GameObject m_slotObj = null;             //slot ������  

    private SlotCtrl m_dragSlotCtrl = null;         //�巡�� ������ SlotCtrl ��ũ��Ʈ�� ������ ����
    private SlotCtrl m_slotCtrl = null;             //OnBeginDrag �Ǵ� ����� SlotCtrl ��ũ��Ʈ�� ������ ����
    private SlotCtrl m_targetSlotCtrl = null;       //OnDrop �Ǵ� ����� SlotCtrl ��ũ��Ʈ�� ������ ����

    public GameObject m_equipmentPanel = null;      //����ǳ��� ��� ����
    public GameObject m_invenPanel = null;          //�κ��ǳ��� ��� ����
    public GameObject m_rootPanel = null;           //��Ʈ�ǳ��� ��� ����  

    public GameObject m_worldItem = null;           //�ΰ��� �󿡼� ������ ������          
    private int m_rootFullSlotCount = 28;           //rootPanel�� �־��� ��ư ����     

    void Awake()
    {
        m_dragSlotCtrl = m_dragSlot.GetComponent<SlotCtrl>();                
    }    

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.tag.Contains("Slot"))
        {
            m_slotCtrl = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotCtrl>();       //����� SlotCtrl ��ũ��Ʈ ��������            

            if (m_slotCtrl.m_itemInfo.m_itName != ItemName.Null)            //����� ���������� ���ϰ� �ִٸ�
            {
                m_dragSlot.SetActive(true);                                 //�巡�� ���� ������Ʈ�� Ű��
                m_dragSlotCtrl.m_itemInfo = m_slotCtrl.m_itemInfo;          //�巡�� ���Կ� ���������� ���                
                m_dragSlotCtrl.ChangeImg();
            }
        }           
    }

    // ���콺 �巡�� ���� �� ��� �߻��ϴ� �̺�Ʈ
    public void OnDrag(PointerEventData eventData)
    {
        if (m_dragSlot.activeSelf == false)             //ó�� �巡�� ����� �Ϲݽ����� �ƴϿ��ٸ� ����
            return;

        m_dragSlot.transform.position = eventData.position;             //�巡�� ���� ������Ʈ�� ��ġ�� ���콺�� ��ġ�� ����
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (m_dragSlot.activeSelf == false)             //ó�� �巡�� ����� �Ϲݽ����� �ƴϿ��ٸ� ����
            return;       

        if (eventData.pointerCurrentRaycast.gameObject.tag.Contains("Slot"))       //���Կ� ���������
        {
            m_targetSlotCtrl = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotCtrl>();     //��� ������ SlotCtrl ��ũ��Ʈ ��������            

            if (m_targetSlotCtrl.gameObject.tag.Contains("Equip") &&                                 //��� ������ ����ǳھ��� �����ε� ������ �����۰� ���� Ÿ���� �ƴ϶�� ���
                m_targetSlotCtrl.gameObject.name != m_dragSlotCtrl.m_itemInfo.m_itType.ToString())
                return;
           
            m_slotCtrl.m_itemInfo = m_targetSlotCtrl.m_itemInfo;                 //�����Ϸ��� �����۰� ���� ���â �������� ���� ��ȯ
            m_targetSlotCtrl.m_itemInfo = m_dragSlotCtrl.m_itemInfo;             //�����Ϸ��� �����۰� ���� ���â �������� ���� ��ȯ

            m_slotCtrl.ChangeImg();                                              //�ٲ� ������� �̹��� ��ü
            m_targetSlotCtrl.ChangeImg();                                        //�ٲ� ������� �̹��� ��ü
            m_slotCtrl.SaveList(m_slotCtrl.transform.parent.gameObject);         //�ǳ� �������� ������ ����Ʈ�� ���� ����
            m_targetSlotCtrl.SaveList(m_targetSlotCtrl.transform.parent.gameObject);    //�ǳ� �������� ������ ����Ʈ�� ���� ����

            //DelItemList(m_slotCtrl);
            //DelItemList(m_targetSlotCtrl);
        }

        // RootPanel ���� Inventory Panel�� ����ҋ� ����ʿ� �ִ� ������ ����
        ItemCtrl[] a_items = FindObjectsOfType<ItemCtrl>();
        for (int i = 0; i < a_items.Length; i++)
        {
            if (a_items[i].m_itemInfo.m_isDropped == false)
            {                
                Destroy(a_items[i].gameObject);
            }
        }

        // Inventory Panel ���� RootPanel �� ����� �� ����ʿ� �ִ� ������ ����
        for (int WorlditCount = 0; WorlditCount < PlayerCtrl.inst.m_itemList.Count; WorlditCount++)
        {
            if (PlayerCtrl.inst.m_itemList[WorlditCount].m_itName == ItemName.Null)         //�� �����̸� �Ѿ
                continue;
            
            if (PlayerCtrl.inst.m_itemList[WorlditCount].m_isDropped == false)              //���� ��������� ����(����Ʈ�� �����θ� �����ϴ�)���Ը� �������� ���� ����
            {                
                //����
                PlayerCtrl.inst.m_itemList[WorlditCount].m_isDropped = true;

                int a_rndX = Random.Range(-1, 2);
                int a_rndZ = Random.Range(-1, 2);
                GameObject a_Item = Instantiate(m_worldItem);                                //������ ���� ���� ������������ ����
                a_Item.transform.position = PlayerCtrl.inst.transform.position + new Vector3(a_rndX, 1, a_rndZ);  //������ġ�� �÷��̾� ������ ������ ��ġ�� ��
                a_Item.GetComponent<ItemCtrl>().m_itemInfo = PlayerCtrl.inst.m_itemList[WorlditCount];            //������ �������� ������ ����Ʈ�� ��ġ�ϰ� ����                              
            }             
        }
        // Inventory Panel ���� RootPanel �� ����� �� ����ʿ� �ִ� ������ ����
    }
    // ���콺 �巡�װ� ������ �� �߻��ϴ� �̺�Ʈ
    public void OnEndDrag(PointerEventData eventData)
    {
        if (m_dragSlot.activeSelf == false)             //ó�� �巡�� ����� �Ϲݽ����� �ƴϿ��ٸ� ����
            return;

        m_dragSlotCtrl.m_itemInfo = null;     //�巡�� ������ ������ ���� �ʱ�ȭ
        m_targetSlotCtrl = null;              //��Ҵ� OnDrop ����� ���� �ʱ�ȭ
        m_slotCtrl = null;                    //��Ҵ� OnBeginDrag ����� ���� �ʱ�ȭ
        m_dragSlot.SetActive(false);          //�巡�� ���� ������Ʈ ����        
    }       

    public void SetSlot()
    {                                      
        Cursor.lockState = CursorLockMode.None;                             //�ݴµ��� ���콺 Ŀ�� ��Ÿ���� �ϱ�

        int a_remainCount = m_rootFullSlotCount - PlayerCtrl.inst.m_itemList.Count;      //��ü ���԰��� - ���� �÷��̾� ������ ������ ���� = �߰��� ����������� ���� ����
        
        for (int rootadd = 0; rootadd < m_rootFullSlotCount; rootadd++)     //rootPanel�� �ִ� ���� ������ŭ ���� ����
        {
            if(rootadd < a_remainCount)             //�߰� ������ �ʿ��� ���� ������ŭ ����Ʈ �߰�
            {
                ItemInfo a_ItemInfo = new ItemInfo();
                PlayerCtrl.inst.m_itemList.Add(a_ItemInfo);
            }
            GameObject a_slotobj = Instantiate(m_slotObj);                   //���� ����
            a_slotobj.transform.SetParent(m_rootPanel.transform, false);    //������ rootPanel�� ���ϵ�� �̵�
            SlotCtrl a_slotc = a_slotobj.GetComponent<SlotCtrl>();           //������ SlotCtrl ��ũ��Ʈ ��������
            a_slotc.m_itemInfo = PlayerCtrl.inst.m_itemList[rootadd];           
        }
        
        for (int invenadd = 0; invenadd < GlobalValue.g_userItem.Count; invenadd++)     //�κ��丮 �гο� �´� ���� ������ŭ ���� ����
        {            
            GameObject a_slotobj = Instantiate(m_slotObj);                    //���� ����
            a_slotobj.transform.SetParent(m_invenPanel.transform, false);   //������ InvenPanel�� ���ϵ�� �̵�                                                      
            SlotCtrl a_slotc = a_slotobj.GetComponent<SlotCtrl>();            //������ SlotCtrl ��ũ��Ʈ ��������             
            a_slotc.m_itemInfo = GlobalValue.g_userItem[invenadd];        //������ �������ִ� �������� ���� ��������                
        }

        for (int equippedSlot = 0; equippedSlot < GlobalValue.g_equippedItem.Count; equippedSlot++)     //��� �гο� �´� ���� ������ŭ ���� ���� ����
        {
            m_equipmentPanel.transform.GetChild(equippedSlot).GetComponent<SlotCtrl>().m_itemInfo =
                GlobalValue.g_equippedItem[equippedSlot];
        }        
    }

    public void DelSlot()
    {        
        for (int i = 0; i < m_rootFullSlotCount; i++)        //rootPanel�� ���� slot������Ʈ ������ŭ ����                                  
            Destroy(m_rootPanel.transform.GetChild(i).gameObject);        //slot ������Ʈ ����        
       
        PlayerCtrl.inst.m_itemList.RemoveAll( a =>          //rootPanel�� ������ �� ������ ������ ����Ʈ���� ����
            a.m_itType == ItemType.Null);

        for (int j = 0; j < m_invenPanel.transform.childCount; j++)       //invenPanel�� ���� slot������Ʈ ������ŭ ����
            Destroy(m_invenPanel.transform.GetChild(j).gameObject);      //�ش� slot������Ʈ ����        

        if (Cursor.lockState == CursorLockMode.None)
            Cursor.lockState = CursorLockMode.Locked;    //���콺Ŀ�� �ٽ� ��ױ�
    }    

    void DelItemList(SlotCtrl a_SlotCtrl)
    {
        if (a_SlotCtrl.gameObject.name != "RootPanel")        
            a_SlotCtrl.m_itemInfo.m_isDropped = false;        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragDropPanelCtrl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler
{
    //public Button m_informBtn = null;               //������ �������� ��ư
    //[HideInInspector] public bool m_informOnOff = false;     //�������� �¿���

    public GameObject m_dragSlot = null;            //�巡�� ���� ���ӿ�����Ʈ�� ���� ����
    public GameObject m_slotObj = null;             //slot ������  

    private SlotCtrl m_dragSlotCtrl = null;         //�巡�� ������ SlotCtrl ��ũ��Ʈ�� ������ ����
    private SlotCtrl m_slotCtrl = null;             //OnBeginDrag �Ǵ� ����� SlotCtrl ��ũ��Ʈ�� ������ ����
    private SlotCtrl m_targetSlotCtrl = null;       //OnDrop �Ǵ� ����� SlotCtrl ��ũ��Ʈ�� ������ ����

    public GameObject m_equipmentPanel = null;      //����ǳ��� ��� ����
    public GameObject m_invenPanel = null;          //�κ��ǳ��� ��� ����
    public GameObject m_rootPanel = null;           //��Ʈ�ǳ��� ��� ����  

    public GameObject m_worldItem = null;           //�ΰ��� �󿡼� ������ ������          
    //private int m_rootFullSlotCount = 28;           //rootPanel�� �־��� ��ư ����

    //------ �������� â
    public GameObject m_information = null;
    public Text m_nameText = null;
    public Text m_statText = null;
    public Text m_explainText = null;
    [HideInInspector] public Vector3[] m_rectCorner = new Vector3[4];
    //------ �������� â

    void Start()
    {
        GetComponent<RectTransform>().GetWorldCorners(m_rectCorner);

        m_dragSlotCtrl = m_dragSlot.GetComponent<SlotCtrl>();

        ListSort();

        SetSlot();
    }    

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.tag.Contains("Slot"))
        {
            m_slotCtrl = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotCtrl>();       //����� SlotCtrl ��ũ��Ʈ ��������            

            if (m_slotCtrl.m_itemInfo.m_itName != ItemName.Kick)            //����� ���������� ���ϰ� �ִٸ�
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
        }

        ItemSetting();
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

        //int a_remainCount = m_rootFullSlotCount - PlayerCtrl.inst.m_itemList.Count;      //��ü ���԰��� - ���� �÷��̾� ������ ������ ���� = �߰��� ����������� ���� ����

        SlotCtrl[] a_rSlotC = m_rootPanel.GetComponentsInChildren<SlotCtrl>();             //��Ʈ �ǳ��� �ڽ� ������ SlotCtrl ��ũ��Ʈ ��������
        for (int rootadd = 0; rootadd < a_rSlotC.Length; rootadd++)                        //��Ʈ �ǳ��� ���� ������ŭ ����
            a_rSlotC[rootadd].m_itemInfo = PlayerCtrl.inst.m_itemList[rootadd];

        SlotCtrl[] a_iSlotC = m_invenPanel.GetComponentsInChildren<SlotCtrl>();             //�κ��丮 �ǳ��� �ڽ� ������ SlotCtrl ��ũ��Ʈ ��������
        for (int invenadd = 0; invenadd < a_iSlotC.Length; invenadd++)                      //�κ��丮 �ǳ��� ���� ������ŭ ����
            a_iSlotC[invenadd].m_itemInfo = GlobalValue.g_userItem[invenadd];

        SlotCtrl[] a_eSlotC = m_equipmentPanel.GetComponentsInChildren<SlotCtrl>();         //��� �ǳ��� �ڽ� ������ SlotCtrl ��ũ��Ʈ ��������
        for (int equippedadd = 0; equippedadd < a_eSlotC.Length; equippedadd++)             //��� �ǳ��� ���� ������ŭ ����
            a_eSlotC[equippedadd].m_itemInfo = GlobalValue.g_equippedItem[equippedadd];
    }

    //public void DelSlot()
    //{
    //    ListSort();         //�÷��̾� ���� ������ ����Ʈ ����

    //    //for (int i = 0; i < m_rootFullSlotCount; i++)        //rootPanel�� ���� slot������Ʈ ������ŭ ����                                  
    //    //    Destroy(m_rootPanel.transform.GetChild(i).gameObject);        //slot ������Ʈ ����        
       
    //    //PlayerCtrl.inst.m_itemList.RemoveAll( a =>          //rootPanel�� ������ �� ������ ������ ����Ʈ���� ����
    //    //    a.m_itType == ItemType.Kick);

    //    //for (int j = 0; j < m_invenPanel.transform.childCount; j++)       //invenPanel�� ���� slot������Ʈ ������ŭ ����
    //    //    Destroy(m_invenPanel.transform.GetChild(j).gameObject);      //�ش� slot������Ʈ ����        

    //    if (Cursor.lockState == CursorLockMode.None)
    //        Cursor.lockState = CursorLockMode.Locked;    //���콺Ŀ�� �ٽ� ��ױ�
    //}  

    public void ItemSetting()
    {
        // RootPanel ���� Inventory Panel�� ����ҋ� ����ʿ� �ִ� ������ ����
        ItemCtrl[] a_items = FindObjectsOfType<ItemCtrl>();
        for (int i = 0; i < a_items.Length; i++)
        {
            if (a_items[i].m_itemInfo.m_isDropped == false)
                Destroy(a_items[i].gameObject);
        }

        // Inventory Panel ���� RootPanel �� ����� �� ����ʿ� �ִ� ������ ����
        for (int WorlditCount = 0; WorlditCount < PlayerCtrl.inst.m_itemList.Count; WorlditCount++)
        {
            if (PlayerCtrl.inst.m_itemList[WorlditCount].m_itName == ItemName.Kick)         //�� �����̸� �Ѿ
                continue;

            if (PlayerCtrl.inst.m_itemList[WorlditCount].m_isDropped == false)              //���� ��������� ����(����Ʈ�� �����θ� �����ϴ�)���Ը� �������� ���� ����
            {
                PlayerCtrl.inst.m_itemList[WorlditCount].m_isDropped = true;

                int a_rndX = Random.Range(-1, 2);
                int a_rndZ = Random.Range(-1, 2);
                GameObject a_Item = Instantiate(m_worldItem);                                //������ ���� ���� ������������ ����
                a_Item.transform.position = PlayerCtrl.inst.transform.position + new Vector3(a_rndX, 1, a_rndZ);  //������ġ�� �÷��̾� ������ ������ ��ġ�� ��
                a_Item.GetComponent<ItemCtrl>().m_itemInfo = PlayerCtrl.inst.m_itemList[WorlditCount];            //������ �������� ������ ����Ʈ�� ��ġ�ϰ� ����                              
            }
        }

        SoundMgr.inst.m_audioSource.Play();
        // Inventory Panel ���� RootPanel �� ����� �� ����ʿ� �ִ� ������ ����
    }

    void ListSort()                             //�÷��̾� ���� �����۸���Ʈ ����
    {
        bool a_listEnd = false;                 //���̻� ã�� �ʾƵ� �� 

        for (int i = 0; i < PlayerCtrl.inst.m_itemList.Count; i++)
        {
            if (a_listEnd == true)
                break;

            if (PlayerCtrl.inst.m_itemList[i].m_itType == ItemType.Null)                    //�⺻����(�̹��� ����)�� ���
                for (int j = i + 1; j < PlayerCtrl.inst.m_itemList.Count; j++)              //�ϳ� �ڿ������� ������ ����Ʈ��� ����
                    if (PlayerCtrl.inst.m_itemList[j].m_itType != ItemType.Null)            //������� �����Ͱ� ���� ��ü
                    {
                        a_listEnd = false;                                                  //������ ������ �ʾ���
                        ItemInfo a_itemInfo = PlayerCtrl.inst.m_itemList[j];
                        PlayerCtrl.inst.m_itemList[j] = PlayerCtrl.inst.m_itemList[i];               //�⺻ ����� ���� ����
                        PlayerCtrl.inst.m_itemList[i] = a_itemInfo;                        
                        break;                                                              //�ѹ� ���࿡ �ϳ����� �ٲٱ�
                    }
                    else
                        a_listEnd = true;                                                   //����Ʈ���� �ٲ��� ���� ã�����ϸ� ���� ����

        }
    }

    private void OnDestroy()
    {
        ListSort();         //�÷��̾� ���� ������ ����Ʈ ����

        if (Cursor.lockState == CursorLockMode.None)
            Cursor.lockState = CursorLockMode.Locked;    //���콺Ŀ�� �ٽ� ��ױ�
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_information.activeSelf == true)
            m_information.SetActive(false);
    }
}

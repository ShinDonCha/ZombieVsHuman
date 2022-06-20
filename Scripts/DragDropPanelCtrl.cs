using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragDropPanelCtrl : MonoBehaviour ,IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public GameObject m_dragSlot = null;            //�巡�� ���� ���ӿ�����Ʈ�� ���� ����    
    
    public static DragDropPanelCtrl instance = null;    

    private SlotCtrl m_dragSlotCtrl = null;         //�巡�� ������ SlotCtrl ��ũ��Ʈ�� ������ ����
    private SlotCtrl m_slotCtrl = null;             //OnBeginDrag �Ǵ� ����� SlotCtrl ��ũ��Ʈ�� ������ ����
    private SlotCtrl m_targetSlotCtrl = null;       //OnDrop �Ǵ� ����� SlotCtrl ��ũ��Ʈ�� ������ ����
    

    void Awake()
    {
        instance = this;
        m_dragSlotCtrl = m_dragSlot.GetComponent<SlotCtrl>();

       
    }


    public void OnBeginDrag(PointerEventData eventData)
    {        
        if (eventData.pointerCurrentRaycast.gameObject.name.Contains("Slot"))        //�巡�� ���� �� ���� ���ӿ�����Ʈ�� �Ϲ� �����̶��
        {            
            m_slotCtrl = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotCtrl>();       //����� SlotCtrl ��ũ��Ʈ ��������

            if (m_slotCtrl.m_itemInfo.m_itType != ItemType.Null)            //����� ���������� ���ϰ� �ִٸ�
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

        if (eventData.pointerCurrentRaycast.gameObject.CompareTag("UserItem"))       //�κ��丮 ���Կ� ���������
        {
            m_targetSlotCtrl = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotCtrl>();     //��� ������ SlotCtrl ��ũ��Ʈ ��������
            ChangeSlot();           //������ ���� �ٲٱ� �Լ�
        }
        else if (eventData.pointerCurrentRaycast.gameObject.CompareTag("RootItem"))
        {
            m_targetSlotCtrl = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotCtrl>();
            ChangeSlot();
            
        }
        
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
   

    private void ChangeSlot()           //������ ���� �ٲ��ֱ�
    {
        m_slotCtrl.m_itemInfo = m_targetSlotCtrl.m_itemInfo;                
        m_targetSlotCtrl.m_itemInfo = m_dragSlotCtrl.m_itemInfo;
        m_slotCtrl.ChangeImg();
        m_targetSlotCtrl.ChangeImg();
    }
}

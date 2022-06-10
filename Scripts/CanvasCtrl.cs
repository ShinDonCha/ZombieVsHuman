using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCtrl : MonoBehaviour
{
    public Text m_explainMessage;                   //"FŰ�� ������ �������� �ֿ� �� �ֽ��ϴ�." �ؽ�Ʈ
    public GameObject m_invenPanel;                 //�κ��丮 �ǳ� ��� ����
    public GameObject m_rootContent = null;         //InventoryPanel ���� RootBackImg ���� �ִ� ������ ��ư�� ���� RootPanel
    public GameObject m_invenContent = null;        //InventoryPanel ���� inventoryBackImg ���� �ִ� ������ ��ư�� ���� inventoryPanel
    public GameObject m_slotObj = null;             //slot ���ӿ�����Ʈ      
    private int m_invenFullSlotCount = 12;                   //invenPanel�� �־��� ��ư ����
    private int m_rootFullSlotCount = 28;               //rootPanel�� �־��� ��ư ����
    
    private Vector3 slotSize = Vector3.one;         //Slot ������ ���������� ���� Vector ���� (1,1,1)

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ExplainMsg();   //"FŰ ~" �޽��� ������� �Լ�                
    }

    void ExplainMsg()   //������ �ݱ� Ű ǥ��
    {
        if (PlayerCtrl.inst.m_itemList.Count <= 0)
            m_explainMessage.gameObject.SetActive(false);
        else
            m_explainMessage.gameObject.SetActive(true);
    }

    public void PanelContrl()       // Panel�� �ΰ��� �Լ��̸��� ������.
    {
        m_invenPanel.SetActive(PlayerCtrl.inst.m_isLoot);       //�ǳ� ���̱�, �Ⱥ��̱�

        if (PlayerCtrl.inst.m_isLoot == true)                //�ݴ� �ִϸ��̼��� �ϰ�����
        {
            PlayerCtrl.inst.m_isRun = !PlayerCtrl.inst.m_isLoot;            //�ݴµ��� �̵����ϰ� ����                                
            Cursor.lockState = CursorLockMode.None;         //�ݴµ��� ���콺 Ŀ�� ��Ÿ���� �ϱ�

            for (int rootadd = 0; rootadd < m_rootFullSlotCount; rootadd++)           //��Ʈ �гο� �´� ���� ������ŭ ���� ����
            {
                Debug.Log(SlotCtrl.itemCount);
                Debug.Log(rootadd);
                GameObject a_slotobj = Instantiate(m_slotObj);
                a_slotobj.transform.SetParent(m_rootContent.transform);
                a_slotobj.transform.localScale = slotSize;                       //SetParent���� �ҷ����� ����� �ڵ������Ǽ� �������� �ٽ� 1�� �ٲ���.                
                SlotCtrl a_slotc = a_slotobj.GetComponent<SlotCtrl>();

                if (rootadd < PlayerCtrl.inst.m_itemList.Count)                   //�������ִ� ������ ������ŭ�� ����
                {
                    a_slotc.itemImage.sprite = GlobalValue.g_itemDic[PlayerCtrl.inst.m_itemList[rootadd].m_itemType].m_iconImg; //��ųʸ����� �����ۿ� �´� �̹��� ������
                    a_slotc.item = GlobalValue.g_itemDic[PlayerCtrl.inst.m_itemList[rootadd].m_itemType].m_itType;      //��ųʸ����� �����ۿ� �´� ������ Ÿ����������
                    a_slotc.itemImage.GetComponent<RectTransform>().sizeDelta =
                    GlobalValue.g_itemDic[PlayerCtrl.inst.m_itemList[rootadd].m_itemType].m_iconSize
                        * m_rootContent.GetComponent<GridLayoutGroup>().cellSize;
                    SlotCtrl.itemCount++;
                    
                }
                    
            }
            for (int invenadd = 0; invenadd < m_invenFullSlotCount; invenadd++)     //�κ��丮 �гο� �´� ���� ������ŭ ���� ����
            {
                GameObject b_slotobj = Instantiate(m_slotObj);
                b_slotobj.transform.SetParent(m_invenContent.transform);
                b_slotobj.transform.localScale = slotSize;                      //SetParent���� �ҷ����� ����� �ڵ������Ǽ� �������� �ٽ� 1�� �ٲ���.
                //SlotCtrl b_slotc = b_slotobj.GetComponent<SlotCtrl>();
            }
        }

        else if (PlayerCtrl.inst.m_isLoot == false)           //�ݴ� �ִϸ��̼� ��
        {
            for (int i = 0; i < m_rootContent.transform.childCount; i++)        //RootPanel�� ���� slot������Ʈ ���� ����
                Destroy(m_rootContent.transform.GetChild(i).gameObject);

            for (int j = 0; j < m_invenContent.transform.childCount; j++)       //invenPanel�� ���� slot������Ʈ ���� ����
                Destroy(m_invenContent.transform.GetChild(j).gameObject);

            if (Cursor.lockState == CursorLockMode.None)
                Cursor.lockState = CursorLockMode.Locked;    //���콺Ŀ�� �ٽ� ��ױ�
        }
        //else if (Input.GetKeyDown(KeyCode.Escape))            //�̰� ���� �ڵ�?
        //{
        //    m_isLoot = false;
        //    PlayerCtrl.inst.m_animController.SetBool("isLoot", m_isLoot);
        //    m_invenPanel.SetActive(m_isLoot);
        //}     
    }
}

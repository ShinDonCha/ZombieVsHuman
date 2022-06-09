using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCtrl : MonoBehaviour
{
    public Text m_explainMessage;                   //"FŰ�� ������ �������� �ֿ� �� �ֽ��ϴ�." �ؽ�Ʈ
    public GameObject m_invenPanel;                 //�κ��丮 �ǳ� ��� ����
    public GameObject m_rootContent = null;         //Canvas�� Scroll Rect�� Content
    public GameObject m_slotObj = null;             //slot ���ӿ�����Ʈ      
    private int m_slotCount = 12;                   //Content�� �־��� ���� ����

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

    public void InvenPanel()
    {
        m_invenPanel.SetActive(PlayerCtrl.inst.m_isLoot);       //�ǳ� ���̱�, �Ⱥ��̱�

        if (PlayerCtrl.inst.m_isLoot == true)                //�ݴ� �ִϸ��̼��� �ϰ�����
        {
            PlayerCtrl.inst.m_isRun = !PlayerCtrl.inst.m_isLoot;            //�ݴµ��� �̵����ϰ� ����                                

            for (int i = 0; i < m_slotCount; i++)           //���� ������ŭ ���� ����
            {
                GameObject a_slotobj = Instantiate(m_slotObj);
                a_slotobj.transform.SetParent(m_rootContent.transform);
                SlotCtrl a_slotc = a_slotobj.GetComponent<SlotCtrl>();

                if (i < PlayerCtrl.inst.m_itemList.Count)                   //�������ִ� ������ ������ŭ�� ����
                    a_slotc.m_img.sprite = GlobalValue.g_itemDic[PlayerCtrl.inst.m_itemList[i].m_itemType].m_iconImg;
                //��ųʸ����� �����ۿ� �´� �̹��� ������
            }
        }

        else if (PlayerCtrl.inst.m_isLoot == false)           //�ݴ� �ִϸ��̼� ��
        {
            for (int i = 0; i < m_rootContent.transform.childCount; i++)        //Content�� ���� slot������Ʈ ���� ����
                Destroy(m_rootContent.transform.GetChild(i).gameObject);
        }
        //else if (Input.GetKeyDown(KeyCode.Escape))            //�̰� ���� �ڵ�?
        //{
        //    m_isLoot = false;
        //    PlayerCtrl.inst.m_animController.SetBool("isLoot", m_isLoot);
        //    m_invenPanel.SetActive(m_isLoot);
        //}     
    }
}

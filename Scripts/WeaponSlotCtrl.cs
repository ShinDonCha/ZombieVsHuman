using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlotCtrl : MonoBehaviour
{
    public ItemInfo m_itemInfo = new ItemInfo();    // Weapon â�� ������ ����

    public static WeaponSlotCtrl instance = null;   // ��ũ��Ʈ ��ü�� ��ӹޱ����� ����

    private ItemInfo Change = new ItemInfo();       // ������ ������ ������ �����ϴ� �ӽ� ����

    private int m_invenSlot = 0;                    // Equied�� �ƴ� �κ��丮 ������ �˻��ϱ� ���� ���� ����
    public float m_Timer = 0;                       // FillAmount ȿ���� ���� ����
    public float m_CoolTimer = 2;                   // �����ϴ� �ð�
    public bool m_isClicked = false;                // Ŭ���� �޾��� ��..

    public GameObject Inventory = null;

    // Start is called before the first frame update
    void Start()
    {       
        instance = this;
    }

    void Update()
    {
        CoolTime();
    }

    public void ChangeSlot()
    {
        if (m_itemInfo.m_isEquied == true)
        {
            this.GetComponent<Image>().sprite = m_itemInfo.m_iconImg;            
        }

    }

    public void CoolTime()
    {
        if (m_itemInfo.m_itType != ItemType.Null && m_isClicked == true)
        {
            if (m_Timer >= 0 && m_Timer <= 2)
            {
                m_Timer += Time.deltaTime;
                transform.GetChild(2).GetComponent<Image>().fillAmount = m_Timer / m_CoolTimer;
                if (m_Timer > 1.9f)                              // ��Ÿ���� �� ������ ��..
                {
                    foreach(var ivPanelSlot in GlobalValue.g_userItem)
                    {
                        if (ivPanelSlot.m_isEquied == false)
                            m_invenSlot++;                        
                    }   //�κ��丮 �гο��� �ִ� ������ �˻��ϱ� ���� �ݺ���
                    
                    SlotCtrl IvChild = new SlotCtrl();
                    
                    if (m_invenSlot == 0)
                    {
                        IvChild = Inventory.transform.GetChild(0).GetComponent<SlotCtrl>();
                    }
                    else
                    {
                        IvChild = Inventory.transform.GetChild(m_invenSlot + 1)
                            .GetComponent<SlotCtrl>();
                    }
                    if (IvChild.m_itemInfo.m_itType == ItemType.Null)
                    {
                        Change = IvChild.m_itemInfo;
                        IvChild.m_itemInfo = m_itemInfo;
                        IvChild.m_itemInfo.m_isEquied = false;
                        IvChild.ChangeImg();
                    }

                    m_itemInfo = Change;
                    m_itemInfo.m_isEquied = true;
                    ChangeSlot();
                    m_Timer = 0;                                 // ��Ÿ�� �ʱ�ȭ
                    transform.GetChild(2).GetComponent<Image>().fillAmount = 0;  // ��Ÿ�� ȿ�� ���ڸ�
                    m_isClicked = false;
                }




            }


        }
    }
}


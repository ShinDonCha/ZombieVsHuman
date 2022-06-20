using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlotCtrl : MonoBehaviour
{
    public ItemInfo m_itemInfo = new ItemInfo();    // Weapon 창의 아이템 정보

    public static WeaponSlotCtrl instance = null;   // 스크립트 전체를 상속받기위한 변수

    private ItemInfo Change = new ItemInfo();       // 변경할 아이템 인포를 저장하는 임시 변수

    private int m_invenSlot = 0;                    // Equied가 아닌 인벤토리 슬롯을 검사하기 위한 정수 변수
    public float m_Timer = 0;                       // FillAmount 효과를 위한 변수
    public float m_CoolTimer = 2;                   // 장착하는 시간
    public bool m_isClicked = false;                // 클릭을 받았을 때..

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
                if (m_Timer > 1.9f)                              // 쿨타임이 다 돌았을 때..
                {
                    foreach(var ivPanelSlot in GlobalValue.g_userItem)
                    {
                        if (ivPanelSlot.m_isEquied == false)
                            m_invenSlot++;                        
                    }   //인벤토리 패널에만 있는 슬롯을 검사하기 위한 반복문
                    
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
                    m_Timer = 0;                                 // 쿨타임 초기화
                    transform.GetChild(2).GetComponent<Image>().fillAmount = 0;  // 쿨타임 효과 제자리
                    m_isClicked = false;
                }




            }


        }
    }
}


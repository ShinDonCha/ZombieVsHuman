using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotCtrl : MonoBehaviour 
{
    public ItemInfo m_itemInfo = new ItemInfo();

    private ItemInfo Change = new ItemInfo();

    public float m_Timer = 0;           // FillAmount 효과를 위한 변수
    public float m_CoolTimer = 2;       // 장착하는 시간

    public int m_UniqueNum = 0;     //슬롯 구별하는 정수변수

    public bool m_isClicked = false;    // 버튼 클릭 유무



    void Start()
    {      
        
        ChangeImg();        //이미지 변경 함수 실행        
    }

    void Update()
    {
        CoolTime();
    }
 
    public void ChangeImg()
    {
        if (m_itemInfo.m_itType == ItemType.Null)       //해당 슬롯이 비어있는 슬롯이면
        {
            transform.GetChild(0).GetComponent<Image>().sprite = null;      //슬롯의 이미지를 빈 이미지로
        }
        else if (m_itemInfo.m_itType != ItemType.Null)  //해당 슬롯이 무기정보가 들어있는 슬롯이면
        {
            if (transform.childCount != 0)          //일반 슬롯 일경우 차일드의 이미지 변경
                transform.GetChild(0).GetComponent<Image>().sprite = m_itemInfo.m_iconImg;
            else                                   //드래그 슬롯 일경우 본인의 이미지 변경
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
                transform.GetChild(1).GetComponent<Image>().fillAmount = m_Timer / m_CoolTimer;
                if (m_Timer > 1.9f)                              // 쿨타임이 다 돌았을 때..
                {
                    Change = WeaponSlotCtrl.instance.m_itemInfo;
                    WeaponSlotCtrl.instance.m_itemInfo = m_itemInfo;
                    WeaponSlotCtrl.instance.m_itemInfo.m_isEquied = true;
                    WeaponSlotCtrl.instance.ChangeSlot();
                    GlobalValue.g_userItem.Add(WeaponSlotCtrl.instance.m_itemInfo);
                    m_itemInfo = Change;
                    m_itemInfo.m_isEquied = false;         
   
                    ChangeImg();                        
                    m_Timer = 0;                                 // 쿨타임 초기화
                    transform.GetChild(1).GetComponent<Image>().fillAmount = 0;  // 쿨타임 효과 제자리
                    m_isClicked = false;                           
                                       
                }
             
                
            }
        }
    }
}
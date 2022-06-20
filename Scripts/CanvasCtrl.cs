using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCtrl : MonoBehaviour
{
    public Text m_explainMessage;                   //"F키를 누르면 아이템을 주울 수 있습니다." 텍스트
    public GameObject m_invenPanel;                 //인벤토리 판넬 담는 변수
    public GameObject m_rootContent = null;         //InventoryPanel 안의 RootBackImg 안의 있는 아이템 버튼을 담을 RootPanel
    public GameObject m_invenContent = null;        //InventoryPanel 안의 inventoryBackImg 안의 있는 아이템 버튼을 담을 inventoryPanel
    public GameObject m_slotObj = null;             //slot 게임오브젝트      
    private int m_invenFullSlotCount = 12;          //Content에 넣어줄 슬롯 개수    
    private int m_rootFullSlotCount = 28;           //rootPanel에 넣어줄 버튼 개수    

    //------------ Config Box
    [Header ("--------- Config Box ---------")]
    public GameObject m_configBox = null;    
    //------------ Config Box


    // Start is called before the first frame update
    void Start()
    {        
    }

    // Update is called once per frame
    void Update()
    {
        ExplainMsg();   //"F키 ~" 메시지 출력조절 함수
        ConfigBox();    //configBox 관련 함수
    }

    void ExplainMsg()   //아이템 줍기 키 표시
    {
        if (PlayerCtrl.inst.m_itemList.Count <= 0)                //플레이어 주위에 아이템이 없다면
            m_explainMessage.gameObject.SetActive(false);         //비활성화
        else                                                      //플레이어 주위에 아이템이 있다면
            m_explainMessage.gameObject.SetActive(true);          //활성화
    }

    public void PanelContrl()
    {
        m_invenPanel.SetActive(PlayerCtrl.inst.m_isLoot);       //판넬 보이기, 안보이기

        if (PlayerCtrl.inst.m_isLoot == true)                   //줍는 애니메이션을 하고있음
        {
            PlayerCtrl.inst.m_isRun = !PlayerCtrl.inst.m_isLoot;   //줍는동안 이동못하게 막기                                
            Cursor.lockState = CursorLockMode.None;             //줍는동안 마우스 커서 나타나게 하기

            for (int rootadd = 0; rootadd < m_rootFullSlotCount; rootadd++)      //rootPanel의 최대 슬롯 개수만큼 슬롯 생성
            {
                GameObject a_slotobj = Instantiate(m_slotObj);                   //슬롯 생성
                a_slotobj.transform.SetParent(m_rootContent.transform, false);   //슬롯을 rootPanel의 차일드로 이동
                a_slotobj.tag = "RootItem";
                SlotCtrl a_slotc = a_slotobj.GetComponent<SlotCtrl>();           //슬롯의 SlotCtrl 스크립트 가져오기

                if (rootadd < PlayerCtrl.inst.m_itemList.Count)                   //플레이어 주위에있는 아이템 개수만큼만 실행                               
                    a_slotc.m_itemInfo = GlobalValue.g_itemDic[PlayerCtrl.inst.m_itemList[rootadd].m_itemType];   //플레이어 주위의 아이템들의 정보 가져오기
            }

            for (int invenadd = 0; invenadd < m_invenFullSlotCount; invenadd++)     //인벤토리 패널에 맞는 슬롯 개수만큼 슬롯 생성
            {
               
                GameObject a_slotobj = Instantiate(m_slotObj);                    //슬롯 생성
                a_slotobj.transform.SetParent(m_invenContent.transform, false);   //슬롯을 InvenPanel의 차일드로 이동          
                a_slotobj.tag = "UserItem";                                       //인벤토리 슬롯은 태그 변경
                SlotCtrl a_slotc = a_slotobj.GetComponent<SlotCtrl>();            //슬롯의 SlotCtrl 스크립트 가져오기
      
                if (invenadd < GlobalValue.g_userItem.Count && a_slotc.m_itemInfo.m_isEquied == false)                      //유저가 가지고있는 아이템의 개수만큼 실행                
                {
                    a_slotc.m_itemInfo = GlobalValue.g_userItem[invenadd];        //유저가 가지고있는 아이템의 정보 가져오기
               
                }                                                                
            }
        }

        else if (PlayerCtrl.inst.m_isLoot == false)           //줍는 애니메이션 끝
        {
            GlobalValue.g_userItem.Clear();                                     //유저의 아이템 정보 초기화

            for (int i = 0; i < m_rootContent.transform.childCount; i++)        //rootContent에 들은 slot오브젝트 개수만큼 실행
                Destroy(m_rootContent.transform.GetChild(i).gameObject);        //slot 오브젝트 삭제

            for (int j = 0; j < m_invenContent.transform.childCount; j++)       //invenContent에 들은 slot오브젝트 개수만큼 실행
            {
                ItemInfo a_itemInfo = m_invenContent.transform.GetChild(j).gameObject.GetComponent<SlotCtrl>().m_itemInfo;  //slot오브젝트의 아이템 정보 가져오기
                if (a_itemInfo.m_itType !=  ItemType.Null)          //아이템 정보가 들어있는 슬롯일 경우
                    GlobalValue.g_userItem.Add(a_itemInfo);         //유저의 보유아이템 리스트에 추가                

                Destroy(m_invenContent.transform.GetChild(j).gameObject);      //해당 slot오브젝트 삭제
            }

            if (Cursor.lockState == CursorLockMode.None)
                Cursor.lockState = CursorLockMode.Locked;    //마우스커서 다시 잠그기
        }
 
    }

    void ConfigBox()
    {
        if (Input.GetKeyDown(KeyCode.Escape))                   //esc키 누르면 실행
        {
            if(m_configBox.activeSelf == true)                  //환경설정 박스가 켜져있을 경우
            {
                m_configBox.SetActive(false);                   //환경설정 박스 끄기
                Cursor.lockState = CursorLockMode.Locked;       //마우스 커서 잠그기
                Time.timeScale = 1.0f;                          //일시정지 해제
            }
            else //if(m_configBox.activeSelf == false)          //환경설정 박스가 꺼져있을 경우
            {
                m_configBox.SetActive(true);                    //환경설정 박스 켜기
                Cursor.lockState = CursorLockMode.None;         //마우스 커서 나타나게 하기
                Time.timeScale = 0.0f;                          //일시정지
            }
        }
    }
}

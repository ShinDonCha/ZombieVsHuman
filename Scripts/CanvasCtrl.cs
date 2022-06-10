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
    private int m_invenFullSlotCount = 12;                   //invenPanel에 넣어줄 버튼 개수
    private int m_rootFullSlotCount = 28;               //rootPanel에 넣어줄 버튼 개수
    
    private Vector3 slotSize = Vector3.one;         //Slot 사이즈 수동조절을 위한 Vector 변수 (1,1,1)

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ExplainMsg();   //"F키 ~" 메시지 출력조절 함수                
    }

    void ExplainMsg()   //아이템 줍기 키 표시
    {
        if (PlayerCtrl.inst.m_itemList.Count <= 0)
            m_explainMessage.gameObject.SetActive(false);
        else
            m_explainMessage.gameObject.SetActive(true);
    }

    public void PanelContrl()       // Panel이 두개라 함수이름만 수정함.
    {
        m_invenPanel.SetActive(PlayerCtrl.inst.m_isLoot);       //판넬 보이기, 안보이기

        if (PlayerCtrl.inst.m_isLoot == true)                //줍는 애니메이션을 하고있음
        {
            PlayerCtrl.inst.m_isRun = !PlayerCtrl.inst.m_isLoot;            //줍는동안 이동못하게 막기                                
            Cursor.lockState = CursorLockMode.None;         //줍는동안 마우스 커버 나타나게 하기

            for (int rootadd = 0; rootadd < m_rootFullSlotCount; rootadd++)           //루트 패널에 맞는 슬롯 개수만큼 슬롯 생성
            {
                Debug.Log(SlotCtrl.itemCount);
                Debug.Log(rootadd);
                GameObject a_slotobj = Instantiate(m_slotObj);
                a_slotobj.transform.SetParent(m_rootContent.transform);
                a_slotobj.transform.localScale = slotSize;                       //SetParent에서 불러오면 사이즈가 자동조절되서 수동으로 다시 1로 바꿔줌.                
                SlotCtrl a_slotc = a_slotobj.GetComponent<SlotCtrl>();

                if (rootadd < PlayerCtrl.inst.m_itemList.Count)                   //주위에있는 아이템 개수만큼만 실행
                {
                    a_slotc.itemImage.sprite = GlobalValue.g_itemDic[PlayerCtrl.inst.m_itemList[rootadd].m_itemType].m_iconImg; //딕셔너리에서 아이템에 맞는 이미지 가져옴
                    a_slotc.item = GlobalValue.g_itemDic[PlayerCtrl.inst.m_itemList[rootadd].m_itemType].m_itType;      //딕셔너리에서 아이템에 맞는 아이템 타입을가져옴
                    a_slotc.itemImage.GetComponent<RectTransform>().sizeDelta =
                    GlobalValue.g_itemDic[PlayerCtrl.inst.m_itemList[rootadd].m_itemType].m_iconSize
                        * m_rootContent.GetComponent<GridLayoutGroup>().cellSize;
                    SlotCtrl.itemCount++;
                    
                }
                    
            }
            for (int invenadd = 0; invenadd < m_invenFullSlotCount; invenadd++)     //인벤토리 패널에 맞는 슬롯 개수만큼 슬롯 생성
            {
                GameObject b_slotobj = Instantiate(m_slotObj);
                b_slotobj.transform.SetParent(m_invenContent.transform);
                b_slotobj.transform.localScale = slotSize;                      //SetParent에서 불러오면 사이즈가 자동조절되서 수동으로 다시 1로 바꿔줌.
                //SlotCtrl b_slotc = b_slotobj.GetComponent<SlotCtrl>();
            }
        }

        else if (PlayerCtrl.inst.m_isLoot == false)           //줍는 애니메이션 끝
        {
            for (int i = 0; i < m_rootContent.transform.childCount; i++)        //RootPanel에 들은 slot오브젝트 전부 삭제
                Destroy(m_rootContent.transform.GetChild(i).gameObject);

            for (int j = 0; j < m_invenContent.transform.childCount; j++)       //invenPanel에 들은 slot오브젝트 전부 삭제
                Destroy(m_invenContent.transform.GetChild(j).gameObject);

            if (Cursor.lockState == CursorLockMode.None)
                Cursor.lockState = CursorLockMode.Locked;    //마우스커서 다시 잠그기
        }
        //else if (Input.GetKeyDown(KeyCode.Escape))            //이거 무슨 코드?
        //{
        //    m_isLoot = false;
        //    PlayerCtrl.inst.m_animController.SetBool("isLoot", m_isLoot);
        //    m_invenPanel.SetActive(m_isLoot);
        //}     
    }
}

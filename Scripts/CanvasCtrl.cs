using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCtrl : MonoBehaviour
{
    public Text m_explainMessage;                   //"F키를 누르면 아이템을 주울 수 있습니다." 텍스트
    public GameObject m_invenPanel;                 //인벤토리 판넬 담는 변수
    public GameObject m_rootContent = null;         //Canvas의 Scroll Rect의 Content
    public GameObject m_slotObj = null;             //slot 게임오브젝트      
    private int m_slotCount = 12;                   //Content에 넣어줄 슬롯 개수

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

    public void InvenPanel()
    {
        m_invenPanel.SetActive(PlayerCtrl.inst.m_isLoot);       //판넬 보이기, 안보이기

        if (PlayerCtrl.inst.m_isLoot == true)                //줍는 애니메이션을 하고있음
        {
            PlayerCtrl.inst.m_isRun = !PlayerCtrl.inst.m_isLoot;            //줍는동안 이동못하게 막기                                

            for (int i = 0; i < m_slotCount; i++)           //슬롯 개수만큼 슬롯 생성
            {
                GameObject a_slotobj = Instantiate(m_slotObj);
                a_slotobj.transform.SetParent(m_rootContent.transform);
                SlotCtrl a_slotc = a_slotobj.GetComponent<SlotCtrl>();

                if (i < PlayerCtrl.inst.m_itemList.Count)                   //주위에있는 아이템 개수만큼만 실행
                    a_slotc.m_img.sprite = GlobalValue.g_itemDic[PlayerCtrl.inst.m_itemList[i].m_itemType].m_iconImg;
                //딕셔너리에서 아이템에 맞는 이미지 가져옴
            }
        }

        else if (PlayerCtrl.inst.m_isLoot == false)           //줍는 애니메이션 끝
        {
            for (int i = 0; i < m_rootContent.transform.childCount; i++)        //Content에 들은 slot오브젝트 전부 삭제
                Destroy(m_rootContent.transform.GetChild(i).gameObject);
        }
        //else if (Input.GetKeyDown(KeyCode.Escape))            //이거 무슨 코드?
        //{
        //    m_isLoot = false;
        //    PlayerCtrl.inst.m_animController.SetBool("isLoot", m_isLoot);
        //    m_invenPanel.SetActive(m_isLoot);
        //}     
    }
}

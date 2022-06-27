using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct ItemTest
{       
    public string m_name;                  //아이템 이름
    public int m_damage;                    //아이템 공격력   
    public int m_maganize;                  //아이템 총알갯수
    public float m_expandScale;          //발당 늘어나는 십자선 크기
    public float m_maxScale;             //십자선 최대 크기
    public float m_shrinkSpeed;          //십자선 줄어드는 속도
    public float m_attackDelay;          //공격 딜레이

    public bool m_isDropped;

    public ItemName m_itName;   //아이템 이름
    public ItemType m_itType;   //아이템 타입
    public Sprite m_iconImg;   //캐릭터 아이템에 사용될 이미지
    public Vector2 m_iconSize;  //아이템 이미지의 가로 사이즈, 세로 사이즈
    public string m_itemEx;      //아이템 설명
}

public class CanvasCtrl : MonoBehaviour
{
    public Text m_explainMessage;                   //"F키를 누르면 아이템을 주울 수 있습니다." 텍스트
    public GameObject m_dragdropPanel = null;       //인벤토리 판넬 담는 변수             

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

    public void DDPanelSet(bool a_bool)
    {
        if (a_bool == true)
        {
            PlayerCtrl.inst.m_isRun = !a_bool;                //줍는동안 이동못하게 막기  
            m_dragdropPanel.SetActive(a_bool);
            m_dragdropPanel.GetComponent<DragDropPanelCtrl>().SetSlot();        //슬롯 세팅
        }
        else
        {
            m_dragdropPanel.GetComponent<DragDropPanelCtrl>().DelSlot();        //슬롯 삭제
            m_dragdropPanel.SetActive(a_bool);
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

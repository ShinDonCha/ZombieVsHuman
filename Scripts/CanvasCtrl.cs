using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCtrl : MonoBehaviour
{
    public static CanvasCtrl inst = null;

    public Text m_explainMessage;                   //"F키를 누르면 아이템을 주울 수 있습니다." 텍스트
    public GameObject m_dragdropPanel = null;       //드래그 드랍 판넬 프리팹
    private GameObject m_ddGO = null;               //생성된 드래그 드랍 판넬

    [Header("--------- Player State ---------")]
    //-------- 플레이어 상태창
    public Image m_hpBar = null;
    public Text m_hpText = null;
    public Image m_staminaBar = null;

    public GameObject m_weaponImage = null;         //현재 무기 이미지
    public Text m_magazineText = null;              //장탄 수 표시해 줄 텍스트 변수
    //-------- 플레이어 상태창

    //------------ Config Box
    [Header ("--------- Config Box ---------")]
    public GameObject m_configBox = null;           //환경설정박스 프리팹 담을 변수
    [HideInInspector] public bool m_cfbOnOff = false;
    private GameObject m_go = null;
    //------------ Config Box

    public GameObject m_bloodPrefab = null;

    //--------- GameOverPanel
    [Header("--------- Game Over ---------")]
    public GameObject m_gameoverPanel = null;       //게임오버판넬 프리팹
    //--------- GameOverPanel

    private void Awake()
    {
        inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ExplainMsg();   //"F키 ~" 메시지 출력조절 함수
        ConfigBox();    //configBox 관련 함수
        PlayerState();
    }

    public void ExplainMsg()   //아이템 줍기 키 표시
    {
        //m_explainMessage.gameObject.SetActive(a_bool); 
        if (PlayerCtrl.inst.m_itemList[0].m_itType != ItemType.Null && m_explainMessage.gameObject.activeSelf == false)        //플레이어 주위에 아이템이 있다면
            m_explainMessage.gameObject.SetActive(true);         //활성화
        else if (PlayerCtrl.inst.m_itemList[0].m_itType == ItemType.Null && m_explainMessage.gameObject.activeSelf == true)   //플레이어 주위에 아이템이 없다면
            m_explainMessage.gameObject.SetActive(false);          //비활성화
    }

    public void DDPanelSet(bool a_bool)
    {
        //GameObject a_ddGO = null;

        if (a_bool == true)
        {
            PlayerCtrl.inst.m_isRun = !a_bool;
            m_ddGO = Instantiate(m_dragdropPanel, gameObject.transform);
            SoundMgr.inst.AudioChange(SoundList.Change);
            //m_dragdropPanel.SetActive(a_bool);
            //m_dragdropPanel.GetComponent<DragDropPanelCtrl>().SetSlot();        //슬롯 세팅
        }
        else
        {
            NetworkMgr.inst.PushPacket(PacketType.ItemChange);  //서버에 정보 저장
            Destroy(m_ddGO);
            SoundMgr.inst.AudioChange(SoundList.Weapon);
            //m_dragdropPanel.GetComponent<DragDropPanelCtrl>().DelSlot();        //슬롯 삭제
            //m_dragdropPanel.SetActive(a_bool);
        }            
    }

    void ConfigBox()
    {
        if (Input.GetKeyDown(KeyCode.Escape))                   //esc키 누르면 실행
        {
            m_cfbOnOff = !m_cfbOnOff;

            if (m_cfbOnOff == true)
            {
                m_go = Instantiate(m_configBox, gameObject.transform);
                Cursor.lockState = CursorLockMode.None;         //마우스 커서 나타나게 하기
                Time.timeScale = 0.0f;                          //일시정지
                InGameMgr.s_gameState = GameState.GamePaused;
            }
            else
                Destroy(m_go);
        }
    }

    private void PlayerState()          //플레이어 상태창 정보
    {
        if (PlayerCtrl.inst.m_groggy == true)
            m_staminaBar.color = new Color32(90, 90, 90, 255);
        else
            m_staminaBar.color = new Color32(244, 255, 0, 255);

        m_hpBar.fillAmount = PlayerCtrl.inst.m_curHp / PlayerCtrl.inst.m_maxHp;
        m_staminaBar.fillAmount = PlayerCtrl.inst.m_curSt / PlayerCtrl.inst.m_maxSt;
        m_hpText.text = ((int)PlayerCtrl.inst.m_curHp).ToString();

        if (PlayerCtrl.inst.m_nowWeapon.m_itemInfo.m_itName == ItemName.Kick)
            m_weaponImage.GetComponent<Image>().sprite = Resources.Load("Weapons/Fist", typeof(Sprite)) as Sprite;
        else
            m_weaponImage.GetComponent<Image>().sprite = PlayerCtrl.inst.m_nowWeapon.m_itemInfo.m_iconImg;

        m_weaponImage.transform.localScale = PlayerCtrl.inst.m_nowWeapon.m_itemInfo.m_iconSize;

        m_magazineText.text = string.Format("{0:D2} / {1:D2}",
            PlayerCtrl.inst.m_nowWeapon.m_itemInfo.m_curMagazine,
            PlayerCtrl.inst.m_nowWeapon.m_itemInfo.m_maxMagazine);        
    }

    public void SetBlood()
    {
        GameObject a_go = Instantiate(m_bloodPrefab, gameObject.transform);
        a_go.transform.SetAsFirstSibling();
    }

    public void PlayerDie()
    {
        Time.timeScale = 0.0f;          //일시정지
        InGameMgr.s_gameState = GameState.GamePaused;
        Cursor.lockState = CursorLockMode.None;
        m_gameoverPanel.SetActive(true);
    }
}

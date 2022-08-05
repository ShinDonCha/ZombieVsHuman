using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ConfigBoxCtrl : MonoBehaviour
{
    //private CrosshairCtrl m_refCrHair = null;

    //----- 장비교체 사운드
    public DragDropPanelCtrl m_refDDPCtrl = null;

    public Button m_cancelBtn = null;            //취소버튼

    //------ 로그아웃 & 게임종료
    public Button m_logoutBtn = null;
    public Button m_exitGameBtn = null;
    public GameObject m_confirmBox = null;        //게임종료 확인 판넬
    //------ 로그아웃 & 게임종료

    //---- 버튼 이미지    
    public Sprite[] m_buttonSlideImg = null;
    public Sprite m_buttonOffImg = null;
    //---- 버튼 이미지

    public Button m_bgBtn = null;                    //배경음 버튼
    public Slider m_bgSlider = null;                 //배경음 슬라이더

    public Button m_effBtn = null;                   //효과음 버튼
    public Slider m_effSlider = null;                //효과음 슬라이더

    //private AudioSource m_fireSound = null;
    //private AudioSource m_reloadSound = null;
    //private AudioSource m_changeSound = null;

    // Start is called before the first frame update
    void Start()
    {    
        if (m_cancelBtn != null)
            m_cancelBtn.onClick.AddListener(() =>
            {
                Destroy(gameObject);
                CanvasCtrl.inst.m_cfbOnOff = false;
            });

        if (m_logoutBtn != null)
            m_logoutBtn.onClick.AddListener(() =>
            {
                NetworkMgr.inst.PushPacket(PacketType.ItemChange);              //로그아웃 전에 한번 더 서버에 저장
                NetworkMgr.inst.PushPacket(PacketType.ConfigSet);               //로그아웃 전에 한번 더 서버에 저장
                InGameMgr.s_gameState = GameState.GoTitle;
            });

        if (m_exitGameBtn != null)
            m_exitGameBtn.onClick.AddListener(() =>
            {
                NetworkMgr.inst.PushPacket(PacketType.ItemChange);              //게임종료 전에 한번 더 서버에 저장
                NetworkMgr.inst.PushPacket(PacketType.ConfigSet);               //게임종료 전에 한번 더 서버에 저장
                InGameMgr.s_gameState = GameState.GameEnd;
            });

        if (m_bgBtn != null)                //배경음 버튼
            m_bgBtn.onClick.AddListener(() =>
            {
                
            });

        if (m_effBtn != null)               //효과음 버튼
            m_effBtn.onClick.AddListener(() =>
            {
                Image a_btnImg = m_effBtn.GetComponent<Image>();
                if (a_btnImg.sprite == m_buttonOffImg)              //음소거 상태였다면
                    ButtonImgChange(a_btnImg, m_effSlider);
                else                                                //음소거 상태가 아니었다면
                    a_btnImg.sprite = m_buttonOffImg;

                VolumeChange(a_btnImg.sprite);
            });        
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}    

    public void SlideValueChange(Image a_btnImg)       //슬라이더가 움직일 때 실행
    {
        if (EventSystem.current.currentSelectedGameObject == null)
            return;

        Slider a_Slider = EventSystem.current.currentSelectedGameObject.GetComponent<Slider>();
        ButtonImgChange(a_btnImg, a_Slider);
        VolumeChange(a_btnImg.sprite);
    }

    void ButtonImgChange(Image a_btnImg, Slider a_slider)
    {
        if (a_slider.value <= 0.0f)
            a_btnImg.sprite = m_buttonSlideImg[0];
        else if (0.0f < a_slider.value && a_slider.value <= 0.33f)
            a_btnImg.sprite = m_buttonSlideImg[1];
        else if (0.33f < a_slider.value && a_slider.value <= 0.66f)
            a_btnImg.sprite = m_buttonSlideImg[2];
        else if (0.66f < a_slider.value)
            a_btnImg.sprite = m_buttonSlideImg[3];
    }

    public void VolumeChange(Sprite a_btnSprite)
    {
        //m_refCrHair = CanvasCtrl.inst.GetComponentInChildren<CrosshairCtrl>();
        //m_refDDPCtrl = CanvasCtrl.inst.GetComponentInChildren<DragDropPanelCtrl>(true);          //오브젝트가 꺼져있어도 스크립트 가져오기
        ////m_fireSound = PlayerCtrl.inst.m_nowWeapon.m_fireAudio;
        //m_reloadSound = m_refCrHair.m_reloadAudio;
        //m_changeSound = m_refDDPCtrl.m_changeAudio;

        //----------- 효과음 설정
        if (a_btnSprite != m_buttonOffImg)           //음소거 이미지가 아니라면
        {
            SoundMgr.inst.m_audioSource.volume = SoundMgr.inst.m_curDefault * m_effSlider.value;
            //m_fireSound.volume = PlayerCtrl.inst.m_nowWeapon.m_defaultVolume * m_effSlider.value;
            //m_reloadSound.volume = m_refCrHair.m_defaultVolume * m_effSlider.value;
            //m_changeSound.volume = m_refDDPCtrl.m_defaultVolume * m_effSlider.value;
        }
        else
        {
            SoundMgr.inst.m_audioSource.volume = 0.0f;
            //m_fireSound.volume = 0.0f;
            //m_reloadSound.volume = 0.0f;
            //m_changeSound.volume = 0.0f;
        }
        //----------- 효과음 설정
    }

    public void OnEnable()
    {
        if (GlobalValue.g_cfBGImg != null && GlobalValue.g_cfEffImg != null)
        {
            m_bgBtn.GetComponent<Image>().sprite = GlobalValue.g_cfBGImg;
            m_effBtn.GetComponent<Image>().sprite = GlobalValue.g_cfEffImg;
            m_bgSlider.value = GlobalValue.g_cfBGValue;
            m_effSlider.value = GlobalValue.g_cfEffValue;
        }
    }

    private void OnDestroy()
    {
        GlobalValue.g_cfBGImg = m_bgBtn.GetComponent<Image>().sprite;
        GlobalValue.g_cfBGValue = m_bgSlider.value;
        GlobalValue.g_cfEffImg = m_effBtn.GetComponent<Image>().sprite;
        GlobalValue.g_cfEffValue = m_effSlider.value;

        NetworkMgr.inst.PushPacket(PacketType.ConfigSet);

        if (CanvasCtrl.inst.m_dragdropPanel.activeSelf == false)
            Cursor.lockState = CursorLockMode.Locked;       //마우스 커서 잠그기
        Time.timeScale = 1.0f;                          //일시정지 해제
        InGameMgr.s_gameState = GameState.GameIng;
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ConfigBoxCtrl : MonoBehaviour
{
    //private CrosshairCtrl m_refCrHair = null;

    //----- ���ü ����
    public DragDropPanelCtrl m_refDDPCtrl = null;

    public Button m_cancelBtn = null;            //��ҹ�ư

    //------ �α׾ƿ� & ��������
    public Button m_logoutBtn = null;
    public Button m_exitGameBtn = null;
    public GameObject m_confirmBox = null;        //�������� Ȯ�� �ǳ�
    //------ �α׾ƿ� & ��������

    //---- ��ư �̹���    
    public Sprite[] m_buttonSlideImg = null;
    public Sprite m_buttonOffImg = null;
    //---- ��ư �̹���

    public Button m_bgBtn = null;                    //����� ��ư
    public Slider m_bgSlider = null;                 //����� �����̴�

    public Button m_effBtn = null;                   //ȿ���� ��ư
    public Slider m_effSlider = null;                //ȿ���� �����̴�

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
                NetworkMgr.inst.PushPacket(PacketType.ItemChange);              //�α׾ƿ� ���� �ѹ� �� ������ ����
                NetworkMgr.inst.PushPacket(PacketType.ConfigSet);               //�α׾ƿ� ���� �ѹ� �� ������ ����
                InGameMgr.s_gameState = GameState.GoTitle;
            });

        if (m_exitGameBtn != null)
            m_exitGameBtn.onClick.AddListener(() =>
            {
                NetworkMgr.inst.PushPacket(PacketType.ItemChange);              //�������� ���� �ѹ� �� ������ ����
                NetworkMgr.inst.PushPacket(PacketType.ConfigSet);               //�������� ���� �ѹ� �� ������ ����
                InGameMgr.s_gameState = GameState.GameEnd;
            });

        if (m_bgBtn != null)                //����� ��ư
            m_bgBtn.onClick.AddListener(() =>
            {
                
            });

        if (m_effBtn != null)               //ȿ���� ��ư
            m_effBtn.onClick.AddListener(() =>
            {
                Image a_btnImg = m_effBtn.GetComponent<Image>();
                if (a_btnImg.sprite == m_buttonOffImg)              //���Ұ� ���¿��ٸ�
                    ButtonImgChange(a_btnImg, m_effSlider);
                else                                                //���Ұ� ���°� �ƴϾ��ٸ�
                    a_btnImg.sprite = m_buttonOffImg;

                VolumeChange(a_btnImg.sprite);
            });        
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}    

    public void SlideValueChange(Image a_btnImg)       //�����̴��� ������ �� ����
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
        //m_refDDPCtrl = CanvasCtrl.inst.GetComponentInChildren<DragDropPanelCtrl>(true);          //������Ʈ�� �����־ ��ũ��Ʈ ��������
        ////m_fireSound = PlayerCtrl.inst.m_nowWeapon.m_fireAudio;
        //m_reloadSound = m_refCrHair.m_reloadAudio;
        //m_changeSound = m_refDDPCtrl.m_changeAudio;

        //----------- ȿ���� ����
        if (a_btnSprite != m_buttonOffImg)           //���Ұ� �̹����� �ƴ϶��
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
        //----------- ȿ���� ����
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
            Cursor.lockState = CursorLockMode.Locked;       //���콺 Ŀ�� ��ױ�
        Time.timeScale = 1.0f;                          //�Ͻ����� ����
        InGameMgr.s_gameState = GameState.GameIng;
    }

    
}

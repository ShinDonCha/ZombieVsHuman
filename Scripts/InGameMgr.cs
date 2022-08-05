using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    GameIng,
    GamePaused,
    GameEnd,
    GoTitle,
    ReStart,
}

public class InGameMgr : MonoBehaviour
{
    //public static InGameMgr inst = null;
    public static GameState s_gameState = GameState.GameIng;

    public GameObject m_dragDropPanel = null;
    public ConfigBoxCtrl m_cbCtrl = null;


    //public GameObject m_soundBackBtn = null;
    //public GameObject m_soundeffBtn = null;

    //private void Awake()
    //{
    //    inst = this;   
    //}

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        Time.timeScale = 1.0f;
        s_gameState = GameState.GameIng;
        Cursor.lockState = CursorLockMode.Locked;       //마우스 커서를 윈도우 중앙에 고정시킨 후 보이지 않게 하기

        //------ 유저가 장착하고있던 아이템 적용(나중에 배열로 변경)
        EquipmentCtrl a_equipCtrl = m_dragDropPanel.GetComponentInChildren<EquipmentCtrl>(true);
        a_equipCtrl.m_slotCtrl.m_itemInfo = GlobalValue.g_equippedItem[2];
        a_equipCtrl.ItemOnOff();
        //------ 유저가 장착하고있던 아이템 적용(나중에 배열로 변경)

        //------ 저장된 환경설정 적용
        m_cbCtrl.OnEnable();
        m_cbCtrl.VolumeChange(GlobalValue.g_cfBGImg);
        m_cbCtrl.VolumeChange(GlobalValue.g_cfEffImg);
        //------ 저장된 환경설정 적용
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(2))
            Cursor.lockState = CursorLockMode.None;     //마우스 커서 잠긴거 풀기 (테스트 할때만 사용 나중에 삭제)
    }


    //private void OnApplicationQuit()
    //{
    //    PlayerCtrl.s_gameState = GameState.GameEnd;
    //    //StartCoroutine(SaveInfomation());
    //}
}

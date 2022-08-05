using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverCtrl : MonoBehaviour
{
    public Button m_restartBtn = null;
    public Button m_exitBtn = null;

    // Start is called before the first frame update
    void Start()
    {
        if (m_restartBtn != null)       //����� ��ư
            m_restartBtn.onClick.AddListener(() =>
            {
                NetworkMgr.inst.PushPacket(PacketType.ItemChange);
                InGameMgr.s_gameState = GameState.ReStart;
            });

        if (m_exitBtn != null)          //�������� ��ư
            m_exitBtn.onClick.AddListener(() =>
            {
                NetworkMgr.inst.PushPacket(PacketType.ItemChange);
                InGameMgr.s_gameState = GameState.GameEnd;
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

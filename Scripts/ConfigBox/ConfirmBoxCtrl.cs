using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmBoxCtrl : MonoBehaviour
{
    public Button m_OKBtn = null;
    public Button m_CancelBtn = null;

    // Start is called before the first frame update
    void Start()
    {
        if (m_OKBtn != null)
            m_OKBtn.onClick.AddListener(() =>
            {
                NetworkMgr.inst.PushPacket(PacketType.ItemChange);
                InGameMgr.s_gameState = GameState.GameEnd;
            });

        if (m_CancelBtn != null)
            m_CancelBtn.onClick.AddListener(() =>
                Destroy(gameObject));
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

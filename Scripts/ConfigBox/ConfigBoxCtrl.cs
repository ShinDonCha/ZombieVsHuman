using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigBoxCtrl : MonoBehaviour
{
    public Button m_cancelBtn = null;

    //------ 게임종료
    public Button m_exitGameBtn = null;
    public GameObject m_confirmBox = null;        //게임종료 확인 판넬
    //------ 게임종료

    // Start is called before the first frame update
    void Start()
    {
        if (m_cancelBtn != null)
            m_cancelBtn.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1.0f;
            });

        if (m_exitGameBtn != null)
            m_exitGameBtn.onClick.AddListener(() =>
            {
                GameObject a_ConfirmBox = Instantiate(m_confirmBox);
                a_ConfirmBox.transform.SetParent(this.gameObject.transform, false);
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }    
}

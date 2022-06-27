using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMgr : MonoBehaviour
{   
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        GlobalValue.InitData();
        Cursor.lockState = CursorLockMode.Locked;       //마우스 커서를 윈도우 중앙에 고정시킨 후 보이지 않게 하기
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(2))
            Cursor.lockState = CursorLockMode.None;     //마우스 커서 잠긴거 풀기 (테스트 할때만 사용 나중에 삭제)            
    }
}

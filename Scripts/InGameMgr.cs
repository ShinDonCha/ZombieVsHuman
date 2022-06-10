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
        Cursor.lockState = CursorLockMode.Locked;       //���콺 Ŀ���� ������ �߾ӿ� ������Ų �� ������ �ʰ� �ϱ�
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(2))
            Cursor.lockState = CursorLockMode.None;     //���콺 Ŀ�� ���� Ǯ�� (�׽�Ʈ �Ҷ��� ��� ���߿� ����)    
    }
}

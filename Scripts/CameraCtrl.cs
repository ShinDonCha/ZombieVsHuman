using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    private GameObject m_player = null;             //플레이어 저장 할 변수

    //---- 회전값 계산
    private Vector3 m_basicPos = Vector3.zero;      //플레이어에게 적용하기 전에 줌 값만 적용한 카메라 위치
    private Vector3 m_changedRot = Vector3.zero;    //변경 된 회전값
    private Vector3 m_targetPos = Vector3.zero;     //카메라가 바라볼 위치
    private Quaternion m_calcRot;                   //변경 된 회전값(vector3)을 Quaternion으로 변경해서 담는 변수
    private float m_rotSpeed = 50.0f;               //회전값 변경 속도
    //private float m_startRotx = 10.0f;           //처음에 설정하는 카메라 rotation x 값
    //---- 회전값 계산
    
    private float m_zoomDistance = 0.0f;      //플레이어와 카메라 사이의 거리       
    private float m_maxX = 60.0f;           //카메라 위아래 최대 rotation값
    

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;       //마우스 커서를 윈도우 중앙에 고정시킨 후 보이지 않게 하기
        m_player = GameObject.Find("Player");
        //m_player = transform.parent.gameObject;

        m_zoomDistance = 2.0f;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {        
        if (Input.GetMouseButton(2))
            Cursor.lockState = CursorLockMode.None;     //마우스 커서 잠긴거 풀기 (테스트 할때만 사용 나중에 삭제)      

        //------- 카메라 회전
        m_targetPos = m_player.transform.position;
        m_targetPos.y += 2.8f;              
        
        m_changedRot.y += Input.GetAxis("Mouse X") * m_rotSpeed * Time.deltaTime;
        m_changedRot.x -= Input.GetAxis("Mouse Y") * m_rotSpeed * Time.deltaTime;

        if (m_maxX < m_changedRot.x)         //카메라 위아래 각도 제한
            m_changedRot.x = m_maxX;
        if (m_changedRot.x < -m_maxX)
            m_changedRot.x = -m_maxX;

        m_zoomDistance = 2 + Mathf.Sin((m_changedRot.x / (m_maxX * 2)) * Mathf.PI);     //카메라와 타겟의 거리 1 ~ 3      

        m_basicPos.x = 0.0f;
        m_basicPos.y = 0.0f;
        m_basicPos.z = -m_zoomDistance;
        m_calcRot = Quaternion.Euler(m_changedRot.x, m_changedRot.y, 0);
        transform.position = m_calcRot * m_basicPos + m_targetPos;
        transform.LookAt(m_targetPos);
        //------- 카메라 회전

        //------- 플레이어 회전        
        Vector3 a_CamForward = transform.forward;
        a_CamForward.y = 0.0f;
        m_player.transform.forward = a_CamForward;        
        //------- 플레이어 회전
    }
}

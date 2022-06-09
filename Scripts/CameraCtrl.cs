using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    private GameObject m_player = null;             //�÷��̾� ���� �� ����

    //---- ȸ���� ���
    private Vector3 m_basicPos = Vector3.zero;      //�÷��̾�� �����ϱ� ���� �� ���� ������ ī�޶� ��ġ
    private Vector3 m_changedRot = Vector3.zero;    //���� �� ȸ����
    private Vector3 m_targetPos = Vector3.zero;     //ī�޶� �ٶ� ��ġ
    private Quaternion m_calcRot;                   //���� �� ȸ����(vector3)�� Quaternion���� �����ؼ� ��� ����
    private float m_rotSpeed = 50.0f;               //ȸ���� ���� �ӵ�
    //private float m_startRotx = 10.0f;           //ó���� �����ϴ� ī�޶� rotation x ��
    //---- ȸ���� ���
    
    private float m_zoomDistance = 0.0f;      //�÷��̾�� ī�޶� ������ �Ÿ�       
    private float m_maxX = 60.0f;           //ī�޶� ���Ʒ� �ִ� rotation��
    

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;       //���콺 Ŀ���� ������ �߾ӿ� ������Ų �� ������ �ʰ� �ϱ�
        m_player = GameObject.Find("Player");
        //m_player = transform.parent.gameObject;

        m_zoomDistance = 2.0f;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {        
        if (Input.GetMouseButton(2))
            Cursor.lockState = CursorLockMode.None;     //���콺 Ŀ�� ���� Ǯ�� (�׽�Ʈ �Ҷ��� ��� ���߿� ����)      

        //------- ī�޶� ȸ��
        m_targetPos = m_player.transform.position;
        m_targetPos.y += 2.8f;              
        
        m_changedRot.y += Input.GetAxis("Mouse X") * m_rotSpeed * Time.deltaTime;
        m_changedRot.x -= Input.GetAxis("Mouse Y") * m_rotSpeed * Time.deltaTime;

        if (m_maxX < m_changedRot.x)         //ī�޶� ���Ʒ� ���� ����
            m_changedRot.x = m_maxX;
        if (m_changedRot.x < -m_maxX)
            m_changedRot.x = -m_maxX;

        m_zoomDistance = 2 + Mathf.Sin((m_changedRot.x / (m_maxX * 2)) * Mathf.PI);     //ī�޶�� Ÿ���� �Ÿ� 1 ~ 3      

        m_basicPos.x = 0.0f;
        m_basicPos.y = 0.0f;
        m_basicPos.z = -m_zoomDistance;
        m_calcRot = Quaternion.Euler(m_changedRot.x, m_changedRot.y, 0);
        transform.position = m_calcRot * m_basicPos + m_targetPos;
        transform.LookAt(m_targetPos);
        //------- ī�޶� ȸ��

        //------- �÷��̾� ȸ��        
        Vector3 a_CamForward = transform.forward;
        a_CamForward.y = 0.0f;
        m_player.transform.forward = a_CamForward;        
        //------- �÷��̾� ȸ��
    }
}

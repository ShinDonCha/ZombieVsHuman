using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSensing : MonoBehaviour
{
    private ZombieCtrl m_zombieCtrl = null;
    private float m_traceDist = 10.0f;                   //���� �����Ÿ�

    // Start is called before the first frame update
    void Start()
    {
        m_zombieCtrl = transform.parent.GetComponent<ZombieCtrl>();

        GetComponent<SphereCollider>().radius = m_traceDist;      //�����Ÿ� ��ŭ �ݶ��̴� ũ�� ����
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))              //�����Ÿ��� �ȿ� ���� ����� �÷��̾���
        {            
            m_zombieCtrl.m_zombiestate = ZombieState.Trace;      //���� �������·� ����
            m_zombieCtrl.m_aggroTarget = other.gameObject;       //�ش��÷��̾ ����������� ����
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))              //�����Ÿ� ������ ����� ������
        {
            m_zombieCtrl.m_zombiestate = ZombieState.Idle;       //���� �⺻���·� ����
            m_zombieCtrl.m_aggroTarget = null;
        }
    }
}

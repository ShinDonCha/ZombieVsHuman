using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSensing : MonoBehaviour
{
    private ZombieCtrl m_zombieCtrl = null;
    private float m_traceDist = 10.0f;                   //좀비 추적거리

    // Start is called before the first frame update
    void Start()
    {
        m_zombieCtrl = transform.parent.GetComponent<ZombieCtrl>();

        GetComponent<SphereCollider>().radius = m_traceDist;      //추적거리 만큼 콜라이더 크기 변경
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))              //추적거리에 안에 들어온 대상이 플레이어라면
        {            
            m_zombieCtrl.m_zombiestate = ZombieState.Trace;      //좀비를 추적상태로 변경
            m_zombieCtrl.m_aggroTarget = other.gameObject;       //해당플레이어를 추적대상으로 잡음
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))              //추적거리 밖으로 대상이 나가면
        {
            m_zombieCtrl.m_zombiestate = ZombieState.Idle;       //좀비를 기본상태로 변경
            m_zombieCtrl.m_aggroTarget = null;
        }
    }
}

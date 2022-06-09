using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ZombieState
{
    idle,
    trace,        
    attack,
    die,
}

[System.Serializable]
class ZombieAniClip
{
    public AnimationClip[] m_idleAni = null;
    public AnimationClip[] m_attackAni = null;
    public AnimationClip[] m_runAni = null;
}

public class ZombieCtrl : MonoBehaviour
{
    //----- ���� ���� ����
    [Header("----- Zombie -----")]        
    [SerializeField]
    private ZombieAniClip m_aniClip;              //����Ƽ���� ���� �ִϸ��̼� Ŭ�� �������� ����
    Animator m_zombieAni = null;                  //�ִϸ����� ���� ����
    GameObject m_aggroTarget = null;              //������ Ÿ��(�÷��̾�)
    ZombieState m_zombiestate = ZombieState.idle; //���� ������ ����

    //---- ���� ����
    float m_moveSpeed = 3.0f;                    //���� �̵��ӵ�    
    float m_traceDist = 15.0f;                   //���� �����Ÿ�
    float m_attackDist = 2.0f;                   //���� ���ݰŸ�    
    float m_curHp = 0.0f;                       //������ ���� ü��
    float m_maxHp = 100.0f;                     //������ �ִ� ü��
    //---- ���� ����

    Vector3 m_calcVec = Vector3.zero;           //Ÿ�ٰ� ��������� ���� ��� ����
    Vector3 m_calcNor = Vector3.zero;           //Ÿ�ٰ� ��������� ���⺤��
    float m_calcMag = 0.0f;                     //Ÿ�ٰ� ��������� �Ÿ�
    float m_attackTime = 0.0f;                 //������ ���ݸ���� ���ӽð��� ��� ����
    //----- ���� ���� ����    

    // Start is called before the first frame update
    void Start()
    {
        m_zombieAni = GetComponentInChildren<Animator>();

        GetComponent<SphereCollider>().radius = m_traceDist;      //�����Ÿ� ��ŭ �ݶ��̴� ũ�� Ű���

        SetAni();                                                 //����鿡�� ���� �ִϸ��̼� �������ֱ�

        m_curHp = m_maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        ZombieStateUp();                                        //������ ���� ����
    }

    void ZombieStateUp()
    {
        if (m_aggroTarget != null)                          //Ÿ���� �ִٸ�(������ Sphere Collider �ȿ� ���� �÷��̾ Ÿ��)
        { 
            m_calcVec = m_aggroTarget.transform.position - transform.position;      //����� �÷��̾� ������ ����
            m_calcMag = m_calcVec.magnitude;            //����� �÷��̾� ������ �Ÿ�
            m_calcNor = m_calcVec.normalized;           //���� �÷��̾ ���� ����
            transform.forward = m_calcNor;             //���� Ÿ���� ������ ��
        }

        if (m_zombiestate == ZombieState.trace)       //���� �������¶��
        {
            m_attackTime = 0;                       
            ZAnimSet("Trace");                      //���� �ִϸ��̼� ���          
            
            transform.position = Vector3.MoveTowards(transform.position,
                    m_aggroTarget.transform.position, m_moveSpeed * Time.deltaTime);        //���� Ÿ�������� �̵� ��Ű��
            
            if(m_calcMag <= m_attackDist)               //����� �÷��̾� ������ �Ÿ��� ���ݰŸ����� ���ʿ� �ִٸ�            
                m_zombiestate = ZombieState.attack;            
        }

        else if (m_zombiestate == ZombieState.attack)   //���� ���ݻ��¶��
        {
            ZAnimSet("Attack");                         //���� �ִϸ��̼� ���
            m_attackTime += Time.deltaTime;
            if (m_attackTime > 2.0f && m_attackDist < m_calcMag)        //���ݰŸ��� �����, ������ ���ݸ���� �����ٸ�     
                m_zombiestate = ZombieState.trace;            
        }

        else if (m_zombiestate == ZombieState.idle)     //���� �⺻���¶��
        {   
            ZAnimSet("Idle");                           //�⺻ �ִϸ��̼� ���
        }
    }
    private void OnTriggerEnter(Collider other)
    {        
        if(other.CompareTag("player"))              //�����Ÿ��� �ȿ� ���� ����� �÷��̾���
        {            
            m_zombiestate = ZombieState.trace;      //���� �������·� ����
            m_aggroTarget = other.gameObject;       //�ش��÷��̾ ����������� ����           
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("player"))              //�����Ÿ� ������ ����� ������
        {            
            m_zombiestate = ZombieState.idle;       //���� �⺻���·� ����
            m_aggroTarget = null;
        }
    }

    void ZAnimSet(string newAnim)               //�ִϸ������� State �ٲ��ִ� �Լ�
    {
        if(newAnim == "Trace")              //���� �ִϸ��̼�
        {
            if(m_zombieAni.GetBool("IsRun") == false)
                m_zombieAni.SetBool("IsRun", true);
            if(m_zombieAni.GetBool("IsAttack") == true)
                m_zombieAni.SetBool("IsAttack", false);
        }
        else if (newAnim == "Attack")       //���� �ִϸ��̼�
        {            
            if(m_zombieAni.GetBool("IsAttack") == false)
                m_zombieAni.SetBool("IsAttack", true);
        }
        else if (newAnim == "Idle")         //�⺻ �ִϸ��̼�
        {
            if (m_zombieAni.GetBool("IsRun") == true)
                m_zombieAni.SetBool("IsRun", false);
        }
    }
    void SetAni()           //���񸶴� ���� �ִϸ��̼� �ο����ִ� �Լ�
    {
        int a_num = Random.Range(0, m_aniClip.m_idleAni.Length);
        m_zombieAni.SetFloat("idleBlend", (float)a_num);
        a_num = Random.Range(0, m_aniClip.m_runAni.Length);
        m_zombieAni.SetFloat("runBlend", (float)a_num);
        a_num = Random.Range(0, m_aniClip.m_attackAni.Length);
        m_zombieAni.SetFloat("attackBlend", (float)a_num);        
    }
    void Event_Attack()         //�ִϸ��̼� �̺�Ʈ���� ���۽�Ŵ(������ ���ݸ�� �߿� ������� �ֱ� ����)
    {
        //if (m_calcMag <= m_attackDist)                           //���ݰŸ� �ȿ� �ִٸ� ����� �ֱ�                   
            //m_aggroTarget.GetComponent<PlayerCtrl>().TakeDamage(20);                  
    }

    public void TakeDamage(float damage)
    {
        if (m_curHp < 0.0f)
            return;

        if (0.0f < m_curHp)
        {
            m_curHp -= damage;
            if (m_curHp < 0.0f)
                m_curHp = 0.0f;
        }
    }
}

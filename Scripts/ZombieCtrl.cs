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
    //----- 좀비 관련 변수
    [Header("----- Zombie -----")]        
    [SerializeField]
    private ZombieAniClip m_aniClip;              //유니티에서 좀비 애니메이션 클립 지정해줄 변수
    Animator m_zombieAni = null;                  //애니메이터 담을 변수
    GameObject m_aggroTarget = null;              //좀비의 타겟(플레이어)
    ZombieState m_zombiestate = ZombieState.idle; //현재 좀비의 상태

    //---- 좀비 스텟
    float m_moveSpeed = 3.0f;                    //좀비 이동속도    
    float m_traceDist = 1.5f;                   //좀비 추적거리
    float m_attackDist = 2.0f;                   //좀비 공격거리    
    float m_curHp = 0.0f;                       //좀비의 현재 체력
    float m_maxHp = 100.0f;                     //좀비의 최대 체력
    //---- 좀비 스텟

    Vector3 m_calcVec = Vector3.zero;           //타겟과 좀비사이의 벡터 담는 변수
    Vector3 m_calcNor = Vector3.zero;           //타겟과 좀비사이의 방향벡터
    float m_calcMag = 0.0f;                     //타겟과 좀비사이의 거리
    float m_attackTime = 0.0f;                 //좀비의 공격모션의 지속시간을 담는 변수
    //----- 좀비 관련 변수    

    // Start is called before the first frame update
    void Start()
    {
        m_zombieAni = GetComponentInChildren<Animator>();

        GetComponent<SphereCollider>().radius = m_traceDist;      //추적거리 만큼 콜라이더 크기 키우기

        SetAni();                                                 //좀비들에게 랜덤 애니메이션 지정해주기

        m_curHp = m_maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        ZombieStateUp();                                        //좀비의 현재 상태
    }

    void ZombieStateUp()
    {
        if (m_aggroTarget != null)                          //타겟이 있다면(좀비의 Sphere Collider 안에 들어온 플레이어가 타겟)
        { 
            m_calcVec = m_aggroTarget.transform.position - transform.position;      //좀비와 플레이어 사이의 벡터
            m_calcMag = m_calcVec.magnitude;            //좀비와 플레이어 사이의 거리
            m_calcNor = m_calcVec.normalized;           //좀비가 플레이어를 보는 방향
            transform.forward = m_calcNor;             //좀비가 타겟을 보도록 함
        }

        if (m_zombiestate == ZombieState.trace)       //좀비가 추적상태라면
        {
            m_attackTime = 0;                       
            ZAnimSet("Trace");                      //추적 애니메이션 재생          
            
            transform.position = Vector3.MoveTowards(transform.position,
                    m_aggroTarget.transform.position, m_moveSpeed * Time.deltaTime);        //좀비를 타겟쪽으로 이동 시키기
            
            if(m_calcMag <= m_attackDist)               //좀비와 플레이어 사이의 거리가 공격거리보다 안쪽에 있다면            
                m_zombiestate = ZombieState.attack;            
        }

        else if (m_zombiestate == ZombieState.attack)   //좀비가 공격상태라면
        {
            ZAnimSet("Attack");                         //공격 애니메이션 재생
            m_attackTime += Time.deltaTime;
            if (m_attackTime > 2.0f && m_attackDist < m_calcMag)        //공격거리를 벗어났고, 좀비의 공격모션이 끝났다면     
                m_zombiestate = ZombieState.trace;            
        }

        else if (m_zombiestate == ZombieState.idle)     //좀비가 기본상태라면
        {   
            ZAnimSet("Idle");                           //기본 애니메이션 재생
        }
    }
    private void OnTriggerEnter(Collider other)
    {        
        if(other.CompareTag("player"))              //추적거리에 안에 들어온 대상이 플레이어라면
        {            
            m_zombiestate = ZombieState.trace;      //좀비를 추적상태로 변경
            m_aggroTarget = other.gameObject;       //해당플레이어를 추적대상으로 잡음           
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("player"))              //추적거리 밖으로 대상이 나가면
        {            
            m_zombiestate = ZombieState.idle;       //좀비를 기본상태로 변경
            m_aggroTarget = null;
        }
    }

    void ZAnimSet(string newAnim)               //애니메이터의 State 바꿔주는 함수
    {
        if(newAnim == "Trace")              //추적 애니메이션
        {
            if(m_zombieAni.GetBool("IsRun") == false)
                m_zombieAni.SetBool("IsRun", true);
            if(m_zombieAni.GetBool("IsAttack") == true)
                m_zombieAni.SetBool("IsAttack", false);
        }
        else if (newAnim == "Attack")       //공격 애니메이션
        {            
            if(m_zombieAni.GetBool("IsAttack") == false)
                m_zombieAni.SetBool("IsAttack", true);
        }
        else if (newAnim == "Idle")         //기본 애니메이션
        {
            if (m_zombieAni.GetBool("IsRun") == true)
                m_zombieAni.SetBool("IsRun", false);
        }
    }
    void SetAni()           //좀비마다 랜덤 애니메이션 부여해주는 함수
    {
        int a_num = Random.Range(0, m_aniClip.m_idleAni.Length);
        m_zombieAni.SetFloat("idleBlend", (float)a_num);
        a_num = Random.Range(0, m_aniClip.m_runAni.Length);
        m_zombieAni.SetFloat("runBlend", (float)a_num);
        a_num = Random.Range(0, m_aniClip.m_attackAni.Length);
        m_zombieAni.SetFloat("attackBlend", (float)a_num);        
    }
    void Event_Attack()         //애니메이션 이벤트에서 동작시킴(좀비의 공격모션 중에 대미지를 주기 위함)
    {
        //if (m_calcMag <= m_attackDist)                           //공격거리 안에 있다면 대미지 주기                   
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

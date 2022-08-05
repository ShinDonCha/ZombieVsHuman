using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum ZombieState
{
    Idle,
    Trace,        
    Attack,
    Die,
}

public class ZombieCtrl : MonoBehaviour
{
    //----- 좀비 관련 변수
    [Header("----- Zombie -----")]
    private Animator m_zombieAni = null;                  //애니메이터 담을 변수
    [HideInInspector] public GameObject m_aggroTarget = null;              //좀비의 타겟(플레이어)
    [HideInInspector] public ZombieState m_zombiestate = ZombieState.Idle; //현재 좀비의 상태
    private Rigidbody m_zombieRigid = null;

    //---- 좀비 스텟
    //float m_moveSpeed = 1.5f;                    //좀비 이동속도   
    [HideInInspector] public float m_attackDist = 1.0f;                   //좀비 공격거리
    float m_curHp = 0.0f;                       //좀비의 현재 체력
    //---- 좀비 스텟

    Vector3 m_calcVec = Vector3.zero;           //타겟과 좀비사이의 벡터 담는 변수
    Vector3 m_calcNor = Vector3.zero;           //타겟과 좀비사이의 방향벡터
    float m_calcMag = 0.0f;                     //타겟과 좀비사이의 거리
    public Image m_hpBarImg = null;

    public ZCommonSet m_zCommon = null;         //ZCommonSet ScriptableObject(좀비의 공통 변수를 담은 오브젝트) 가져올 변수
    int m_aniNum = 0;                           //애니메이션 넘버
    //----- 좀비 관련 변수    

    [HideInInspector] public bool m_collCheck = false;          //좀비가 지형과 충돌했는지 체크

    // Start is called before the first frame update
    void Start()
    {
        m_zombieRigid = GetComponent<Rigidbody>();

        m_zombieAni = GetComponent<Animator>();        

        SetAni();                                                 //좀비들에게 랜덤 애니메이션 지정해주기

        m_curHp = m_zCommon.m_maxHp;
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
            Vector3 a_aggroPos = m_aggroTarget.transform.position;      //타겟의 위치정보 가져오기
            a_aggroPos.y = transform.position.y;                //좀비의 높이와 같은높이로 보정
            m_calcVec = a_aggroPos - transform.position;      //좀비와 플레이어 사이의 벡터
            m_calcMag = m_calcVec.magnitude;            //좀비와 플레이어 사이의 거리
            m_calcNor = m_calcVec.normalized;           //좀비가 플레이어를 보는 방향
            transform.forward = m_calcNor;             //좀비가 타겟을 보도록 함
        }

        if (m_zombiestate == ZombieState.Trace)       //좀비가 추적상태라면
        {
            if(m_collCheck == true)
                m_zombieRigid.constraints = (RigidbodyConstraints)84;

            ZAnimSet("Trace");                      //추적 애니메이션 재생          

            //m_zombieRigid.MovePosition(transform.position + transform.forward * m_moveSpeed * Time.deltaTime);
            //transform.position = Vector3.MoveTowards(transform.position,
            //        m_aggroTarget.transform.position, m_moveSpeed * Time.deltaTime);        //좀비를 타겟쪽으로 이동 시키기(현재는 좀비가 애니메이션 속도로 움직이고 있음)
            
            if(m_calcMag <= m_attackDist)               //좀비와 플레이어 사이의 거리가 공격거리보다 안쪽에 있다면            
                m_zombiestate = ZombieState.Attack;            
        }

        else if (m_zombiestate == ZombieState.Attack)   //좀비가 공격상태라면
        {
            if(m_collCheck == true)
                m_zombieRigid.constraints = RigidbodyConstraints.FreezeAll;     //좀비 안밀리게 고정

            ZAnimSet("Attack");                         //공격 애니메이션 재생

            if(0.95 < m_zombieAni.GetCurrentAnimatorStateInfo(0).normalizedTime)     //공격 애니메이션이 끝났을 때
                if (m_attackDist < m_calcMag)                                        //좀비의 공격거리를 벗어났다면 추적상태로 변경
                    m_zombiestate = ZombieState.Trace;
        }

        else if (m_zombiestate == ZombieState.Idle)     //좀비가 기본상태라면
        {
            if (m_collCheck == true)
                m_zombieRigid.constraints = (RigidbodyConstraints)84;

            ZAnimSet("Idle");                           //기본 애니메이션 재생
        }

        else if (m_zombiestate == ZombieState.Die)
        {
            ZAnimSet("Die");
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
        else if (newAnim == "Die")
        {
            if (m_zombieAni.GetCurrentAnimatorStateInfo(0).IsName("dieBlend"))
                return;

            m_zombieAni.SetTrigger("IsDie");
        }
    }

    public void SetAni()           //좀비마다 랜덤 애니메이션 부여해주는 함수
    {
        m_aniNum = Random.Range(0, m_zCommon.m_aniClip.m_idleAni.Length);        //0~1까지 2개의 애니메이션 존재
        m_zombieAni.SetFloat("idleBlend", (float)m_aniNum);

        m_aniNum = Random.Range(0, m_zCommon.m_aniClip.m_runAni.Length);        //0~1까지 2개의 애니메이션 존재
        m_zombieAni.SetFloat("runBlend", (float)m_aniNum);

        m_aniNum = Random.Range(0, m_zCommon.m_aniClip.m_attackAni.Length);        //0~1까지 2개의 애니메이션 존재
        m_zombieAni.SetFloat("attackBlend", (float)m_aniNum);

        m_aniNum = Random.Range(0, m_zCommon.m_aniClip.m_dieAni.Length);        //0~2까지 3개의 애니메이션 존재
        m_zombieAni.SetFloat("dieBlend", (float)m_aniNum);
    }

    public void TakeDamage(Vector3 hitPoint, float damage, float bwRange)
    {
        if (m_curHp <= 0.0f)
            return;

        if (0.0f < m_curHp)
        {            
            m_curHp -= damage;

            //------- 좀비가 밀려날 방향 계산
            Vector3 a_corVec = PlayerCtrl.inst.transform.position;
            a_corVec.y = transform.position.y;
            Vector3 a_dirVec = (a_corVec - transform.position).normalized;
            //------- 좀비가 밀려날 방향 계산

            transform.position = transform.position - a_dirVec * bwRange;          //일정거리만큼 뒤로 밀림
            CreateBloodEff(hitPoint);

            if (m_curHp <= 0.0f)
            {
                m_curHp = 0.0f;
                m_zombiestate = ZombieState.Die;
                Destroy(gameObject, m_zCommon.m_aniClip.m_dieAni[m_aniNum].length);         //죽는 애니메이션 재생시간 후에 삭제
            }

            m_hpBarImg.fillAmount = m_curHp / m_zCommon.m_maxHp;
        }
    }

    public void CreateBloodEff(Vector3 a_Pos)
    {
        if (m_curHp <= 0.0f)
            return;

        GameObject a_bloodEff = Instantiate(m_zCommon.m_bloodEff, a_Pos, Quaternion.identity);        //피 이펙트 생성
        a_bloodEff.GetComponent<ParticleSystem>().Play();                                             //피 이펙트 재생
        Destroy(a_bloodEff, 2.0f);      //2초뒤 삭제

        //--------- 바닥에 떨어질 피 위치계산
        Vector3 a_vec = transform.position;
        int a_rnd = Random.Range(0, 3);
        a_vec.x += m_zCommon.m_decPos[a_rnd];
        a_rnd = Random.Range(0, 3);
        a_vec.z += m_zCommon.m_decPos[a_rnd];
        //--------- 바닥에 떨어질 피 위치계산

        Quaternion a_rot = Quaternion.Euler(90, 0, Random.Range(0, 360));               //피 각도 랜덤으로

        a_rnd = Random.Range(0, m_zCommon.m_decTex.Length);                             //피 텍스트 여러개중 랜덤

        GameObject a_bloodDec = Instantiate(m_zCommon.m_bloodDec, a_vec, a_rot);
        a_bloodDec.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", m_zCommon.m_decTex[a_rnd]);         //마테리얼 텍스트 변경

        float a_scale = Random.Range(0.4f, 0.6f);                                       //스케일 조절
        a_bloodDec.transform.localScale = Vector3.one * a_scale;
        Destroy(a_bloodDec, 3.0f);      //3초뒤 삭제
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Terrain"))                   //처음에 좀비가 지형과 닿았음을 감지
        {
            m_collCheck = true;
        }
    }

    void ZombieAttack()
    {
        if(m_calcMag <= m_attackDist)       //좀비와 플레이어의 거리가 좀비의 공격거리보다 작다면...(좀비의 공격거리 안에 있을 경우)
        {
            m_aggroTarget.GetComponent<PlayerCtrl>().TakeDamage(10);
        }
    }
}

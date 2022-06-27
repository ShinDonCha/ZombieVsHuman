using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UI 항목에 접근하기 위해 반드시 추가
//using UnityEngine.SceneManagement;


[RequireComponent(typeof(Rigidbody))]   //자동으로 typeof("")  ""안에 있는 컴포넌트를 추가함.
public class PlayerCtrl : MonoBehaviour
{
    public static PlayerCtrl inst = null; 

    Rigidbody m_myRigidbody;          //플레이어 리지드바디

    //------- 플레이어 이동
    //x 좌표는 기본 수평 Input 값(h), z 좌표는 기본 수직 Input 값 (v)
    private float h = 0.0f;         // 기본 수평 Input 값
    private float v = 0.0f;         // 기본 수직 Input 값    
    private float m_moveSpeed = 0.0f;         //플레이어의 속도를 담을 변수
    private float m_normalSpeed = 5.0f;       //기본 속도
    private float m_runSpeed = 10.0f;         //뛸 때 속도
    private Vector3 m_moveInputV;     //스크립트로 캐릭터를 움직일때 쓰는 벡터값(앞뒤)                                           
    private Vector3 m_moveInputH;     //스크립트로 캐릭터를 움직일때 쓰는 벡터값(좌우) 
    private Vector3 m_nextMoveV;      //방향 벡터값에 움직이는 속도를 곱해줄 벡터값
    private Vector3 m_nextMoveH;      //방향 벡터값에 움직이는 속도를 곱해줄 벡터값
    //------- 플레이어 이동

    //------- 플레이어 애니메이션
    private float m_aniSpeed = 5.0f;  // 애니메이션 블렌더에 적용할 스프린트 / 달리기 변경에 적용되는 변수
    private float m_aniRot = 0.0f;   // 애니메이션 블렌더에 적용할 좌측 / 중간 / 우측 캐릭터 회전에 적용되는 변수        
    [HideInInspector] public Animator m_animController;    //플레이어가 사용하는 모델에 적용될 애니메이션 컨트롤러
    //------- 플레이어 애니메이션        

    //----- 플레이어 스텟
    float m_curHp = 0.0f;               //현재 체력
    float m_maxHp = 100.0f;             //최대 체력
    //----- 플레이어 스텟       

    //----- 플레이어 공격
    [HideInInspector] public float m_atkDelayTimer = 0.0f;       //공격딜레이 계산용 변수  
    //----- 플레이어 공격

    [HideInInspector] public bool m_isRun = true;       //움직일 수 있는 상태인지
    [HideInInspector] public bool m_isLoot = false;     //줍기 상태 인지     
    [HideInInspector] public bool m_getGun = false;     //현재 무기가 총기인지
    public Image m_hpBar;                               //hpbar 이미지
                                                
    [HideInInspector] public List<ItemInfo> m_itemList = new List<ItemInfo>();     //현재 플레이어의 충돌반경에 들어온 아이템 리스트    

    [HideInInspector] public WeaponCtrl m_nowWeapon = null;     //현재 플레이어가 소지하고있는 무기    

    [HideInInspector] public GameObject m_Test = null;

    // Start is called before the first frame update
    void Awake()
    {       
        inst = this;

        m_myRigidbody = GetComponent<Rigidbody>();

        m_animController = GetComponentInChildren<Animator>();        

        m_moveSpeed = m_normalSpeed;

        m_curHp = m_maxHp;        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Animation();
        Attack();
    }

    void Move()
    {
        v = Input.GetAxis("Vertical");
        h = Input.GetAxisRaw("Horizontal");

        if (m_isRun == false)               //줍기 동작 중 이동 불가
            return;

        if (h == 0 && v == 0 && m_animController.GetBool("Roll") == false) //입력 없을 시 아래 코드 동작 안함
            return;

        m_moveInputV = transform.forward * v;                   //회전상태에서의 정면값과 v을 곱해서 현재 바라보고있는 방향을 기준으로 움직이도록 함
        m_moveInputH = transform.right * h;                     //회전상태에서의 우측값과 h를 곱해서 현재 바라보고있는 방향을 기준으로 움직이도록 함

        m_nextMoveV = m_moveInputV.normalized * m_moveSpeed;       //방향 벡터값에 움직이는 속도를 곱해서 앞뒤로 이동할 거리를 구함
        m_nextMoveH = m_moveInputH.normalized * m_moveSpeed;       //방향 벡터값에 움직이는 속도를 곱해서 좌우로 이동할 거리를 구함

        //---------- 달리기
        if (Input.GetKey(KeyCode.LeftShift) && v > 0
            && m_nowWeapon.m_zoomInOut == false)    // 앞이나 뒤로 이동하고있고 줌인이 아니라면 LeftShift를 눌렀을때 속도 변경
        {           
            m_moveSpeed = m_runSpeed;
            if (m_aniSpeed < m_runSpeed)                     //Ani_Speed -> 블렌더에서 달리기 / 뛰기를 변경시켜줄 
                m_aniSpeed += Time.deltaTime * 7;
            else if (m_aniSpeed > m_runSpeed)
                m_aniSpeed = m_runSpeed;
        }
        //if (!Input.GetKey(KeyCode.LeftShift))
        else                                          //기존 상태일때, 속도값 조절  (기본 움직임)
        {            
            m_moveSpeed = m_normalSpeed;

            if (m_nowWeapon.m_zoomInOut == true)
            { 
                m_moveSpeed /= 2;                   //줌인 상태라면 이동속도가 절반
                m_animController.speed = 0.5f;      //애니메이션 재생속도 절반
            }
            else            
                m_animController.speed = 1.0f;      //애니메이션 재생속도 원래대로               

            if (m_aniSpeed > m_normalSpeed)
            {
                m_aniSpeed -= Time.deltaTime * 7;
                if (m_aniSpeed < m_normalSpeed)
                    m_aniSpeed = m_normalSpeed;
            }
        }
        //---------- 달리기

        if(m_animController.GetBool("Roll") == true)          //구르기 할 때
            m_myRigidbody.MovePosition(m_myRigidbody.position + transform.forward * m_moveSpeed * Time.deltaTime);        
        else                                                    //구르기 아닐 때
        {
            m_myRigidbody.MovePosition(m_myRigidbody.position + m_nextMoveV * Time.deltaTime);         //플레이어 이동
            m_myRigidbody.MovePosition(m_myRigidbody.position + m_nextMoveH * Time.deltaTime);         //플레이어 이동
        }
        
    }

    void Animation()
    {
        m_animController.SetBool("Get Gun", m_getGun);          //애니메이션 변경

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (m_itemList.Count <= 0)
                return;
            
            m_isLoot = !m_isLoot;

            m_animController.SetBool("isLoot", m_isLoot);       //줍기 애니메이션 제어

            GameObject.Find("UICanvas").GetComponent<CanvasCtrl>().DDPanelSet(m_isLoot);        //슬롯 세팅
        }        

        if (Input.GetKeyDown(KeyCode.Space))                //구르기 애니메이션 출력
            m_animController.SetBool("Roll", true);

        if (m_animController.GetCurrentAnimatorStateInfo(0).IsName("Roll"))          //구르기 애니메이션 끝날 때 파라미터 변경             
            if (1.0f < m_animController.GetCurrentAnimatorStateInfo(0).normalizedTime)
                m_animController.SetBool("Roll", false);

        if (m_animController.GetCurrentAnimatorStateInfo(0).IsName("Loot_off"))     //줍기 애니메이션 끝날 때 이동 가능하게 변경
            if (0.9f < m_animController.GetCurrentAnimatorStateInfo(0).normalizedTime)
                m_isRun = true;        
                

        //----- 회전 애니메이션        //이부분 수정?
        if (h > 0)
        {
            m_aniRot += Time.deltaTime * 30;
            if (m_aniRot > 10)
                m_aniRot = 10;
        }
        else if (h < 0)
        {
            m_aniRot -= Time.deltaTime * 30;
            if (m_aniRot < -10)
                m_aniRot = -10;
        }
        else    //좌우측으로의 이동이 없을 때
        {
            if (m_aniRot > 0.1)
                m_aniRot -= Time.deltaTime * 30;
            else if (m_aniRot < -0.1)
                m_aniRot += Time.deltaTime * 30;
            else
                m_aniRot = 0;
        }
        //----- 회전 애니메이션

        m_animController.SetFloat("Vertical", v * 10);    //애니메이션 컨트롤러에 변수값 전달
        m_animController.SetFloat("Horizontal", m_aniRot); //애니메이션 컨트롤러에 변수값 전달
        m_animController.SetFloat("Speed", m_aniSpeed);    //애니메이션 컨트롤러에 변수값 전달      
    }       

    void Attack()
    {
        //---- 공격 가능 상태인지 확인
        if (m_nowWeapon.m_crossCtrl.m_isReloading == true
            || m_animController.GetBool("Roll") == true
            || 5 < m_animController.GetFloat("Speed")
            || m_isRun == false)        //재장전상태나 구르기상태, 뛰기상태, 줍기상태일 경우 공격 불가
            m_nowWeapon.m_misFire = true;
        else
            m_nowWeapon.m_misFire = false;
        //---- 공격 가능 상태인지 확인

        //----- 공격
        if (0.0f < m_atkDelayTimer)
            m_atkDelayTimer -= Time.deltaTime;
        else if (m_atkDelayTimer <= 0.0f)
        {
            m_atkDelayTimer = 0.0f;
            if (Input.GetMouseButton(0) && m_nowWeapon.m_misFire == false)
            {                
                if (m_nowWeapon.m_weaponType == ItemName.Bat)         //무기가 야구방망이일 때
                    m_nowWeapon.Swing();
                else                                    //무기가 총기류일 때
                    m_nowWeapon.Fire();
            }
        }
        //----- 공격        
    }

    public void TakeDamage(float damage)        //얘는 좀비의 공격을 맞았을 때 동작
    {
        if (m_curHp < 0.0f)
            return;

        if (0.0f < m_curHp)
        {
            m_curHp -= damage;
            m_hpBar.fillAmount = m_curHp / m_maxHp;
            if (m_curHp < 0.0f)
            {
                m_curHp = 0.0f;
                //Die();
            }
        }       
    }

    
    private void OnTriggerEnter(Collider other)
    {
        // 1.물건과 관련된 UI 가 켜지게 만듦,
        // 2.특정키를 누르면 씬 상의 물건(프리팹)이 삭제되고 플레이어 인벤토리에 옮길수있게 만듦,
        // 3. 2번의 과정에서 씬상의 물건이름과 공용스크립트의 클래스와 비교하여 인벤토리에 옮기는 for문호출
        if (other.CompareTag("Item"))
        {
            for (int i = 0; i < m_itemList.Count; i++)
            {
                if (m_itemList[i] == other.GetComponent<ItemCtrl>().m_itemInfo)
                {                    
                    return;
                }
            }

            m_itemList.Add(other.GetComponent<ItemCtrl>().m_itemInfo);     //아이템 리스트에 추가
            
            //AddList(other.GetComponent<ItemCtrl>().m_itemInfo);
            m_Test = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 1. 물건과 관련된 UI가 꺼지게 만듦,
        // 2. 특정키가 작동안되게함.
        if (other.CompareTag("Item"))
        {            
            m_itemList.Remove(other.GetComponent<ItemCtrl>().m_itemInfo);  //아이템 리스트에서 삭제   
        }
    }        
    
    void AddList(ItemInfo a_itemInfo)
    {
        m_itemList.Add(a_itemInfo);
    }
}

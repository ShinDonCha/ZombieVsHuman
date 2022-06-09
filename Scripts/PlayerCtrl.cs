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
    
    [HideInInspector] public bool m_isRun = true;       //움직일 수 있는 상태인지
    [HideInInspector] public bool m_isLoot = false;     //줍기 상태 인지     
    public Image m_hpBar;                               //hpbar 이미지
                                                
    [HideInInspector] public List<ItemCtrl> m_itemList = new List<ItemCtrl>();     //현재 플레이어의 충돌반경에 들어온 아이템 리스트

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
        if (Input.GetKey(KeyCode.LeftShift) && v > 0)    // 앞을 보는상태에서 LeftShift를 누르면 속도가 바뀌게 조절 (달리는 중)
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
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (m_itemList.Count <= 0)
                return;

            m_isLoot = !m_isLoot;

            m_animController.SetBool("isLoot", m_isLoot);       //줍기 애니메이션 제어

            GameObject.Find("UICanvas").GetComponent<CanvasCtrl>().InvenPanel();
        }

        if (Input.GetKeyDown(KeyCode.Space))                //구르기 애니메이션 출력
            m_animController.SetBool("Roll", true);

        if (m_animController.GetCurrentAnimatorStateInfo(0).IsName("Roll"))          //구르기 애니메이션 끝날 때 파라미터 변경             
            if (1.0f < m_animController.GetCurrentAnimatorStateInfo(0).normalizedTime)
                m_animController.SetBool("Roll", false);

        if (m_animController.GetCurrentAnimatorStateInfo(0).IsName("Loot_off"))     //줍기 애니메이션 끝날 때 이동 가능하게 변경
            if (0.9f < m_animController.GetCurrentAnimatorStateInfo(0).normalizedTime)
                m_isRun = true;       

        //----- 회전 애니메이션
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
            m_itemList.Add(other.GetComponent<ItemCtrl>());     //아이템 리스트에 추가
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 1. 물건과 관련된 UI가 꺼지게 만듦,
        // 2. 특정키가 작동안되게함.
        if (other.CompareTag("Item"))
        {
            m_itemList.Remove(other.GetComponent<ItemCtrl>());  //아이템 리스트에서 삭제   
        }
    }           
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UI �׸� �����ϱ� ���� �ݵ�� �߰�
//using UnityEngine.SceneManagement;


[RequireComponent(typeof(Rigidbody))]   //�ڵ����� typeof("")  ""�ȿ� �ִ� ������Ʈ�� �߰���.
public class PlayerCtrl : MonoBehaviour
{
    public static PlayerCtrl inst = null; 

    Rigidbody m_myRigidbody;          //�÷��̾� ������ٵ�

    //------- �÷��̾� �̵�
    //x ��ǥ�� �⺻ ���� Input ��(h), z ��ǥ�� �⺻ ���� Input �� (v)
    private float h = 0.0f;         // �⺻ ���� Input ��
    private float v = 0.0f;         // �⺻ ���� Input ��    
    private float m_moveSpeed = 0.0f;         //�÷��̾��� �ӵ��� ���� ����
    private float m_normalSpeed = 5.0f;       //�⺻ �ӵ�
    private float m_runSpeed = 10.0f;         //�� �� �ӵ�
    private Vector3 m_moveInputV;     //��ũ��Ʈ�� ĳ���͸� �����϶� ���� ���Ͱ�(�յ�)                                           
    private Vector3 m_moveInputH;     //��ũ��Ʈ�� ĳ���͸� �����϶� ���� ���Ͱ�(�¿�) 
    private Vector3 m_nextMoveV;      //���� ���Ͱ��� �����̴� �ӵ��� ������ ���Ͱ�
    private Vector3 m_nextMoveH;      //���� ���Ͱ��� �����̴� �ӵ��� ������ ���Ͱ�
    //------- �÷��̾� �̵�

    //------- �÷��̾� �ִϸ��̼�
    private float m_aniSpeed = 5.0f;  // �ִϸ��̼� ������ ������ ������Ʈ / �޸��� ���濡 ����Ǵ� ����
    private float m_aniRot = 0.0f;   // �ִϸ��̼� ������ ������ ���� / �߰� / ���� ĳ���� ȸ���� ����Ǵ� ����        
    [HideInInspector] public Animator m_animController;    //�÷��̾ ����ϴ� �𵨿� ����� �ִϸ��̼� ��Ʈ�ѷ�
    //------- �÷��̾� �ִϸ��̼�        

    //----- �÷��̾� ����
    float m_curHp = 0.0f;               //���� ü��
    float m_maxHp = 100.0f;             //�ִ� ü��
    //----- �÷��̾� ����       
    
    [HideInInspector] public bool m_isRun = true;       //������ �� �ִ� ��������
    [HideInInspector] public bool m_isLoot = false;     //�ݱ� ���� ����     
    public Image m_hpBar;                               //hpbar �̹���
                                                
    [HideInInspector] public List<ItemCtrl> m_itemList = new List<ItemCtrl>();     //���� �÷��̾��� �浹�ݰ濡 ���� ������ ����Ʈ

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

        if (m_isRun == false)               //�ݱ� ���� �� �̵� �Ұ�
            return;

        if (h == 0 && v == 0 && m_animController.GetBool("Roll") == false) //�Է� ���� �� �Ʒ� �ڵ� ���� ����
            return;

        m_moveInputV = transform.forward * v;                   //ȸ�����¿����� ���鰪�� v�� ���ؼ� ���� �ٶ󺸰��ִ� ������ �������� �����̵��� ��
        m_moveInputH = transform.right * h;                     //ȸ�����¿����� �������� h�� ���ؼ� ���� �ٶ󺸰��ִ� ������ �������� �����̵��� ��

        m_nextMoveV = m_moveInputV.normalized * m_moveSpeed;       //���� ���Ͱ��� �����̴� �ӵ��� ���ؼ� �յڷ� �̵��� �Ÿ��� ����
        m_nextMoveH = m_moveInputH.normalized * m_moveSpeed;       //���� ���Ͱ��� �����̴� �ӵ��� ���ؼ� �¿�� �̵��� �Ÿ��� ����

        //---------- �޸���
        if (Input.GetKey(KeyCode.LeftShift) && v > 0)    // ���� ���»��¿��� LeftShift�� ������ �ӵ��� �ٲ�� ���� (�޸��� ��)
        {           
            m_moveSpeed = m_runSpeed;
            if (m_aniSpeed < m_runSpeed)                     //Ani_Speed -> �������� �޸��� / �ٱ⸦ ��������� 
                m_aniSpeed += Time.deltaTime * 7;
            else if (m_aniSpeed > m_runSpeed)
                m_aniSpeed = m_runSpeed;
        }
        //if (!Input.GetKey(KeyCode.LeftShift))
        else                                          //���� �����϶�, �ӵ��� ����  (�⺻ ������)
        {            
            m_moveSpeed = m_normalSpeed;
            if (m_aniSpeed > m_normalSpeed)
            {
                m_aniSpeed -= Time.deltaTime * 7;
                if (m_aniSpeed < m_normalSpeed)
                    m_aniSpeed = m_normalSpeed;
            }
        }
        //---------- �޸���

        if(m_animController.GetBool("Roll") == true)          //������ �� ��
            m_myRigidbody.MovePosition(m_myRigidbody.position + transform.forward * m_moveSpeed * Time.deltaTime);        
        else                                                    //������ �ƴ� ��
        {
            m_myRigidbody.MovePosition(m_myRigidbody.position + m_nextMoveV * Time.deltaTime);         //�÷��̾� �̵�
            m_myRigidbody.MovePosition(m_myRigidbody.position + m_nextMoveH * Time.deltaTime);         //�÷��̾� �̵�
        }
        
    }

    void Animation()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (m_itemList.Count <= 0)
                return;

            m_isLoot = !m_isLoot;

            m_animController.SetBool("isLoot", m_isLoot);       //�ݱ� �ִϸ��̼� ����

            GameObject.Find("UICanvas").GetComponent<CanvasCtrl>().InvenPanel();
        }

        if (Input.GetKeyDown(KeyCode.Space))                //������ �ִϸ��̼� ���
            m_animController.SetBool("Roll", true);

        if (m_animController.GetCurrentAnimatorStateInfo(0).IsName("Roll"))          //������ �ִϸ��̼� ���� �� �Ķ���� ����             
            if (1.0f < m_animController.GetCurrentAnimatorStateInfo(0).normalizedTime)
                m_animController.SetBool("Roll", false);

        if (m_animController.GetCurrentAnimatorStateInfo(0).IsName("Loot_off"))     //�ݱ� �ִϸ��̼� ���� �� �̵� �����ϰ� ����
            if (0.9f < m_animController.GetCurrentAnimatorStateInfo(0).normalizedTime)
                m_isRun = true;       

        //----- ȸ�� �ִϸ��̼�
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
        else    //�¿��������� �̵��� ���� ��
        {
            if (m_aniRot > 0.1)
                m_aniRot -= Time.deltaTime * 30;
            else if (m_aniRot < -0.1)
                m_aniRot += Time.deltaTime * 30;
            else
                m_aniRot = 0;
        }
        //----- ȸ�� �ִϸ��̼�

        m_animController.SetFloat("Vertical", v * 10);    //�ִϸ��̼� ��Ʈ�ѷ��� ������ ����
        m_animController.SetFloat("Horizontal", m_aniRot); //�ִϸ��̼� ��Ʈ�ѷ��� ������ ����
        m_animController.SetFloat("Speed", m_aniSpeed);    //�ִϸ��̼� ��Ʈ�ѷ��� ������ ����      
    }       

    public void TakeDamage(float damage)        //��� ������ ������ �¾��� �� ����
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
        // 1.���ǰ� ���õ� UI �� ������ ����,
        // 2.Ư��Ű�� ������ �� ���� ����(������)�� �����ǰ� �÷��̾� �κ��丮�� �ű���ְ� ����,
        // 3. 2���� �������� ������ �����̸��� ���뽺ũ��Ʈ�� Ŭ������ ���Ͽ� �κ��丮�� �ű�� for��ȣ��
        if (other.CompareTag("Item"))
        {
            m_itemList.Add(other.GetComponent<ItemCtrl>());     //������ ����Ʈ�� �߰�
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 1. ���ǰ� ���õ� UI�� ������ ����,
        // 2. Ư��Ű�� �۵��ȵǰ���.
        if (other.CompareTag("Item"))
        {
            m_itemList.Remove(other.GetComponent<ItemCtrl>());  //������ ����Ʈ���� ����   
        }
    }           
}

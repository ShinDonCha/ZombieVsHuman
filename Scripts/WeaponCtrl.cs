using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TooManyCrosshairs;

public class WeaponCtrl : MonoBehaviour
{                       
    [HideInInspector] public GameObject m_crossHair = null;       //instantiate�� ũ�ν���� ���� ����
    private CrosshairCtrl m_crossCtrl = null;       //UICanvas�� CrossHair�� ���� CrossHairCtrl��ũ��Ʈ�� ���� ����

    //---- ���������� �ʿ��� �κ�
    [HideInInspector] public ItemType m_weaponType;                
    
    private GameObject m_UICanvas = null;   //UICanvas ���� ����

    //----- �������� ���� �� �ʿ��� ����
    public GameObject m_bulletObj = null;       //�Ѿ� ���ҽ� ���� ����
    public MeshRenderer m_muzzleFlash = null;   //�ѱ� �Һ� ����Ʈ�� Meshrenderer
    public Transform m_firePos = null;          //�ѱ� transform ���� ����
    private bool m_zoomInOut = false;           //true�� �� ����
    //----- �������� ���� �� �ʿ��� ����

    private bool m_misFire = false;               //���� �Ұ��� ����    
    private float m_attackDelay = 0.0f;           //���� ���ݱ��� ������
    private float m_shootTimer = 0.0f;            //���ݵ����� ���� ����     

    private RaycastHit m_hitInfo;               //������ ���� ���
    Vector3 m_targetPos = Vector3.zero;         //Ÿ������ ��� ����
    //---- ���������� �ʿ��� �κ�

    // Start is called before the first frame update
    void Start()
    {
        //----- � ���������� ���� Ÿ�� �ٲ��ֱ�
        if (this.gameObject.name.Contains("K2c1"))
            m_weaponType = ItemType.K2;
        else if (this.gameObject.name.Contains("M16"))
            m_weaponType = ItemType.M16;
        else if (this.gameObject.name.Contains("Bat"))
            m_weaponType = ItemType.Bat;
        //----- � ���������� ���� Ÿ�� �ٲ��ֱ�

        m_attackDelay = GlobalValue.g_itemDic[m_weaponType].m_attackDelay;          //������ ���� ������ ����

        if (m_muzzleFlash != null)                       //�߱�������� ��� muzzleFlash ����
            m_muzzleFlash.enabled = false;              //ó���� �ѱ� �Һ� ����Ʈ ���ֱ�        

        m_UICanvas = GameObject.Find("UICanvas");                     //Hierarchy�� Canvas ã�ƿ���
        m_crossHair = m_UICanvas.transform.GetChild(0).gameObject;    //UICanvas�� CrossHair������Ʈ ã�ƿ���   
        m_crossCtrl = m_crossHair.GetComponent<CrosshairCtrl>();      //CrossHair������Ʈ�� ��ũ��Ʈ ��������        
        m_crossCtrl.SetShrinkSpeed(GlobalValue.g_itemDic[m_weaponType].m_shrinkSpeed);  //ũ�ν���� expanding�� scale ���� �ӵ� ����
        m_crossCtrl.SetReloadSpeed(0.1f);                                               //������ �ӵ� ����        
        m_crossCtrl.m_expandSize = GlobalValue.g_itemDic[m_weaponType].m_expandScale;   //�ߴ� �����ϴ� expanding�� scale ������
        
    }

    // Update is called once per frame
    void Update()
    {
        //---- ���� ���� �������� Ȯ��
        if (m_crossCtrl.m_isReloading == true
            || PlayerCtrl.inst.m_animController.GetBool("Roll") == true
            || 5 < PlayerCtrl.inst.m_animController.GetFloat("Speed")
            || PlayerCtrl.inst.m_isRun == false)        //���������³� ���������, �ٱ����, �ݱ������ ��� ���� �Ұ�
            m_misFire = true;
        else
            m_misFire = false;
        //---- ���� ���� �������� Ȯ��

        //----- ����
        if (0.0f < m_shootTimer)
            m_shootTimer -= Time.deltaTime;
        else if (m_shootTimer <= 0.0f)
        {
            m_shootTimer = 0.0f;
            if (Input.GetMouseButton(0) && m_misFire == false)
            {
                if (m_weaponType == ItemType.Bat)         //���Ⱑ �߱�������� ��
                    Swing();
                else                                    //���Ⱑ �ѱ���� ��
                    Fire();
            }
        }
        //----- ����        

        if (m_weaponType != ItemType.Bat)         //���Ⱑ ���� ��츸...
        {
            CrossHairCon();

            if (Input.GetMouseButtonDown(1))
                m_zoomInOut = !m_zoomInOut;
        }
    }

    void Swing()        //�߱�����̷� ������ ��
    {

    }

    void Fire()         //�ѱ���� ������ �� 
    {
        //ī�޶��� �������κ��� 10.0f�Ÿ��� ������ �´� ����� ������� �� ����� Ÿ�������� ����
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out m_hitInfo, 10.0f))  
            m_targetPos = m_hitInfo.point;

        //���� ��� 10.0f��ġ�� Ÿ�������� ����
        else
            m_targetPos = Camera.main.transform.position + (Camera.main.transform.forward * 10.0f);        
        
        float a_RndX = 0.0f;            //�Ѿ��� ������ ������ ������ ����
        float a_RndY = 0.0f;            //�Ѿ��� ������ ������ ������ ����

        float a_RndRange = (m_crossCtrl.m_expanding.transform.localScale.x - 1) * 6; //���� ũ�ν������ �þ scale�� * 6
        a_RndX = (int)Random.Range(-a_RndRange, a_RndRange);  //�þ scale�� * 6�� ����
        a_RndY = (int)Random.Range(-a_RndRange, a_RndRange);  //�þ scale�� * 6�� ����      

        GameObject a_bullet = Instantiate(m_bulletObj, m_firePos.position, Quaternion.identity);   //�ѱ���ġ�� �Ѿ� ����
        a_bullet.transform.LookAt(m_targetPos);     //�Ѿ��� Ÿ���� �ٶ󺸵��� ����        
        Vector3 a_rot = a_bullet.transform.rotation.eulerAngles;        //���� �Ѿ��� ȸ���� ��������
        a_rot.x += a_RndX;                                              //�Ѿ��� ȸ������ ������ ���� ���ϱ�
        a_rot.y += a_RndY;                                              //�Ѿ��� ȸ������ ������ ���� ���ϱ�
        a_rot.z = 0.0f;
        a_bullet.transform.rotation = Quaternion.Euler(a_rot);          //�Ѿ� ���� ����

        m_crossCtrl.ExpandCrosshair();                       //ũ�ν������ expanding Ȯ��
        StartCoroutine(ShowMuzzleFlash());                   //�ѱ� �Һ� ����Ʈ ���
        m_shootTimer = m_attackDelay;                        //���� ���ݱ����� ������ ����
    }

    IEnumerator ShowMuzzleFlash()
    {
        //MuzzleFlash�� Z���� �������� �ұ�Ģ�ϰ� ȸ��
        Quaternion rot = Quaternion.Euler(-90, 0, Random.Range(0, 360));
        m_muzzleFlash.transform.localRotation = rot;

        //Ȱ��ȭ�ؼ� ���̰� ��
        m_muzzleFlash.enabled = true;

        //�ұ�Ģ���� �ð� ���� Delay�� ���� MeshRenderer�� ��Ȱ��ȭ
        yield return new WaitForSeconds(Random.Range(0.01f, 0.03f));

        //��Ȱ��ȭ�ؼ� ������ �ʰ� ��
        m_muzzleFlash.enabled = false;
    }

    void CrossHairCon()
    {       
        if (m_zoomInOut == true)             //���� ����        
            m_crossCtrl.SetMaxScale(1.3f);   //ũ�ν���� expanding�� �ִ� scale ����

        else                                //�ܾƿ� ����        
            m_crossCtrl.SetMaxScale(GlobalValue.g_itemDic[m_weaponType].m_maxScale); //ũ�ν���� expanding�� �ִ� scale ����

        if (Input.GetKeyDown(KeyCode.R))     //������        
            m_crossCtrl.DoReload();

        if (PlayerCtrl.inst.m_animController.GetBool("Roll") == true)            //�÷��̾ �����⸦ �ϸ�
        {
            m_crossCtrl.m_expanding.rectTransform.localScale = m_crossCtrl.m_crosshairMaxScale;      //expanding�� scale�� maxScale ������
            StartCoroutine(m_crossCtrl.ShrinkCrosshair());              //expanding�� scale ���� �ڷ�ƾ ����
            StopCoroutine(m_crossCtrl.ShrinkCrosshair());               //expanding�� scale ���� �ڷ�ƾ ����

        }
        //�� �� �̵��ӵ��� �÷��̾� ��Ʈ�ѽ�ũ��Ʈ ������ ������ ����.
        //else if (5 < PlayerCtrl.inst.m_animController.GetFloat("Speed") &&
        //        m_crossCtrl.m_expanding.rectTransform.localScale.x < m_crossCtrl.m_crosshairMaxScale.x * 1.2f)  //�÷��̾ �ٰ� �ִٸ�
        //{
        //    m_crossCtrl.m_expanding.rectTransform.localScale = m_crossCtrl.m_crosshairMaxScale * 1.2f;      //expanding�� scale�� 1.3����          
        //    StartCoroutine(m_crossCtrl.ShrinkCrosshair());              //expanding�� scale ���� �ڷ�ƾ ����
        //    StopCoroutine(m_crossCtrl.ShrinkCrosshair());               //expanding�� scale ���� �ڷ�ƾ ����
        //}
    }
}

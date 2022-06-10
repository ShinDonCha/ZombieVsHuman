using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TooManyCrosshairs;

public class WeaponCtrl : MonoBehaviour
{
    [HideInInspector] public GameObject m_crossHair = null;       //instantiate한 크로스헤어 담을 변수
    private CrosshairCtrl m_crossCtrl = null;       //UICanvas의 CrossHair에 붙은 CrossHairCtrl스크립트를 담을 변수

    //---- 공통적으로 필요한 부분
    [HideInInspector] public ItemType m_weaponType;

    private GameObject m_UICanvas = null;   //UICanvas 담을 변수

    //----- 아이템이 총일 때 필요한 변수
    public GameObject m_bulletObj = null;       //총알 리소스 담을 변수
    public MeshRenderer m_muzzleFlash = null;   //총구 불빛 이펙트의 Meshrenderer
    public Transform m_firePos = null;          //총구 transform 담을 변수
    [HideInInspector] public bool m_zoomInOut = false;           //true일 때 줌인
    //----- 아이템이 총일 때 필요한 변수

    private bool m_misFire = false;               //공격 불가능 상태    
    private float m_attackDelay = 0.0f;           //다음 공격까지 딜레이
    private float m_shootTimer = 0.0f;            //공격딜레이 계산용 변수     

    private RaycastHit m_hitInfo;               //광선에 맞은 대상
    Vector3 m_targetPos = Vector3.zero;         //타격점을 담는 변수
    //---- 공통적으로 필요한 부분

    void Awake()
    {
        PlayerCtrl.inst.m_nowWeapon = this;           //현재 이 무기가 플레이어가 착용한 무기임
    }

    // Start is called before the first frame update
    void Start()
    {
        //----- 어떤 무기인지에 따라서 타입 바꿔주기
        if (this.gameObject.name.Contains("K2c1"))
            m_weaponType = ItemType.K2;
        else if (this.gameObject.name.Contains("M16"))
            m_weaponType = ItemType.M16;
        else if (this.gameObject.name.Contains("Bat"))
            m_weaponType = ItemType.Bat;
        //----- 어떤 무기인지에 따라서 타입 바꿔주기        

        m_attackDelay = GlobalValue.g_itemDic[m_weaponType].m_attackDelay;          //무기의 공격 딜레이 설정

        if (m_muzzleFlash != null)                       //야구방망이일 경우 muzzleFlash 없음
            m_muzzleFlash.enabled = false;              //처음에 총구 불빛 이펙트 꺼주기        

        m_UICanvas = GameObject.Find("UICanvas");                     //Hierarchy의 Canvas 찾아오기
        m_crossHair = m_UICanvas.transform.GetChild(0).gameObject;    //UICanvas의 CrossHair오브젝트 찾아오기   
        m_crossCtrl = m_crossHair.GetComponent<CrosshairCtrl>();      //CrossHair오브젝트의 스크립트 가져오기        
        m_crossCtrl.SetShrinkSpeed(GlobalValue.g_itemDic[m_weaponType].m_shrinkSpeed);  //크로스헤어 expanding의 scale 감소 속도 설정
        m_crossCtrl.SetReloadSpeed(0.1f);                                               //재장전 속도 설정        
        m_crossCtrl.m_expandSize = GlobalValue.g_itemDic[m_weaponType].m_expandScale;   //발당 증가하는 expanding의 scale 사이즈

    }

    // Update is called once per frame
    void Update()
    {
        //---- 공격 가능 상태인지 확인
        if (m_crossCtrl.m_isReloading == true
            || PlayerCtrl.inst.m_animController.GetBool("Roll") == true
            || 5 < PlayerCtrl.inst.m_animController.GetFloat("Speed")
            || PlayerCtrl.inst.m_isRun == false
            || PlayerCtrl.inst.m_isLoot == true)        //재장전상태나 구르기상태, 뛰기상태, 줍기상태일 경우 공격 불가 ( 줍기상태일때 코드추가 )
        {
            m_misFire = true;
            
        }
        else
        {
            m_misFire = false;
            m_crossHair.SetActive(true);
        }
            //---- 공격 가능 상태인지 확인

        //----- 공격
        if (0.0f < m_shootTimer)
            m_shootTimer -= Time.deltaTime;
        else if (m_shootTimer <= 0.0f)
        {
            m_shootTimer = 0.0f;
            if (Input.GetMouseButton(0) && m_misFire == false)
            {
                if (m_weaponType == ItemType.Bat)         //무기가 야구방망이일 때
                    Swing();
                else                                    //무기가 총기류일 때
                    Fire();
            }
        }
        //----- 공격        

        if (m_weaponType != ItemType.Bat)         //무기가 총일 경우만...
        {
            CrossHairCon();

            if (Input.GetMouseButtonDown(1))
                m_zoomInOut = !m_zoomInOut;
        }
    }

    void Swing()        //야구방망이로 공격할 때
    {

    }

    void Fire()         //총기류로 공격할 때 
    {
        //카메라의 정면으로부터 10.0f거리에 광선에 맞는 대상이 있을경우 그 대상을 타격점으로 잡음
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out m_hitInfo, 10.0f))
            m_targetPos = m_hitInfo.point;

        //없을 경우 10.0f위치를 타격점으로 잡음
        else
            m_targetPos = Camera.main.transform.position + (Camera.main.transform.forward * 10.0f);

        float a_RndX = 0.0f;            //총알이 빗나갈 각도를 저장할 변수
        float a_RndY = 0.0f;            //총알이 빗나갈 각도를 저장할 변수

        float a_RndRange = (m_crossCtrl.m_expanding.transform.localScale.x - 1) * 6; //현재 크로스헤어의 늘어난 scale값 * 6
        a_RndX = (int)Random.Range(-a_RndRange, a_RndRange);  //늘어난 scale값 * 6의 범위
        a_RndY = (int)Random.Range(-a_RndRange, a_RndRange);  //늘어난 scale값 * 6의 범위      

        GameObject a_bullet = Instantiate(m_bulletObj, m_firePos.position, Quaternion.identity);   //총구위치에 총알 생성
        a_bullet.transform.LookAt(m_targetPos);     //총알이 타겟을 바라보도록 설정        
        Vector3 a_rot = a_bullet.transform.rotation.eulerAngles;        //현재 총알의 회전값 가져오기
        a_rot.x += a_RndX;                                              //총알의 회전값에 빗나갈 각도 더하기
        a_rot.y += a_RndY;                                              //총알의 회전값에 빗나갈 각도 더하기
        a_rot.z = 0.0f;
        a_bullet.transform.rotation = Quaternion.Euler(a_rot);          //총알 각도 변경

        m_crossCtrl.ExpandCrosshair();                       //크로스헤어의 expanding 확장
        StartCoroutine(ShowMuzzleFlash());                   //총구 불빛 이펙트 출력
        m_shootTimer = m_attackDelay;                        //다음 공격까지의 딜레이 설정
    }

    IEnumerator ShowMuzzleFlash()
    {
        //MuzzleFlash를 Z축을 기준으로 불규칙하게 회전
        Quaternion rot = Quaternion.Euler(-90, 0, Random.Range(0, 360));
        m_muzzleFlash.transform.localRotation = rot;

        //활성화해서 보이게 함
        m_muzzleFlash.enabled = true;

        //불규칙적인 시간 동안 Delay한 다음 MeshRenderer를 비활성화
        yield return new WaitForSeconds(Random.Range(0.01f, 0.03f));

        //비활성화해서 보이지 않게 함
        m_muzzleFlash.enabled = false;
    }

    void CrossHairCon()
    {
        if (m_zoomInOut == true)             //줌인 상태        
            m_crossCtrl.SetMaxScale(1.3f);   //크로스헤어 expanding의 최대 scale 설정

        else                                //줌아웃 상태        
            m_crossCtrl.SetMaxScale(GlobalValue.g_itemDic[m_weaponType].m_maxScale); //크로스헤어 expanding의 최대 scale 설정

        if (Input.GetKeyDown(KeyCode.R))     //재장전        
            m_crossCtrl.DoReload();

        if (PlayerCtrl.inst.m_animController.GetBool("Roll") == true)            //플레이어가 구르기를 하면
        {
            m_crossCtrl.m_expanding.rectTransform.localScale = m_crossCtrl.m_crosshairMaxScale;      //expanding의 scale을 maxScale 값으로
            StartCoroutine(m_crossCtrl.ShrinkCrosshair());              //expanding의 scale 감소 코루틴 시작
            StopCoroutine(m_crossCtrl.ShrinkCrosshair());               //expanding의 scale 감소 코루틴 정지

        }
        //뛸 때 이동속도도 플레이어 컨트롤스크립트 변수를 가져와 수정.
        //else if (5 < PlayerCtrl.inst.m_animController.GetFloat("Speed") &&
        //        m_crossCtrl.m_expanding.rectTransform.localScale.x < m_crossCtrl.m_crosshairMaxScale.x * 1.2f)  //플레이어가 뛰고 있다면
        //{
        //    m_crossCtrl.m_expanding.rectTransform.localScale = m_crossCtrl.m_crosshairMaxScale * 1.2f;      //expanding의 scale을 1.3으로          
        //    StartCoroutine(m_crossCtrl.ShrinkCrosshair());              //expanding의 scale 감소 코루틴 시작
        //    StopCoroutine(m_crossCtrl.ShrinkCrosshair());               //expanding의 scale 감소 코루틴 정지
        //}
    }
}

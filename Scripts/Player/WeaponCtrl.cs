﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCtrl : MonoBehaviour
{
    [HideInInspector] public ItemInfo m_itemInfo = null;

    public CrosshairCtrl m_crossCtrl = null;       //UICanvas의 CrossHair에 붙은 CrossHairCtrl스크립트를 담을 변수
    
    //----- 아이템이 총일 때 필요한 변수
    public GameObject m_bulletObj = null;       //총알 리소스 담을 변수
    public MeshRenderer m_muzzleFlash = null;   //총구 불빛 이펙트의 Meshrenderer
    public Transform m_firePos = null;          //총구 transform 담을 변수
    //----- 아이템이 총일 때 필요한 변수

    //----- 아이템이 배트나 주먹일 때 필요한 변수
    [HideInInspector] public bool m_closeAtk = false;
    //----- 아이템이 배트나 주먹일 때 필요한 변수

    [HideInInspector] public bool m_misFire = false;               //공격 불가능 상태

    private RaycastHit m_hitInfo;               //광선에 맞은 대상
    Vector3 m_targetPos = Vector3.zero;         //타격점을 담는 변수    

    // Start is called before the first frame update
    void Start()
    {              
    }

    // Update is called once per frame
    void Update()
    {
        if (InGameMgr.s_gameState != GameState.GameIng)
            return;

        if (PlayerCtrl.inst.m_isRun == false)
            return;

        if (m_itemInfo.m_itName == ItemName.Bat || m_itemInfo.m_itName == ItemName.Kick)  //무기가 야구배트와 맨손일 경우
        {
            if (m_crossCtrl.m_border.activeSelf == true)
                m_crossCtrl.m_border.SetActive(false);
        }
        else            //무기가 총기류일 경우
        {
            if (m_crossCtrl.m_border.activeSelf == false)
                m_crossCtrl.m_border.SetActive(true);

            if (m_itemInfo.m_curMagazine == 0 && 0 < m_itemInfo.m_maxMagazine
                && m_crossCtrl.m_isReloading == false)                           //총알을 다썼다면 자동으로 재장전
                m_crossCtrl.DoReload();

            m_crossCtrl.CrossHairCon();
                        
            if (Input.GetMouseButtonDown(1))
                m_crossCtrl.m_zoomInOut = !m_crossCtrl.m_zoomInOut;
        }
    }

    public void Init()
    {
        PlayerCtrl.inst.m_nowWeapon = this;           //현재 이 무기가 플레이어가 착용한 무기임

        //m_fireAudio.clip = m_itemInfo.m_audioClip;

        if (m_itemInfo.m_itName == ItemName.Bat || m_itemInfo.m_itName == ItemName.Kick)        //아이템이 야구배트나 기본무기일경우
            PlayerCtrl.inst.m_getGun = false;
        else
            PlayerCtrl.inst.m_getGun = true;        

        //m_attackDelay = m_itemInfo.m_attackDelay;          //무기의 공격 딜레이 설정

        if (m_muzzleFlash != null)                       //야구방망이일 경우 muzzleFlash 없음
            m_muzzleFlash.enabled = false;              //처음에 총구 불빛 이펙트 꺼주기

        m_crossCtrl.m_itemInfo = m_itemInfo;            //현재무기의 정보 연동
        m_crossCtrl.SetShrinkSpeed(m_itemInfo.m_shrinkSpeed);  //크로스헤어 expanding의 scale 감소 속도 설정
        m_crossCtrl.SetReloadSpeed(0.15f);                      //재장전 속도 설정        
        m_crossCtrl.m_expandSize = m_itemInfo.m_expandScale;   //발당 증가하는 expanding의 scale 사이즈
    }

    //public void Swing()        //야구방망이로 공격할 때
    //{        
    //    PlayerCtrl.inst.m_animController.SetTrigger("Swing");
    //    //PlayerCtrl.inst.m_atkDelayTimer = m_attackDelay;
    //}

    public void Fire()         //총기류로 공격할 때 
    {
        if (0 < m_itemInfo.m_curMagazine)           //총알이 있다면
            m_itemInfo.m_curMagazine -= 1;             //한발 감소
        else          
            return;

        PlayerCtrl.inst.m_animController.SetTrigger("Fire");
        SoundMgr.inst.m_audioSource.Play();

        //카메라의 정면으로부터 10.0f거리에 광선에 맞는 대상이 있을경우 그 대상을 타격점으로 잡음
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out m_hitInfo, 10.0f, 128))
        {
            m_targetPos = m_hitInfo.point;
        }

        //없을 경우 10.0f위치를 타격점으로 잡음
        else
            m_targetPos = Camera.main.transform.position + (Camera.main.transform.forward * 10.0f);        
        
        float a_RndX = 0.0f;            //총알이 빗나갈 각도를 저장할 변수
        float a_RndY = 0.0f;            //총알이 빗나갈 각도를 저장할 변수

        float a_RndRange = (m_crossCtrl.m_curExpanding.transform.localScale.x - 1) * 6; //현재 크로스헤어의 늘어난 scale값 * 6
        a_RndX = (int)Random.Range(-a_RndRange, a_RndRange);  //늘어난 scale값 * 6의 범위
        a_RndY = (int)Random.Range(-a_RndRange, a_RndRange);  //늘어난 scale값 * 6의 범위      

        GameObject a_bullet = Instantiate(m_bulletObj, m_firePos.position, Quaternion.identity);   //총구위치에 총알 생성
        a_bullet.GetComponent<BulletCtrl>().m_bulletDmg = m_itemInfo.m_damage;      //총알의 대미지 설정
        a_bullet.transform.LookAt(m_targetPos);     //총알이 타겟을 바라보도록 설정        
        Vector3 a_rot = a_bullet.transform.rotation.eulerAngles;        //현재 총알의 회전값 가져오기
        a_rot.x += a_RndX;                                              //총알의 회전값에 빗나갈 각도 더하기
        a_rot.y += a_RndY;                                              //총알의 회전값에 빗나갈 각도 더하기
        a_rot.z = 0.0f;
        a_bullet.transform.rotation = Quaternion.Euler(a_rot);          //총알 각도 변경

        if (m_crossCtrl.m_zoomInOut == true && m_itemInfo.m_itName == ItemName.SniperRifle)
            return;

        m_crossCtrl.ExpandCrosshair();                       //크로스헤어의 expanding 확장

        if(m_muzzleFlash != null)
            StartCoroutine(ShowMuzzleFlash());                   //총구 불빛 이펙트 출력        
    }

    IEnumerator ShowMuzzleFlash()
    {
        //MuzzleFlash를 Z축을 기준으로 불규칙하게 회전
        Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0, 360));
        m_muzzleFlash.transform.localRotation = rot;

        //활성화해서 보이게 함
        m_muzzleFlash.enabled = true;

        //불규칙적인 시간 동안 Delay한 다음 MeshRenderer를 비활성화
        yield return new WaitForSeconds(Random.Range(0.01f, 0.03f));

        //비활성화해서 보이지 않게 함
        m_muzzleFlash.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Contains("Zombie") && m_closeAtk == true)
        {
            SoundMgr.inst.m_audioSource.Play();
            ZombieCtrl a_ZCtrl = other.GetComponent<ZombieCtrl>();
            a_ZCtrl.TakeDamage(transform.position, m_itemInfo.m_damage, a_ZCtrl.m_attackDist / 2.0f);      //좀비의 공격거리의 절반만큼 밀림
        }
    }
}

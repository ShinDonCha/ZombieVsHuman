using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CrosshairCtrl : MonoBehaviour
{
    [HideInInspector] public ItemInfo m_itemInfo = null;        //현재 적용된 무기의 정보 받아오기

    public GameObject m_border = null;
    [HideInInspector] public bool m_zoomInOut = false;           //true일 때 줌인

    public GameObject m_defaultCross = null;
    public GameObject m_sniperCross = null;
    //[Header("M16")]
    //public CrosshairOption m_M16crossHair;

    //public CrosshairOption m_K2crossHair;

    public RawImage m_curDot;        //현재 크로스 헤어의 dot 이미지
    public RawImage m_curInner;      //현재 크로스 헤어의 inner 이미지
    public RawImage m_curExpanding;  //현재 크로스 헤어의 expanding 이미지
    public Image m_curReload;        //현재 크로스 헤어의 reload 이미지

    ////alt versions to make crosshair look animated
    //[Header("------- Alt -------")]
    //public Texture m_altDot;
    //public Texture m_altInner;
    //public Texture m_altExpanding;

    //[Header("------- Default -------")]
    //public Texture m_defaultDot;           //크로스 헤어의 기본 dot 이미지를 담을 변수
    //public Texture m_defaultInner;         //크로스 헤어의 기본 inner 이미지를 담을 변수
    //public Texture m_defaultExpanding;     //크로스 헤어의 기본 expanding 이미지를 담을 변수

    float m_reloadSpeed;          //재장전 속도
    float m_shrinkSpeed;          //expanding의 scale 감소 속도
    [HideInInspector] public Vector3 m_crosshairMaxScale;     //크로스 헤어의 expanding의 maxScale        
    [HideInInspector] public float m_expandSize;              //발당 늘어나는 expanding의 scale 사이즈

    [HideInInspector] public bool m_isReloading;  //재장전 중인지 여부(재장전 중일 때 총 못쏘게 하기위해서 사용 중)
    bool m_isShrinking;           //크로스 헤어의 expanding이 감소하고있는지 여부 (이미 코루틴을 통해서 감소중이면 겹치지 않게 하기 위함)

    Vector3 m_crosshairOriginalScale;   //크로스 헤어의 expanding의 기본 scale

    //Color m_defaultColor; //used to store the initial crosshair color



    void Start()
    {
        //turn off the reload image at the start of the game
        m_curReload.enabled = false;       //크로스 헤어의 재장전 이미지 끄기

        //remember the current size of the crosshair, current textures and current color, so we can expand/shrink it back to normal size, default textures and default tint color

        this.m_crosshairOriginalScale = m_curExpanding.rectTransform.localScale;
        //this.crosshairOriginalScale = this.transform.localScale;
        //this.m_defaultColor = m_dot.color;
        //this.m_defaultDot = m_curDot.texture;
        //this.m_defaultInner = m_curInner.texture;
        //this.m_defaultExpanding = m_curExpanding.texture;
    }

    //public void ShowAlternates() //switch to alt textures
    //{
    //    m_curDot.texture = m_altDot;
    //    //m_inner.texture = altInner;
    //    //m_expanding.texture = altExpanding;
    //}

    //public void HideAlternates() //switch back to original textures
    //{
    //    m_curDot.texture = m_defaultDot;
    //    //m_curInner.texture = m_defaultInner;
    //    //m_curExpanding.texture = m_defaultExpanding;
    //}

    //public void EnableTint(Color newTint) //switch to another color (set in the GUN object)
    //{
    //    m_dot.color = newTint;
    //}

    //public void DisableTint() //switch back to original tint/color
    //{
    //    m_dot.color = this.m_defaultColor;
    //}

    public void DoReload()          //재장전 하기
    {
        SoundMgr.inst.AudioChange(SoundList.Reload);
        SoundMgr.inst.m_audioSource.Play();
        m_zoomInOut = false;
        //if we are not already reloading, do the reloading routine.
        if (!m_isReloading)
            StartCoroutine(ReloadTheGun());
    }

    //used by the GUN to expand the crosshair
    public void ExpandCrosshair()     //크로스 헤어의 expanding의 scale 확장
    {
        //if the crosshair is still under the maximum expandable size, then make it expand more
        if (m_curExpanding.rectTransform.localScale.x < m_crosshairMaxScale.x)
        {
            m_curExpanding.rectTransform.localScale += new Vector3(m_expandSize, m_expandSize, m_expandSize);
        }
        else
            m_curExpanding.rectTransform.localScale = m_crosshairMaxScale;

        StartCoroutine(ShrinkCrosshair());          //expanding 감소 코루틴 실행
    }


    // these public functions are used to set up the crosshair to function based on what the GUN object settings are (so that you can set different behaviour per gun)
    public void SetReloadSpeed(float ReloadSpeed)       //재장전 속도 설정
    {
        this.m_reloadSpeed = ReloadSpeed;
    }

    public void SetShrinkSpeed(float ShrinkSpeed)       //expanding의 감소 속도 설정
    {
        this.m_shrinkSpeed = ShrinkSpeed;
    }

    public void SetMaxScale(float MaxScale)             //expanding의 maxscale 설정
    {
        this.m_crosshairMaxScale = new Vector3(MaxScale, MaxScale, MaxScale);
    }

    //shrinks the crosshair progressively over many frames - called by the ExpandCrosshair() function 
    public IEnumerator ShrinkCrosshair()
    {
        if (m_isShrinking == true)          //이미 감소중이면 또 안함      
            yield break;

        m_isShrinking = true;

        //while the crosshair is bigger than default size, keep shrinking
        do
        {
            m_curExpanding.rectTransform.localScale = new Vector3(m_curExpanding.rectTransform.localScale.x - Time.deltaTime * m_shrinkSpeed,
                                                             m_curExpanding.rectTransform.localScale.y - Time.deltaTime * m_shrinkSpeed,
                                                             m_curExpanding.rectTransform.localScale.z - Time.deltaTime * m_shrinkSpeed);
            yield return new WaitForEndOfFrame();       //모든 프레임이 다 실행되고 나서 진행           
        }
        while (m_crosshairOriginalScale.x < m_curExpanding.rectTransform.localScale.x);    //expanding의 원래 scale로 돌아갈 때 까지 반복

        m_isShrinking = false;
        yield return new WaitForEndOfFrame();
    }


    IEnumerator ReloadTheGun()              //총알 재장전
    {
        m_isReloading = true;

        m_curReload.fillAmount = 0;            //reload 진행 정도 체크
        m_curReload.enabled = true;            //reload 이미지 켜기
        m_curInner.enabled = false;            //inner 이미지 끄기
        m_curDot.enabled = false;              //dot 이미지 끄기
        m_curExpanding.enabled = false;        //expanding 이미지 끄기

        do
        {
            m_curReload.fillAmount += Time.deltaTime * m_reloadSpeed;
            yield return new WaitForEndOfFrame();
        }
        while (m_curReload.fillAmount < 1f);       //reload 완료까지 반복

        int a_Calcint = m_itemInfo.m_reMagazine - m_itemInfo.m_curMagazine;     //재장전 시 장전개수 - 현재 남아있는 총알 개수

        if (a_Calcint <= m_itemInfo.m_maxMagazine)
            m_itemInfo.m_maxMagazine -= a_Calcint;          //재장전한 총알 개수만큼 줄이기
        else
            m_itemInfo.m_maxMagazine = 0;                   //남아있는 총알 개수만큼 줄이기

        m_itemInfo.m_curMagazine += a_Calcint;          //재장전한 총알 개수만큼 충전


        m_curReload.enabled = false;           //reload 이미지 끄기
        m_curInner.enabled = true;             //inner 이미지 켜기
        m_curDot.enabled = true;               //dot 이미지 켜기
        m_curExpanding.enabled = true;         //expanding 이미지 켜기

        m_isReloading = false;

        m_curExpanding.rectTransform.localScale = m_crosshairOriginalScale; //크로스 헤어의 expanding의 scale 기본으로 변경

        SoundMgr.inst.AudioChange(SoundList.Weapon);

        yield return new WaitForEndOfFrame();
    }

    private void OnDisable()
    {
        m_isShrinking = false;
        m_curExpanding.rectTransform.localScale = m_crosshairOriginalScale;
    }

    public void CrossHairCon()
    {
        if (m_zoomInOut == true)             //줌인 상태
            if (m_itemInfo.m_itName == ItemName.SniperRifle)      //저격총일 경우
            {
                m_defaultCross.gameObject.SetActive(false);        //기본 크로스헤어 끄기
                m_sniperCross.gameObject.SetActive(true);          //스나이퍼 크로스헤어 켜기
                Camera.main.fieldOfView = 15.0f;
                Camera.main.GetComponent<CameraCtrl>().m_maxX = 15.0f;
            }
            else
                SetMaxScale(1.3f);   //크로스헤어 expanding의 최대 scale 설정

        else  //(m_zoomInOut == false)                              //줌아웃 상태        
        {
            if (m_itemInfo.m_itName == ItemName.SniperRifle)      //저격총일 경우
            {
                m_defaultCross.gameObject.SetActive(true);        //기본 크로스헤어 끄기
                m_sniperCross.gameObject.SetActive(false);          //스나이퍼 크로스헤어 켜기
                Camera.main.fieldOfView = 60.0f;
                Camera.main.GetComponent<CameraCtrl>().m_maxX = 60.0f;
            }

            SetMaxScale(m_itemInfo.m_maxScale); //크로스헤어 expanding의 최대 scale 설정
        }

        if (Input.GetKeyDown(KeyCode.R))     //재장전        
            if (0 < m_itemInfo.m_maxMagazine)       //남은 총알이 있을 경우만 실행
                DoReload();

        if (PlayerCtrl.inst.m_animController.GetBool("Roll") == true)            //플레이어가 구르기를 하면
        {
            m_curExpanding.rectTransform.localScale = m_crosshairMaxScale;      //expanding의 scale을 maxScale 값으로
            StartCoroutine(ShrinkCrosshair());              //expanding의 scale 감소 코루틴 시작
            StopCoroutine(ShrinkCrosshair());               //expanding의 scale 감소 코루틴 정지
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

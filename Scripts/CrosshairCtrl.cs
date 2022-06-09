using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TooManyCrosshairs
{
    public class CrosshairCtrl : MonoBehaviour
    {
        public RawImage m_dot;        //크로스 헤어의 dot 이미지
        public RawImage m_inner;      //크로스 헤어의 inner 이미지
        public RawImage m_expanding;  //크로스 헤어의 expanding 이미지
        public Image m_reload;        //크로스 헤어의 reload 이미지

        ////alt versions to make crosshair look animated
        //public Texture m_altDot;
        //public Texture m_altInner;
        //public Texture m_altExpanding;

        float m_reloadSpeed;          //재장전 속도
        float m_shrinkSpeed;          //expanding의 scale 감소 속도
        [HideInInspector] public Vector3 m_crosshairMaxScale;     //크로스 헤어의 expanding의 maxScale        
        [HideInInspector] public float m_expandSize;              //발당 늘어나는 expanding의 scale 사이즈

        [HideInInspector] public bool m_isReloading;  //재장전 중인지 여부(재장전 중일 때 총 못쏘게 하기위해서 사용 중)
        bool m_isShrinking;           //크로스 헤어의 expanding이 감소하고있는지 여부 (이미 코루틴을 통해서 감소중이면 겹치지 않게 하기 위함)

        Vector3 m_crosshairOriginalScale;   //크로스 헤어의 expanding의 기본 scale

        //Color m_defaultColor; //used to store the initial crosshair color

        //used to store the initial textures (so they can be returned after the alt crosshair is shown)
        Texture m_defaultDot;           //크로스 헤어의 기본 dot 이미지를 담을 변수
        Texture m_defaultInner;         //크로스 헤어의 기본 inner 이미지를 담을 변수
        Texture m_defaultExpanding;     //크로스 헤어의 기본 expanding 이미지를 담을 변수

        void Start()
        {
            //turn off the reload image at the start of the game
            m_reload.enabled = false;       //크로스 헤어의 재장전 이미지 끄기

            //remember the current size of the crosshair, current textures and current color, so we can expand/shrink it back to normal size, default textures and default tint color
            
            this.m_crosshairOriginalScale = m_expanding.rectTransform.localScale;            
            //this.crosshairOriginalScale = this.transform.localScale;
            //this.m_defaultColor = m_dot.color;
            this.m_defaultDot = m_dot.texture;
            this.m_defaultInner = m_inner.texture;
            this.m_defaultExpanding = m_expanding.texture;
        }        

        //public void ShowAlternates() //switch to alt textures
        //{
        //    m_dot.texture = altDot;
        //    m_inner.texture = altInner;
        //    m_expanding.texture = altExpanding;
        //}

        //public void HideAlternates() //switch back to original textures
        //{
        //    m_dot.texture = m_defaultDot;
        //    m_inner.texture = m_defaultInner;
        //    m_expanding.texture = m_defaultExpanding;
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
            //if we are not already reloading, do the reloading routine.
            if (!m_isReloading)
                StartCoroutine(ReloadTheGun());
        }

        //used by the GUN to expand the crosshair
        public void ExpandCrosshair()     //크로스 헤어의 expanding의 scale 확장
        {
            //if the crosshair is still under the maximum expandable size, then make it expand more
            if (m_expanding.rectTransform.localScale.x < m_crosshairMaxScale.x)
            {
                m_expanding.rectTransform.localScale += new Vector3(m_expandSize, m_expandSize, m_expandSize);
            }
            else
                m_expanding.rectTransform.localScale = m_crosshairMaxScale;
                        
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
            this.m_crosshairMaxScale = new Vector3 (MaxScale, MaxScale, MaxScale);
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
                m_expanding.rectTransform.localScale = new Vector3(m_expanding.rectTransform.localScale.x - Time.deltaTime * m_shrinkSpeed,
                                                                 m_expanding.rectTransform.localScale.y - Time.deltaTime * m_shrinkSpeed,
                                                                 m_expanding.rectTransform.localScale.z - Time.deltaTime * m_shrinkSpeed);
                yield return new WaitForEndOfFrame();       //모든 프레임이 다 실행되고 나서 진행           
            }
            while (m_crosshairOriginalScale.x < m_expanding.rectTransform.localScale.x);    //expanding의 원래 scale로 돌아갈 때 까지 반복

            m_isShrinking = false;
            yield return new WaitForEndOfFrame();
        }
        
        
        IEnumerator ReloadTheGun()              //총알 재장전
        {            
            m_isReloading = true;            
                        
            m_reload.fillAmount = 0;            //reload 진행 정도 체크
            m_reload.enabled = true;            //reload 이미지 켜기
            m_inner.enabled = false;            //inner 이미지 끄기
            m_dot.enabled = false;              //dot 이미지 끄기
            m_expanding.enabled = false;        //expanding 이미지 끄기
                        
            do
            {
                m_reload.fillAmount += Time.deltaTime * m_reloadSpeed;
                yield return new WaitForEndOfFrame();
            }
            while (m_reload.fillAmount < 1f);       //reload 완료까지 반복
                        
            m_reload.enabled = false;           //reload 이미지 끄기
            m_inner.enabled = true;             //inner 이미지 켜기
            m_dot.enabled = true;               //dot 이미지 켜기
            m_expanding.enabled = true;         //expanding 이미지 켜기

            m_isReloading = false;

            m_expanding.rectTransform.localScale = m_crosshairOriginalScale; //크로스 헤어의 expanding의 scale 기본으로 변경

            yield return new WaitForEndOfFrame();
        }

        private void OnDisable()
        {            
            m_isShrinking = false;
            m_expanding.rectTransform.localScale = m_crosshairOriginalScale;
        }
    }
}

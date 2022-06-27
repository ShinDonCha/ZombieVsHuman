using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundBtnCtrl : MonoBehaviour
{
    //---- 버튼 이미지    
    public Sprite[] m_buttonSlideImg = null;
    public Sprite m_buttonOffImg = null;
    //---- 버튼 이미지

    public Slider m_soundSlider = null;                 //버튼과 같은 위치에있는 Slider오브젝트 담을 변수

    private bool m_buttonOnOff = true;                  //버튼 온오프 상태 확인용 (true일경우 On)    

    private Image m_buttonImg = null;


    // Start is called before the first frame update
    void Start()
    {
        m_buttonImg = GetComponent<Image>();        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnOffImgChange()        //이 버튼 게임오브젝트 클릭 시 실행
    {        
        m_buttonOnOff = !m_buttonOnOff;

        if (m_buttonOnOff == true)        //On상태가 됐다면        
            SlideImgChange();            
        
        else                              //Off상태가 됐다면        
            m_buttonImg.sprite = m_buttonOffImg;            
        
    }

    public void SlideImgChange()       //슬라이더가 움직일 때 실행
    {
        m_buttonOnOff = true;

        if (m_soundSlider.value <= 0.0f)
            m_buttonImg.sprite = m_buttonSlideImg[0];
        else if (0.0f < m_soundSlider.value && m_soundSlider.value <= 0.33f)
            m_buttonImg.sprite = m_buttonSlideImg[1];
        else if (0.33f < m_soundSlider.value && m_soundSlider.value <= 0.66f)
            m_buttonImg.sprite = m_buttonSlideImg[2];
        else if (0.66f < m_soundSlider.value)
            m_buttonImg.sprite = m_buttonSlideImg[3];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundBtnCtrl : MonoBehaviour
{
    //---- ��ư �̹���    
    public Sprite[] m_buttonSlideImg = null;
    public Sprite m_buttonOffImg = null;
    //---- ��ư �̹���

    public Slider m_soundSlider = null;                 //��ư�� ���� ��ġ���ִ� Slider������Ʈ ���� ����

    private bool m_buttonOnOff = true;                  //��ư �¿��� ���� Ȯ�ο� (true�ϰ�� On)    

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

    public void OnOffImgChange()        //�� ��ư ���ӿ�����Ʈ Ŭ�� �� ����
    {        
        m_buttonOnOff = !m_buttonOnOff;

        if (m_buttonOnOff == true)        //On���°� �ƴٸ�        
            SlideImgChange();            
        
        else                              //Off���°� �ƴٸ�        
            m_buttonImg.sprite = m_buttonOffImg;            
        
    }

    public void SlideImgChange()       //�����̴��� ������ �� ����
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

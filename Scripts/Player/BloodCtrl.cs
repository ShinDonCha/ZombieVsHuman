using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodCtrl : MonoBehaviour
{
    public Sprite[] m_bloodImg = null;

    private Image m_Img = null;
    private int m_alphaCon = 44;
    // Start is called before the first frame update
    void Start()
    {
        int a_num = Random.Range(0, m_bloodImg.Length);
        m_Img = GetComponent<Image>();
        m_Img.sprite = m_bloodImg[a_num];

        a_num = Random.Range(-14, 15);
        gameObject.transform.localPosition = new Vector3(a_num * 10, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Color a_color = m_Img.color;
        a_color.a -= (m_alphaCon * Time.deltaTime) / 255;
        m_Img.color = a_color;

        if(m_Img.color.a <= 0)
            Destroy(gameObject);
    }
}

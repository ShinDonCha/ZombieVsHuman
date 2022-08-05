using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCtrl : MonoBehaviour
{    
    [HideInInspector] public ItemInfo m_itemInfo = new ItemInfo();

    public GameObject[] m_itemInven = null;

    // Start is called before the first frame update
    void Start()
    {       
        ModelSet();                
    }

    public void ModelSet()
    {
        int a_Num = 0;

        if (m_itemInfo.m_itName == ItemName.Kick)           //좀비에게서 드랍된 아이템일 경우 랜덤으로 설정
        {
            a_Num = Random.Range(0, m_itemInven.Length);
            m_itemInfo.SetType((ItemName)a_Num);
        }
        else
            a_Num = (int)m_itemInfo.m_itName;

        m_itemInfo.m_isDropped = true;
        GameObject a_go = Instantiate(m_itemInven[a_Num]);          //아이템 정보에따라 정해진 외형으로 생성
        a_go.transform.SetParent(transform, false);                 //아이템 프리팹의 차일드로 모델 넣기        
    }
}

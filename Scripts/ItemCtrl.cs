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

        switch (m_itemInfo.m_itName)
        {
            case ItemName.Bat:
                a_Num = (int)ItemName.Bat;                
                break;
            case ItemName.M16:
                a_Num = (int)ItemName.M16;
                break;
            case ItemName.K2:
                a_Num = (int)ItemName.K2;
                break;
            case ItemName.Null:                 //특정된 아이템이 아닐경우 랜덤으로 생성
                a_Num = Random.Range(0, m_itemInven.Length);
                m_itemInfo.SetType((ItemName)a_Num);
                break;                
        }
        
        m_itemInfo.m_isDropped = true;
        GameObject a_go = Instantiate(m_itemInven[a_Num]);          //아이템 정보에따라 정해진 외형으로 생성
        a_go.transform.SetParent(transform, false);                 //아이템 프리팹의 차일드로 모델 넣기        
    }
}

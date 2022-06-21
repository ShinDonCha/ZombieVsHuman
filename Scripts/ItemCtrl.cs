using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCtrl : MonoBehaviour
{
    [HideInInspector] public ItemType m_itemType;

    public bool m_isVisible = false;
    public GameObject[] m_itemInven = null;

    public int m_UniqueNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        int a_rnd = Random.Range(0, m_itemInven.Length);        //등록된 아이템 개수의 범위에서 랜덤 숫자 뽑기
        GameObject a_go = Instantiate(m_itemInven[a_rnd]);      //랜덤으로 아이템 모델 결정
        a_go.transform.SetParent(transform, false);             //Item오브젝트의 차일드로 붙이기

        //----- 어떤 아이템인지에 따라서 타입 바꿔주기
        if (a_go.name.Contains("K2"))
            m_itemType = ItemType.K2;
        else if (a_go.name.Contains("M16"))
            m_itemType = ItemType.M16;
        else if (a_go.name.Contains("Bat"))
            m_itemType = ItemType.Bat;
        //----- 어떤 아이템인지에 따라서 타입 바꿔주기
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //첫번째로 들어온 스프라이트에 bool형을 추가해서 true로 만든다?
    //검사하는 함수를 만들어야하는데..

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
         
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {

            

        }
    }

    public void OnRooted()
    {
        Destroy(this.gameObject);
    }
}

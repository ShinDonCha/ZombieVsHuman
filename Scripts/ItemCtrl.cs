using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCtrl : MonoBehaviour
{
    [HideInInspector] public ItemType m_itemType;

    public GameObject[] m_itemInven = null;

    // Start is called before the first frame update
    void Start()
    {
        int a_rnd = Random.Range(0, m_itemInven.Length);        //��ϵ� ������ ������ �������� ���� ���� �̱�
        GameObject a_go = Instantiate(m_itemInven[a_rnd]);      //�������� ������ �� ����
        a_go.transform.SetParent(transform, false);             //Item������Ʈ�� ���ϵ�� ���̱�

        //----- � ������������ ���� Ÿ�� �ٲ��ֱ�
        if (a_go.name.Contains("K2c1"))
            m_itemType = ItemType.K2;
        else if (a_go.name.Contains("M16"))
            m_itemType = ItemType.M16;
        else if (a_go.name.Contains("Bat"))
            m_itemType = ItemType.Bat;
        //----- � ������������ ���� Ÿ�� �ٲ��ֱ�
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ù��°�� ���� ��������Ʈ�� bool���� �߰��ؼ� true�� �����?
    //�˻��ϴ� �Լ��� �������ϴµ�..

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
}

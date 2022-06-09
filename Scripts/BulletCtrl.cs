using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletCtrl : MonoBehaviour
{
    private float m_bulletSpeed = 2000.0f;          //�Ѿ��� �ӵ�    

    // Start is called before the first frame update
    void Start()
    {        
        GetComponent<Rigidbody>().AddForce(transform.forward * m_bulletSpeed);

        Destroy(gameObject, 3.0f);          //3�ʵ� �ڵ����� ����
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            other.GetComponent<ZombieCtrl>().TakeDamage(10);        //�ӽ÷� 10������
            Destroy(gameObject);
        }
        else if (other.CompareTag("Terrain"))
            Destroy(gameObject);
    }
}

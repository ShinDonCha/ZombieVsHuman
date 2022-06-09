using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 수정수정수정123
[RequireComponent(typeof(Rigidbody))]
public class BulletCtrl : MonoBehaviour
{
    private float m_bulletSpeed = 2000.0f;          //총알의 속도    

    // Start is called before the first frame update
    void Start()
    {        
        GetComponent<Rigidbody>().AddForce(transform.forward * m_bulletSpeed);

        Destroy(gameObject, 3.0f);          //3초뒤 자동으로 삭제
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            other.GetComponent<ZombieCtrl>().TakeDamage(10);        //임시로 10데미지
            Destroy(gameObject);
        }
        else if (other.CompareTag("Terrain"))
            Destroy(gameObject);
    }
}

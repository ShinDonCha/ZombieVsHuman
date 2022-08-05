using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletCtrl : MonoBehaviour
{
    [HideInInspector] public float m_bulletSpeed = 2000.0f;          //총알의 속도
    [HideInInspector] public int m_bulletDmg = 0;

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

    private void OnTriggerEnter(Collider other) //상대와 나중에 하나만 istrigger면 됨, 둘중 하나 rigidbody 있어야함
    {
        if (other.CompareTag("Zombie"))
        {
            ZombieCtrl a_ZCtrl = other.GetComponent<ZombieCtrl>();
            a_ZCtrl.TakeDamage(transform.position, m_bulletDmg, a_ZCtrl.m_attackDist / 4.0f);        //좀비 공격거리의 25%만큼 밀려남
            Destroy(gameObject);
        }
        else if (other.CompareTag("Terrain") || other.CompareTag("Item"))
            Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawn : MonoBehaviour
{
    private int m_spawnCount = 10;             //������ ���� ����
    private int m_range = 10;                  //���� ������ ����

    public GameObject[] m_zombiePrefab = null;    //���� ������


    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            SpawnZombie();
            Destroy(gameObject);
        }
    }

    void SpawnZombie()
    {
        List<Vector3> a_pointList = new List<Vector3>();            //������� ������ ��ġ�� ���� ����Ʈ
        Vector3 a_point = Vector3.zero;                             //������ ���� ��ġ(ó���� ����)

        while (a_pointList.Count != m_spawnCount)                   //���� ���� �� ��ŭ ����
        {
            a_pointList.Add(a_point);                                               
            bool a_enable = false;                                  //���� ��ġ ��� ���� ����
            do
            {                
                int a_xRange = Random.Range(-m_range, m_range + 1);
                int a_zRange = Random.Range(-m_range, m_range + 1);

                a_point = new Vector3(a_xRange, -4.0f, a_zRange);

                for (int i = 0; i < a_pointList.Count; i++)
                {
                    if (a_pointList[i] != a_point)
                        a_enable = true;
                    else
                        a_enable = false;
                }
            }
            while (a_enable == false);
        }

        for (int i = 0; i < a_pointList.Count; i++)          //���� ����
        {
            int a_rnd = Random.Range(0, m_zombiePrefab.Length);
            Instantiate(m_zombiePrefab[a_rnd], gameObject.transform.position + a_pointList[i], Quaternion.identity);
        }
    }
}

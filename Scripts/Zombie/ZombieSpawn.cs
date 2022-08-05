using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawn : MonoBehaviour
{
    private int m_spawnCount = 10;             //생성할 좀비 숫자
    private int m_range = 10;                  //좀비가 생성될 범위

    public GameObject[] m_zombiePrefab = null;    //좀비 프리팹


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
        List<Vector3> a_pointList = new List<Vector3>();            //좀비들의 스폰될 위치를 담을 리스트
        Vector3 a_point = Vector3.zero;                             //무작위 스폰 위치(처음만 지정)

        while (a_pointList.Count != m_spawnCount)                   //좀비 스폰 수 만큼 진행
        {
            a_pointList.Add(a_point);                                               
            bool a_enable = false;                                  //스폰 위치 사용 가능 여부
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

        for (int i = 0; i < a_pointList.Count; i++)          //좀비 스폰
        {
            int a_rnd = Random.Range(0, m_zombiePrefab.Length);
            Instantiate(m_zombiePrefab[a_rnd], gameObject.transform.position + a_pointList[i], Quaternion.identity);
        }
    }
}

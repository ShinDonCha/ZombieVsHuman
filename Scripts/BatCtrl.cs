using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (PlayerCtrl.inst.m_animController.GetCurrentAnimatorStateInfo(1).IsName("Swing"))
        {            
            if (other.gameObject.tag.Contains("Zombie"))
            {                
                other.GetComponent<ZombieCtrl>().TakeDamage(10);                
            }
        }

    }
}

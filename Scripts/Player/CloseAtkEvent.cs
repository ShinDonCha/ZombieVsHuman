using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseAtkEvent : MonoBehaviour
{
    void CloseAttack()
    {
        PlayerCtrl.inst.m_nowWeapon.m_closeAtk = !PlayerCtrl.inst.m_nowWeapon.m_closeAtk;
    }
}

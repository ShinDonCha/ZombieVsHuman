using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentCtrl : MonoBehaviour
{
    public SlotCtrl m_slotCtrl = null;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        ItemOnOff();
    }

    public void ItemOnOff()
    {
        WeaponCtrl[] a_playerWeapon = PlayerCtrl.inst.gameObject.GetComponentsInChildren<WeaponCtrl>(true);     //플레이어에 들어있는 무기들의 스크립트 찾아오기

        for (int i = 0; i < a_playerWeapon.Length; i++)
        {
            if (a_playerWeapon[i].gameObject.name == m_slotCtrl.m_itemInfo.m_itName.ToString())        //현재 착용 장비와 플레이어의 무기목록 중 이름이 일치하는 것이 있다면
            {
                a_playerWeapon[i].gameObject.SetActive(true);                                   //해당하는 플레이어 무기를 온
                a_playerWeapon[i].m_itemInfo = m_slotCtrl.m_itemInfo;                           //현재 착용 장비의 정보로 변경
                a_playerWeapon[i].Init();
            }
            else
                a_playerWeapon[i].gameObject.SetActive(false);
        }
    }
}

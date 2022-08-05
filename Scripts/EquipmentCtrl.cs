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
        WeaponCtrl[] a_playerWeapon = PlayerCtrl.inst.gameObject.GetComponentsInChildren<WeaponCtrl>(true);     //�÷��̾ ����ִ� ������� ��ũ��Ʈ ã�ƿ���

        for (int i = 0; i < a_playerWeapon.Length; i++)
        {
            if (a_playerWeapon[i].gameObject.name == m_slotCtrl.m_itemInfo.m_itName.ToString())        //���� ���� ���� �÷��̾��� ������ �� �̸��� ��ġ�ϴ� ���� �ִٸ�
            {
                a_playerWeapon[i].gameObject.SetActive(true);                                   //�ش��ϴ� �÷��̾� ���⸦ ��
                a_playerWeapon[i].m_itemInfo = m_slotCtrl.m_itemInfo;                           //���� ���� ����� ������ ����
                a_playerWeapon[i].Init();
            }
            else
                a_playerWeapon[i].gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
 

public class CoolTimeCtrl : MonoBehaviour ,IPointerClickHandler
{
    private SlotCtrl m_slotCtrl = null;

    private WeaponSlotCtrl m_WeaponSlotCtrl = null;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)         // 슬롯에 마우스 오른쪽 클릭시..
        {          
            if (eventData.pointerCurrentRaycast.gameObject.name.Contains("Slot"))
            {
                m_slotCtrl = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotCtrl>();
                m_slotCtrl.m_isClicked = true;                
            }
            else if (eventData.pointerCurrentRaycast.gameObject.name.Contains("Weapon"))
            {
                m_WeaponSlotCtrl = eventData.pointerCurrentRaycast.gameObject.GetComponent<WeaponSlotCtrl>();
                m_WeaponSlotCtrl.m_isClicked = true;
            }
        }
    }
}

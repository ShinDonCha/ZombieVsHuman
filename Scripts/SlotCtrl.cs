using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotCtrl : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Image itemImage;
    public ItemType item;
    public static int itemCount; // 획득한 아이템의 개수
    public int UniqueitemNum;   // 이 슬롯 아이템의 넘버

    void Awake()
    {
        itemImage = this.gameObject.GetComponent<Image>();
        UniqueitemNum = itemCount;

    }

    void Update()
    {
        //Debug.Log(UniqueitemNum);
    }

    // 마우스 드래그가 시작 됐을 때 발생하는 이벤트
    public void OnBeginDrag(PointerEventData eventData)
    {          
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);
            DragSlot.instance.transform.position = eventData.position;
        
    }

    // 마우스 드래그 중일 때 계속 발생하는 이벤트
    public void OnDrag(PointerEventData eventData)
    {       
            DragSlot.instance.transform.position = eventData.position;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
        {
            ChangeSlot();
        }
    } 

    // 마우스 드래그가 끝났을 때 발생하는 이벤트
    public void OnEndDrag(PointerEventData eventData)
    {
        
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    
    }


    private void ChangeSlot()
    {
        ItemType _tempItem = item;        
        int _tempItemCount = itemCount;

        AddItem(DragSlot.instance.dragSlot.item,DragSlot.instance.dragSlot.UniqueitemNum);

        if ((int)_tempItem > 0 || (int)_tempItem <= (int)ItemType.ItemCount)
            DragSlot.instance.dragSlot.AddItem(_tempItem);
        else
            DragSlot.instance.dragSlot.ClearSlot();
    }

    public void AddItem(ItemType _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = GlobalValue.g_itemDic[_item].m_iconImg;

        //if (item.itemType != Item.ItemType.Equipment)
        //{
        //    go_CountImage.SetActive(true);
        //    text_Count.text = itemCount.ToString();
        //}
        //else
        //{
        //    text_Count.text = "0";
        //    go_CountImage.SetActive(false);
        //}

        SetColor(1);
    }

    private void ClearSlot()
    {
        item = ItemType.Bat;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);


    }

    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

}
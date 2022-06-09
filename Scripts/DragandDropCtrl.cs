//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;

//public class Selectitem
//{
//    public Image Img;
//    public ItemType Type;
//    public int TypeIdx;
//}

//public class DragandDropCtrl : MonoBehaviour, IDragHandler, IBeginDragHandler, IDropHandler, IEndDragHandler
//{
//    public Selectitem m_SelectItem = new Selectitem();
        
//    //data �� ���� ���� ��涧 �ٲ�� �̹��� �� , ���� ���� �ִ� �̹���
//    public DragandDropContainer dragAndDropContainer;

//    private GameObject Canvas; // Instantiate �� Obj �θ� ��ġ�� ����
//    private ScrollRect ParentSR;    // ScrollView ������ ����

//    private Rect RectSR;

//    bool isDragging = false;

//    void Start()
//    {

//        Canvas = GameObject.Find("Canvas");
//        ParentSR = transform.parent.parent.GetComponent<ScrollRect>();
//        Vector2 Size = ParentSR.GetComponent<RectTransform>().sizeDelta;
//        Vector2 LTPos = new Vector2(ParentSR.transform.position.x - Size.x / 2f, ParentSR.transform.position.y - Size.y / 2f);
//        RectSR = new Rect(LTPos, Size);
//    }

//    // �巡�� ������Ʈ���� �߻�
//    public void OnBeginDrag(PointerEventData eventData)
//    {
//        ParentSR.OnBeginDrag(eventData);

//        //// Activate Container
//        //dragAndDropContainer.gameObject.SetActive(true);
//        //// Set Data 
//        //dragAndDropContainer.image.sprite = data.sprite;
//        isDragging = true;
//    }
//    // �巡�� ������Ʈ���� �߻�
//    public void OnDrag(PointerEventData eventData)
//    {
//        if (isDragging)
//        {
//            dragAndDropContainer.transform.position = eventData.position;
//        }
//    }
//    // �巡�� ������Ʈ���� �߻�
//    public void OnEndDrag(PointerEventData eventData)
//    {
//        if (isDragging)
//        {
//            if (dragAndDropContainer.image.sprite != null)
//            {
//                // set data from dropped object  
//                data.sprite = dragAndDropContainer.image.sprite;
//            }
//            else
//            {
//                // Clear Data
//                data.sprite = null;
//            }
//        }

//        isDragging = false;
//        // Reset Contatiner
//        dragAndDropContainer.image.sprite = null;
//        dragAndDropContainer.gameObject.SetActive(false);
//    }

//    // ��� ������Ʈ���� �߻�
//    public void OnDrop(PointerEventData eventData)
//    {
//        if (dragAndDropContainer.image.sprite != null)
//        {
//            // keep data instance for swap 
//            Sprite tempSprite = data.sprite;

//            // set data from drag object on Container
//            data.sprite = dragAndDropContainer.image.sprite;

//            // put data from drop object to Container.  
//            dragAndDropContainer.image.sprite = tempSprite;
//        }
//        else
//        {
//            dragAndDropContainer.image.sprite = null;
//        }
//    }
//}
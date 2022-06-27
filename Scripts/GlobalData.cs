using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    HeadGear,
    Armor,
    Weapon,
    Count,
    Null
}

public enum ItemName
{
    Bat = 0,
    M16 = 1,
    K2 = 2,
    Ak47 = 3,
    SniperRifle = 4,       
    ItemCount = 5,
    Null    
}

public class ItemInfo //각 Item 정보
{
    public string m_name = "";                  //아이템 이름
    public int m_damage = 0;                    //아이템 공격력   
    public int m_maganize = 0;                  //아이템 총알갯수
    public float m_expandScale = 0.0f;          //발당 늘어나는 십자선 크기
    public float m_maxScale = 0.0f;             //십자선 최대 크기
    public float m_shrinkSpeed = 0.0f;          //십자선 줄어드는 속도
    public float m_attackDelay = 0.0f;          //공격 딜레이

    public bool m_isDropped = false;

    public ItemName m_itName = ItemName.Null;   //아이템 이름
    public ItemType m_itType = ItemType.Null;   //아이템 타입
    public Sprite m_iconImg = null;   //캐릭터 아이템에 사용될 이미지
    public Vector2 m_iconSize = Vector2.one;  //아이템 이미지의 가로 사이즈, 세로 사이즈
    public string m_itemEx = "";      //아이템 설명
    

    public void SetType(ItemName itemName)
    {
        m_itName = itemName;
        if (itemName == ItemName.Bat)
        {
            m_name = "야구방망이";
            m_itType = ItemType.Weapon;
            m_iconSize.x = 0.85f;   //Content의 cell크기 0.85배
            m_iconSize.y = 1.0f;     //Content의 cell크기
            m_maganize = 100;
            m_damage = 20;            
            m_attackDelay = 0.5f;          //공격 딜레이
            m_isDropped = false;
            m_itemEx = "쓰임에 따라 스포츠가 될지 , 느와르물이 될지 정해진다. 지금은 후자일지도..";

            m_iconImg = Resources.Load("Weapons/Bat", typeof(Sprite)) as Sprite;
        }
        else if (itemName == ItemName.M16)
        {
            m_name = "M16";
            m_itType = ItemType.Weapon;
            m_iconSize.x = 0.9f;    //Content의 cell크기 0.9배
            m_iconSize.y = 0.8f;     //Content의 cell크기 0.8배
            m_damage = 30;
            m_maganize = 20;
            m_expandScale = 0.12f;         //발당 늘어나는 십자선 크기
            m_maxScale = 2.0f;            //십자선 최대 크기
            m_shrinkSpeed = 0.18f;         //십자선 줄어드는 속도
            m_attackDelay = 0.1f;          //공격 딜레이
            m_isDropped = false;
            m_itemEx = "예비군 아저씨들의 상징과도 같은 무기, 명중률.. 생각보다 뛰어나다!";
            m_iconImg = Resources.Load("Weapons/M16", typeof(Sprite)) as Sprite;
        }
        else if (itemName == ItemName.K2)
        {
            m_name = "K2";
            m_itType = ItemType.Weapon;
            m_iconSize.x = 1.0f;    //Content의 cell크기
            m_iconSize.y = 0.7f;     //Content의 cell크기 0.7배
            m_damage = 30;
            m_maganize = 30;
            m_expandScale = 0.15f;         //발당 늘어나는 십자선 크기
            m_maxScale = 2.0f;            //십자선 최대 크기
            m_shrinkSpeed = 0.15f;         //십자선 줄어드는 속도
            m_attackDelay = 0.1f;          //공격 딜레이
            m_isDropped = false;

            m_itemEx = "현역들의 상징이자 못다루는 한국남자가 없을 정도, 근데 목숨처럼 소중한 이 무기가 왜 떨어져있지?";
            m_iconImg = Resources.Load("Weapons/K2", typeof(Sprite)) as Sprite;
        }
        else if (itemName == ItemName.Ak47)
        {
            m_name = "AK47";
            m_itType = ItemType.Weapon;
            m_iconSize.x = 0.946f;     //미정
            m_iconSize.y = 1.0f;     //미정
            m_damage = 40;
            m_maganize = 30;
            m_expandScale = 0.28f;         //발당 늘어나는 십자선 크기
            m_maxScale = 3.0f;            //십자선 최대 크기
            m_shrinkSpeed = 0.25f;         //십자선 줄어드는 속도
            m_attackDelay = 0.15f;          //공격 딜레이
            m_isDropped = false;

            m_itemEx = "이제는 총기소유를 할 수 있는 나라에서는 아무나 볼 수 있는 무기, 근데 여긴 한국인데?";
            //m_IconImg = Resources.Load("IconImg/m0054", typeof(Sprite)) as Sprite;
        }
        else if (itemName == ItemName.SniperRifle)
        {
            m_name = "SniperRifle";
            m_itType = ItemType.Weapon;
            m_iconSize.x = 0.93f;     //미정
            m_iconSize.y = 1.0f;     //미정
            m_damage = 150;
            m_maganize = 5;
            m_attackDelay = 0.0f;          //공격 딜레이
            m_isDropped = false;

            m_itemEx = "원샷 원킬? 생각보다 어려울껄";
            //m_IconImg = Resources.Load("IconImg/m0423", typeof(Sprite)) as Sprite;
        }
        //else if (itemType == ItemType.Fist)
        //{
        //    m_name = "Fist";
        //    m_itType = ItemType.Weapon;
        //    //m_iconSize.x = 0.9f;    //Content의 cell크기 0.9배
        //    //m_iconSize.y = 0.8f;     //Content의 cell크기 0.8배
        //    m_damage = 10;
        //    m_maganize = int.MaxValue;
        //    //m_expandScale = 0.12f;         //발당 늘어나는 십자선 크기
        //    //m_maxScale = 2.0f;            //십자선 최대 크기
        //    //m_shrinkSpeed = 0.18f;         //십자선 줄어드는 속도
        //    m_attackDelay = 0.3f;          //공격 딜레이
        //    m_isDropped = false;
        //    //m_itemEx = "예비군 아저씨들의 상징과도 같은 무기, 명중률.. 생각보다 뛰어나다!";
        //    //m_iconImg = Resources.Load("Weapons/M16", typeof(Sprite)) as Sprite;
        //}
    }//public void SetType(CharType a_CrType)
}

public class GlobalValue
{
    public static string g_Unique_ID = "";  //유저의 고유번호
    public static string g_NickName = "";   //유저의 별명
    public static int g_BestScore = 0;      //게임점수
    public static int g_UserGold = 0;       //게임머니
    public static int g_Exp = 0;            //경험치 Experience
    public static int g_Level = 0;          //레벨

    public static List<ItemInfo> g_equippedItem = new List<ItemInfo>();     //플레이어가 장착하고 있는 아이템 목록
    public static List<ItemInfo> g_userItem = new List<ItemInfo>();         //플레이어가 소유하고 있는 아이템 목록
    public static int g_invenFullSlotCount = 12;          //InvenPanel에 넣어줄 슬롯 개수        
    public static int g_equipFullSlotCount = 3;           //equipPanel에 넣어줄 슬롯 개수

    public static Dictionary<ItemName, ItemInfo> g_itemDic = new Dictionary<ItemName, ItemInfo>();      //전체 아이템 정보를 담을 딕셔너리
        
    public static void InitData()       //딕셔너리에 아이템 정보 저장
    {        
        if (0 < g_itemDic.Count)
            return;

        ItemInfo a_ItemList;
        for (int ii = 0; ii < (int)ItemName.ItemCount; ii++)
        {            
            a_ItemList = new ItemInfo();
            a_ItemList.SetType((ItemName)ii);
            g_itemDic.Add((ItemName)ii, a_ItemList);                       
        }
        
        for (int invenadd = 0; invenadd < g_invenFullSlotCount; invenadd++)         //인벤토리 슬롯의 개수만큼 리스트 생성
        {
            a_ItemList = new ItemInfo();
            g_userItem.Add(a_ItemList);
        }             
        
        for (int equipadd = 0; equipadd < 3; equipadd++)
        {
            a_ItemList = new ItemInfo();            
            g_equippedItem.Add(a_ItemList);
        }
    }//public static void InitData()

}


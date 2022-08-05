using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    Bat,
    M16,
    K2,
    AK47,
    SniperRifle,
    Kick,
    ItemCount,
}

public class ItemInfo //각 Item 정보
{
    public ItemName m_itName = ItemName.Kick;   //아이템 이름
    public int m_curMagazine = 0;               //현재 장전되어 있는 총알개수
    public int m_maxMagazine = 0;               //최대 총알개수

    public bool m_isDropped = false;

    public string m_name = "";                  //아이템 이름
    public int m_damage = 10;                    //아이템 공격력   
    
    public int m_reMagazine = 0;                //재장전 시 총알 개수
    
    public float m_expandScale = 0.0f;          //발당 늘어나는 십자선 크기
    public float m_maxScale = 0.0f;             //십자선 최대 크기
    public float m_shrinkSpeed = 0.0f;          //십자선 줄어드는 속도
    public float m_attackDelay = 1.8f;          //공격 딜레이
   
    public ItemType m_itType = ItemType.Null;   //아이템 타입
    public Sprite m_iconImg = null;   //캐릭터 아이템에 사용될 이미지
    public Vector2 m_iconSize = Vector2.one;  //아이템 이미지의 가로 사이즈, 세로 사이즈
    public string m_itemEx = "";      //아이템 설명
    
    public void SetType(ItemName itemName)
    {
        m_itName = itemName;
        if (itemName == ItemName.Bat)
        {
            m_name = "Baseball Bat";
            m_itType = ItemType.Weapon;
            m_iconSize.x = 1.0f;   //Content의 cell크기 0.85배
            m_iconSize.y = 1.0f;     //Content의 cell크기
            m_curMagazine = 100;
            m_reMagazine = 0;
            m_maxMagazine = 100;
            m_damage = 20;            
            m_attackDelay = 2.0f;          //공격 딜레이
            m_isDropped = false;
            m_itemEx = "쓰임에 따라 스포츠가 될지 , 느와르물이 될지 정해진다. 지금은 후자일지도..";

            //m_audioClip = null;
            m_iconImg = Resources.Load("Weapons/Bat", typeof(Sprite)) as Sprite;
        }
        else if (itemName == ItemName.M16)
        {
            m_name = "M16";
            m_itType = ItemType.Weapon;
            m_iconSize.x = 0.95f;    //Content의 cell크기 0.9배
            m_iconSize.y = 0.7f;     //Content의 cell크기 0.8배
            m_damage = 30;
            m_curMagazine = 20;
            m_reMagazine = 20;
            m_maxMagazine = 200;
            m_expandScale = 0.12f;         //발당 늘어나는 십자선 크기
            m_maxScale = 2.0f;            //십자선 최대 크기
            m_shrinkSpeed = 0.18f;         //십자선 줄어드는 속도
            m_attackDelay = 0.1f;          //공격 딜레이
            m_isDropped = false;
            m_itemEx = "예비군 아저씨들의 상징과도 같은 무기, 명중률.. 생각보다 뛰어나다!";

            //m_audioClip = Resources.Load("Sound/M16_Fire", typeof(AudioClip)) as AudioClip;
            m_iconImg = Resources.Load("Weapons/M16", typeof(Sprite)) as Sprite;
        }
        else if (itemName == ItemName.K2)
        {
            m_name = "K2";
            m_itType = ItemType.Weapon;
            m_iconSize.x = 1.0f;    //Content의 cell크기
            m_iconSize.y = 0.7f;     //Content의 cell크기 0.7배
            m_damage = 30;
            m_curMagazine = 30;
            m_reMagazine = 30;
            m_maxMagazine = 200;
            m_expandScale = 0.15f;         //발당 늘어나는 십자선 크기
            m_maxScale = 2.0f;            //십자선 최대 크기
            m_shrinkSpeed = 0.15f;         //십자선 줄어드는 속도
            m_attackDelay = 0.1f;          //공격 딜레이
            m_isDropped = false;
            m_itemEx = "현역들의 상징이자 못다루는 한국남자가 없을 정도, 근데 목숨처럼 소중한 이 무기가 왜 떨어져있지?";

            //m_audioClip = null;
            m_iconImg = Resources.Load("Weapons/K2", typeof(Sprite)) as Sprite;            
        }
        else if (itemName == ItemName.AK47)
        {
            m_name = "AK47";
            m_itType = ItemType.Weapon;
            m_iconSize.x = 1.0f;     //미정
            m_iconSize.y = 0.8f;     //미정
            m_damage = 40;
            m_curMagazine = 30;
            m_reMagazine = 30;
            m_maxMagazine = 200;
            m_expandScale = 0.28f;         //발당 늘어나는 십자선 크기
            m_maxScale = 3.0f;            //십자선 최대 크기
            m_shrinkSpeed = 0.25f;         //십자선 줄어드는 속도
            m_attackDelay = 0.15f;          //공격 딜레이
            m_isDropped = false;
            m_itemEx = "이제는 총기소유를 할 수 있는 나라에서는 아무나 볼 수 있는 무기, 근데 여긴 한국인데?";

            //m_audioClip = Resources.Load("Sound/Ak47_Fire", typeof(AudioClip)) as AudioClip;
            m_iconImg = Resources.Load("Weapons/AK47", typeof(Sprite)) as Sprite;
        }
        else if (itemName == ItemName.SniperRifle)
        {
            m_name = "Sniper Rifle";
            m_itType = ItemType.Weapon;
            m_iconSize.x = 1.0f;     //미정
            m_iconSize.y = 0.4f;     //미정
            m_damage = 150;
            m_curMagazine = 1;
            m_reMagazine = 1;
            m_maxMagazine = 15;
            m_expandScale = 0.28f;         //발당 늘어나는 십자선 크기
            m_maxScale = 3.0f;            //십자선 최대 크기
            m_shrinkSpeed = 0.25f;         //십자선 줄어드는 속도
            m_attackDelay = 3.0f;          //공격 딜레이
            m_isDropped = false;
            m_itemEx = "원샷 원킬? 생각보다 어려울껄";

            //m_audioClip = null;
            m_iconImg = Resources.Load("Weapons/Sniper_rifle", typeof(Sprite)) as Sprite;
        }
        else if (itemName == ItemName.Kick)
        {
            m_name = "";
            m_itType = ItemType.Null;
            m_iconSize = Vector2.one;
            m_damage = 10;
            m_curMagazine = 0;
            m_reMagazine = 0;
            m_maxMagazine = 0;
            m_expandScale = 0.0f;         //발당 늘어나는 십자선 크기
            m_maxScale = 0.0f;            //십자선 최대 크기
            m_shrinkSpeed = 0.0f;         //십자선 줄어드는 속도
            m_attackDelay = 1.8f;          //공격 딜레이
            m_isDropped = false;
            m_itemEx = "";

            //m_audioClip = null;
            m_iconImg = null;
        }
}//public void SetType(CharType a_CrType)
}

public class GlobalValue
{
    public static string g_Unique_ID = "";  //유저의 고유아이디
    public static string g_NickName = "";   //유저의 별명
    //public static int g_BestScore = 0;      //게임점수
    //public static int g_UserGold = 0;       //게임머니
    //public static int g_Exp = 0;            //경험치 Experience
    //public static int g_Level = 0;          //레벨

    public static List<ItemInfo> g_equippedItem = new List<ItemInfo>();     //플레이어가 장착하고 있는 아이템 목록
    public static List<ItemInfo> g_userItem = new List<ItemInfo>();         //플레이어가 소유하고 있는 아이템 목록
    public static int g_invenFullSlotCount = 12;          //InvenPanel에 넣어줄 슬롯 개수        
    public static int g_equipFullSlotCount = 3;           //equipPanel에 넣어줄 슬롯 개수

    //----- 환경설정 정보저장
    public static Sprite g_cfBGImg = null;
    public static float g_cfBGValue = 1.0f;
    public static Sprite g_cfEffImg = null;
    public static float g_cfEffValue = 1.0f;
    //----- 환경설정 정보저장

    //public static Dictionary<ItemName, ItemInfo> g_itemDic = new Dictionary<ItemName, ItemInfo>();      //전체 아이템 정보를 담을 딕셔너리

    public static void InitData()
    {
        for (int invenadd = 0; invenadd < g_invenFullSlotCount; invenadd++)         //인벤토리 슬롯의 개수만큼 리스트 생성
        {
            ItemInfo a_ItemList = new ItemInfo();
            g_userItem.Add(a_ItemList);
        }             
        
        for (int equipadd = 0; equipadd < 3; equipadd++)
        {
            ItemInfo a_ItemList = new ItemInfo();            
            g_equippedItem.Add(a_ItemList);
        }
    }//public static void InitData()

}


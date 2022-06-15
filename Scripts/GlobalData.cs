using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Bat = 0,
    M16 = 1,
    K2 = 2,
    Ak47 = 3,
    SniperRifle = 4,
    ItemCount = 5,
    Null
}

public class ItemInfo  //�� Item ����
{
    public string m_name = "";                  //������ �̸�
    public int m_damage = 0;                    //������ ���ݷ�
    public int m_duration = 0;                  //������ ������
    public int m_maganize = 0;                  //������ �Ѿ˰���
    public float m_expandScale = 0.0f;          //�ߴ� �þ�� ���ڼ� ũ��
    public float m_maxScale = 0.0f;             //���ڼ� �ִ� ũ��
    public float m_shrinkSpeed = 0.0f;          //���ڼ� �پ��� �ӵ�
    public float m_attackDelay = 0.0f;          //���� ������

    public bool m_isHave;

    public ItemType m_itType = ItemType.Bat; //������ Ÿ��
    public Sprite m_iconImg = null;   //ĳ���� �����ۿ� ���� �̹���
    public Vector2 m_iconSize = Vector2.one;  //������ �̹����� ���� ������, ���� ������
    public string m_itemEx = "";      //������ ����


    public void SetType(ItemType itemType)
    {
        m_itType = itemType;
        if (itemType == ItemType.Bat)
        {
            m_name = "�߱������";
            m_iconSize.x = 0.85f;   //Content�� cellũ�� 0.85��
            m_iconSize.y = 1.0f;     //Content�� cellũ��
            m_damage = 20;
            m_duration = 200;
            m_attackDelay = 0.5f;          //���� ������
            m_isHave = false;
            m_itemEx = "���ӿ� ���� �������� ���� , ���͸����� ���� ��������. ������ ����������..";

            m_iconImg = Resources.Load("Weapons/Bat", typeof(Sprite)) as Sprite;
        }
        else if (itemType == ItemType.M16)
        {
            m_name = "M16";
            m_iconSize.x = 0.9f;    //Content�� cellũ�� 0.9��
            m_iconSize.y = 0.8f;     //Content�� cellũ�� 0.8��
            m_damage = 30;
            m_maganize = 20;
            m_expandScale = 0.12f;         //�ߴ� �þ�� ���ڼ� ũ��
            m_maxScale = 2.0f;            //���ڼ� �ִ� ũ��
            m_shrinkSpeed = 0.18f;         //���ڼ� �پ��� �ӵ�
            m_attackDelay = 0.1f;          //���� ������
            m_isHave = false;
            m_itemEx = "���� ���������� ��¡���� ���� ����, ���߷�.. �������� �پ��!";
            m_iconImg = Resources.Load("Weapons/M16", typeof(Sprite)) as Sprite;
        }
        else if (itemType == ItemType.K2)
        {
            m_name = "K2";
            m_iconSize.x = 1.0f;    //Content�� cellũ��
            m_iconSize.y = 0.7f;     //Content�� cellũ�� 0.7��
            m_damage = 30;
            m_maganize = 30;
            m_expandScale = 0.15f;         //�ߴ� �þ�� ���ڼ� ũ��
            m_maxScale = 2.0f;            //���ڼ� �ִ� ũ��
            m_shrinkSpeed = 0.15f;         //���ڼ� �پ��� �ӵ�
            m_attackDelay = 0.1f;          //���� ������
            m_isHave = false;

            m_itemEx = "�������� ��¡���� ���ٷ�� �ѱ����ڰ� ���� ����, �ٵ� ���ó�� ������ �� ���Ⱑ �� ����������?";
            m_iconImg = Resources.Load("Weapons/K2", typeof(Sprite)) as Sprite;
        }
        else if (itemType == ItemType.Ak47)
        {
            m_name = "AK47";
            m_iconSize.x = 0.946f;     //����
            m_iconSize.y = 1.0f;     //����
            m_damage = 40;
            m_maganize = 30;
            m_expandScale = 0.28f;         //�ߴ� �þ�� ���ڼ� ũ��
            m_maxScale = 3.0f;            //���ڼ� �ִ� ũ��
            m_shrinkSpeed = 0.25f;         //���ڼ� �پ��� �ӵ�
            m_attackDelay = 0.15f;          //���� ������
            m_isHave = false;

            m_itemEx = "������ �ѱ������ �� �� �ִ� ���󿡼��� �ƹ��� �� �� �ִ� ����, �ٵ� ���� �ѱ��ε�?";
            //m_IconImg = Resources.Load("IconImg/m0054", typeof(Sprite)) as Sprite;
        }
        else if (itemType == ItemType.SniperRifle)
        {
            m_name = "SniperRifle";
            m_iconSize.x = 0.93f;     //����
            m_iconSize.y = 1.0f;     //����
            m_damage = 150;
            m_maganize = 5;
            m_attackDelay = 0.0f;          //���� ������
            m_isHave = false;

            m_itemEx = "���� ��ų? �������� ����ﲬ";
            //m_IconImg = Resources.Load("IconImg/m0423", typeof(Sprite)) as Sprite;
        }
        else if(itemType == ItemType.Null)
        {
           
        }

    }//public void SetType(CharType a_CrType)
}

public class GlobalValue
{
    public static string g_Unique_ID = "";  //������ ������ȣ
    public static string g_NickName = "";   //������ ����
    public static int g_BestScore = 0;      //��������
    public static int g_UserGold = 0;       //���ӸӴ�
    public static int g_Exp = 0;            //����ġ Experience
    public static int g_Level = 0;          //����

    public static Dictionary<ItemType, ItemInfo> g_itemDic = new Dictionary<ItemType, ItemInfo>();

    public static void InitData()       //��ųʸ��� ������ ���� ����
    {
        if (0 < g_itemDic.Count)
            return;

        ItemInfo a_ItemList;
        for (int ii = 0; ii < (int)ItemType.ItemCount; ii++)
        {
            a_ItemList = new ItemInfo();
            a_ItemList.SetType((ItemType)ii);
            g_itemDic.Add((ItemType)ii, a_ItemList);
        }
    }//public static void InitData()

}
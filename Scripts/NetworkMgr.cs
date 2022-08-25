using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public enum PacketType
{
    ItemChange,
    ConfigSet,
    CharPosSet
}

public class NetworkMgr : MonoBehaviour
{
    public static NetworkMgr inst = null;

    bool isNetworkLock = false;             //서버에 전송중인 정보가 있으면 대기시키기 위해
    List<PacketType> m_packetBuff = new List<PacketType>();

    private string m_saveItemUrl = "";
    private string m_saveConfigUrl = "";
    private string m_saveCharPos = "";

    // Start is called before the first frame update
    void Awake()
    {
        inst = this;
        m_saveItemUrl = "http://zombiehuman.dothome.co.kr/SaveItem.php";
        m_saveConfigUrl = "http://zombiehuman.dothome.co.kr/SaveConfig.php";
        m_saveCharPos = "http://zombiehuman.dothome.co.kr/CharPos.php";

    }

    // Update is called once per frame
    void Update()
    {
        if (isNetworkLock == false)          //패킷 처리중이 아니라면
            if (0 < m_packetBuff.Count)
                ReqNetwork();
            else
                ExitGame();
    }

    void ReqNetwork()
    {
        if (m_packetBuff[0] == PacketType.ItemChange)
        {
            StartCoroutine(UpdateItemCo());
        }
        else if (m_packetBuff[0] == PacketType.ConfigSet)
        {
            StartCoroutine(UpdateConfigCo());
        }
        else if (m_packetBuff[0] == PacketType.CharPosSet)
        {
            StartCoroutine(UpdateCharPos());
        }

        m_packetBuff.RemoveAt(0);
    }

    void ExitGame()
    {
        if (InGameMgr.s_gameState == GameState.GameEnd)
        {
            Application.Quit();
        }
        else if (InGameMgr.s_gameState == GameState.ReStart)
        {
            SceneManager.LoadScene("InGameScene");
        }
        else if (InGameMgr.s_gameState == GameState.GoTitle)
        {
            DataReset();
            SceneManager.LoadScene("TitleScene");
        }
    }



    IEnumerator UpdateItemCo()
    {
        if (string.IsNullOrEmpty(GlobalValue.g_Unique_ID) == true)
            yield break;

        isNetworkLock = true;

        JSONArray[] a_jsArr = { new JSONArray(), new JSONArray(), new JSONArray() };

        for (int i = 0; i < GlobalValue.g_equippedItem.Count; i++)
        {
            a_jsArr[0].Add(GlobalValue.g_equippedItem[i].m_itName.ToString());
            a_jsArr[1].Add(GlobalValue.g_equippedItem[i].m_curMagazine);
            a_jsArr[2].Add(GlobalValue.g_equippedItem[i].m_maxMagazine);

        }

        for (int i = 0; i < GlobalValue.g_userItem.Count; i++)
        {
            a_jsArr[0].Add(GlobalValue.g_userItem[i].m_itName.ToString());
            a_jsArr[1].Add(GlobalValue.g_userItem[i].m_curMagazine);
            a_jsArr[2].Add(GlobalValue.g_userItem[i].m_maxMagazine);


        }

        WWWForm a_form = new WWWForm();
        a_form.AddField("Input_user", GlobalValue.g_Unique_ID, System.Text.Encoding.UTF8);
        a_form.AddField("Item_name", a_jsArr[0].ToString(), System.Text.Encoding.UTF8);
        a_form.AddField("Item_curmag", a_jsArr[1].ToString(), System.Text.Encoding.UTF8);
        a_form.AddField("Item_maxmag", a_jsArr[2].ToString(), System.Text.Encoding.UTF8);
        //a_form.AddField("Item_maxmag", a_jsArr[3].ToString(), System.Text.Encoding.UTF8);




        UnityWebRequest a_www = UnityWebRequest.Post(m_saveItemUrl, a_form);
        yield return a_www.SendWebRequest();

        if (a_www.error != null)     //에러가 있을 경우
        {
            Debug.Log(a_www.error);
        }

        isNetworkLock = false;
    }

    IEnumerator UpdateConfigCo()
    {
        if (string.IsNullOrEmpty(GlobalValue.g_Unique_ID) == true)
            yield break;

        isNetworkLock = true;

        JSONArray a_jsArr = new JSONArray();
        a_jsArr.Add(GlobalValue.g_cfBGImg.name.ToString());
        a_jsArr.Add(string.Format("{0:N2}", GlobalValue.g_cfBGValue));
        a_jsArr.Add(GlobalValue.g_cfEffImg.name.ToString());
        a_jsArr.Add(string.Format("{0:N2}", GlobalValue.g_cfEffValue));

        WWWForm a_form = new WWWForm();
        a_form.AddField("Input_user", GlobalValue.g_Unique_ID, System.Text.Encoding.UTF8);
        a_form.AddField("Sound_option", a_jsArr.ToString(), System.Text.Encoding.UTF8);

        UnityWebRequest a_www = UnityWebRequest.Post(m_saveConfigUrl, a_form);
        yield return a_www.SendWebRequest();

        if (a_www.error != null)     //에러가 있을 경우
        {
            Debug.Log(a_www.error);
        }

        isNetworkLock = false;
    }
    IEnumerator UpdateCharPos()
    {
        if (string.IsNullOrEmpty(GlobalValue.g_Unique_ID) == true)
            yield break;

        Debug.Log(GlobalValue.g_CharPos.ToString());

        isNetworkLock = true;
        JSONArray a_jsArr = new JSONArray();
        a_jsArr.Add(GlobalValue.g_CharPos.x.ToString());
        a_jsArr.Add(GlobalValue.g_CharPos.z.ToString());

        WWWForm a_form = new WWWForm();
        a_form.AddField("Input_user", GlobalValue.g_Unique_ID, System.Text.Encoding.UTF8);
        a_form.AddField("Char_pos", a_jsArr.ToString(), System.Text.Encoding.UTF8);


        UnityWebRequest a_www = UnityWebRequest.Post(m_saveCharPos, a_form);
        yield return a_www.SendWebRequest();
        if (a_www.error != null)     //에러가 있을 경우
        {
            Debug.Log(a_www.error);
        }

        isNetworkLock = false;

    }

    public void PushPacket(PacketType a_packType)
    {
        for (int i = 0; i < m_packetBuff.Count; i++)
            if (m_packetBuff[i] == a_packType)              //이미 패킷이 처리중이면 또 추가하지 않기
                return;

        m_packetBuff.Add(a_packType);                       //아닐경우 추가
    }

    public void DataReset()
    {
        GlobalValue.g_Unique_ID = "";  //유저의 고유아이디
        GlobalValue.g_NickName = "";   //유저의 별명
        // 0이 아닐떄 애니메이션이 실행이 된다 . * 초깃값은 0 이다.
        // 한번만 실행되고 다음부터 실행안되게 만들려면 ?
        // 조건을 걸어야하는데 그 조건이 StandUp 초깃값을 1로하고 . 초기화를 할때 0으로 하면되나?
        GlobalValue.g_CharPos = Vector3.zero;
        GlobalValue.g_StandUpAnim = 0;
        GlobalValue.g_equippedItem.Clear();
        GlobalValue.g_userItem.Clear();
        GlobalValue.g_cfBGImg = null;
        GlobalValue.g_cfBGValue = 1.0f;
        GlobalValue.g_cfEffImg = null;
        GlobalValue.g_cfEffValue = 1.0f;
    }
}

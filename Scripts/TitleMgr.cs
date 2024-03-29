﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using SimpleJSON;


public class TitleMgr : MonoBehaviour
{
    [Header("LogInPanel")]
    public GameObject m_logInPanel = null;              //로그인 판넬 오브젝트를 담을 변수
    public InputField m_id_InputField = null;           //로그인 판넬의 id inputfield
    public InputField m_pw_InputField = null;           //로그인 판넬의 pw inputfield
    public Button m_logIn_Btn = null;                   //로그인 버튼
    public Button m_signUp_Btn = null;                  //회원가입 버튼

    [Header("SignUpPanel")]
    public GameObject m_signUpPanel = null;             //회원가입 판넬 오브젝트를 담을 변수
    public InputField m_newid_InputField = null;        //회원가입 판넬의 new id inputfield
    public InputField m_newpw_InputField = null;        //회원가입 판넬의 new pw inputfield
    public InputField m_newnick_InputField = null;      //회원가입 판넬의 new nick inputfield
    public Button m_create_Btn = null;                  //create 버튼
    public Button m_cancel_Btn = null;                  //취소 버튼

    [Header("Message")]
    public Text m_messageText = null;                   //메세지를 보여주기 위한 텍스트
    private float m_msTimer = 0.0f;                     //메세지 지속 타이머

    string m_logInUrl;                                  //닷홈의 로그인 php url
    string m_signUpUrl;                                 //닷홈의 회원가입 php url
    string m_saveCharUrl;                               //캐릭터 저장 위치

    [Header("Sound Img")]
    public Sprite[] m_soundSprite = null;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;    //마우스커서 열기

        m_id_InputField.Select();                       //처음에 로그인 판넬 id inputfield에 커서 놓기

        if (m_logIn_Btn != null)
            m_logIn_Btn.onClick.AddListener(LogIn);

        if (m_signUp_Btn != null)
            m_signUp_Btn.onClick.AddListener(SignUpPanelOn);

        if (m_cancel_Btn != null)
            m_cancel_Btn.onClick.AddListener(LogInPanelOn);

        if (m_create_Btn != null)
            m_create_Btn.onClick.AddListener(SignUp);

        m_logInUrl = "http://zombiehuman.dothome.co.kr/LogIn.php";
        m_signUpUrl = "http://zombiehuman.dothome.co.kr/SignUp.php";
        m_saveCharUrl = "http://zombiehuman.dothome.co.kr/CharPos.php";
    }

    // Update is called once per frame
    void Update()
    {
        if (0.0f < m_msTimer)                   //메세지 출력, 삭제
        {
            m_msTimer -= Time.deltaTime;
            if (m_msTimer < 0.0f)
                MessageOnOff("", false);
        }

        if (Input.GetKeyDown(KeyCode.Tab) ||        //tab키, enter키로 커서 옮기기
            (Input.GetKeyDown(KeyCode.Return) && !EventSystem.current.currentSelectedGameObject.name.Contains("Btn")))
        {
            Selectable nextSelect = EventSystem.current.currentSelectedGameObject.          //현재 선택되어있는 게임오브젝트의 Select On Down 부분 가져오기
                                    GetComponent<Selectable>().FindSelectableOnDown();

            if (nextSelect != null)
                nextSelect.Select();         //Select On Down에 등록된 오브젝트 선택하기
        }
    }

    void LogIn()
    {
        string a_idStr = m_id_InputField.text.Trim();
        string a_pwStr = m_pw_InputField.text.Trim();

        if (a_idStr == "" || a_pwStr == "")
        {
            MessageOnOff("ID와 PW를 빈칸없이 입력해야 합니다.");
            return;
        }

        if (!(3 <= a_idStr.Length && a_idStr.Length <= 10))
        {
            MessageOnOff("ID는 3글자 이상 10글자 이하로 입력해 주세요.");
            return;
        }

        if (!(4 <= a_pwStr.Length && a_pwStr.Length <= 15))
        {
            MessageOnOff("PW는 4글자 이상 15글자 이하로 입력해 주세요.");
            return;
        }

        StartCoroutine(LoginCo(a_idStr, a_pwStr));
    }

    IEnumerator LoginCo(string idStr, string pwStr)
    {
        GlobalValue.g_Unique_ID = "";

        WWWForm wForm = new WWWForm();

        wForm.AddField("Input_user", idStr, System.Text.Encoding.UTF8);
        wForm.AddField("Input_pass", pwStr, System.Text.Encoding.UTF8);


        UnityWebRequest wRequest = UnityWebRequest.Post(m_logInUrl, wForm);

        yield return wRequest.SendWebRequest();

        if (wRequest.error == null)
        {
            System.Text.Encoding enc = System.Text.Encoding.UTF8;
            string sz = enc.GetString(wRequest.downloadHandler.data);

            if (!sz.Contains("Login-Success!!") || !sz.Contains("{\""))      //로그인에 실패했거나 JSON형식이 아닐 경우
            {
                ErrorMsg(sz);
                yield break;
            }

            string a_getStr = sz.Substring(sz.IndexOf("{\""));

            var N = JSON.Parse(a_getStr);
            if (N == null)
                yield break;

            GlobalValue.g_Unique_ID = idStr;    //글로벌 변수에 저장
            GlobalValue.InitData();             //기본 아이템 목록 생성


            if (N["Char_pos"] != null)
            {
                JSONNode c_JS = JSON.Parse(N["Char_pos"]);
                string[] CharPos = new string[2];

                CharPos[0] = c_JS[0];
                CharPos[1] = c_JS[1];
                Debug.Log(CharPos[0] + ":" + CharPos[1]);
                float a = (float.Parse(CharPos[0]));
                float b = (float.Parse(CharPos[1]));

                GlobalValue.g_CharPos = new Vector3(a, (float)100.1, b);
            }
            else
            {
                GlobalValue.g_CharPos = new Vector3((float)-838.7, (float)100.08, (float)983.72);
            }

            if (N["nick_name"] != null)
                GlobalValue.g_NickName = N["nick_name"];
            //if (N["best_score"] != null)
            //    GlobalValue.g_BestScore = N["best_score"].AsInt;
            //if (N["gold"] != null)
            //    GlobalValue.g_UserGold = N["gold"].AsInt;
            if (N["info1"] != null && N["info2"] != null && N["info3"] != null)         //기존 유저일 경우
            {
                string[] a_strJson = new string[3];
                a_strJson[0] = N["info1"];          //아이템 이름
                a_strJson[1] = N["info2"];          //장전된 총알 수
                a_strJson[2] = N["info3"];          //남은 총알 수

                if (a_strJson[0] != "" && a_strJson[1] != "" && a_strJson[2] != "")
                {
                    JSONNode[] a_JS = new JSONNode[3];
                    for (int i = 0; i < a_JS.Length; i++)
                        a_JS[i] = JSON.Parse(a_strJson[i]);

                    for (int j = 0; j < a_JS[0].Count; j++)
                        if (j < GlobalValue.g_equippedItem.Count)
                        {
                            GlobalValue.g_equippedItem[j].SetType((ItemName)System.Enum.Parse(typeof(ItemName), a_JS[0][j]));
                            GlobalValue.g_equippedItem[j].m_curMagazine = a_JS[1][j];
                            GlobalValue.g_equippedItem[j].m_maxMagazine = a_JS[2][j];
                        }
                        else
                        {
                            GlobalValue.g_userItem[j - 3].SetType((ItemName)System.Enum.Parse(typeof(ItemName), a_JS[0][j]));
                            GlobalValue.g_userItem[j - 3].m_curMagazine = a_JS[1][j];
                            GlobalValue.g_userItem[j - 3].m_maxMagazine = a_JS[2][j];
                        }
                }
            }
            if (N["config"] != null)            //기존 유저라면
            {
                JSONNode a_JS = JSON.Parse(N["config"]);

                foreach (var a_img in m_soundSprite)
                {
                    if (a_img.name == a_JS[0])
                        GlobalValue.g_cfBGImg = a_img;
                    if (a_img.name == a_JS[2])
                        GlobalValue.g_cfEffImg = a_img;
                }
                GlobalValue.g_cfBGValue = float.Parse(a_JS[1]);
                GlobalValue.g_cfEffValue = float.Parse(a_JS[3]);
            }
            else //if (N["config"] == null)
            {

                GlobalValue.g_StandUpAnim = 1;
                GlobalValue.g_cfBGImg = m_soundSprite[2];       //최대 음량 이미지
                GlobalValue.g_cfEffImg = m_soundSprite[2];      //최대 음량 이미지
            }

            SceneManager.LoadScene("InGameScene");
        }
        else
        {
            ErrorMsg(wRequest.error);      //에러 표시
        }
    }

    void SignUp()
    {
        string a_idStr = m_newid_InputField.text.Trim();
        string a_pwStr = m_newpw_InputField.text.Trim();
        string a_nickStr = m_newnick_InputField.text.Trim();

        if (a_idStr == "" || a_pwStr == "" || a_nickStr == "")
        {
            MessageOnOff("ID와 PW, NickName을 빈칸없이 입력해야 합니다.");
            return;
        }

        if (!(3 <= a_idStr.Length && a_idStr.Length <= 10))
        {
            MessageOnOff("ID는 3글자 이상 10글자 이하로 입력해 주세요.");
            return;
        }

        if (!(4 <= a_pwStr.Length && a_pwStr.Length <= 15))
        {
            MessageOnOff("PW는 4글자 이상 15글자 이하로 입력해 주세요.");
            return;
        }

        if (!(2 <= a_nickStr.Length && a_nickStr.Length <= 6))
        {
            MessageOnOff("NickName은 2글자 이상 6글자 이하로 입력해 주세요.");
            return;
        }

        StartCoroutine(CreateCo(a_idStr, a_pwStr, a_nickStr));
    }

    IEnumerator CreateCo(string idStr, string pwStr, string nickStr)
    {
        WWWForm wForm = new WWWForm();
        wForm.AddField("Input_user", idStr, System.Text.Encoding.UTF8);
        wForm.AddField("Input_pass", pwStr);
        wForm.AddField("Input_nick", nickStr, System.Text.Encoding.UTF8);

        UnityWebRequest wRequest = UnityWebRequest.Post(m_signUpUrl, wForm);
        yield return wRequest.SendWebRequest();

        if (wRequest.error == null)
        {
            System.Text.Encoding enc = System.Text.Encoding.UTF8;
            string sz = enc.GetString(wRequest.downloadHandler.data);

            if (sz.Contains("Create Success."))
            {
                MessageOnOff("계정이 생성되었습니다.");
                LogInPanelOn();
            }
            else
            {
                ErrorMsg(sz);       //동일한 ID, NickName이 있을 경우 오류 메시지 표시
                yield break;
            }
        }
        else
            ErrorMsg(wRequest.error);
    }


    #region 판넬 변경
    void LogInPanelOn()
    {
        m_signUpPanel.SetActive(false);
        m_logInPanel.SetActive(true);
        InputFieldClear();
        m_id_InputField.Select();
    }

    void SignUpPanelOn()
    {
        m_logInPanel.SetActive(false);
        m_signUpPanel.SetActive(true);
        InputFieldClear();
        m_newid_InputField.Select();
    }

    void InputFieldClear()
    {
        m_id_InputField.text = "";
        m_pw_InputField.text = "";
        m_newid_InputField.text = "";
        m_newpw_InputField.text = "";
        m_newnick_InputField.text = "";
    }
    #endregion    

    void MessageOnOff(string Message = "", bool isOn = true)
    {
        if (m_messageText == null)
            return;

        if (isOn == true)
        {
            m_messageText.text = Message;
            m_msTimer = 3.0f;
        }
        else
            m_messageText.text = "";

        m_messageText.gameObject.SetActive(isOn);
    }

    void ErrorMsg(string str)
    {
        if (str.Contains("ID does not exist."))
            MessageOnOff("ID가 존재하지 않습니다.");
        else if (str.Contains("Pass does not Match."))
            MessageOnOff("PW가 일치하지 않습니다.");
        else if (str.Contains("ID does exist."))
            MessageOnOff("같은 ID가 이미 존재합니다.");
        else if (str.Contains("Nickname does exist."))
            MessageOnOff("같은 NickName이 이미 존재합니다.");
        else
            MessageOnOff(str);
    }
}
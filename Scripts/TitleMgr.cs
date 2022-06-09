using System.Collections;
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
    public GameObject m_logInPanel = null;              //�α��� �ǳ� ������Ʈ�� ���� ����
    public InputField m_id_InputField = null;           //�α��� �ǳ��� id inputfield
    public InputField m_pw_InputField = null;           //�α��� �ǳ��� pw inputfield
    public Button m_logIn_Btn = null;                   //�α��� ��ư
    public Button m_signUp_Btn = null;                  //ȸ������ ��ư

    [Header("SignUpPanel")]
    public GameObject m_signUpPanel = null;             //ȸ������ �ǳ� ������Ʈ�� ���� ����
    public InputField m_newid_InputField = null;        //ȸ������ �ǳ��� new id inputfield
    public InputField m_newpw_InputField = null;        //ȸ������ �ǳ��� new pw inputfield
    public InputField m_newnick_InputField = null;      //ȸ������ �ǳ��� new nick inputfield
    public Button m_create_Btn = null;                  //create ��ư
    public Button m_cancel_Btn = null;                  //��� ��ư

    [Header("Message")]
    public Text m_messageText = null;                   //�޼����� �����ֱ� ���� �ؽ�Ʈ
    private float m_msTimer = 0.0f;                     //�޼��� ���� Ÿ�̸�

    string m_logInUrl;                                  //��Ȩ�� �α��� php url
    string m_signUpUrl;                                 //��Ȩ�� ȸ������ php url


    // Start is called before the first frame update
    void Start()
    {
        m_id_InputField.Select();                       //ó���� �α��� �ǳ� id inputfield�� Ŀ�� ����

        if (m_logIn_Btn != null)
            m_logIn_Btn.onClick.AddListener(LogIn);        

        if (m_signUp_Btn != null)
            m_signUp_Btn.onClick.AddListener(SignUpPanelOn);

        if (m_cancel_Btn != null)
            m_cancel_Btn.onClick.AddListener(LogInPanelOn);

        if (m_create_Btn != null)
            m_create_Btn.onClick.AddListener(SignUp);

        m_logInUrl = "http://dhosting.dothome.co.kr/LogIn.php";
        m_signUpUrl = "http://dhosting.dothome.co.kr/SignUp.php";
    }

    // Update is called once per frame
    void Update()
    {        
        if (0.0f < m_msTimer)                   //�޼��� ���, ����
        {
            m_msTimer -= Time.deltaTime;
            if (m_msTimer < 0.0f)            
                MessageOnOff("", false);                           
        }

        if (Input.GetKeyDown(KeyCode.Tab) ||        //tabŰ, enterŰ�� Ŀ�� �ű��
            (Input.GetKeyDown(KeyCode.Return) && !EventSystem.current.currentSelectedGameObject.name.Contains("Btn")))
        {            
            Selectable nextSelect = EventSystem.current.currentSelectedGameObject.          //���� ���õǾ��ִ� ���ӿ�����Ʈ�� Select On Down �κ� ��������
                                    GetComponent<Selectable>().FindSelectableOnDown();

            if (nextSelect != null)            
                nextSelect.Select();         //Select On Down�� ��ϵ� ������Ʈ �����ϱ�
        }
    }

    void LogIn()
    {
        string a_idStr = m_id_InputField.text.Trim();
        string a_pwStr = m_pw_InputField.text.Trim();

        if(a_idStr == "" || a_pwStr == "")
        {
            MessageOnOff("ID�� PW�� ��ĭ���� �Է��ؾ� �մϴ�.");
            return;
        }

        if(!(3 <= a_idStr.Length && a_idStr.Length <= 10))
        {
            MessageOnOff("ID�� 3���� �̻� 10���� ���Ϸ� �Է��� �ּ���.");
            return;
        }

        if(!(4 <= a_pwStr.Length && a_pwStr.Length <= 15))
        {
            MessageOnOff("PW�� 4���� �̻� 15���� ���Ϸ� �Է��� �ּ���.");
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

            if (!sz.Contains("Login-Success!!") || !sz.Contains("{\""))      //�α��ο� �����߰ų� JSON������ �ƴ� ���
            {
                Debug.Log(sz);
                //ErrorMsg(sz);
                yield break;
            }

            string a_getStr = sz.Substring(sz.IndexOf("{\""));

            var N = JSON.Parse(a_getStr);
            if (N == null)
                yield break;

            GlobalValue.g_Unique_ID = idStr;    //�۷ι� ������ ����

            if (N["nick_name"] != null)
                GlobalValue.g_NickName = N["nick_name"];
            if (N["best_score"] != null)
                GlobalValue.g_BestScore = N["best_score"].AsInt;
            if (N["gold"] != null)
                GlobalValue.g_UserGold = N["gold"].AsInt;

            SceneManager.LoadScene("SampleScene");
        }
        else        
            ErrorMsg(wRequest.error);      //���� ǥ��               
    }

    void SignUp()
    {
        string a_idStr = m_newid_InputField.text.Trim();
        string a_pwStr = m_newpw_InputField.text.Trim();
        string a_nickStr = m_newnick_InputField.text.Trim();

        if (a_idStr == "" || a_pwStr == "" || a_nickStr == "")
        {
            MessageOnOff("ID�� PW, NickName�� ��ĭ���� �Է��ؾ� �մϴ�.");
            return;
        }

        if (!(3 <= a_idStr.Length && a_idStr.Length <= 10))
        {
            MessageOnOff("ID�� 3���� �̻� 10���� ���Ϸ� �Է��� �ּ���.");
            return;
        }

        if (!(4 <= a_pwStr.Length && a_pwStr.Length <= 15))
        {
            MessageOnOff("PW�� 4���� �̻� 15���� ���Ϸ� �Է��� �ּ���.");
            return;
        }

        if (!(2 <= a_nickStr.Length && a_nickStr.Length <= 6))
        {
            MessageOnOff("NickName�� 2���� �̻� 6���� ���Ϸ� �Է��� �ּ���.");
            return;
        }

        StartCoroutine(CreateCo(a_idStr, a_pwStr, a_nickStr));
    }

    IEnumerator CreateCo(string idStr, string pwStr, string nickStr)
    {
        WWWForm wForm = new WWWForm();
        wForm.AddField("Input_user", idStr, System.Text.Encoding.UTF8);
        wForm.AddField("Input_pass", pwStr, System.Text.Encoding.UTF8);
        wForm.AddField("Input_nick", nickStr, System.Text.Encoding.UTF8);

        UnityWebRequest wRequest = UnityWebRequest.Post(m_signUpUrl, wForm);
        yield return wRequest.SendWebRequest();

        if (wRequest.error == null)
        {
            System.Text.Encoding enc = System.Text.Encoding.UTF8;
            string sz = enc.GetString(wRequest.downloadHandler.data);

            if (sz.Contains("Create Success."))
            {
                MessageOnOff("������ �����Ǿ����ϴ�.");
                LogInPanelOn();
            }
            else
            {
                ErrorMsg(sz);                
                yield break;
            }
        }
        else        
            ErrorMsg(wRequest.error);
    }


    #region �ǳ� ����
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

        if(isOn == true)
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
            MessageOnOff("ID�� �������� �ʽ��ϴ�.");
        else if (str.Contains("Pass does not Match."))
            MessageOnOff("PW�� ��ġ���� �ʽ��ϴ�.");
        else if (str.Contains("ID does exist."))
            MessageOnOff("���� ID�� �̹� �����մϴ�.");
        else if (str.Contains("Nickname does exist."))
            MessageOnOff("���� NickName�� �̹� �����մϴ�.");        
        else
            MessageOnOff(str);
    }
}

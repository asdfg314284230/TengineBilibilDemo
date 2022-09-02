using TEngine;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class AuthLoginUI : UIWindow
{
    #region 脚本工具生成的代码
    private Image m_imgbg;
    private Text m_textTittle;
    private Text m_textVer;
    private Text m_textGongGao;
    private GameObject m_goLoginRoot;
    private InputField m_inputName;
    private InputField m_inputPassword;
    private Button m_btnLogin;
    private Button m_btnUnlock;
    protected override void ScriptGenerator()
    {
        m_imgbg = FindChildComponent<Image>("m_imgbg");
        m_textTittle = FindChildComponent<Text>("m_textTittle");
        m_textVer = FindChildComponent<Text>("m_textVer");
        m_textGongGao = FindChildComponent<Text>("m_textGongGao");
        m_goLoginRoot = FindChild("m_goLoginRoot").gameObject;
        m_inputName = FindChildComponent<InputField>("m_goLoginRoot/m_inputName");
        m_inputPassword = FindChildComponent<InputField>("m_goLoginRoot/m_inputPassword");
        m_btnLogin = FindChildComponent<Button>("m_goLoginRoot/m_btnLogin");
        m_btnUnlock = FindChildComponent<Button>("m_goLoginRoot/m_btnUnlock");
        m_btnLogin.onClick.AddListener(OnClickLoginBtn);
        m_btnUnlock.onClick.AddListener(OnClickUnlockBtn);
    }
    #endregion

    protected override void BindMemberProperty()
    {
        base.BindMemberProperty();
        //TLogger.LogInfo("TEngineUI BindMemberProperty");
    }

    protected override void OnCreate()
    {
        base.OnCreate();

        //TLogger.LogInfo("TEngineUI OnCreate");
    }

    protected override void OnVisible()
    {
        base.OnVisible();

        // 先获取一下公告信息
        m_textGongGao.text = NetWorkAuthSys.Instance.isGetGongGao();

        // 获取下本地缓存信息
        AuthPleyr pleyr = PlayerPrefsDataMgr.Instance.LoadData(typeof(AuthPleyr), "authPlayer") as AuthPleyr;
        m_inputName.text = pleyr.RoomId;
        m_inputPassword.text = pleyr.PassWord;
    }

    protected override void OnUpdate()
    {
        //TEngineHotUpdate.GameLogicMain.Update();
        //TLogger.LogInfo("TEngineUI OnUpdate");
    }

    #region 事件

    /// <summary>
    /// 登录
    /// </summary>
    void OnClickLoginBtn()
    {
        if (m_inputPassword.text != string.Empty)
        {
            // 第三个参数代表强制过卡密系统
            bool isGet = NetWorkAuthSys.Instance.isLoginPass(m_inputPassword.text, m_inputName.text, true);

            if (isGet)
            {
                // 跳转场景
                TLogger.LogInfo("登录成功。");

                // 存储下房间号
                GameModel.Instance.RoomId = int.Parse(m_inputName.text);

                UISys.Mgr.CloseWindow<AuthLoginUI>();
                // 打开新的UI界面
                UISys.Mgr.ShowWindow<MainUI>(true);
            }
            else
            {
                TLogger.LogError("卡密验证失败");
            }

        }
    }

    /// <summary>
    /// 解绑
    /// </summary>
    void OnClickUnlockBtn()
    {
        bool isGet = NetWorkAuthSys.Instance.isUnlock(m_inputPassword.text);
    }

    #endregion

}
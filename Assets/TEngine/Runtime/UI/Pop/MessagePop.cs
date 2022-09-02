using TEngine;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class MessagePop : UIWindow
{
    #region 脚本工具生成的代码
    private Button m_btnClose;
    private Image m_imgbg;
    private Text m_textInfo;
    protected override void ScriptGenerator()
    {
        m_btnClose = FindChildComponent<Button>("m_btnClose");
        m_imgbg = FindChildComponent<Image>("m_imgbg");
        m_textInfo = FindChildComponent<Text>("m_textInfo");
        m_btnClose.onClick.AddListener(OnClickCloseBtn);
    }
    #endregion

    protected override void BindMemberProperty()
    {
        base.BindMemberProperty();
    }

    protected override void OnCreate()
    {
        base.OnCreate();
    }

    protected override void OnVisible()
    {
        base.OnVisible();
    }


    /// <summary>
    /// 设置报错信息
    /// </summary>
    /// <param name="info"></param>
    public void SetLogInfo(string info)
    {
        m_textInfo.text = info;
    }


    #region 事件

    /// <summary>
    /// 登录
    /// </summary>
    void OnClickCloseBtn()
    {
        UIManager.Instance.CloseWindow("MessagePop");
    }



    #endregion

}

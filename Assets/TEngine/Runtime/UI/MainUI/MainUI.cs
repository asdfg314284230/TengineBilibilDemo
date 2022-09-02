using TEngine;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Liluo.BiliBiliLive;
using System.Collections;
using System.Collections.Generic;


public class MainUI : UIWindow
{
    // Start is called before the first frame update

    #region 脚本工具生成的代码
    private Image m_imgbg;
    private Text m_textTittle;
    private ScrollRect m_scroll;
    private RectTransform m_rectContent;
    private Text m_text_TextInfo;
    protected override void ScriptGenerator()
    {
        m_imgbg = FindChildComponent<Image>("m_imgbg");
        m_textTittle = FindChildComponent<Text>("m_textTittle");
        m_scroll = FindChildComponent<ScrollRect>("m_scroll");
        m_rectContent = FindChildComponent<RectTransform>("m_scroll/Viewport/m_rectContent");
        m_text_TextInfo = FindChildComponent<Text>("m_scroll/Viewport/m_rectContent/m_text_TextInfo");
    }
    #endregion



    // B站第三方接口
    IBiliBiliLiveRequest req;

    protected override void BindMemberProperty()
    {
        base.BindMemberProperty();
        //TLogger.LogInfo("TEngineUI BindMemberProperty");
    }

    protected override void OnCreate()
    {
        base.OnCreate();
        //TLogger.LogInfo("TEngineUI OnCreate");

        // 开始创建监听
        InitBiliBiliSocket();
    }

    protected override void OnVisible()
    {
        base.OnVisible();

    }

    protected override void OnUpdate()
    {
        //TEngineHotUpdate.GameLogicMain.Update();
        //TLogger.LogInfo("TEngineUI OnUpdate");
    }


    async void InitBiliBiliSocket()
    {
        // 创建一个监听对象
        req = await BiliBiliLive.Connect(GameModel.Instance.RoomId);
        req.OnDanmuCallBack += GetDanmu;
        req.OnGiftCallBack += GetGift;
        req.OnSuperChatCallBack += GetSuperChat;
        bool flag = true;
        req.OnRoomViewer += number =>
        {
            // 仅首次显示
            if (flag) TLogger.LogInfo($"当前房间人数为: {number}");
        };
    }


    // UI
    void CreateItem(string info)
    {
        var obj = GameObject.Instantiate(m_text_TextInfo.gameObject);
        obj.gameObject.SetActive(true);
        obj.GetComponent<Text>().text = info;
        obj.transform.SetParent(m_rectContent);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
    }


    #region 实现弹幕回调的地方

    /// <summary>
    /// 接收到礼物的回调
    /// </summary>
    public async void GetGift(BiliBiliLiveGiftData data)
    {
        TLogger.LogInfo($"<color=#FEA356>礼物</color> 用户名: {data.username}, 礼物名: {data.giftName}, 数量: {data.num}, 总价: {data.total_coin}");
        //img.sprite = await BiliBiliLive.GetHeadSprite(data.userId);
        CreateItem($"<color=#FEA356>礼物</color> 用户名: {data.username}, 礼物名: {data.giftName}, 数量: {data.num}, 总价: {data.total_coin}");
    }

    /// <summary>
    /// 接收到弹幕的回调
    /// </summary>
    public async void GetDanmu(BiliBiliLiveDanmuData data)
    {
        TLogger.LogInfo($"<color=#60B8E0>弹幕</color> 用户名: {data.username}, 内容: {data.content}, 舰队等级: {data.guardLevel}");
        //Sprite img = await BiliBiliLive.GetHeadSprite(data.userId);
        CreateItem($"<color=#60B8E0>弹幕</color> 用户名: {data.username}, 内容: {data.content}, 舰队等级: {data.guardLevel}");
    }

    /// <summary>
    /// 接收到SC的回调
    /// </summary>
    public async void GetSuperChat(BiliBiliLiveSuperChatData data)
    {
        //Debug.Log($"<color=#FFD766>SC</color> 用户名: {data.username}, 内容: {data.content}, 金额: {data.price}");
        //img.sprite = await BiliBiliLive.GetHeadSprite(data.userId);
    }

    #endregion

}

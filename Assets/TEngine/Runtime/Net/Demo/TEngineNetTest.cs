using TEngine;
using TEngine.Net;
using TEngineProto;
using UI;

public class TEngineNetTest : TEngineEntry
{
    /// <summary>
    /// 注册系统
    /// </summary>
    protected override void RegisterAllSystem()
    {
        base.RegisterAllSystem();
        //注册系统，例如UI系统，网络系统，战斗系统等等
        AddLogicSys(BehaviourSingleSystem.Instance);
        AddLogicSys(UISys.Instance);
        AddLogicSys(DataCenterSys.Instance);

        DataCenterSys.Instance.InitModule(UserDataCenter.Instance);
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GameClient.Instance.ConnectAsync("127.0.0.1", 54809);

        UISys.Mgr.ShowWindow<TEngineLoginUI>(true);
    }
}

public class UserDataCenter : DataCenterModule<UserDataCenter>
{
    public override void Init()
    {
        GameClient.Instance.RegActionHandle((int)ActionCode.Login,LoginCallBack);
        base.Init();
    }

    private void LoginCallBack(MainPack mainPack)
    {
        TLogger.LogInfo(mainPack.ToString());
        if (!string.IsNullOrEmpty(mainPack.Extstr))
        {
            TLogger.LogError(mainPack.Extstr);
            return;
        }
    }
}

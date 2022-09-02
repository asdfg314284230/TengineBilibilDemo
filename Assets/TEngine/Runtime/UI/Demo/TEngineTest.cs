
using System.Collections;
using TEngine;
using UI;
using UnityEngine;
using TEngine.Net;

public class TEngineTest : TEngineEntry
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


        // 数据中心系统
        AddLogicSys(DataCenterSys.Instance);
        // 注册卡密系统
        AddLogicSys(NetWorkAuthSys.Instance);


    }

    protected override void OnLoad()
    {
        base.OnLoad();

        ObjMgr.Instance.Active();

        //UISys.Mgr.ShowWindow<TEngineUI>(true);
        //UISys.Mgr.ShowWindow<MsgUI>(true);



        // 打开卡密登录界面
        UISys.Mgr.ShowWindow<AuthLoginUI>(true);
    }
}

public class ObjMgr : TSingleton<ObjMgr>
{
    public override void Active()
    {
        //外部注入Update
        MonoUtility.AddUpdateListener(Update);

        GameEventMgr.Instance.Send<string>(TipsEvent.Log, "WelCome To Use TEngine");
    }

    private void Update()
    {

    }
}

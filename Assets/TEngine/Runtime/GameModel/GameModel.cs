using TEngine;
using TEngine.Net;
using TEngineProto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameModel : DataCenterModule<GameModel>
{

    public int RoomId = 0;

    public Dictionary<RoleData, GameObject> roleDic = new Dictionary<RoleData, GameObject>();


    #region 实现的基本接口

    public override void Init()
    {
        base.Init();
    }

    public override void OnRoleLogout()
    {
        base.OnRoleLogout();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnMainPlayerMapChange()
    {
        base.OnMainPlayerMapChange();
    }

    #endregion
}


/// <summary>
/// 玩家实际数据类
/// </summary>
public class RoleData
{
    public string Name;
}
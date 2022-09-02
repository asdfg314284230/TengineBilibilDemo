using System.Collections.Generic;

namespace TEngine.Net
{
    /// <summary>
    /// 数据中心系统
    /// </summary>
    public class DataCenterSys : BaseLogicSys<DataCenterSys>
    {
        private List<IDataCenterModule> m_listModule = new List<IDataCenterModule>();

        public override bool OnInit()
        {
            RegCmdHandle();
            InitModule();
            return true;
        }

        public override void OnUpdate()
        {
            GameClient.Instance.OnUpdate();
            var listModule = m_listModule;
            for (int i = 0; i < listModule.Count; i++)
            {
                listModule[i].OnUpdate();
            }
        }

        public override void OnDestroy()
        {
            GameClient.Instance.Shutdown();
            base.OnDestroy();
        }

        private void RegCmdHandle()
        {
            var client = GameClient.Instance;
            //client.RegActionHandle();
        }

        /// <summary>
        /// 常驻初始化的数据类的地方
        /// </summary>
        void InitModule()
        {
            //InitModule(LoginDataMgr.Instance);
            InitModule(GameModel.Instance);
        }

        /// <summary>
        /// 手动添加
        /// </summary>
        /// <param name="module"></param>
        public void InitModule(IDataCenterModule module)
        {
            if (!m_listModule.Contains(module))
            {
                module.Init();
                m_listModule.Add(module);
            }
        }
    }
}

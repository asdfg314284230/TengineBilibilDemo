﻿using System.Collections.Generic;
using UnityEngine;

namespace TEngine
{
    public class TEngineEntry : UnitySingleton<TEngineEntry>
    {
        protected override void OnLoad()
        {
            InitLibImp();

            RegisterAllSystem();
        }

        /// <summary>
        /// 注册实现库
        /// </summary>
        protected virtual void InitLibImp()
        {
            
        }

        /// <summary>
        /// 注册系统（例如BaseLogic/TEngineObject/MonoManger）
        /// </summary>
        protected virtual void RegisterAllSystem()
        {
            
        }

        protected void SetTargetFrameRate(int targetFrameRate)
        {
            Application.targetFrameRate = targetFrameRate;
        }

        //-------------------------------------------------------系统注册--------------------------------------------------------//
        private List<ILogicSys> m_LogicMgrList = new List<ILogicSys>();

        public bool ContainLogicSys(ILogicSys logicSys)
        {
            return m_LogicMgrList.Contains(logicSys);
        }

        /// <summary>
        /// 系统注册
        /// </summary>
        /// <param name="logicSys"></param>
        /// <returns></returns>
        public bool AddLogicSys(ILogicSys logicSys)
        {
            if (m_LogicMgrList.Contains(logicSys))
            {
                TLogger.LogInfo("Repeat add logic system: " + logicSys.GetType().Name);
                return false;
            }

            if (!logicSys.OnInit())
            {
                TLogger.LogInfo(" Init failed " + logicSys.GetType().Name);
                return false;
            }

            m_LogicMgrList.Add(logicSys);

            return true;
        }

        /// <summary>
        /// 系统注销
        /// </summary>
        /// <param name="logicSys"></param>
        /// <returns></returns>
        public void RemoveLogicSys(ILogicSys logicSys)
        {
            if (m_LogicMgrList.Contains(logicSys))
            {
                TLogger.LogInfo("Remove logic system: " + logicSys.GetType().Name);

                logicSys.OnDestroy();

                m_LogicMgrList.Remove(logicSys);

                logicSys = null;
            }
        }

        #region 生命周期
        public void Start()
        {
            GameTime.Start();
            GameTime.StartFrame();
            var listLogic = m_LogicMgrList;
            var logicCnt = listLogic.Count;
            for (int i = 0; i < logicCnt; i++)
            {
                var logic = listLogic[i];
                logic.OnStart();
            }
        }

        public void Update()
        {
            GameTime.StartFrame();
            var listLogic = m_LogicMgrList;
            var logicCnt = listLogic.Count;
            for (int i = 0; i < logicCnt; i++)
            {
                var logic = listLogic[i];
                logic.OnUpdate();
            }
        }

        public void LateUpdate()
        {
            GameTime.StartFrame();
            var listLogic = m_LogicMgrList;
            var logicCnt = listLogic.Count;
            for (int i = 0; i < logicCnt; i++)
            {
                var logic = listLogic[i];
                logic.OnLateUpdate();
            }
        }

        public void OnPause()
        {
            GameTime.StartFrame();
            for (int i = 0; i < m_LogicMgrList.Count; i++)
            {
                var logicSys = m_LogicMgrList[i];
                logicSys.OnPause();
            }
        }

        public void OnResume()
        {
            GameTime.StartFrame();
            for (int i = 0; i < m_LogicMgrList.Count; i++)
            {
                var logicSys = m_LogicMgrList[i];
                logicSys.OnResume();
            }
        }

        protected override void OnDestroy()
        {
            GameTime.StartFrame();
            for (int i = 0; i < m_LogicMgrList.Count; i++)
            {
                var logicSys = m_LogicMgrList[i];
                logicSys.OnDestroy();
            }
            base.OnDestroy();
            SingletonMgr.Release();
        }
        #endregion
    }
}
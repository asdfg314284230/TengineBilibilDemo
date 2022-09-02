using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TEngine;
using UnityEngine.Networking;
using System.Text;
using UI;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace TEngine
{
    /// <summary>
    /// 卡密验证系统
    /// </summary>
    public class NetWorkAuthSys : BaseLogicSys<NetWorkAuthSys>
    {
        // 默认访问配置
        public static string AppId
        {
            get { return "20220806"; }
        }

        public static string M
        {
            get { return "e2e6f5c4fef2a8221f201952b8a979c2"; }
        }

        public static string Key
        {
            get { return "39d405ff762e2db8c7b4145cc7d425a0"; }
        }




        public override void OnUpdate()
        {

        }


        public override bool OnInit()
        {
            base.OnInit();
            return true;
        }


        #region 辅助工具


        /// <summary>
        /// 返回机器特征码
        /// </summary>
        /// <returns></returns>
        public string GetMachineID() { return SystemInfo.deviceUniqueIdentifier; }


        /// <summary>
        /// 返回MD5序列
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string GetMD5Hash(string input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }



        /// <summary>
        /// 获取服务器时间轴
        /// </summary>
        /// <returns></returns>
        public string GetServerTime(string pass)
        {
            //获取当前系统时间
            System.DateTime dt = System.DateTime.Now;
            //将系统时间转换成字符串
            string strTime = dt.ToString("yyyy-MM-dd HH:mm:ss");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("api", "date.in"); // 固定调用登录API
            dic.Add("appid", AppId);// APPID
            dic.Add("m", M); // 软件特征码
            dic.Add("BSphpSeSsl", "6666"); // 固定
            dic.Add("date", strTime);
            dic.Add("md5", "");
            dic.Add("mutualkey", Key);// 通信Key
            dic.Add("appsafecode", "");
            dic.Add("sgin", "");
            dic.Add("icid", pass); // 激活码
            dic.Add("icpwd", "");
            var json = Get("http://116.205.162.8/AppEn.php", dic);
            var mj = JObject.Parse(json);
            return mj["response"]["date"].ToString();
        }


        #endregion


        /// <summary>
        /// 获取公告信息
        /// </summary>
        /// <returns></returns>
        public string isGetGongGao()
        {
            //获取当前系统时间
            System.DateTime dt = System.DateTime.Now;
            //将系统时间转换成字符串
            string strTime = dt.ToString("yyyy-MM-dd HH:mm:ss");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("api", "gg.in"); // 固定调用登录API
            dic.Add("appid", AppId);// APPID
            dic.Add("m", M); // 软件特征码
            dic.Add("BSphpSeSsl", "6666"); // 固定
            dic.Add("date", strTime);
            dic.Add("md5", "");
            dic.Add("mutualkey", Key);// 通信Key
            dic.Add("appsafecode", "");
            dic.Add("sgin", "");
            dic.Add("icid", ""); // 激活码
            dic.Add("icpwd", "");
            var json = Get("http://116.205.162.8/AppEn.php", dic);
            var mj = JObject.Parse(json);

            string str = mj["response"]["data"].ToString();
            return str;

        }

        /// <summary>
        /// 请求访问卡密系统
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public bool isLoginPass(string pass, string roomId, bool isGet = false)
        {
            string strTime1 = GetServerTime(pass);
            string md = GetMD5Hash(UnityEngine.Random.Range(0, 10000).ToString());

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("api", "login.ic"); // 固定调用登录API
            dic.Add("appid", AppId);// APPID
            dic.Add("m", M); // 软件特征码
            dic.Add("BSphpSeSsl", "6666"); // 固定
            dic.Add("date", strTime1);
            dic.Add("md5", "");
            dic.Add("appsafecode", md);
            dic.Add("sgin", "");
            dic.Add("mutualkey", Key);// 通信Key
            dic.Add("icid", pass); // 激活码
            dic.Add("icpwd", "");
            dic.Add("key", GetMachineID());
            dic.Add("maxoror", GetMachineID());
            var json = Get("http://116.205.162.8/AppEn.php", dic);
            var mj = JObject.Parse(json);

            string str = mj["response"]["data"].ToString();
            if (str.Split('|')[0] == "01")
            {
                if (isGet)
                {
                    // 强制过卡密
                }
                else
                {
                    // 验证时间戳
                    if (mj["response"]["date"].ToString() != strTime1)
                    {
                        // 老封包,处理封号操作
                        //Debug.LogError("封包对不上" + mj["response"]["date"].ToString() + "!!!" + strTime1);
                        MessagePop pop = UISys.Mgr.ShowWindow<MessagePop>(true);
                        pop.SetLogInfo("封包对不上：" + mj["response"]["date"].ToString() + "!!!" + strTime1);
                        return false;
                    }

                    if (mj["response"]["appsafecode"].ToString() != md)
                    {
                        // MD5不一样
                        //Debug.LogError("md5对不上");
                        MessagePop pop = UISys.Mgr.ShowWindow<MessagePop>(true);
                        pop.SetLogInfo("MD5对不上");
                        return false;
                    }
                }




                // 本地化操作
                AuthPleyr authPleyr = new AuthPleyr();
                authPleyr.RoomId = roomId;
                authPleyr.PassWord = pass;
                PlayerPrefsDataMgr.Instance.SaveData(authPleyr, "authPlayer");

                return true;
            }
            else
            {
                // 这里应该抛出一个消息,给外部的弹窗信息
                if (isGet)
                {
                    // 强制过卡密
                    return true;
                }
                else
                {
                    MessagePop pop = UISys.Mgr.ShowWindow<MessagePop>(true);
                    pop.SetLogInfo(str);
                }

                return false;
            }
        }


        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="pass"></param>
        /// <returns></returns>
        public bool isUnlock(string pass)
        {
            //获取当前系统时间
            System.DateTime dt = System.DateTime.Now;
            //将系统时间转换成字符串
            string strTime = dt.ToString("yyyy-MM-dd HH:mm:ss");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("api", "setcarnot.ic"); // 固定调用登录API
            dic.Add("appid", AppId);// APPID
            dic.Add("m", M); // 软件特征码
            dic.Add("BSphpSeSsl", "6666"); // 固定
            dic.Add("date", strTime);
            dic.Add("md5", "");
            dic.Add("mutualkey", Key);// 通信Key
            dic.Add("appsafecode", "");
            dic.Add("sgin", "");
            dic.Add("icid", pass); // 激活码
            dic.Add("icpwd", "");
            var json = Get("http://116.205.162.8/AppEn.php", dic);
            var mj = JObject.Parse(json);

            string str = mj["response"]["data"].ToString();
            if (str.Split('|')[0] == "01")
            {
                //Info.text = str;
                return true;
            }
            else
            {
                //Info.text = str;
                return false;
            }
        }


        #region 卡密Http访问
        public string Get(string url, Dictionary<string, string> dic)
        {
            string result = "";
            StringBuilder builder = new StringBuilder();
            builder.Append(url);
            if (dic.Count > 0)
            {
                builder.Append("?");
                int i = 0;
                foreach (var item in dic)
                {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    i++;
                }
            }
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(builder.ToString());
            //添加参数
            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
            Stream stream = resp.GetResponseStream();
            try
            {
                //获取内容
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                    //Debug.Log(result);
                    TEngine.TLogger.LogInfo(result);
                }
            }
            finally
            {
                stream.Close();
            }

            return result;

        }

        #endregion
    }


    /// <summary>
    /// 卡密信息实体类
    /// </summary>
    public class AuthPleyr
    {
        public string RoomId;
        public string PassWord;
    }
}
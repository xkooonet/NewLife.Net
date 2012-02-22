﻿using System;
using System.Net;
using System.Net.Sockets;

namespace NewLife.Net.Common
{
    /// <summary>网络地址标识</summary>
    public class NetUri
    {
        #region 属性
        private ProtocolType _ProtocolType;
        /// <summary>协议类型</summary>
        public ProtocolType ProtocolType { get { return _ProtocolType; } set { _ProtocolType = value; _Protocol = value.ToString(); } }

        private String _Protocol;
        /// <summary>协议</summary>
        public String Protocol
        {
            get { return _Protocol; }
            set
            {
                _Protocol = value;
                try
                {
                    _ProtocolType = (ProtocolType)Enum.Parse(typeof(ProtocolType), value, true);
                }
                catch { }
            }
        }

        private IPAddress _Address;
        /// <summary>地址</summary>
        public IPAddress Address { get { return _Address; } set { _Address = value; _Host = value + ""; } }

        private String _Host;
        /// <summary>主机</summary>
        public String Host { get { return _Host; } set { _Host = value; _Address = NetHelper.ParseAddress(value); } }

        private Int32 _Port;
        /// <summary>端口</summary>
        public Int32 Port { get { return _Port; } set { _Port = value; } }

        /// <summary>终结点</summary>
        public IPEndPoint EndPoint
        {
            get { return new IPEndPoint(Address, Port); }
            set
            {
                if (value != null)
                {
                    Address = value.Address;
                    Port = value.Port;
                }
                else
                {
                    Address = null;
                    Port = 0;
                }
            }
        }
        #endregion

        #region 构造
        /// <summary>实例化</summary>
        public NetUri() { }

        /// <summary>实例化</summary>
        /// <param name="uri"></param>
        public NetUri(String uri) { Parse(uri); }

        /// <summary>实例化</summary>
        /// <param name="protocol"></param>
        /// <param name="endpoint"></param>
        public NetUri(ProtocolType protocol, IPEndPoint endpoint)
        {
            ProtocolType = protocol;
            EndPoint = endpoint;
        }

        /// <summary>实例化</summary>
        /// <param name="protocol"></param>
        /// <param name="address"></param>
        /// <param name="port"></param>
        public NetUri(ProtocolType protocol, IPAddress address, Int32 port)
        {
            ProtocolType = protocol;
            Address = address;
            Port = port;
        }
        #endregion

        #region 方法
        static readonly String Sep = "://";

        /// <summary>分析</summary>
        /// <param name="uri"></param>
        public void Parse(String uri)
        {
            if (uri.IsNullOrWhiteSpace()) return;

            // 分析协议
            var p = uri.IndexOf(Sep);
            if (p > 0)
            {
                Protocol = uri.Substring(0, p);
                uri = uri.Substring(p + Sep.Length);
            }

            // 分析端口
            p = uri.IndexOf(":");
            if (p > 0)
            {
                Port = Convert.ToInt32(uri.Substring(p + 1));
                uri = uri.Substring(0, p);
            }

            Host = uri;
        }
        #endregion

        #region 辅助
        /// <summary>已重载。</summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Port > 0)
                return String.Format("{0}://{1}:{2}", Protocol, Host, Port);
            else
                return String.Format("{0}://{1}", Protocol, Host);
        }
        #endregion
    }
}
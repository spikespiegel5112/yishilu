using Fleck;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NLS.Socket
{
    public class NLSSocket
    {
        ///<summary>
        ///待推送消息列表
        ///</summary>
        private static Stack<PushMsg> USER_MSG_STACK;

        ///<summary>
        ///用户ID与连接对应关系
        ///</summary>
        private static Dictionary<string, IWebSocketConnection> USER_DIC;

        ///<summary>
        ///推送开始标识
        ///</summary>
        private static bool STARTING { get; set; } = true;

        ///<summary>
        ///初始化WebSocket
        ///</summary>
        private static void InitWebSocket(string location)
        {
            USER_DIC = new Dictionary<string, IWebSocketConnection>();
            var server = new WebSocketServer(location);

            if (USER_MSG_STACK == null)
            {
                USER_MSG_STACK = new Stack<PushMsg>();
            }
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                };
                socket.OnClose = () =>
                {
                };
                socket.OnMessage = message =>
                {
                    //接收来自连接端的内容
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        string user_tag = $"push_{message}";
                        if (USER_DIC.ContainsKey(user_tag))
                        {
                            USER_DIC[user_tag] = socket;
                        }
                        else
                        {
                            USER_DIC.Add(user_tag, socket);
                        }
                        USER_MSG_STACK.Push(new PushMsg { user_tag = user_tag, push_msg = $"{DateTime.Now}连接成功" });
                    }
                };
                socket.OnBinary = file =>
                {
                };
            });
        }

        ///<summary>
        ///添加推送消息
        ///</summary>
        public static void Push(PushMsg message)
        {
            if (USER_MSG_STACK == null)
            {
                USER_MSG_STACK = new Stack<PushMsg>();
            }
            USER_MSG_STACK.Push(message);
        }

        ///<summary>
        ///开始消息推送服务
        ///</summary>
        public static void Start(string location)
        {
            InitWebSocket(location);
            Task.Run(() =>
            {
                while (STARTING)
                {
                    if (USER_MSG_STACK.Count > 0)
                    {
                        PushMsg message = USER_MSG_STACK.Pop();//从栈顶取出一个元素并删除原有
                        if (message != null)
                        {
                            var connectionInfo = USER_DIC[message.user_tag];
                            if (connectionInfo != null)
                            {
                                connectionInfo.Send(message.push_msg);
                            }
                        }
                    }
                    Thread.Sleep(500);
                }
            });
        }

        ///<summary>
        ///消息推送服务停止
        ///</summary>
        public static void Stop()
        {
            STARTING = false;
        }
    }
}
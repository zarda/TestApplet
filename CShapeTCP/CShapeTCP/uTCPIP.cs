using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.IO;

namespace HTA_Libs
{
    //Connect() 啟動監聽
    //OnClientConnect(iasyncresult) 當客戶端建立連線時叫用
    //WaitForData(socket) 建立連線等待資料傳入
    //OnDataReceived(iasyncresult)  已連線連線並有資料傳入時叫用
    //FindEmptyChannel() 尋找未用的頻道
    //FindActiveChannel() 尋找啟用的頻道

    public class TCPIP
    {

        /// <summary>
        /// from：封包從何處來
        /// to：封包送給誰
        /// time：傳送時間
        /// msg：傳送內容
        /// </summary>
        public class LogNode
        {
            public string from;
            public string to;
            public string time;
            public string msg;
        }

        /// <summary>
        /// 該結構為TCP/IP內部紀錄用
        /// </summary>
        public class TCPIP_History
        {
            public List<LogNode> LogList = new List<LogNode>();
            public int MaxCount = 1000;
            //private System.Object lockThis = new System.Object();

            /// <summary>
            /// 清除封包log紀錄
            /// </summary>
            public Task Clear()
            {
                //lock (lockThis)
                LogList.Clear();
                return Task.CompletedTask;
            }

            /// <summary>
            /// 加入一筆紀錄
            /// </summary>
            /// <param name="node">node為LogNode結構</param>
            public async Task Add(LogNode node)
            {
                await Insert(node);
            }

            /// <summary>
            /// 加入一筆紀錄
            /// </summary>
            /// <param name="source">來源IP</param>
            /// <param name="destination">目的地IP</param>
            /// <param name="msg">訊息</param>
            public async Task Add(string source, string destination, string msg)
            {
                LogNode temp = new LogNode
                {
                    from = source,
                    to = destination,
                    time = DateTime.Now.ToString("yyyy/MM/dd-HH:mm:ss:fff"),
                    msg = msg
                };
                await Insert(temp);
            }

            private Task Insert(LogNode temp)
            {
                //lock (lockThis)
                //{
                LogList.Add(temp);
                if (LogList.Count > MaxCount)
                    LogList.RemoveAt(0);
                //}
                return Task.CompletedTask;
            }

            /// <summary>
            /// 將內部Log轉成字串輸出
            /// </summary>
            /// <returns></returns>
            public Task<string> GetLogSring()
            {
                string ret = "";
                //lock (lockThis)
                //{
                //    for (int i = 0; i < LogList.Count; i++)
                //    {
                //        string logstring = LogList[i].time + "," + LogList[i].msg;
                //        ret = logstring + Environment.NewLine + ret;
                //    }
                //}
                foreach (var item in LogList)
                {
                    string logstring = item.time + "," + item.msg;
                    ret = logstring + Environment.NewLine + ret;
                }
                return Task.FromResult(ret);
            }
        }

        /// <summary>
        /// 自型定義的物件封包的類別
        /// 該結構包含socket與資料buffer
        /// </summary>
        public class SocketPacket
        {
            public System.Net.Sockets.Socket m_currentSocket;
            public byte[] dataBuffer = new byte[1500];
        }

        public class PackageData : EventArgs
        {
            public string Text { get; set; }
            public Socket CurrentSocket { get; set; }
        }

        /// <summary>
        /// 封包剖析器
        /// 1.用來分離封包沾黏，以迴車鍵(\r\n)做切割點
        /// 2.分離出的每一個封包將會觸發OnThresholdReached
        /// 3.OnThresholdReached引入事件PackageData
        /// </summary>
        class PackageParser
        {
            public PackageParser() { }

            /// <summary>
            /// 分析封包的資料，若有黏包則獨立拆開
            /// </summary>
            /// <param name="m_currentSocket">負責接收封包的socket</param>
            /// <param name="raw">該封包的原始資料</param>
            public void Parse(Socket m_currentSocket, string raw)
            {
                string[] Package = raw.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (var item in Package)
                {
                    if (item != "")
                    {
                        PackageData args = new PackageData() { Text = item, CurrentSocket = m_currentSocket };
                        OnThresholdReached(args);
                    }
                }
            }

            protected virtual void OnThresholdReached(PackageData e)
            {
                EventHandler<PackageData> handler = PackageReached;
                handler?.Invoke(this, e);
            }

            public event EventHandler<PackageData> PackageReached;
        }

        public class Client
        {
            public TCPIP_History History;
            private string ip;
            private int port;
            private char delimiterChar = ' ';
            private Socket tcpClient;
            private bool IsConnect;
            private bool bRetry;
            private System.Timers.Timer TimerRetry = new System.Timers.Timer();
            private Object Lock_Retry = new Object();

            /// <summary>
            /// 創建TCP/IP Client端 (base on socket)
            /// </summary>
            public Client()
            {
                History = new TCPIP_History();
                IsConnect = false;
                TimerRetry.Interval = 1000;
                TimerRetry.Enabled = false;
                TimerRetry.Elapsed += new System.Timers.ElapsedEventHandler(TimerReconnection);

            }
            ~Client()
            {
            }

            /// <summary>
            /// Client端的連接阜
            /// </summary>
            public int Port
            {
                get { return port; }
                set { port = value; }
            }
            /// <summary>
            /// Client端的IP位址
            /// </summary>
            public string Ip
            {
                get { return ip; }
                set { ip = value; }
            }
            /// <summary>
            /// 若連線失敗或斷線，Client是否持續嘗試連線
            /// </summary>
            public bool Retry
            {
                get { return bRetry; }
                set { bRetry = value; }
            }
            /// <summary>
            /// Server端的指令分段符號(預設為空白斷行『 』)
            /// Ex：『cmd Test123』將拆成字串『cmd』與『Test123』
            /// </summary>
            public char DelimiterChar
            {
                get { return delimiterChar; }
                set { delimiterChar = value; }
            }

            //自動重新連線
            // private void TimerReconnection(object sender, EventArgs e)
            public async void TimerReconnection(object source, System.Timers.ElapsedEventArgs e)
            {
                //lock (Lock_Retry)
                //{
                //    if (IsConnect == true)
                //    {
                //        TimerRetry.Enabled = false;
                //        return;
                //    }
                //    else
                //    {
                //        bool ret = Connect();
                //        if (ret == true)
                //        {
                //            IsConnect = true;
                //            TimerRetry.Enabled = false;
                //        }
                //    }
                //}
                if (!await Task.Run<bool>(() =>
                {
                    if (IsConnect == true)
                    {
                        TimerRetry.Enabled = false;
                        return true;
                    }
                    else
                    {
                        if (Connect().Result)
                        {
                            IsConnect = true;
                            TimerRetry.Enabled = false;
                        }
                    }
                    return true;
                }))
                {
                    throw new Exception("Unable to clear LogList");
                }
            }

            /// <summary>
            /// 產生client端實體，並進行連線。
            /// 連線成功將回傳true ; 失敗回傳false
            /// </summary>
            /// <returns></returns>
            public Task<bool> Connect()
            {
                try
                {
                    IPAddress ipAddress;
                    if (IPAddress.TryParse(ip, out ipAddress) != true)
                        return Task.FromResult(false);

                    tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    tcpClient.Connect(ip, port);
                    WaitForData(tcpClient);

                    //if (!await Task.Run<bool>(() =>
                    //{
                    //    //Connect to the remote endpoint.
                    //    tcpClient.BeginConnect(new IPEndPoint(ipAddress, port),
                    //         new AsyncCallback(ConnectCallback), tcpClient);
                    //    WaitForData(tcpClient);
                    //    return true;
                    //}).ConfigureAwait(false))
                    //{
                    //    throw new Exception("Unable to Connect tcpClient");
                    //}
                    //Connect to the remote endpoint.
                    //tcpClient.BeginConnect(new IPEndPoint(ipAddress, port),
                    //     new AsyncCallback(ConnectCallback), tcpClient);
                    //WaitForData(tcpClient);

                    IsConnect = true;
                    TimerRetry.Enabled = false;
                    return Task.FromResult(true);
                }
                catch (ArgumentNullException)
                {
                    //MessageBox.Show("ArgumentNullException:{0}" + A);
                    return Task.FromResult(false);
                }
                catch (SocketException)
                {
                    //MessageBox.Show("SocketException:{0}" + S);
                    return Task.FromResult(false);
                }
                catch (Exception)
                {
                    return Task.FromResult(false);
                }
            }

            private static void ConnectCallback(IAsyncResult ar)
            {
                try
                {
                    // Retrieve the socket from the state object.  
                    Socket client = (Socket)ar.AsyncState;

                    // Complete the connection.  
                    client.EndConnect(ar);
                }
                catch (Exception)
                {
                    //throw new Exception(e.ToString());
                }
            }

            /// <summary>
            /// 中斷Client端的連線
            /// </summary>
            /// <returns></returns>
            public Task<bool> Disconnect()
            {

                tcpClient.Close();
                //if (!await Task.Run<bool>(() =>
                //{
                //    tcpClient.Shutdown(SocketShutdown.Both);
                //    tcpClient.Close();
                //    return true;
                //}))
                //{
                //    throw new Exception("Unable to Disconnect tcpClient");
                //}
                IsConnect = false;
                return Task.FromResult(true);
            }

            public virtual void OnDataReceived(IAsyncResult asyn)
            {
                string msg;
                SocketPacket socketData = (SocketPacket)asyn.AsyncState;//取得接受的資料
                try
                {
                    int iRx = 0;//宣告及定義訊息長度
                                //叫用 EndReceive 完成指定的非同步接收作業, 參數asyn 識別要完成的非同步接收作業，並要從其中擷取最終結果。
                    iRx = socketData.m_currentSocket.EndReceive(asyn);
                    //當 bytesRead大於0時表示Server傳遞資料過來 等於0時代表Client"正常"斷線
                    if (iRx <= 0)
                    {
                        //處理斷線動作
                        socketData.m_currentSocket.Shutdown(SocketShutdown.Both);
                        socketData.m_currentSocket.Close();
                        IsConnect = false;
                        if (bRetry == true)
                            TimerRetry.Enabled = true;
                        return;
                    }

                    //以下為將取出的部分轉成Default的編碼方式，有關編碼方式轉換可依需求自行修改
                    byte[] databuff = socketData.dataBuffer;
                    msg = Encoding.Default.GetString(databuff).TrimEnd('\0');

                    PackageParser c = new PackageParser();
                    c.PackageReached += C_PackageReached;
                    c.Parse(socketData.m_currentSocket, msg);
                    //傳回空頻道，函式FindActiveChannel() 列於後，主要是找尋啟用的Socket(Channel)

                    //…分析處理部分略，主要msg訊息的利用和重組…
                    //重新叫用 WaitForData 重新開始接收資料
                    WaitForData(socketData.m_currentSocket);
                }
                catch (SocketException)
                {
                    //…例外處理略...
                    //處理斷線動作
                    socketData.m_currentSocket.Shutdown(SocketShutdown.Both);
                    socketData.m_currentSocket.Close();
                    IsConnect = false;
                    if (bRetry == true)
                        TimerRetry.Enabled = true;
                }
                catch (System.ObjectDisposedException)
                {

                }
            }

            private void C_PackageReached(object sender, PackageData e)
            {
                string msg = e.Text;
                //加入紀錄
                IPEndPoint remoteIpEndPoint = e.CurrentSocket.RemoteEndPoint as IPEndPoint;
                IPEndPoint localIpEndPoint = e.CurrentSocket.LocalEndPoint as IPEndPoint;
                History.Add(remoteIpEndPoint.Address.ToString(), localIpEndPoint.Address.ToString(), msg).Wait();
                //動作處理
                string[] words = msg.Split(delimiterChar);
                string reply = DoWork(words);
                if (reply != "")
                {
                    reply = reply + Environment.NewLine;
                    byte[] reply_data = Encoding.Default.GetBytes(reply);
                    e.CurrentSocket.Send(reply_data);
                    History.Add(localIpEndPoint.Address.ToString(), remoteIpEndPoint.Address.ToString(), msg).Wait();
                }
            }

            //宣告AsyncCallback類別的變數 pfnWorkerCallBack
            private AsyncCallback pfnWorkerCallBack;
            private Task WaitForData(System.Net.Sockets.Socket soc)
            {
                try
                {
                    //當pfnWorkerCallBack物件尚未實體化時，進行實體化
                    if (pfnWorkerCallBack == null)
                    {
                        //假設已連線的RCC客戶端要傳送資料給RCS時，所指定的回呼函式-OnDataReceive，這也是C#中的delagate型別，此函式內容詳述於後
                        pfnWorkerCallBack = new AsyncCallback(OnDataReceived);
                    }
                    //自行定義的型別 SocketPacket，附於此小節尾，內容只有一個Socket類和一個int。
                    //指定此一建立連線之Socket soc 給定義的 theSocPkt
                    SocketPacket theSocPkt = new SocketPacket() { m_currentSocket = soc };

                    // 接下來定義當Socket 接收到資料時要交給哪一個函數處理，這裡定義了BeginReceive()
                    // 參數第一個為存放的位置，為byte[]，傳入的資料會置於此陣列中；第二參數為啟始位置、第三為一次置入的長度；第四為封包的旗標，例如標記為廣播封包 之類，第五參數為開始接受資料時叫用的回呼函式，由於pfnWorkerCallBack已實體化指定OnDataReceived為回呼函式，所以當有 客戶端將資料傳進來時，OnDataReceived函式會被叫用；第六為狀態的參數，這個是用戶自訂，當回呼函式OnDataReceived被叫用 時，此參數的值會被傳遞到OnDataReceived，因此雖然SocketPacket theSocPkt宣告的是區域變數，但是回呼函式仍會曉得是哪一個Socket在傳送資料。
                    soc.BeginReceive(theSocPkt.dataBuffer, 0, theSocPkt.dataBuffer.Length, SocketFlags.None, pfnWorkerCallBack, theSocPkt);
                }
                catch (SocketException)
                {
                    //…例外處理略... 
                }
                return Task.CompletedTask;
            }

            /// <summary>
            /// 發送訊息
            /// </summary>
            /// <param name="text">要傳送的資料</param>
            /// <returns></returns>
            public bool Send(string text)
            {
                if (IsConnect)
                {
                    text = text + Environment.NewLine;
                    byte[] data = System.Text.Encoding.Default.GetBytes(text);
                    try
                    {
                        tcpClient.Send(data);
                        IPEndPoint remoteIpEndPoint = tcpClient.RemoteEndPoint as IPEndPoint;
                        IPEndPoint localIpEndPoint = tcpClient.LocalEndPoint as IPEndPoint;
                        History.Add(localIpEndPoint.Address.ToString(), remoteIpEndPoint.Address.ToString(), text).Wait();
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                    return true;
                }
                return false;
            }

            /// <summary>
            /// 利用override方式分析指令並做執行動作
            /// </summary>
            /// <param name="data"></param>
            /// <returns></returns>
            public virtual string DoWork(string[] data)
            {
                //指令解析
                string reply = "";
                return reply;
            }

            /// <summary>
            /// 若有必要更新介面，override該指令
            /// </summary>
            public virtual void UpdateUI()
            {
                //指令解析
            }
        }

        public class Server
        {
            public TCPIP_History History;
            List<ClientNode> ClientList = new List<ClientNode>();
            private Socket mainSocket;
            private int port = 12345;
            private char delimiterChar = ' ';

            /// <summary>
            /// 創建TCP/IP Server (base on socket)
            /// </summary>
            public Server()
            {
                History = new TCPIP_History();
            }
            ~Server()
            {
                History.Clear();
            }

            /// <summary>
            /// 連線清單紀錄用節點
            /// </summary>
            public struct ClientNode
            {
                public Socket socket;
                public string ip;
                public string port;
            }

            /// <summary>
            /// 以字串型態回傳當前連線的Client端資訊
            /// </summary>
            /// <returns></returns>
            public string GetClientToString()
            {
                string ret = "";
                foreach (var item in ClientList)
                {
                    string logstring = item.ip + ":" + item.port;
                    ret = logstring + Environment.NewLine + ret;
                }
                return ret;
            }

            /// <summary>
            /// Server端的連接阜
            /// </summary>
            public int Port
            {
                get { return port; }
                set { port = value; }
            }
            /// <summary>
            /// Server端的指令分段符號(預設為空白斷行『 』)
            /// Ex：『cmd Test123』將拆成字串『cmd』與『Test123』
            /// </summary>
            public char DelimiterChar
            {
                get { return delimiterChar; }
                set { delimiterChar = value; }
            }

            /// <summary>
            /// 產生server端實體，並啟動服務。
            /// 成功將回傳true ; 失敗回傳false
            /// </summary>
            /// <returns></returns>
            public bool Connect()
            {
                if (mainSocket != null)
                    return false;
                // 實體化 mainSocket
                mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                //建立本機監聽位址及埠號，IPAddress.Any表示監聽所有的介面。
                IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, port);

                //socket連繫到該位址
                mainSocket.Bind(ipLocal);

                // 啟動監聽
                //backlog=4 參數會指定可在佇列中等候接收的輸入連接數。若要決定可指定的最大連接數，除非同時間的連線非常的大，否則值4應該很夠用。
                mainSocket.Listen(4);

                mainSocket.IOControl(IOControlCode.KeepAliveValues, KeepAlive(1, 1000, 1000), null);
                //將這個Socket使用keep-alive來保持長連線
                //KeepAlive函數參數說明: onOff:是否開啟Keep-Alive(開 1/ 關 0) , 
                //keepAliveTime:當開啟keep-Alive後經過多久時間(ms)開啟偵測
                //keepAliveInterval: 多久偵測一次(ms)

                // 以上完成了SERVER的Listening，BeginAccept()  開啟執行緒準備接收client的要求。
                // 此處的BeginAccept 第一個參數就是當Socket一開始接收到Client的連線要求時，立刻會呼叫delegate的OnClientConnect的方法，這個OnClientConnect名稱是我們自己取的，你也可以叫用別的名稱。因此我們要另外寫一個OnClientConnect()的函式。
                mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);

                return true;
            }

            /// <summary>
            /// 中斷Servert端的監聽
            /// </summary>
            /// <returns></returns>
            public bool Disconnect()
            {
                foreach (var item in workerSocket)
                {
                    if (item != null && item.Connected)
                    {
                        item.Shutdown(SocketShutdown.Both);
                        item.Close();
                        item.Dispose();
                    }
                }

                this.mainSocket.Close();
                this.mainSocket = null;
                ClientList.Clear();
                return true;
            }

            /// <summary>
            /// 由Server端主動發送訊息，client端必須存在連線list中才允許傳送。
            /// user_index指定為特定user(您可以透過ClientList得知當前連線狀態)
            /// </summary>
            /// <param name="user_index"></param>
            /// <param name="text"></param>
            /// <returns></returns>
            public bool Send(int user_index, string text)
            {
                //index不符跳出
                if (user_index > ClientList.Count)
                    return false;

                //回傳 ClientList
                text = text + Environment.NewLine;
                byte[] data = System.Text.Encoding.Default.GetBytes(text);
                try
                {
                    ClientList[user_index].socket.Send(data);
                    IPEndPoint remoteIpEndPoint = ClientList[user_index].socket.RemoteEndPoint as IPEndPoint;
                    IPEndPoint localIpEndPoint = ClientList[user_index].socket.LocalEndPoint as IPEndPoint;
                    History.Add(localIpEndPoint.Address.ToString(), remoteIpEndPoint.Address.ToString(), text).Wait();
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }

            private byte[] KeepAlive(int onOff, int keepAliveTime, int keepAliveInterval)
            {
                byte[] buffer = new byte[12];
                BitConverter.GetBytes(onOff).CopyTo(buffer, 0);
                BitConverter.GetBytes(keepAliveTime).CopyTo(buffer, 4);
                BitConverter.GetBytes(keepAliveInterval).CopyTo(buffer, 8);
                return buffer;
            }

            // 定義最大Clients 常數
            const int MAX_CLIENTS = 20;

            // 定義每個連線的工作類別域變數 Socket陣列
            private Socket[] workerSocket = new Socket[MAX_CLIENTS];

            private void OnClientConnect(IAsyncResult asyn)
            {
                try
                {
                    //宣告並定義空頻道的ID=-1，表示無空頻道資料
                    int Empty_channel_ID = -1;
                    //若主Socket為空則跳出
                    if (mainSocket == null) return;
                    //將主要正在 listening 的主要Socket轉交給另一個臨時的Socket變數，並且結束接受此一客戶端的連線
                    Socket temp_Socket = mainSocket.EndAccept(asyn);
                    //取得遠端節點的EndPoint
                    EndPoint RemoteEP = temp_Socket.RemoteEndPoint;

                    //Form
                    ClientNode ClientInfo = new ClientNode()
                    {
                        socket = temp_Socket,
                        ip = ((IPEndPoint)RemoteEP).Address.ToString(),
                        port = ((IPEndPoint)RemoteEP).Port.ToString()
                    };

                    ClientList.Add(ClientInfo);
                    UpdateList(ClientList);

                    //傳回空頻道，函式FindEmptyChannel() 列於後，主要是找尋未佔用的Socket(Channel)
                    Empty_channel_ID = FindEmptyChannel();
                    //這裡自定了一個錯誤的Exception類型，當找不到空頻道表示所有頻道都被佔用，則會丟出一個 NoSocketAvailableException() 的錯誤
                    if (Empty_channel_ID == -1) throw new NoSocketAvailableException();
                    //將方才暫存的Socket交給空的 Socket接收
                    workerSocket[Empty_channel_ID] = temp_Socket;
                    //將暫存的Socket設為空
                    temp_Socket = null;
                    //開始接收方才的Socket，此函式內容詳述於使用C# 撰寫非同步方法 TCP socket --3
                    WaitForData(workerSocket[Empty_channel_ID]);

                }
                catch (ObjectDisposedException)
                {
                    //…處理已釋放記憶體的資源例外處理略... 
                }
                catch (SocketException)
                {
                    //…因TCP Socket造成的例外處理略... 
                }
                //自定了一個錯誤的Exception類型，當找不到空頻道表示所有頻道都被佔用
                //catch (NoSocketAvailableException err)
                //{
                //    //…無可用的頻道例外處理略... 
                //}
                finally
                {
                    if (mainSocket != null)
                    {
                        //將方才關閉的主要Socket重新接收新的連線
                        mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);
                    }
                }
            }

            //自定錯誤類型，當所有頻道都被佔用時叫用，其中只覆寫了Exception.Message 屬性
            private class NoSocketAvailableException : System.Exception
            {
                //new public string Message = "所有的頻道都已佔滿，請先釋放其他的頻道";
                public override string Message
                {
                    get
                    {
                        return "所有的頻道都已佔滿，請先釋放其他的頻道";
                    }
                }
            }

            // 尋找第一個空頻道的函式，傳回Channel ID或是-1 全被佔滿
            private int FindEmptyChannel()
            {
                return Array.FindIndex(workerSocket, s => s == null || !s.Connected);
                //for (int i = 0; i < MAX_CLIENTS; i++)
                //{
                //    if (workerSocket[i] == null || !workerSocket[i].Connected)
                //    {
                //        return i;
                //    }
                //}
                //return -1;
            }

            // 尋找不為空頻道的函式，傳回Channel ID或是-1 全為空
            private int FindActiveChannel()
            {
                return Array.FindIndex(workerSocket, s => s != null && s.Connected);
                //for (int i = 0; i < MAX_CLIENTS; i++)
                //{
                //    if (workerSocket[i] != null && workerSocket[i].Connected)
                //    {
                //        return i;
                //    }
                //}
                //return -1;
            }

            //宣告AsyncCallback類別的變數 pfnWorkerCallBack
            private AsyncCallback pfnWorkerCallBack;
            private void WaitForData(System.Net.Sockets.Socket soc)
            {
                try
                {
                    //當pfnWorkerCallBack物件尚未實體化時，進行實體化
                    if (pfnWorkerCallBack == null)
                    {
                        //假設已連線的RCC客戶端要傳送資料給RCS時，所指定的回呼函式-OnDataReceive，這也是C#中的delagate型別，此函式內容詳述於後
                        pfnWorkerCallBack = new AsyncCallback(OnDataReceived);
                    }
                    //自行定義的型別 SocketPacket，附於此小節尾，內容只有一個Socket類和一個int。
                    //指定此一建立連線之Socket soc 給定義的 theSocPkt
                    SocketPacket theSocPkt = new SocketPacket() { m_currentSocket = soc };

                    // 接下來定義當Socket 接收到資料時要交給哪一個函數處理，這裡定義了BeginReceive()
                    // 參數第一個為存放的位置，為byte[]，傳入的資料會置於此陣列中；第二參數為啟始位置、第三為一次置入的長度；第四為封包的旗標，例如標記為廣播封包 之類，第五參數為開始接受資料時叫用的回呼函式，由於pfnWorkerCallBack已實體化指定OnDataReceived為回呼函式，所以當有 客戶端將資料傳進來時，OnDataReceived函式會被叫用；第六為狀態的參數，這個是用戶自訂，當回呼函式OnDataReceived被叫用 時，此參數的值會被傳遞到OnDataReceived，因此雖然SocketPacket theSocPkt宣告的是區域變數，但是回呼函式仍會曉得是哪一個Socket在傳送資料。
                    soc.BeginReceive(theSocPkt.dataBuffer, 0, theSocPkt.dataBuffer.Length, SocketFlags.None, pfnWorkerCallBack, theSocPkt);
                }
                catch (SocketException)
                {
                    //…例外處理略... 
                }
            }

            public virtual void OnDataReceived(IAsyncResult asyn)
            {
                string msg = "";
                SocketPacket socketData = (SocketPacket)asyn.AsyncState;//取得接受的資料
                try
                {
                    int iRx = 0;//宣告及定義訊息長度
                                //叫用 EndReceive 完成指定的非同步接收作業, 參數asyn 識別要完成的非同步接收作業，並要從其中擷取最終結果。
                    iRx = socketData.m_currentSocket.EndReceive(asyn);
                    //當 bytesRead大於0時表示Server傳遞資料過來 等於0時代表Client"正常"斷線
                    if (iRx <= 0)
                    {
                        //移除紀錄List
                        RemoveClient(socketData.m_currentSocket.RemoteEndPoint);
                        //處理斷線動作
                        socketData.m_currentSocket.Shutdown(SocketShutdown.Both);
                        socketData.m_currentSocket.Close();
                        return;
                    }

                    //以下為將取出的部分轉成Default的編碼方式，有關編碼方式轉換可依需求自行修改
                    byte[] databuff = socketData.dataBuffer;
                    msg = Encoding.Default.GetString(databuff).TrimEnd('\0');

                    PackageParser c = new PackageParser();
                    c.PackageReached += C_PackageReached;
                    c.Parse(socketData.m_currentSocket, msg);
                    //傳回空頻道，函式FindActiveChannel() 列於後，主要是找尋啟用的Socket(Channel)

                    //…分析處理部分略，主要msg訊息的利用和重組…
                    //重新叫用 WaitForData 重新開始接收資料
                    WaitForData(socketData.m_currentSocket);
                }
                catch (SocketException)
                {
                    //移除紀錄List
                    RemoveClient(socketData.m_currentSocket.RemoteEndPoint);
                    //…例外處理略...
                    //處理斷線動作
                    socketData.m_currentSocket.Shutdown(SocketShutdown.Both);
                    socketData.m_currentSocket.Close();
                }
                catch (System.ObjectDisposedException)
                {

                }
            }

            private void RemoveClient(EndPoint RemoteEP)
            {
                //移除紀錄List
                string ip = ((IPEndPoint)RemoteEP).Address.ToString();
                string port = ((IPEndPoint)RemoteEP).Port.ToString();
                int index = ClientList.FindIndex(x => (x.ip == ip && x.port == port));
                if (index > -1)
                {
                    ClientList.RemoveAt(index);
                    UpdateList(ClientList);
                }
            }

            private void C_PackageReached(object sender, PackageData e)
            {
                string msg = e.Text;
                IPEndPoint remoteIpEndPoint = e.CurrentSocket.RemoteEndPoint as IPEndPoint;
                IPEndPoint localIpEndPoint = e.CurrentSocket.LocalEndPoint as IPEndPoint;
                History.Add(remoteIpEndPoint.Address.ToString(), localIpEndPoint.Address.ToString(), msg).Wait();
                string[] words = msg.Split(delimiterChar);
                string reply = DoWork(words);
                if (reply != "")
                {
                    reply = reply + Environment.NewLine;
                    byte[] reply_data = Encoding.Default.GetBytes(reply);
                    e.CurrentSocket.Send(reply_data);
                    History.Add(localIpEndPoint.Address.ToString(), remoteIpEndPoint.Address.ToString(), reply).Wait();
                }
            }

            public virtual string DoWork(string[] data)
            {
                //指令解析
                string reply = "";
                return reply;
            }

            public virtual void UpdateUI()
            {
                //指令解析
            }

            public virtual void UpdateList(List<ClientNode> info)
            {
                //更新User連線狀態
            }
        }

    }
}

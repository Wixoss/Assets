using System.Net.Sockets;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{ 
    public class MyNetwork : MonoBehaviour
    {
        public string Ip;
        public int Port;

        public UIButton CreateBtn;
        public UIButton ConnectBtn;

        public UIButton ConnectMenuConnect;
        public UIButton ConnectMenuBack;

        public UIButton ServerCancelBtn;

        public UIInput IpInput;
        public UIInput PortInput;

        public GameObject BtnsObj;
        public GameObject ConnectMenu;
        public GameObject CreateMenu;

        public UILabel IpAddress;
        public UILabel ErrorMes;

        private void Awake()
        {
            CreateBtn.onClick.Add(new EventDelegate(CreateServer));
            ConnectBtn.onClick.Add(new EventDelegate(() => ShowMenu(BtnsObj, ConnectMenu)));
            ConnectMenuConnect.onClick.Add(new EventDelegate(ConnectServer));
            ConnectMenuBack.onClick.Add(new EventDelegate(() => ShowMenu(ConnectMenu, BtnsObj)));
            ServerCancelBtn.onClick.Add(new EventDelegate(ServerCancel));
        }

        private void ConnectServer()
        {
            Ip = IpInput.value;
            Port = System.Convert.ToInt32(PortInput.value);
            if (Network.peerType == NetworkPeerType.Disconnected)
            {
                Network.Connect(Ip, Port);
            }
        }

        private void ShowMenu(GameObject go1, GameObject go2)
        {
            go1.SetActive(false);
            go2.SetActive(true);
        }

        private void CreateServer()
        {
            ShowMenu(BtnsObj, CreateMenu);
            if (Network.peerType == NetworkPeerType.Disconnected)
            {
                Port = RandaomPort();
                Network.InitializeServer(2, Port, false);
                Debug.Log(Network.proxyIP);
                Debug.Log(Network.natFacilitatorIP);
                Debug.Log(Network.connectionTesterIP);
            }
        }

        public string GetLocalIp()
        {
            //Dns.GetHostAddresses(Dns.GetHostName())[0]
            string userip = "";
            var adapters = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            foreach (var adapter in adapters)
            {
                if (adapter.Supports(System.Net.NetworkInformation.NetworkInterfaceComponent.IPv4))
                {
                    var uniCast = adapter.GetIPProperties().UnicastAddresses;
                    if (uniCast.Count > 0)
                    {
                        foreach (var uni in uniCast)
                        {
                            if (uni.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                userip = uni.Address.ToString();
                            }
                        }
                    }
                }
            }
            return userip;
        }

        private int RandaomPort()
        {
            return Random.Range(8000, 10000);
        }

        private void ServerCancel()
        {
            if (Network.peerType == NetworkPeerType.Server)
            {
                Network.Disconnect();
            }
            ShowMenu(CreateMenu, BtnsObj);
        }

        /// <summary>
        /// 客户端连接服务器失败调用
        /// </summary>
        /// <param name="error"></param>
        private void OnFailedToConnect(NetworkConnectionError error)
        {
            ErrorMes.text = error.ToString();
        }

        /// <summary>
        /// 客户端成功连接时调用
        /// </summary>
        private void OnConnectedToServer()
        {
            //成功连接时创建2个对象:自己&对方
            DataSource.ClientPlayer = new Player
            {
                PlayerIp = GetLocalIp(),
            };

            DataSource.ServerPlayer = new Player
            {
                PlayerIp = Ip,
            };
            //转移至选卡组场景
            Application.LoadLevelAsync(1);
        }

        /// <summary>
        /// 服务器被连接上的时候调用
        /// </summary>
        /// <param name="player"></param>
        private void OnPlayerConnected(NetworkPlayer player)
        {
            //成功连接时创建2个对象:自己&对方
            DataSource.ServerPlayer = new Player
            {
                PlayerIp = GetLocalIp(),
            };

            DataSource.ClientPlayer = new Player
            {
                PlayerIp = player.ipAddress,
            };
            //转移至选卡组场景
            Application.LoadLevelAsync(1);
        }

        /// <summary>
        /// 服务器成功创建时调用
        /// </summary>
        private void OnServerInitialized()
        {
            IpAddress.text = "Ip地址:" + GetLocalIp() + "\n" + "  端口号:" + Port;
        }

        /// <summary>
        /// 断开连接时调用
        /// </summary>
        private void OnDisconnectedFromServer(NetworkDisconnection info)
        {
            Application.LoadLevelAsync(0);
        }

		/// <summary>
		/// Player disconnected
		/// </summary>
		/// <param name="player">Player.</param>
		private void OnPlayerDisconnected(NetworkPlayer player)
		{
			Network.RemoveRPCs (player);
		}
    }
}

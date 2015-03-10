using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Decks : MonoBehaviour
    {
        public UILabel Info;
        public UIButton OkBtn;
        public UIButton[] DeckSelects;

        public MyRpc MyRpc;
        public Deck MyDeck;
        public string SelectDeckName;
        private AsyncOperation _async;

        private void Awake()
        {
            OkBtn.onClick.Add(new EventDelegate(SelectDeckOk));
            for (int i = 0; i < DeckSelects.Length; i++)
            {
                int i1 = i;
                DeckSelects[i].onClick.Add(new EventDelegate(() =>
                {
                    MyDeck = DeckSelects[i1].GetComponent<DeckSelect>().MyDeck;
                    SelectDeckName = MyDeck.DeckName;
                }));
            }
        }

        private void SelectDeckOk()
        {
            //客户端
            if (Network.isClient && !DataSource.ClientPlayer.BReady)
            {
                DataSource.ClientDeck = MyDeck;
                DataSource.ClientPlayer.BReady = true;
                MyRpc = GameObject.Find("RPC").GetComponent<MyRpc>();
                if (MyRpc != null)
                {
                    MyRpc.Rpc("ReportClientDeckName", RPCMode.Others, MyDeck.DeckName);
                    StartCoroutine(CheckAndLoadScene());
                }
                Info.text = "已经准备好了,选择的卡组为: " + SelectDeckName;
            }
            //服务器
            if (Network.isServer && !DataSource.ServerPlayer.BReady)
            {
                DataSource.ServerDeck = MyDeck;
                DataSource.ServerPlayer.BReady = true;
                MyRpc = GameObject.Find("RPC").GetComponent<MyRpc>();
                if (MyRpc != null)
                {
                    MyRpc.Rpc("ReportServerDeckName", RPCMode.Others, MyDeck.DeckName);
                    StartCoroutine(CheckAndLoadScene());
                }
                Info.text = "已经准备好了,选择的卡组为: " + SelectDeckName;
            }
        }

        private IEnumerator CheckAndLoadScene()
        {
            while (true)
            {
                if (DataSource.ClientPlayer.BReady && DataSource.ServerPlayer.BReady)
                {
                    _async = Application.LoadLevelAsync(2);
                    if (_async.isDone)
                    {
                        _async.allowSceneActivation = false;
                        if (Network.isClient)
                        {
                            MyRpc.Rpc("ReportClientLoad", RPCMode.Others, true);
                        }
                        if (Network.isServer)
                        {
                            MyRpc.Rpc("ReportServerLoad", RPCMode.Others, true);
                        }
                        if (DataSource.ClientPlayer.BLoad && DataSource.ServerPlayer.BLoad)
                        {
                            _async.allowSceneActivation = true;
                            yield break;
                        }
                    }
                }
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}

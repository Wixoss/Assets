using System.Collections;
using System.Collections.Generic;
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


            int num = Random.Range(0, 4);
            switch (num)
            {
                case 0:
                    DataSource.LrigDeckCardId = new List<string>
            {
                "WD04-001",
                "WD04-002",
                "WD04-003",
                "WD04-004",
                "WD04-005",
                "WD04-006",
                "WD04-007",
                "WD04-008",
            };

                    DataSource.MainDeckCardId = new List<string>
            {
                "WD04-009",
                "WD04-009",
                "WD04-009",
                "WD04-009",
                
                "WD04-010",
                "WD04-010",
                "WD04-010",
                "WD04-010",
                
                "WD04-011",
                "WD04-011",
                "WD04-011",
                "WD04-011",
                
                "WD04-012",
                "WD04-012",
                "WD04-012",
                "WD04-012",
                
                "WD04-013",
                "WD04-013",
                "WD04-013",
                "WD04-013",
                
                "WD04-014",
                "WD04-014",
                "WD04-014",
                "WD04-014",
                
                "WD04-015",
                "WD04-015",
                "WD04-015",
                "WD04-015",
                
//                "WX01-101",
//                "WX01-101",
//                "WX01-101",
//                "WX01-101",
//
//                "WX01-102",
//                "WX01-102",
//                "WX01-102",
//                "WX01-102",
//
//                "WX01-103",
//                "WX01-103",
//                "WX01-103",
//                "WX01-103",

                "WD04-016",
                "WD04-016",
                "WD04-016",
                "WD04-016",

                "WD04-017",
                "WD04-017",
                "WD04-017",
                "WD04-017",

                "WD04-018",
                "WD04-018",
                "WD04-018",
                "WD04-018",
            };
                    break;

                case 1:
                    DataSource.LrigDeckCardId = new List<string>
            {
                "WD01-001",
                "WD01-002",
                "WD01-003",
                "WD01-004",
                "WD01-005",
                "WD01-006",
                "WD01-007",
                "WD01-008",
            };

                    DataSource.MainDeckCardId = new List<string>
            {
                "WD01-009",
                "WD01-009",
                "WD01-009",
                "WD01-009",
                
                "WD01-010",
                "WD01-010",
                "WD01-010",
                "WD01-010",
                
                "WD01-011",
                "WD01-011",
                "WD01-011",
                "WD01-011",
                
                "WD01-012",
                "WD01-012",
                "WD01-012",
                "WD01-012",
                
                "WD01-013",
                "WD01-013",
                "WD01-013",
                "WD01-013",
                
                "WD01-014",
                "WD01-014",
                "WD01-014",
                "WD01-014",
                
                "WD01-015",
                "WD01-015",
                "WD01-015",
                "WD01-015",
                
                "WX01-101",
                "WX01-101",
                "WX01-101",
                "WX01-101",

                "WX01-102",
                "WX01-102",
                "WX01-102",
                "WX01-102",

                "WX01-103",
                "WX01-103",
                "WX01-103",
                "WX01-103",
            };
                    break;
                case 2:
                    DataSource.LrigDeckCardId = new List<string>
            {
                "WD02-001",
                "WD02-002",
                "WD02-003",
                "WD02-004",
                "WD02-005",
                "WD02-006",
                "WD02-007",
                "WD02-008",
            };

                    DataSource.MainDeckCardId = new List<string>
            {
                "WD02-009",
                "WD02-009",
                "WD02-009",
                "WD02-009",
                
                "WD02-010",
                "WD02-010",
                "WD02-010",
                "WD02-010",
                
                "WD02-011",
                "WD02-011",
                "WD02-011",
                "WD02-011",
                
                "WD02-012",
                "WD02-012",
                "WD02-012",
                "WD02-012",
                
                "WD02-013",
                "WD02-013",
                "WD02-013",
                "WD02-013",
                
                "WD02-014",
                "WD02-014",
                "WD02-014",
                "WD02-014",
                
                "WD02-015",
                "WD02-015",
                "WD02-015",
                "WD02-015",
                
                "WX01-101",
                "WX01-101",
                "WX01-101",
                "WX01-101",

                "WX01-102",
                "WX01-102",
                "WX01-102",
                "WX01-102",

                "WX01-103",
                "WX01-103",
                "WX01-103",
                "WX01-103",
            };
                    break;

                case 3:
                    DataSource.LrigDeckCardId = new List<string>
            {
                "WD03-001",
                "WD03-002",
                "WD03-003",
                "WD03-004",
                "WD03-005",
                "WD03-006",
                "WD03-007",
                "WD03-008",
            };

                    DataSource.MainDeckCardId = new List<string>
            {
                "WD03-009",
                "WD03-009",
                "WD03-009",
                "WD03-009",
                
                "WD03-010",
                "WD03-010",
                "WD03-010",
                "WD03-010",
                
                "WD03-011",
                "WD03-011",
                "WD03-011",
                "WD03-011",
                
                "WD03-012",
                "WD03-012",
                "WD03-012",
                "WD03-012",
                
                "WD03-013",
                "WD03-013",
                "WD03-013",
                "WD03-013",
                
                "WD03-014",
                "WD03-014",
                "WD03-014",
                "WD03-014",
                
                "WD03-015",
                "WD03-015",
                "WD03-015",
                "WD03-015",
                
                "WX01-101",
                "WX01-101",
                "WX01-101",
                "WX01-101",

                "WX01-102",
                "WX01-102",
                "WX01-102",
                "WX01-102",

                "WX01-103",
                "WX01-103",
                "WX01-103",
                "WX01-103",
            };
                    break;

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

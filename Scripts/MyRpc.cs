using UnityEngine;
using System;

namespace Assets.Scripts
{
    public class MyRpc : MonoBehaviour
    {
        //        /// <summary>
        //        /// 用于传输数字式的数据
        //        /// </summary>
        //        public Action<int> NumChangeAction;
        //        /// <summary>
        //        /// 用于传输字符串的数据
        //        /// </summary>
        //        public Action<string> StringChangeAction;

        private GameManager _gameManager = null;

        private void Awake()
        {
            //保证自己不会在load场景的时候被销毁
            //DontDestroyOnLoad(gameObject);
        }

        public void Rpc(string funcname, RPCMode mode, params object[] objs)
        {
            if (Network.peerType != NetworkPeerType.Disconnected)
            {
                networkView.RPC(funcname, mode, objs);
            }
        }

        #region 只能在此定义的蛋疼函数

        [RPC]
        private void ReportClientDeckName(string deckname)
        {
            DataSource.ClientDeckName = deckname;
            DataSource.ClientPlayer.BReady = true;
            var info = GameObject.Find("Info").GetComponent<UILabel>();
            info.text += "\n" + "对方已经准备好了,选择的卡组为: " + deckname;
        }

        [RPC]
        private void ReportServerDeckName(string deckname)
        {
            DataSource.ServerDeckName = deckname;
            DataSource.ServerPlayer.BReady = true;
            var info = GameObject.Find("Info").GetComponent<UILabel>();
            info.text += "\n" + "对方已经准备好了,选择的卡组为: " + deckname;
        }

        [RPC]
        private void ReportClientLoad(bool ready)
        {
            DataSource.ClientPlayer.BLoad = ready;
        }

        [RPC]
        private void ReportServerLoad(bool ready)
        {
            DataSource.ServerPlayer.BLoad = ready;
        }

        [RPC]
        private void ClientSelectFirstAttack(bool bfirst)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            DataSource.ClientPlayer.BFirst = bfirst;
            DataSource.ServerPlayer.BFirst = !bfirst;
            _gameManager.MyGameState = GameManager.GameState.准备阶段;
        }

        [RPC]
        private void ServerSelectFirstAttack(bool bfirst)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            DataSource.ServerPlayer.BFirst = bfirst;
            DataSource.ClientPlayer.BFirst = !bfirst;
            _gameManager.MyGameState = GameManager.GameState.准备阶段;
        }

        [RPC]
        private void ReportClientJyanKen(bool bJyan, int num)
        {
            DataSource.ClientPlayer.BJyanKen = bJyan;
            DataSource.ClientPlayer.JyanKenNum = num;
        }

        [RPC]
        private void ReportServerJyanKen(bool bJyan, int num)
        {
            DataSource.ServerPlayer.BJyanKen = bJyan;
            DataSource.ServerPlayer.JyanKenNum = num;
        }

        [RPC]
        private void ReportClientEndReadyPhase(bool bready)
        {
            DataSource.ClientPlayer.BReadyPhaseEnd = true;
        }

        [RPC]
        private void ReportServerEndReadyPhase(bool bready)
        {
            DataSource.ServerPlayer.BReadyPhaseEnd = true;
        }

        [RPC]
        private void SetOtherSigni(int num, string cardid)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            Card card = new Card(cardid);
            _gameManager.SetSigni.SetOtherSigni(num, card);
            _gameManager.ShowCard.ShowMyCard(card);
        }

        [RPC]
        private void SetMySigniSet(int num, bool bset)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }

            _gameManager.SetSigni.SetOtherSigniSet(num, bset);
        }

        [RPC]
        private void SetOtherSigniSet(int num)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }

            _gameManager.SetSigni.HorizontalSigni(num);
        }

        [RPC]
        private void SetOtherLrigSet()
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            _gameManager.Lrig.HorizontalLrig();
        }

        [RPC]
        private void SetOtherLrig(string cardid)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            _gameManager.Lrig.SetOtherLrig(cardid);
            _gameManager.ShowCard.ShowMyCard(new Card(cardid));
        }

        [RPC]
        private void SetMyLrigSet(bool bset)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            _gameManager.Lrig.SetOtherLrigSet(bset);
        }

        [RPC]
        private void SetOtherEner(string cardid)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            _gameManager.EnerManager.CreateOtherEner(cardid);
        }

        [RPC]
        private void SetBanish(int num)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            _gameManager.SetSigni.BanishMySigni(num);
        }

        [RPC]
        private void SetOtherBanish(int num)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            _gameManager.SetSigni.ShowBanishOtherSigni(num);
        }

        [RPC]
        private void SetOtherBackHand(int num)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            _gameManager.SetSigni.BackToHand(num);
        }

        [RPC]
        private void SetOtherTrash(string cardid, int num)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            if (num != -1)
            {
                _gameManager.SetSigni.TrashOtherSigni(num);
            }
            _gameManager.Trash.AddOtherTrash(cardid);
        }

        [RPC]
        private void DestoryOtherHand(int num)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }

            StartCoroutine(_gameManager.CreateHands.DestoryOtherHands(num));

            //_gameManager.CreateHands.DestoryHands(num);
        }

        [RPC]
        private void ShowGameResult(string word)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            _gameManager.GameOver.ShowGameResoult(word);
        }

        [RPC]
        private void CreateOtherHands(int num)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            _gameManager.CreateHands.CreateOtherHands(num);
        }

        [RPC]
        private void DeleteOtherEner(string cardid)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            _gameManager.EnerManager.DestoryOtherEner(cardid);
        }

        [RPC]
        private void SetOtherCheck(string cardid)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            _gameManager.Check.SetOtherCheck(cardid);
            _gameManager.ShowCard.ShowMyCard(new Card(cardid));
        }

        [RPC]
        private void SetOtherGuard(int bguard)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            _gameManager.Lrig.Bguard = bguard;
            _gameManager.Lrig.ShowAttackBtn(false);
            _gameManager.MyGameState = GameManager.GameState.结束阶段;
            _gameManager.WordInfo.ShowTheEndPhaseBtn(false);
        }

        [RPC]
        private void SetOtherMainDeck(int num)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            _gameManager.ShowDeck.OtherMainDeck(num);
        }

        [RPC]
        private void SetOtherUseArt(int bUse)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            _gameManager.BUseArt = bUse;
        }

        [RPC]
        private void ShowOtherGuard()
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            _gameManager.CreateHands.ShowGuardBtn();
        }

        [RPC]
        private void SetOtherTiming(int num)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            if (num == 1)
            {
                _gameManager.MyTiming = GameManager.Timing.攻击宣言阶段;
            }
            else if (num == 2)
            {
                _gameManager.MyTiming = GameManager.Timing.魔法切入阶段;
            }
            else
            {
                _gameManager.MyTiming = GameManager.Timing.其他阶段;
            }
        }

        [RPC]
        private void ShowOtherUseArt(bool bshow)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            _gameManager.CreateHands.ShowUseArtBtn(bshow);
        }

        [RPC]
        private void SetOtherCloth(int num)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            _gameManager.LifeCloth.CreateOtherCloth(num);
        }

        [RPC]
        private void CrashOtherCloth(bool bHurt)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            _gameManager.LifeCloth.CrashCloth(bHurt);
            _gameManager.CreateHands.DisTheGuardBtn();
        }

        [RPC]
        private void CrashMyCloth(bool bHurt)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            _gameManager.LifeCloth.CrashOtherCloth(false);
        }

        [RPC]
        private void OtherGetCardFromTrash(bool brewriteDeck, string cardid)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }

            _gameManager.Trash.OtherGetCardFromTrash(brewriteDeck, cardid);
        }

        [RPC]
        private void SetBuff(int typenum, int num, bool bset)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }

            switch (typenum)
            {
                case 1:
                    _gameManager.SetSigni.OtherSigni[num].Blancer = bset;
                    break;
                case 2:
                    _gameManager.SetSigni.OtherSigni[num].Bdouble = bset;
                    break;
            }
        }

        [RPC]
        private void SetOtherBuff(int typenum, int num, bool bset)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }

            switch (typenum)
            {
                case 1:
                    _gameManager.SetSigni.Signi[num].Bfreeze = bset;
                    break;
                case 2:
                    _gameManager.SetSigni.Signi[num].BCantAttack = bset;
                    break;
            }
        }

        //        [RPC]
        //        private void CrashCloth()
        //        {
        //            if (_gameManager == null)
        //            {
        //                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //            }
        //            _gameManager.LifeCloth.CrashCloth();
        //        }

        [RPC]
        private void ReportOtherStuff(string word)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            _gameManager.Reporting.text = "对方回合:" + "\n" + word;
        }

        [RPC]
        private void RoundChange(bool bMyRound, int roundNum)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            GameManager.BLocalRound = !bMyRound;
            _gameManager.MyGameState = GameManager.GameState.竖置阶段;
            _gameManager.Rounds = roundNum;
        }

        #endregion
    }
}

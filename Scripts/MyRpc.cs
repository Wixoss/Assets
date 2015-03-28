using UnityEngine;
using System;
using System.Collections.Generic;

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
            info.text += "\n" + "对方已经准备好了";
        }

        [RPC]
        private void ReportServerDeckName(string deckname)
        {
            DataSource.ServerDeckName = deckname;
            DataSource.ServerPlayer.BReady = true;
            var info = GameObject.Find("Info").GetComponent<UILabel>();
            info.text += "\n" + "对方已经准备好了";
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
            var card = _gameManager.GetCardFromDictionary(cardid);
            _gameManager.SetSigni.SetOtherSigni(num, card);
            _gameManager.ShowCard.ShowMyCard(card);
        }

        [RPC]
        private void ShowOtherCardBuff(string cardid)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            var card = _gameManager.GetCardFromDictionary(cardid);
            _gameManager.ShowCard.ShowMyCardEffect(card);
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
        private void SetOtherLrigCantAttack()
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
            var card = _gameManager.GetCardFromDictionary(cardid);
            _gameManager.Lrig.SetOtherLrig(card);
            _gameManager.ShowCard.ShowMyCard(card);
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
            var card = _gameManager.GetCardFromDictionary(cardid);
            _gameManager.EnerManager.CreateOtherEner(card);
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
            var card = _gameManager.GetCardFromDictionary(cardid);
            _gameManager.Trash.AddOtherTrash(card);
        }

        [RPC]
        private void DestoryOtherHand(int num)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }

            _gameManager.CreateHands.DestoryOtherHands(num);

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

            var card = _gameManager.GetCardFromDictionary(cardid);
            _gameManager.Check.SetOtherCheck(card);
            _gameManager.ShowCard.ShowMyCard(card);
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
            _gameManager.WordInfo.ShowTheEndPhaseBtn(true);
            //不防御的话掉护甲
            Debug.Log(bguard);
            if (bguard == -1)
            {
                GameManager.RpcCrashOtherLifeCloth(true);
                _gameManager.LifeCloth.CrashOtherCloth(true);
            }
            //弧光！！
            //            _gameManager.MyGameState = GameManager.GameState.结束阶段;
            //            _gameManager.WordInfo.ShowTheEndPhaseBtn(false);
        }

        [RPC]
        private void SendOtherMyShowCard(string cardid)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            var card = _gameManager.GetCardFromDictionary(cardid);
            _gameManager.CreateHands.OtherShowCards.Add(card);
        }

        [RPC]
        private void SendOtherMyCard(string cardid)
        {
//          if (_gameManager == null)
//          {
//             _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
//          }
            //            _gameManager.SkillManager.MyCard.OtherCardid.Add(cardid);
            GameManager.OtherCards.Add(cardid);
        }

        [RPC]
        private void SendOtherMyCardOk(bool bdone)
        {
//            if (_gameManager == null)
//            {
//                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
//            }
            GameManager.Bdone = bdone;
        }

        [RPC]
        private void ShowOtherMyCards(string info)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }

            _gameManager.CardInfo.ShowCardInfo(true);
            _gameManager.CardInfo.SetUp(info, _gameManager.CreateHands.OtherShowCards, 0, () =>
            {
                _gameManager.CreateHands.OtherShowCards.Clear();
                _gameManager.CardInfo.ShowCardInfo(false);
            });
        }

        [RPC]
        private void GetOtherMyHandsDesLevelOne()
        {
            var cards = _gameManager.CreateHands.MyHands;
            
            for(int i =0;i<cards.Count;i++)
            {
                Rpc("SendOtherMyShowCard",RPCMode.Others,cards[i].MyCard.CardId);
            }

            Rpc("ShowOtherMyhandsDesLevelOne",RPCMode.Others);
        }

        [RPC]
        private void GetOtherHands()
        {
            var cards = _gameManager.CreateHands.MyHands;
            for(int i =0;i<cards.Count;i++)
            {
                Rpc("SendOtherMyShowCard",RPCMode.Others,cards[i].MyCard.CardId);
            }
            Rpc("ShowOtherMyCards", RPCMode.Others, "对方手卡");
        }

        [RPC]
        private void ShowOtherMyhandsDesLevelOne()
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
 
            _gameManager.CardInfo.ShowCardInfo(true);
            _gameManager.CardInfo.SetUp("对方手卡", _gameManager.CreateHands.OtherShowCards, 0, () =>
            {
                _gameManager.CardInfo.ShowCardInfo(true);

                var target = new List<Card>();
                for(int i =0;i<_gameManager.CreateHands.OtherShowCards.Count;i++)
                {
                    if(_gameManager.CreateHands.OtherShowCards[i].Level==1)
                    {
                        target.Add(_gameManager.CreateHands.OtherShowCards[i]);
                    }
                }

                _gameManager.CardInfo.SetUp("选择一张等级一的精灵卡丢弃",target,1,()=>
                {
                    if(_gameManager.CardInfo.SelectHands.Count>0)
                    {
                        GameManager.RpcDesHandById(_gameManager.CardInfo.SelectHands[0].Cardid,true);
                    }
                    _gameManager.CreateHands.OtherShowCards.Clear();
                    _gameManager.CardInfo.ShowCardInfo(false);
                });
            });
        }

        [RPC]
        private void DesOtherHandByLevel(int level, bool bOne)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            _gameManager.CreateHands.DestoryHandCondiction(x => x.Level == level, bOne);
        }

        bool bOk = false;

        [RPC]
        private void DesHand(int i)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }

            if (_gameManager.CreateHands.MyHands.Count < 1)
            {
                _gameManager.SetSigni.BWaiting = false;
                bOk = true;
            } 
            else
            {
                SkillManager.DesCard(i,()=>
                {
                    _gameManager.SetSigni.BWaiting = false;
                    bOk = true;
                });
                Invoke("OverTimeDesHandRandom",10f);
            }
        }

        private void OverTimeDesHandRandom()
        {
            if (bOk == false)
            {
                var num = _gameManager.CreateHands.MyHands.Count - 1;
                SkillManager.DesCardRandom();
                _gameManager.CreateHands.SetDesBtnOverSix(num,()=> bOk = false);
                _gameManager.CreateHands.DesMyHandsOverSix();
            }
        }

        [RPC]
        private void DesHandRandom()
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            SkillManager.DesCardRandom();
        }

        [RPC]
        private void DesOtherHandByCardid(string cardid, bool bOne)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            _gameManager.CreateHands.DestoryHandCondiction(x => x.CardId == cardid, bOne);
        }

        [RPC]
        private void ShowOtherMyCardsAtkChange(int num, int value)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            if (_gameManager.SetSigni.OtherSigni [num] != null)
            {
                _gameManager.SetSigni.OtherSigni[num].Atk += value;
            }
        }

        [RPC]
        private void RpcStopOtherAttack()
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            _gameManager.Reporting.text = "等待对方操作中...";
            _gameManager.SetSigni.DisAllAttackBtn();
            _gameManager.WordInfo.ShowTheEndPhaseBtn(false);
        }

        [RPC]
        private void RpcContinueOtherAttack()
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            _gameManager.SetSigni.BWaitFinish = true;
            Debug.Log(_gameManager.SetSigni.BWaiting);
            if (!_gameManager.SetSigni.BWaiting)
            {
                if (_gameManager.MyGameState == GameManager.GameState.精灵攻击阶段 ||
                    _gameManager.MyGameState == GameManager.GameState.分身攻击阶段)
                {
                    _gameManager.Reporting.text = "继续攻击阶段";
                    _gameManager.SetSigni.ShowAttackBtn();
                    _gameManager.WordInfo.ShowTheEndPhaseBtn(true);
                }
            }
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
            _gameManager.LifeCloth.CrashOtherCloth(bHurt);
        }

        [RPC]
        private void OtherGetCardFromTrash(bool brewriteDeck, string cardid)
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            var card = _gameManager.GetCardFromDictionary(cardid);
            _gameManager.Trash.OtherGetCardFromTrash(brewriteDeck, card);
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
                    if (num == 3)
                    {
                        _gameManager.Lrig.MyLrig.Bfreeze = bset;
                        break;
                    }
                    if (_gameManager.SetSigni.Signi[num] != null)
                    {
                        _gameManager.SetSigni.Signi[num].Bfreeze = bset;
                    }
                    break;
                case 2:
                    if (num == 3)
                    {
                        _gameManager.Lrig.MyLrig.BCantAttack = bset;
                        break;
                    }
                    if (_gameManager.SetSigni.Signi[num] != null)
                    {
                        _gameManager.SetSigni.Signi[num].BCantAttack = bset;
                    }
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

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class SkillManager : MonoBehaviour
    {
        private static CreateHands _createHands;
        //private static Lrig _lrig;
        private static SetSigni _setSigni;
        //private static EnerManager _enerManager;
        //private static LifeCloth _lifeCloth;
        private static Trash _trash;
        //private static Check _check;
        private static CardInfo _cardInfo;
        private static ShowDeck _showDeck;
        private static ShowCard _showCard;

        public GameManager GameManager;
        public SkillChang SkillChang;
        public SkillChu SkillChu;
        public SkillQi SkillQi;
        public SkillSpell SkillSpell;
        public SkillBrust SkillBrust;
        public MyCard MyCard;

        public void Setup()
        {

            _createHands = GameManager.CreateHands;
            _setSigni = GameManager.SetSigni;
            _trash = GameManager.Trash;
            _cardInfo = GameManager.CardInfo;
            _showDeck = GameManager.ShowDeck;
            _showCard = GameManager.ShowCard;

            SkillChang.Setup();
            SkillChu.Setup();
            SkillQi.Setup();
            SkillSpell.Setup();
            SkillBrust.Setup();
            MyCard.Setup();
        }
        /// <summary>
        /// 设置精灵时
        /// </summary>
        /// <param name="card"></param>
        public void SigniSet(Card card)
        {
            SkillChang.SigniSet(card);
        }
        /// <summary>
        /// 精灵离场时
        /// </summary>
        public void SigniOut()
        {
            SkillChang.SigniOut();
        }
        /// <summary>
        /// 我方回合开始时/对方回合结束时
        /// </summary>
        public void MyRoundStart()
        {
            SkillChang.MyRoundStart();
        }
        /// <summary>
        /// 我方回合结束时/对方回合开始时
        /// </summary>
        public void MyRoundOver()
        {
            SkillChang.MyRoundOver();
        }
        /// <summary>
        /// 能量变化时(双方)
        /// </summary>
        public void EnerCharge()
        {
            SkillChang.EnerCharge();
        }
        /// <summary>
        /// 设置分身时
        /// </summary>
        public void LrigSet()
        {
            SkillChang.LrigSet();
        }
        /// <summary>
        /// 手卡变化时(双方)
        /// </summary>
        public void HandChange()
        {
            SkillChang.HandChange();
        }

        /// <summary>
        /// 赋值效果至卡处
        /// </summary>
        /// <param name="card">赋值的卡</param>
        public void GetEffectByCard(Card card)
        {
            if (SkillChang.CardEffectChangDictionary.ContainsKey(card.CardId))
            {
                Debug.Log("Chang:" + card.CardId);
                card.EffectChang = SkillChang.CardEffectChangDictionary[card.CardId];
            }
            if (SkillChu.CardEffectChuDictionary.ContainsKey(card.CardId))
            {
                Debug.Log("Chu:" + card.CardId);
                card.EffectChu = SkillChu.CardEffectChuDictionary[card.CardId];
            }
            if (SkillQi.CardEffectQiDictionary.ContainsKey(card.CardId))
            {
                Debug.Log("Qi:" + card.CardId);
                card.EffectQi = SkillQi.CardEffectQiDictionary[card.CardId];
            }
            if (SkillSpell.CardEffectSpellDictionary.ContainsKey(card.CardId))
            {
                Debug.Log("Spell:" + card.CardId);
                card.EffectSpell = SkillSpell.CardEffectSpellDictionary[card.CardId];
            }
            if (SkillBrust.CardEffectBrustDictionary.ContainsKey(card.CardId))
            {
                Debug.Log("Brust:" + card.CardId);
                card.Brust = SkillBrust.CardEffectBrustDictionary[card.CardId];
            }
        }

        #region 卡组操作

        /// <summary>
        /// 抽卡
        /// </summary>
        /// <param name="num">需要抽卡的数量</param>
        public void DropCard(int num,Action succeed = null)
        {
            StartCoroutine(_createHands.DropCard(num));
            _createHands.ShowTheUseBtn();
            if (succeed != null)
                succeed();
        }

        public static void WashDeck()
        {
            _showDeck.WashMainDeck();
        }

        /// <summary>
        /// 丢弃手牌(自己选择)
        /// </summary>
        /// <param name="num">需要丢弃的手牌数</param>
        /// <param name="myAction"></param>
        public static void DesCard(int num, Action myAction = null)
        {
            _createHands.DisTheUseBtn();
            int desnum = _createHands.MyHands.Count - num;
            _createHands.SetDesBtnOverSix(desnum, () =>
            {
                if (myAction != null)
                    myAction();
                _createHands.ShowTheUseBtn();
            });
            _createHands.DesMyHandsOverSix();
        }

        /// <summary>
        /// 随机丢弃手牌
        /// </summary>
        public static void DesCardRandom()
        {
            _createHands.DestoryHandRamdom();
        }

        /// <summary>
        /// 随机丢弃对方手卡
        /// </summary>
        public static void DesOtherCardRamdom()
        {
            GameManager.RpcDesHandRandom();
        }

        /// <summary>
        /// 对方自己选择丢弃手卡
        /// </summary>
        /// <param name="num">丢弃数量</param>
        public static void DesOtherCard(int num)
        {
            GameManager.RpcDesHand(num);
        }

        /// <summary>
        /// 查看卡组顶第N张卡,并按选择的顺序排列,且不选择的话会丢至废弃(可选)
        /// </summary>
        /// <param name="num">N</param>
        /// <param name="bTrash">是否附带丢弃效果</param>
        public static void CheckDeckNumAndSort(int num, bool bTrash)
        {
            var showlist = new List<Card>();
            for (int i = 0; i < num; i++)
            {
                var card = _showDeck.MainDeck[0 + i];
                showlist.Add(card);
            }

            string strash = bTrash ? "不选择的话会被丢弃" : "";
            _cardInfo.ShowCardInfo(true);
            _cardInfo.SetUp("查看卡组顶" + num + "张卡,按选择的顺序排列" + strash, showlist, num, () =>
            {
                for (int i = 0; i < _cardInfo.SelectHands.Count; i++)
                {
                    _showDeck.MainDeck[0 + i] = _cardInfo.SelectHands[i].MyCard;
                    showlist.Remove(_cardInfo.SelectHands[i].MyCard);
                }

                if (bTrash)
                {
                    Card card;
                    for (int j = 0; j < showlist.Count; j++)
                    {
                        card = showlist[j];
                        _trash.AddTrash(card);
                        _showDeck.MainDeck.Remove(card);
                    }
                }

                _cardInfo.ShowCardInfo(false);
            });

        }


        public static List<Card> FindCardByCondition(Func<Card, bool> condition)
        {
            var cards = _showDeck.MainDeck;
            var show = cards.Where(condition);
            return show.ToList();

        }

        /// <summary>
        /// 查看卡组顶第N张卡，且是某些种组的话加入手卡
        /// </summary>
        /// <param name="num">Number.</param>
        /// <param name="type">Type.</param>
        public void CheckDeckAndAddHand(int num, List<string> type)
        {
            var showlist = new List<Card>();
            for (int i = 0; i < num; i++)
            {
                var card = _showDeck.MainDeck[0 + i];
                showlist.Add(card);
            }
            _cardInfo.ShowCardInfo(true);
            _cardInfo.SetUp("查看卡组顶" + num + "张卡", showlist, num, () =>
            {
                for (int i = 0; i < type.Count; i++)
                {
                    for (int j = 0; j < showlist.Count; j++)
                    {
                        if (showlist[j].Type == type[i])
                        {
                            _createHands.CreateHandFromDeck(showlist[j]);
                        }
                    }
                }

                _cardInfo.ShowCardInfo(false);
            });

        }

        /// <summary>
        /// 返回手卡
        /// </summary>
        /// <param name="i">The index.</param>
        public void BackHand(int i)
        {
            GameManager.RpcBackHand(i);
        }

        /// <summary>
        /// 等级是多少的全部删除
        /// </summary>
        /// <param name="sourcecard"></param>
        /// <param name="level">等级</param>
        /// <param name="bOne">单张还是全部</param>
        public static void DesHandByLevel(Card sourcecard, int level, bool bOne)
        {
            GameManager.RpcDesHandByLevel(level, bOne);
            _showCard.ShowMyCardEffect(sourcecard);
            GameManager.RpcOtherCardBuff(sourcecard.CardId);
        }

        #endregion

        #region 记得出场与离场都调用一次

        /// <summary>
        /// 全体攻击力增加
        /// </summary>
        /// <param name="value">Value.</param>
        public static void AddAtkAll(int value)
        {
            var signis = _setSigni.Signi;
            for (int i = 0; i < signis.Length; i++)
            {
                if (signis[i] != null)
                {
                    signis[i].Atk += value;
                    GameManager.RpcOtherCardAtkChange(i, value);
                }
            }
        }

        public static Card[] GetSignis(bool bmy)
        {
            return bmy ? _setSigni.Signi : _setSigni.OtherSigni;
        }

        /// <summary>
        /// 满足条件后攻击力加
        /// </summary>
        /// <param name="card">Card.</param>
        /// <param name="value">Value.</param>
        /// <param name="condition">If set to <c>true</c> condition.</param>
        public static void AddAtk(Card card, int value)
        {
            //            card.Atk += value;
            for (int i = 0; i < _setSigni.Signi.Length; i++)
            {
                if (_setSigni.Signi[i] == card)
                {
                    _setSigni.Signi[i].Atk += value;
                    GameManager.RpcOtherCardAtkChange(i, value);
                }
            }
        }

        #endregion

        #region 某些条件判断

        public static bool BSigniInGround(string cardname)
        {
            bool bin = false;
            var signis = _setSigni.Signi;
            for (int i = 0; i < signis.Length; i++)
            {
                if (signis[i] != null && signis[i].CardName == cardname)
                {
                    bin = true;
                }
            }
            return bin;
        }

        public static bool BSigniInGround(Card card)
        {
            bool bin = false;
            var signis = _setSigni.Signi;
            for (int i = 0; i < signis.Length; i++)
            {
                if (signis[i] != null && signis[i] == card)
                {
                    bin = true;
                }
            }
            return bin;
        }

        #endregion

        /// <summary>
        /// 是否选择好了
        /// </summary>
        public static bool BSelected;

        #region 选定发动类,要多一个succeed的回调

        /// <summary>
        /// 选择返回对方一只精灵回手卡
        /// </summary>
        public static void BackHand(Card sourcecard, Action succeed = null, Func<Card, bool> condiction = null)
        {
            DebuffOtherSigni(sourcecard, GameManager.RpcBackHand, succeed, condiction);
        }


        /// <summary>
        /// 选择破坏对方一只精灵
        /// </summary>
        public static void Baninish(Card sourcecard, Action succeed = null, Func<Card, bool> condiction = null)
        {
            DebuffOtherSigni(sourcecard, _setSigni.BanishOtherSigni, succeed, condiction);
        }

        /// <summary>
        /// 横置对方精灵
        /// </summary>
        public static void HorizionSigni(Card sourcecard, Action succeed = null, Func<Card, bool> condiction = null)
        {
            DebuffOtherSigni(sourcecard, GameManager.RpcSetOtherSigniSet, succeed, condiction);
        }

        /// <summary>
        /// 冰冻对方精灵
        /// </summary>
        public static void FreezedSigni(Card sourcecard, Action succeed = null, Func<Card, bool> condiction = null)
        {
            DebuffOtherSigni(sourcecard, i =>
            {
                _setSigni.OtherSigni[i].Bfreeze = true;
                GameManager.RpcOtherDebuff(1, i, true);
            }, succeed, condiction);

        }

        /// <summary>
        /// 双击
        /// </summary>
        public static void DoubleSigni(Card sourcecard, Action succeed = null, Func<Card, bool> condiction = null)
        {
            BuffSigni(sourcecard, i =>
            {
                _setSigni.Signi[i].Bdouble = true;
                GameManager.RpcMyBuff(2, i, true);
            }, succeed, condiction);
        }

        /// <summary>
        /// 枪兵
        /// </summary>
        public static void LancerSigni(Card sourcecard, Action succeed = null, Func<Card, bool> condiction = null)
        {
            BuffSigni(sourcecard, i =>
            {
                _setSigni.Signi[i].Blancer = true;
                GameManager.RpcMyBuff(1, i, true);
            }, succeed, condiction);
        }


        /// <summary>
        /// 选择对方一只精灵给予debuff
        /// </summary>
        /// <param name="sourcecard">发动效果的卡</param>
        /// <param name="debuff">负面效果</param>
        /// <param name="succeed">成功后下一步</param>
        /// <param name="condiction">是否需要符合条件</param>
        private static void DebuffOtherSigni(Card sourcecard, Action<int> debuff, Action succeed = null, Func<Card, bool> condiction = null)
        {
            _setSigni.ShowOtherSelections(true, condiction);
            _setSigni.SetSelections(false, null, true, i =>
            {
                debuff(i);
                _showCard.ShowMyCardEffect(sourcecard);
                GameManager.RpcOtherCardBuff(sourcecard.CardId);
                BSelected = true;
                if (succeed != null)
                    succeed();
            }, false);
        }

        /// <summary>
        /// 选择我方一只精灵给予buff
        /// </summary>
        /// <param name="sourcecard">发动效果的卡</param>
        /// <param name="buff">buff</param>
        /// <param name="succeed">成功后下一步</param>
        /// <param name="condiction">是否需要符合条件</param>
        private static void BuffSigni(Card sourcecard, Action<int> buff, Action succeed = null,
            Func<Card, bool> condiction = null)
        {
            _setSigni.ShowMySelections(true, condiction);
            _setSigni.SetSelections(true, i =>
            {
                buff(i);
                _showCard.ShowMyCardEffect(sourcecard);
                GameManager.RpcOtherCardBuff(sourcecard.CardId);

                BSelected = true;
                if (succeed != null)
                    succeed();

            }, false, null, false);
        }


        public static void ShowMyCard(Card card)
        {
            _showCard.ShowMyCardEffect(card);
            GameManager.RpcOtherCardBuff(card.CardId);
        }

        /// <summary>
        /// 10s后隐藏选择按钮
        /// </summary>
        public static void DisSelect()
        {
            _setSigni.ShowOtherSelections(false);
            _setSigni.ShowMySelections(false);
        }

        #endregion


        #region 103张卡的效果!!!! 1.常 2.出 3.起





        #endregion
    }
}

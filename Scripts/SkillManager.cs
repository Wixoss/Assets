using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class SkillManager : MonoBehaviour
    {
        public CreateHands CreateHands;
        public Lrig Lrig;
        public SetSigni SetSigni;
        public EnerManager EnerManager;
        public LifeCloth LifeCloth;
        public Trash Trash;
        public Check Check;
        public CardInfo CardInfo;
        public ShowDeck ShowDeck;
        public GameManager GameManager;

        #region 卡组操作

        /// <summary>
        /// 抽卡
        /// </summary>
        /// <param name="num">需要抽卡的数量</param>
        public void DropCard(int num)
        {
            StartCoroutine(CreateHands.DropCard(num));
        }

        /// <summary>
        /// 丢弃手牌(自己选择)
        /// </summary>
        /// <param name="num">需要丢弃的手牌数</param>
        public void DesCard(int num)
        {
            CreateHands.SetDesBtnOverSix(CreateHands.MyHands.Count - num, null);
            CreateHands.DesMyHandsOverSix();
        }

        /// <summary>
        /// 随机丢弃手牌
        /// </summary>
        public void DesCardRandom()
        {
            CreateHands.DestoryHandRamdom();
        }

        /// <summary>
        /// 查看卡组顶第N张卡,并按选择的顺序排列,且不选择的话会丢至废弃(可选)
        /// </summary>
        /// <param name="num">N</param>
        /// <param name="bTrash">是否附带丢弃效果</param>
        public void CheckDeckNumAndSort(int num,bool bTrash)
        {
            var showlist = new List<Card>();
            for(int i =0;i<num;i++)
            {
                var card = ShowDeck.MainDeck[ShowDeck.MainDeck.Count - 1 - i];
                showlist.Add(card);
            }
            CardInfo.ShowCardInfo(true);
            CardInfo.SetUp("查看卡组顶" + num + "张卡", showlist, num, () => 
            {
                for(int i =0;i<CardInfo.SelectHands.Count;i++)
                {
                    ShowDeck.MainDeck[ShowDeck.MainDeck.Count - 1 - i] = CardInfo.SelectHands[i].MyCard;
                    showlist.Remove(CardInfo.SelectHands[i].MyCard);
                }

                if(bTrash)
                {
                    Card card;
                    for(int j = 0;j < showlist.Count;j++)
                    {
                        card = showlist[j];
                        Trash.AddTrash(card);
                        ShowDeck.MainDeck.Remove(card);
                    }
                }

                CardInfo.ShowCardInfo(false);
            });
        }

        /// <summary>
        /// 查看卡组顶第N张卡，且是某些种组的话加入手卡
        /// </summary>
        /// <param name="num">Number.</param>
        /// <param name="type">Type.</param>
        public void CheckDeckAndAddHand(int num,List<string> type)
        {
            var showlist = new List<Card>();
            for(int i =0;i<num;i++)
            {
                var card = ShowDeck.MainDeck[ShowDeck.MainDeck.Count - 1 - i];
                showlist.Add(card);
            }
            CardInfo.ShowCardInfo(true);
            CardInfo.SetUp("查看卡组顶" + num + "张卡", showlist, num, () => 
            {
                for(int i = 0;i<type.Count;i++)
                {
                    for(int j = 0;j<showlist.Count;j++)
                    {
                        if(showlist[j].Type == type[i])
                        {
                            CreateHands.CreateHandByCard(showlist[j]);
                            ShowDeck.MainDeck.Remove(showlist[j]);
                        }
                    }
                }

                CardInfo.ShowCardInfo(false);
            });
        }

        #endregion

        #region 记得出场与离场都调用一次

        /// <summary>
        /// 全体攻击力增加
        /// </summary>
        /// <param name="value">Value.</param>
        public void AddAtkAll(int value)
        {
            var signis = SetSigni.Signi;
            for (int i =0; i<signis.Length; i++)
            {
                signis[i].Atk += value;
            }
        }

        /// <summary>
        /// 满足条件后攻击力加
        /// </summary>
        /// <param name="card">Card.</param>
        /// <param name="value">Value.</param>
        /// <param name="condition">If set to <c>true</c> condition.</param>
        public void AddAtk(Card card,int value,bool condition)
        {
            if (condition)
            {
                card.Atk += value;
            }
        }

        #endregion

        #region 某些条件判断

        public bool BSigniInGround(string name)
        {
            bool bIn = false;
            var signis = SetSigni.Signi;
            for (int i =0; i<signis.Length; i++)
            {
                if(signis[i].CardName == name)
                {
                    bIn = true;
                }
            }
            return bIn;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class Card
    {
        public enum State
        {
            手牌,
            场上,
            能量区,
            废弃区,
            其他,
        }

        public enum CardType
        {
            法术卡,
            技艺卡,
            精灵卡,
            分身卡,
            其他,
        }

        public enum CardColor
        {
            红,
            蓝,
            白,
            黑,
            绿,
            无,
            万花,
        }

        /// <summary>
        /// 卡名字:巴洛克防御
        /// </summary>
        public string CardName;
        /// <summary>
        /// 卡牌id:WX-01
        /// </summary>
        public string CardId;
        /// <summary>
        /// 卡牌描述:直到回合结束...
        /// </summary>
        public string CardDetail;
        /// <summary>
        /// 使用时点
        /// </summary>
        public GameManager.Timing Timing;
        /// <summary>
        /// 卡牌种类
        /// </summary>
        public CardType MyCardType;
        /// <summary>
        /// 卡牌颜色
        /// </summary>
        public CardColor MyCardColor;
        /// <summary>
        /// 卡牌的状态,手牌中还是?
        /// </summary>
        public State MyState = State.其他;
        /// <summary>
        /// 卡图
        /// </summary>
        public Texture2D CardTexture;

        /// <summary>
        /// 以防丧心病狂的6种颜色都需求,使用是最好判断下数组数量
        /// </summary>
        public List<Ener> Cost = new List<Ener>();

        /// <summary>
        /// 出场是需要达成的条件
        /// </summary>
        public Action ConditionAction;


        /// <summary>
        /// 能量的能量类型与数量
        /// </summary>
        public struct Ener
        {
            public enum EnerType
            {
                红,
                蓝,
                白,
                黑,
                绿,
                万花,
                无,
            }

            public EnerType MyEnerType;
            public int Num;
        }

        /// <summary>
        /// 被当作能量时的能量
        /// </summary>
        public Ener MyEner;

        /// <summary>
        /// 等级
        /// </summary>
        public int Level;

        /// <summary>
        /// 上限
        /// </summary>
        public int Limit;

        /// <summary>
        /// 攻击力
        /// </summary>
        public int Atk;

        /// <summary>
        /// 能否防御
        /// </summary>
        public bool BCanGuard;

        /// <summary>
        ///有无迸发
        /// </summary>
        public bool HasBrust;

        /// <summary>
        /// 效果s(待实现)
        /// </summary>
        public Action<Card> Effect1 = card => Debug.Log("Effect1");
        public Action<Card> Effect2 = card => Debug.Log("Effect2");
        public Action<Card> Effect3 = card => Debug.Log("Effect3");
        public Action<Card> Brust = card => Debug.Log("Brust");

        public Card(string cardid)
        {
            MyState = State.其他;
            MyCardType = CardType.其他;
            SetCardById(cardid);
            CardTexture = Resources.Load<Texture2D>(CardId);
        }

        /// <summary>
        /// 读取xml
        /// </summary>
        /// <param name="cardid"></param>
        private void SetCardById(string cardid)
        {
            //CardName
            CardId = cardid;
        }
    }

    public class MyCard : MonoBehaviour
    {
        public List<Texture2D> CardTextures = new List<Texture2D>();
        public List<string> MyCardid = new List<string>();
        public List<string> MyLrigid = new List<string>();

        [ContextMenu("创建卡组")]
        public void Awake()
        {
            CreateCardByXml();

            for (int i = 0; i < MyLrigid.Count; i++)
            {
                var card = new Card(MyLrigid[i]);
                CreateDetail(card);
                DataSource.LrigDeck.Add(card);
                CardTextures.Add(card.CardTexture);
            }

            for (int i = 0; i < MyCardid.Count; i++)
            {
                var card = new Card(MyCardid[i]);
                CreateDetail(card);
                DataSource.MainDeck.Add(card);
                CardTextures.Add(card.CardTexture);
            }

        }

        /// <summary>
        /// 假数据
        /// </summary>
        public void CreateCardByXml()
        {
            string id = "";
            for (int i = 0; i < 40; i++)
            {
                var num = Random.Range(1, 41);
                id = num < 10 ? "0" + num : num.ToString();
                MyCardid.Add("WX1-0" + id);
            }

            for (int i = 0; i < 10; i++)
            {
                var num = Random.Range(1, 41);
                id = num < 10 ? "0" + num : num.ToString();
                MyLrigid.Add("WX1-0" + id);
            }

        }

        private int i = 0;

        public void CreateDetail(Card card)
        {
            int cardtype = 0;
            int cardColor = 0;
            int cardEner = 0;

            i++;

            cardtype = i / 10 + 1;
            cardColor = 1;
            cardEner = cardColor;

            card.Level = i % 2 == 0 ? 0 : 1;
            card.Limit = i / 10 + 3;
            card.BCanGuard = true;

            switch (cardtype)
            {
                case 1:
                    card.MyCardType = Card.CardType.分身卡;
                    break;
                case 2:
                    card.MyCardType = Card.CardType.技艺卡;
                    break;
                case 4:
                    card.MyCardType = Card.CardType.法术卡;
                    break;
                case 5:
                    card.MyCardType = Card.CardType.精灵卡;
                    break;
            }

            switch (cardColor)
            {
                case 1:
                    card.MyCardColor = Card.CardColor.万花;
                    break;
                case 2:
                    card.MyCardColor = Card.CardColor.无;
                    break;
                case 3:
                    card.MyCardColor = Card.CardColor.白;
                    break;
                case 4:
                    card.MyCardColor = Card.CardColor.红;
                    break;
                case 5:
                    card.MyCardColor = Card.CardColor.绿;
                    break;
                case 6:
                    card.MyCardColor = Card.CardColor.蓝;
                    break;
                case 7:
                    card.MyCardColor = Card.CardColor.黑;
                    break;
            }

            switch (cardEner)
            {
                case 1:
                    card.MyEner.MyEnerType = Card.Ener.EnerType.万花;
                    card.MyEner.Num = 1;
                    break;
                case 2:
                    card.MyEner.MyEnerType = Card.Ener.EnerType.无;
                    card.MyEner.Num = 1;
                    break;
                case 3:
                    card.MyEner.MyEnerType = Card.Ener.EnerType.白;
                    card.MyEner.Num = 1;
                    break;
                case 4:
                    card.MyEner.MyEnerType = Card.Ener.EnerType.红;
                    card.MyEner.Num = 1;
                    break;
                case 5:
                    card.MyEner.MyEnerType = Card.Ener.EnerType.绿;
                    card.MyEner.Num = 1;
                    break;
                case 6:
                    card.MyEner.MyEnerType = Card.Ener.EnerType.蓝;
                    card.MyEner.Num = 1;
                    break;
                case 7:
                    card.MyEner.MyEnerType = Card.Ener.EnerType.黑;
                    card.MyEner.Num = 1;
                    break;
            }

            card.Cost.Add(new Card.Ener()
            {
                MyEnerType = Card.Ener.EnerType.万花,
                Num = i % 2 == 0 ? 0 : 1,
            });

            card.Cost.Add(new Card.Ener()
            {
                MyEnerType = Card.Ener.EnerType.无,
                Num = i % 2 == 0 ? 0 : 1,
            });

            //            card.Cost[0].MyEnerType= Card.Ener.EnerType.万花;
            //            card.Cost[0].Num = i % 2 == 0 ? 0 : 1;
        }
    }
}

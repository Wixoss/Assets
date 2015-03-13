﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        /// 使用时点最多有3个
        /// </summary>
        public List<GameManager.Timing> MyTiming = new List<GameManager.Timing>();
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
        //public State MyState = State.其他;
        /// <summary>
        /// 卡图
        /// </summary>
        public Texture2D CardTexture;

        /// <summary>
        /// 发动效果所需要的费用
        /// </summary>
        public List<Ener> Cost = new List<Ener>();

        /// <summary>
        /// 成长所需要的费用
        /// </summary>
        public List<Ener> GrowCost = new List<Ener>();

        /// <summary>
        /// 出场是需要达成的条件
        /// </summary>
        public Action ConditionAction;

        /// <summary>
        /// 类型,小玉还是其他(分身专用)
        /// </summary>
        public string Type;

        /// <summary>
        /// 小玉限定还是什么?(主卡专用)
        /// </summary>
        public string TypeOnly;

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
        public Action<Card> Effect_Qi = card => Debug.Log("Effect_Qi");
        public Action<Card> Effect_Chang = card => Debug.Log("Effect_Chang");
        public Action<Card> Effect_Chu = card => Debug.Log("Effect_Chu");
        public Action<Card> Effect_Spell = card => Debug.Log("Spell!" + card.CardId);
        public Action<Card> Brust = card => Debug.Log("Brust!" + card.CardId);

        public Card(string cardid)
        {
            //MyState = State.其他;
            SetCardById(cardid);
            CardTexture = Resources.Load<Texture2D>(CardId);
        }

        /// <summary>
        /// 读取xml
        /// </summary>
        /// <param name="cardid"></param>
        private void SetCardById(string cardid)
        {
            var card = CreateCardByXml.GetCardByCardId(cardid);

            foreach (var i in card)
            {
                CardId = i.Element("CardId").Value;
                CardName = i.Element("CardName").Value;
                MyCardColor = GetCardColorByString(i.Element("Color").Value);
                MyCardType = GetCardTypeByString(i.Element("CardType").Value);
                MyEner.MyEnerType = GetEnerTypeByString(i.Element("Color").Value);

                if (i.Element("Level") != null)
                {
                    Level = Convert.ToInt16(i.Element("Level").Value);
                }
                if (i.Element("BBrust") != null)
                {
                    HasBrust = Convert.ToInt16(i.Element("BBrust").Value) != 0;
                }
                if (i.Element("CardDetail") != null)
                {
                    CardDetail = i.Element("CardDetail").Value;
                }
                if (i.Element("EffectCost") != null)
                {
                    Cost = GetCostByString(i.Element("EffectCost").Value);
                }
                if (i.Element("GrowCost") != null)
                {
                    GrowCost = GetCostByString(i.Element("GrowCost").Value);
                }
                if (i.Element("Type") != null)
                {
                    Type = i.Element("Type").Value;
                }
                if (i.Element("Limit") != null)
                {
                    Limit = Convert.ToInt16(i.Element("Limit").Value);
                }
                if (i.Element("Timing") != null)
                {
                    MyTiming = GetTimingByString(i.Element("Timing").Value);
                }
                if (i.Element("TypeOnly") != null)
                {
                    TypeOnly = i.Element("TypeOnly").Value;
                }
                if (i.Element("BDef") != null)
                {
                    BCanGuard = Convert.ToInt16(i.Element("BDef").Value) != 0;
                }
                if (i.Element("Atk") != null)
                {
                    Atk = Convert.ToInt16(i.Element("Atk").Value);
                }
            }

            //			switch (MyCardType) 
            //			{
            //			case CardType.法术卡:
            //
            //			}
            MyEner.Num = 1;
        }

        private CardColor GetCardColorByString(string color)
        {
            switch (color)
            {
                case "白":
                    return CardColor.白;
                case "黑":
                    return CardColor.黑;
                case "红":
                    return CardColor.红;
                case "蓝":
                    return CardColor.蓝;
                case "绿":
                    return CardColor.绿;
                case "万花":
                    return CardColor.万花;
                case "无":
                    return CardColor.无;
                default:
                    return CardColor.无;
            }
        }

        private CardType GetCardTypeByString(string type)
        {
            switch (type)
            {
                case "魔法":
                    return CardType.法术卡;
                case "分身":
                    return CardType.分身卡;
                case "技艺":
                    return CardType.技艺卡;
                case "精灵":
                    return CardType.精灵卡;
                default:
                    return CardType.其他;
            }
        }

        private Ener.EnerType GetEnerTypeByString(string type)
        {
            switch (type)
            {
                case "白":
                    return Ener.EnerType.白;
                case "黑":
                    return Ener.EnerType.黑;
                case "红":
                    return Ener.EnerType.红;
                case "蓝":
                    return Ener.EnerType.蓝;
                case "绿":
                    return Ener.EnerType.绿;
                case "万花":
                    return Ener.EnerType.万花;
                case "无":
                    return Ener.EnerType.无;
                default:
                    return Ener.EnerType.无;
            }
        }

        private List<Ener> GetCostByString(string cost)
        {
            var myEners = new List<Ener>();
            var ener = cost.Split(',');

            for (int i = 0; i < ener.Length; i++)
            {
                string[] typecolor = ener[i].Split(':');
                myEners.Add(new Ener
                {
                    MyEnerType = GetEnerTypeByString(typecolor[0]),
                    Num = Convert.ToInt16(typecolor[1])
                });
            }
            return myEners;
        }

        private List<GameManager.Timing> GetTimingByString(string timing)
        {
            var time = timing.Split(',');
            List<GameManager.Timing> myTimings = new List<GameManager.Timing>();
            for (int i = 0; i < time.Length; i++)
            {
                switch (time[i])
                {
                    case "主要阶段":
                        myTimings.Add(GameManager.Timing.主要阶段);
                        break;
                    case "攻击阶段":
                        myTimings.Add(GameManager.Timing.攻击宣言阶段);
                        break;
                    case "魔法切入":
                        myTimings.Add(GameManager.Timing.魔法切入阶段);
                        break;
                }
            }
            return myTimings;
        }
    }

    public class MyCard : MonoBehaviour
    {
        public List<Texture2D> CardTextures = new List<Texture2D>();
        public List<string> MyCardid;
        public List<string> MyLrigid;

        public ShowDeck ShowDeck;

        [ContextMenu("创建卡组")]
        public void Awake()
        {
            CreateCardByXml();

            for (int i = 0; i < MyLrigid.Count; i++)
            {
                var card = new Card(MyLrigid[i]);
                MyLrigid[i] = MyLrigid[i] + "   " + card.CardName + "   " + card.MyCardType;
                //DataSource.LrigDeck.Add(card);
                CardTextures.Add(card.CardTexture);
                ShowDeck.LrigDeck.Add(card);
            }

            for (int i = 0; i < MyCardid.Count; i++)
            {
                var card = new Card(MyCardid[i]);
                MyCardid[i] = MyCardid[i] + "   " + card.CardName + "   " + card.MyCardType;
                //DataSource.MainDeck.Add(card);
                CardTextures.Add(card.CardTexture);
                ShowDeck.MainDeck.Add(card);
            }

            ShowDeck.WashMainDeck();
        }

        public List<Card> RandomCards(List<Card> old)
        {
            var newcard = new List<Card>();
            int rand;
            for (int i = 0; i < old.Count; i++)
            {
                rand = Random.Range(0, old.Count);
                newcard.Add(old[rand]);
                old.Remove(old[rand]);
            }
            return newcard;
        }

        /// <summary>
        /// 假Deck数据
        /// </summary>
        public void CreateCardByXml()
        {
            MyLrigid = new List<string>()
            {
                "WD01-001",
                "WD01-002",
                "WD01-003",
                "WD01-004",
                "WD01-005",
                "WD01-006",
                "WD01-007",
                "WD01-008",
                "WX01-023",
                "WX01-018",
            };
            
            MyCardid = new List<string>()
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
        }
    }
}

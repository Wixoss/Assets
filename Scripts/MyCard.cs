using System;
using System.Collections;
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
        /// 起动效果所需要的费用
        /// </summary>
        public List<Ener> EffectCost_Qi = new List<Ener>();

        /// <summary>
        /// 出场效果所需要的费用
        /// </summary>
        public List<Ener> EffectCost_Chu = new List<Ener>();

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
        public string TypeOnly = "";

        /// <summary>
        /// 精灵出场时调用
        /// </summary>
        public SkillChang.EffectChang MyEffectChangSigniSet = new SkillChang.EffectChang();
        /// <summary>
        /// 精灵离场时调用
        /// </summary>
        public SkillChang.EffectChang MyEffectChangSigniOut = new SkillChang.EffectChang();
        /// <summary>
        /// 我放回合开始时
        /// </summary>
        public SkillChang.EffectChang MyEffectChangMyRoundStart = new SkillChang.EffectChang();
        /// <summary>
        /// 敌方回合开始时
        /// </summary>
        public SkillChang.EffectChang MyEffectChangMyRoundOver = new SkillChang.EffectChang();
        /// <summary>
        /// 我方充能时调用
        /// </summary>
        public SkillChang.EffectChang MyEffectChangEnerCharge = new SkillChang.EffectChang();
        /// <summary>
        /// 我发设置分身时
        /// </summary>
        public SkillChang.EffectChang MyEffectChangLrigSet = new SkillChang.EffectChang();

        /// <summary>
        /// 手牌变化时
        /// </summary>
        public SkillChang.EffectChang MyEffectChangHandChange = new SkillChang.EffectChang();

        /// <summary>
        /// 攻击力变化时
        /// </summary>
        public SkillChang.EffectChang MyAttackChange = new SkillChang.EffectChang();

        /// <summary>
        /// 攻击时
        /// </summary>
        public SkillChang.EffectChang MyAtking = new SkillChang.EffectChang();

        /// <summary>
        /// 精灵自己离场时调用
        /// </summary>
        public Action<Card> SigniOutAction;

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

        public int BaseAtk;

        /// <summary>
        /// 是否已经发动了常效果
        /// </summary>
        private bool _bchang;
        public bool BChang 
        {
            get{return _bchang;}
            set
            {
                _bchang = value;
                if(_bchang)
                {
                    SkillManager._showCard.ShowMyCardEffect(this);
                    GameManager.RpcOtherCardBuff(this.CardId);
                }
            }
        }

        /// <summary>
        /// 是否竖置
        /// </summary>
        public bool Bset;

        /// <summary>
        /// 能否防御
        /// </summary>
        public bool BCanGuard;

        /// <summary>
        ///有无迸发
        /// </summary>
        public bool HasBrust;

        /// <summary>
        /// 这卡有的buff，枪兵，双击，冰冻
        /// </summary>
        public string Buff
        {
            get { return _slancer + _sdouble + _sfreeze + _sCantAttack; }
        }

        private bool _blancer;
        /// <summary>
        /// 枪兵
        /// </summary>
        public bool Blancer
        {
            get { return _blancer; }
            set
            {
                _blancer = value;
                _slancer = _blancer ? "[7CFC00]" + "[枪兵]" + "[-]" + "\n" : "";
            }
        }
        private string _slancer;

        private bool _bfreeze;
        /// <summary>
        /// 冰冻
        /// </summary>
        public bool Bfreeze
        {
            get { return _bfreeze; }
            set
            {
                _bfreeze = value;
                _sfreeze = _bfreeze ? "[87CEFA]" + "[冰冻]" + "[-]" + "\n" : "";
            }
        }
        private string _sfreeze;


        private bool _bdouble;
        /// <summary>
        /// 双击
        /// </summary>
        public bool Bdouble
        {
            get { return _bdouble; }
            set
            {
                _bdouble = value;
                _sdouble = _bdouble ? "[DC143C]" + "[双重击溃]" + "[-]" + "\n" : "";
            }
        }
        private string _sdouble;

        private bool _bCantAttack;
        /// <summary>
        /// 不能攻击(巴洛克防御)
        /// </summary>
        public bool BCantAttack
        {
            get { return _bCantAttack; }
            set
            {
                _bCantAttack = value;
                _sCantAttack = _bCantAttack ? "[FFFF99]" + "[不能攻击]" + "[-]" + "\n" : "";
            }
        }

        private string _sCantAttack;

        /// <summary>
        /// 效果s(待实现)
        /// </summary>
        public Action<Card> EffectChang = null; //常
        public Action<Card> EffectChu = null; //出
        public Action<Card> EffectQi = null;//起
        public Action<Card> EffectSpell = null;
        public Action<Card> Brust = null;

        public Card(string cardid)
        {
            //MyState = State.其他;
            //SetCardById(cardid);
            CardId = cardid;
            CardTexture = Resources.Load<Texture2D>(CardId);
        }

        //        public void GetAtkByCardId(string cardid)
        //        {
        //            var card = CreateCardByXml.GetCardByCardId(CardId);
        //            foreach (var i in card)
        //            {
        //                if(i.Element("Atk")!=null)
        //                {
        //                    Atk = Convert.ToInt16(i.Element("Atk").Value);
        //                }
        //            }
        //        }

        /// <summary>
        /// 读取xml
        /// </summary>
        /// <param name="cardid"></param>
        public void SetCardById()
        {
            var card = CreateCardByXml.GetCardByCardId(CardId);

            foreach (var i in card)
            {
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
                if (i.Element("EffectCost_Qi") != null)
                {
                    EffectCost_Qi = GetCostByString(i.Element("EffectCost_Qi").Value);
                }
                if (i.Element("EffectCost_Chu") != null)
                {
                    EffectCost_Chu = GetCostByString(i.Element("EffectCost_Chu").Value);
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
                    BaseAtk = Convert.ToInt16(i.Element("Atk").Value);
                    Atk = BaseAtk;
                }
            }
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

        /// <summary>
        /// 重置几个属性
        /// </summary>
        public void ResetCardConfig()
        {
            BCantAttack = false;
            Bdouble = false;
            Bfreeze = false;
            Blancer = false;
            BChang = false;
            Atk = BaseAtk;
            MyEffectChangSigniSet = new SkillChang.EffectChang();
            MyEffectChangSigniOut = new SkillChang.EffectChang();
            MyEffectChangMyRoundStart = new SkillChang.EffectChang();
            MyEffectChangMyRoundOver = new SkillChang.EffectChang();
            MyEffectChangEnerCharge = new SkillChang.EffectChang();
            MyEffectChangLrigSet = new SkillChang.EffectChang();
            MyEffectChangHandChange = new SkillChang.EffectChang();
            MyAttackChange = new SkillChang.EffectChang();
            MyAtking = new SkillChang.EffectChang();
            SigniOutAction = null;
        }
    }

    public class MyCard : MonoBehaviour
    {
        //public List<Texture2D> CardTextures = new List<Texture2D>();
        public List<string> MyCardid;
        public List<string> MyLrigid;

        public struct AtkDetail
        {
            public int Atk;
            public string CardDetail;
            public Card.CardType CardType;
            public Card.CardColor CardColor;
            public string Type;
            public int Level;
        }

        public Dictionary<string, AtkDetail> CardAtkDetailDictionary = new Dictionary<string, AtkDetail>();

        public ShowDeck ShowDeck;

        //        public bool BGetOtherCard;
        //        public List<string> OtherCardid = new List<string>();

        public void Setup()
        {
            CreateCardByXml();
            Card mycard;
            for (int i = 0; i < MyLrigid.Count; i++)
            {
                mycard = new Card(MyLrigid[i]);
                mycard.SetCardById();
                if (!CardAtkDetailDictionary.ContainsKey(MyLrigid[i]))
                {
                    var detal = new AtkDetail
                    {
                        Atk = mycard.Atk,
                        CardDetail = mycard.CardDetail,
                        CardType = mycard.MyCardType,
                        CardColor = mycard.MyCardColor,
                        Type = mycard.Type,
                        Level = mycard.Level,
                    };
                    CardAtkDetailDictionary.Add(MyLrigid[i], detal);
                    GameManager.RpcOtherMyCardid(MyLrigid[i]);
                }
                ShowDeck.LrigDeck.Add(mycard);
                ShowDeck.SkillManager.GetEffectByCard(mycard);
            }

            for (int i = 0; i < MyCardid.Count; i++)
            {
                mycard = new Card(MyCardid[i]);
                mycard.SetCardById();
                if (!CardAtkDetailDictionary.ContainsKey(MyCardid[i]))
                {
                    var detal = new AtkDetail
                    {
                        Atk = mycard.Atk,
                        CardDetail = mycard.CardDetail,
                        CardType = mycard.MyCardType,
                        CardColor = mycard.MyCardColor,
                        Type = mycard.Type,
                        Level = mycard.Level,
                    };
                    CardAtkDetailDictionary.Add(MyCardid[i], detal);
                    GameManager.RpcOtherMyCardid(MyCardid[i]);
                }
                ShowDeck.MainDeck.Add(mycard);
                ShowDeck.SkillManager.GetEffectByCard(mycard);
            }

            GameManager.RpcOtherMyCardidOk(true);

            ShowDeck.WashMainDeck();
            StartCoroutine(WaitToOtherSendCard());
        }


        private IEnumerator WaitToOtherSendCard()
        {
            while (true)
            {
                if (MyRpc.Bdone)
                {
                    if (MyRpc.OtherCards.Count > 0)
                    {
                        for (int i = 0; i < MyRpc.OtherCards.Count; i++)
                        {
                            if (!CardAtkDetailDictionary.ContainsKey(MyRpc.OtherCards[i]))
                            {
                                var card = new Card(MyRpc.OtherCards[i]);
                                card.SetCardById();
                                var detal = new AtkDetail
                                {
                                    Atk = card.Atk,
                                    CardDetail = card.CardDetail,
                                    CardType = card.MyCardType,
                                    CardColor = card.MyCardColor,
                                    Type = card.Type,
                                    Level = card.Level,
                                };
                                CardAtkDetailDictionary.Add(MyRpc.OtherCards[i], detal);
                            }
                        }
                    }
                    MyRpc.Bdone = false;
                    yield break;
                }
                yield return new WaitForSeconds(0.5f);
            }
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
                "WD03-001",
                "WD03-002",
                "WD03-003",
                "WD03-004",
                "WD03-005",
                "WD03-006",
                "WD03-007",
                "WD03-008",
            };

            MyCardid = new List<string>()
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
//
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
//                "WX01-051",
//                "WX01-051",
//                "WX01-051",
//                "WX01-051",
//                                
//                "WX01-100",
//                "WX01-100",
//                "WX01-100",
//                "WX01-100",

                //------------
                
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

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SkillChang : MonoBehaviour
    {
        public SkillManager SkillManager;

        public struct EffectChang
        {
            public Card Card;
            public Action<Card> CardChangAction;
        }

        /// <summary>
        /// 精灵出场时调用(双方)
        /// </summary>
        public List<EffectChang> SigniSetActions = new List<EffectChang>();

        /// <summary>
        /// 精灵离场时调用(双方)
        /// </summary>
        public List<EffectChang> SigniOutActions = new List<EffectChang>();

        /// <summary>
        /// 我放回合开始时
        /// </summary>
        public List<EffectChang> MyRoundStartActions = new List<EffectChang>();

        /// <summary>
        /// 敌方回合开始时
        /// </summary>
        public List<EffectChang> MyRoundOverActions = new List<EffectChang>();

        /// <summary>
        /// 能量变化时调用(双方)
        /// </summary>
        public List<EffectChang> EnerChangeActions = new List<EffectChang>();

        /// <summary>
        /// 我发设置分身时
        /// </summary>
        public List<EffectChang> LrigSetActions = new List<EffectChang>();

        /// <summary>
        /// 手牌变化时调用(双方)
        /// </summary>
        public List<EffectChang> HandsChangeActions = new List<EffectChang>();

        public void SigniSet(Card card)
        {
            for (int i = SigniSetActions.Count - 1; i >= 0; i--)
            {
                if (SigniSetActions[i].CardChangAction != null)
                {
                    SigniSetActions[i].CardChangAction(card);
                }
            }
        }

        public void SigniOut()
        {
            for (int i = SigniOutActions.Count - 1; i >= 0; i--)
            {
                if (SigniOutActions[i].CardChangAction != null)
                {
                    SigniOutActions[i].CardChangAction(SigniOutActions[i].Card);
                }
            }
        }

        public void MyRoundStart()
        {
            for (int i = MyRoundStartActions.Count - 1; i >= 0; i--)
            {
                if (MyRoundStartActions[i].CardChangAction != null)
                {
                    MyRoundStartActions[i].CardChangAction(MyRoundStartActions[i].Card);
                }
            }
        }

        public void MyRoundOver()
        {
            for (int i = MyRoundOverActions.Count - 1; i >= 0; i--)
            {
                if (MyRoundOverActions[i].CardChangAction != null)
                {
                    MyRoundOverActions[i].CardChangAction(MyRoundOverActions[i].Card);
                }
            }
        }


        public void EnerCharge()
        {
            for (int i = EnerChangeActions.Count - 1; i >= 0; i--)
            {
                if (EnerChangeActions[i].CardChangAction != null)
                {
                    EnerChangeActions[i].CardChangAction(EnerChangeActions[i].Card);
                }
            }
        }

        public void LrigSet()
        {
            for (int i = LrigSetActions.Count - 1; i >= 0; i--)
            {
                if (LrigSetActions[i].CardChangAction != null)
                {
                    LrigSetActions[i].CardChangAction(LrigSetActions[i].Card);
                }
            }
        }

        public void HandChange()
        {
            for (int i = HandsChangeActions.Count - 1; i >= 0; i--)
            {
                if (HandsChangeActions[i].CardChangAction != null)
                {
                    HandsChangeActions[i].CardChangAction(HandsChangeActions[i].Card);
                }
            }
        }

        /// <summary>
        /// 常效果字典
        /// </summary>
        public Dictionary<String, Action<Card>> CardEffectChangDictionary;


        public void Setup()
        {
            CardEffectChangDictionary = new Dictionary<string, Action<Card>>
            {
                {"WD01-001",满月之巫女玉依姬},
                {"WD01-009",甲胄皇家铠},
                {"WD02-001",花代肆},
                {"WD03-001",代号皮璐璐可t},
                {"WD04-001",四之娘绿姬},
            };
        }

        private void 满月之巫女玉依姬(Card card)
        {
            var chang = new EffectChang
            {
                Card = card,
                CardChangAction = card1 =>
                {
                    //如果条件发动后，每有一只怪出场，那只怪＋2000
                    if (card.BChang)
                    {
                        SkillManager.AddAtk(card1, 2000);
                    }

                    //场上没有 皇家铠 且效果没发动
                    if (SkillManager.BSigniInGround("甲胄 皇家铠") && !card.BChang)
                    {
                        SkillManager.AddAtkAll(2000);
                        card.BChang = true;
                    }
                }
            };

            LrigSetActions.Add(chang);
            card.MyEffectChangLrigSet = chang;
            SigniSetActions.Add(chang);
            card.MyEffectChangSigniSet = chang;

            var no = new EffectChang
            {
                Card = card,
                CardChangAction = card1 =>
                {
                    //场上没有 皇家铠 且效果发动了
                    if (!SkillManager.BSigniInGround("甲胄 皇家铠") && card.BChang)
                    {
                        SkillManager.AddAtkAll(-2000);
                        card.BChang = false;
                    }
                }
            };

            SigniOutActions.Add(no);
            card.MyEffectChangSigniOut = no;
        }

        /// <summary>
        /// 只要场上有**,就加攻击力
        /// </summary>
        /// <param name="card">发动效果的卡</param>
        /// <param name="cardname">**</param>
        /// <param name="value"></param>
        private void IfSigniInGround(Card card, string cardname, int value)
        {
            var chang = new EffectChang
            {
                Card = card,
                CardChangAction = card1 =>
                {
                    //如果条件发动后，每有一只怪出场，那只怪＋2000
                    if (card.BChang)
                    {
                        SkillManager.AddAtk(card1, value);
                    }

                    //场上没有 皇家铠 且效果没发动
                    if (SkillManager.BSigniInGround(cardname) && !card.BChang)
                    {
                        SkillManager.AddAtkAll(value);
                        card.BChang = true;
                    }
                }
            };

            LrigSetActions.Add(chang);
            card.MyEffectChangLrigSet = chang;
            SigniSetActions.Add(chang);
            card.MyEffectChangSigniSet = chang;

            var no = new EffectChang
            {
                Card = card,
                CardChangAction = card1 =>
                {
                    //场上没有 皇家铠 且效果发动了
                    if (!SkillManager.BSigniInGround(cardname) && card.BChang)
                    {
                        SkillManager.AddAtkAll(-value);
                        card.BChang = false;
                    }
                }
            };

            SigniOutActions.Add(no);
            card.MyEffectChangSigniOut = no;
        }

        private void 甲胄皇家铠(Card card)
        {
            var chang = new EffectChang
            {
                Card = card,
                CardChangAction = card1 =>
                {
                    if (!card.BChang)
                    {
                        SkillManager.AddAtkAll(1000);
                        card.BChang = true;
                    }
                }
            };

            var chang2 = new EffectChang
            {
                Card = card,
                CardChangAction = card1 =>
                {
                    if (card.BChang)
                    {
                        SkillManager.AddAtkAll(-1000);
                        card.BChang = false;
                    }
                }
            };

            MyRoundStartActions.Add(chang2);
            card.MyEffectChangMyRoundStart = chang2;

            MyRoundOverActions.Add(chang);
            card.MyEffectChangMyRoundOver = chang;

            var chang3 = new EffectChang
            {
                Card = card,
                CardChangAction = card1 =>
                {
                    if (card.BChang)
                    {
                        SkillManager.AddAtkAll(-1000);
                        card.BChang = false;
                    }
                }
            };

            SigniOutActions.Add(chang3);
            card.MyEffectChangSigniOut = chang3;
        }

        private void 花代肆(Card card)
        {
            var chang = new EffectChang
            {
                Card = card,
                CardChangAction = card1 =>
                {
                    //如果条件发动后，每有一只怪出场，那只怪＋2000
                    if (card.BChang)
                    {
                        SkillManager.AddAtk(card1, 2000);
                    }

                    //场上没有 皇家铠 且效果没发动
                    if (SkillManager.BSigniInGround("罗石 火山石") && !card.BChang)
                    {
                        SkillManager.AddAtkAll(2000);
                        card.BChang = true;
                    }
                }
            };

            var chang2 = new EffectChang
            {
                Card = card,
                CardChangAction = card1 =>
                {
                    if (card.BChang)
                    {
                        SkillManager.AddAtkAll(-2000);
                        card.BChang = false;
                    }
                }
            };

            LrigSetActions.Add(chang);
            card.MyEffectChangLrigSet = chang;
            SigniSetActions.Add(chang);
            card.MyEffectChangSigniSet = chang;

            MyRoundStartActions.Add(chang);
            card.MyEffectChangMyRoundStart = chang2;

            MyRoundOverActions.Add(chang2);
            card.MyEffectChangMyRoundOver = chang;

            var no = new EffectChang
            {
                Card = card,
                CardChangAction = card1 =>
                {
                    //场上没有 皇家铠 且效果发动了
                    if (!SkillManager.BSigniInGround("罗石 火山石") && card.BChang)
                    {
                        SkillManager.AddAtkAll(-2000);
                        card.BChang = false;
                    }
                }
            };

            SigniOutActions.Add(no);
            card.MyEffectChangSigniOut = no;
        }

        private void 代号皮璐璐可t(Card card)
        {
            var chang = new EffectChang
            {
                Card = card,
                CardChangAction = card1 =>
                {
                    if (card.BChang)
                    {
                        SkillManager.AddAtk(card1, 2000);
                    }

                    if (SkillManager.BSigniInGround("技艺代号 R•M•N") && !card.BChang && SkillManager.GameManager.CreateHands.OtherHands.Count <= 1)
                    {
                        SkillManager.AddAtkAll(2000);
                        card.BChang = true;
                    }

                    if (card.BChang && SkillManager.GameManager.CreateHands.OtherHands.Count > 1)
                    {
                        SkillManager.AddAtkAll(-2000);
                        card.BChang = false;
                    }
                }
            };

            LrigSetActions.Add(chang);
            card.MyEffectChangLrigSet = chang;
            SigniSetActions.Add(chang);
            card.MyEffectChangSigniSet = chang;
            HandsChangeActions.Add(chang);
            card.MyEffectChangHandChange = chang;

            var no = new EffectChang
            {
                Card = card,
                CardChangAction = card1 =>
                {
                    if (!SkillManager.BSigniInGround("技艺代号 R•M•N") && card.BChang)
                    {
                        SkillManager.AddAtkAll(-2000);
                        card.BChang = false;
                    }
                }
            };

            SigniOutActions.Add(no);
            card.MyEffectChangSigniOut = no;
        }

        private void 四之娘绿姬(Card card)
        {
            var chang = new EffectChang
            {
                Card = card,
                CardChangAction = card1 =>
                {
                    if (card.BChang)
                    {
                        SkillManager.AddAtk(card1, 2000);
                    }

                    if (SkillManager.BSigniInGround("幻兽 青龙") && !card.BChang && SkillManager.GameManager.EnerManager.EnerCards.Count <= 7)
                    {
                        SkillManager.AddAtkAll(2000);
                        card.BChang = true;
                    }

                    if (card.BChang && SkillManager.GameManager.EnerManager.EnerCards.Count > 7)
                    {
                        SkillManager.AddAtkAll(-2000);
                        card.BChang = false;
                    }
                }
            };

            LrigSetActions.Add(chang);
            card.MyEffectChangLrigSet = chang;
            SigniSetActions.Add(chang);
            card.MyEffectChangSigniSet = chang;
            EnerChangeActions.Add(chang);
            card.MyEffectChangEnerCharge = chang;

            var no = new EffectChang
            {
                Card = card,
                CardChangAction = card1 =>
                {
                    if (!SkillManager.BSigniInGround("幻兽 青龙") && card.BChang)
                    {
                        SkillManager.AddAtkAll(-2000);
                        card.BChang = false;
                    }
                }
            };

            SigniOutActions.Add(no);
            card.MyEffectChangSigniOut = no;
        }

    }
}

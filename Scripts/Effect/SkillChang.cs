﻿using System;
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
        /// 精灵出场时调用
        /// </summary>
        public List<EffectChang> SigniSetActions = new List<EffectChang>();

        /// <summary>
        /// 精灵离场时调用
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
        /// 我方充能时调用
        /// </summary>
        public List<EffectChang> EnerChargeActions = new List<EffectChang>();

        /// <summary>
        /// 我发设置分身时
        /// </summary>
        public List<EffectChang> LrigSetActions = new List<EffectChang>();

        public void SigniSet(Card card)
        {
            for (int i = SigniSetActions.Count-1; i >=0 ; i--)
            {
                if (SigniSetActions[i].CardChangAction != null)
                {
                    SigniSetActions[i].CardChangAction(card);
                }
            }
        }

        public void SigniOut()
        {
            for (int i = SigniOutActions.Count-1; i >=0 ; i--)
            {
                if (SigniOutActions[i].CardChangAction != null)
                {
                    SigniOutActions[i].CardChangAction(SigniOutActions[i].Card);
                }
            }
        }

        public void MyRoundStart()
        {
            for (int i = MyRoundStartActions.Count-1; i >=0 ; i--)
            {
                if (MyRoundStartActions[i].CardChangAction != null)
                {
                    MyRoundStartActions[i].CardChangAction(MyRoundStartActions[i].Card);
                }
            }
        }

        public void MyRoundOver()
        {
            for (int i = MyRoundOverActions.Count-1; i >=0 ; i--)
            {
                if (MyRoundOverActions[i].CardChangAction != null)
                {
                    MyRoundOverActions[i].CardChangAction(MyRoundOverActions[i].Card);
                }
            }
        }


        public void EnerCharge()
        {
            for (int i = EnerChargeActions.Count-1; i >=0; i--)
            {
                if (EnerChargeActions[i].CardChangAction != null)
                {
                    EnerChargeActions[i].CardChangAction(EnerChargeActions[i].Card);
                }
            }
        }

        public void LrigSet()
        {
            for (int i = LrigSetActions.Count-1; i >=0 ; i--)
            {
                if (LrigSetActions[i].CardChangAction != null)
                {
                    LrigSetActions[i].CardChangAction(LrigSetActions[i].Card);
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
                {"WD01-001",CardWd01001},
                {"WD01-009",CardWd01009},
                {"WD02-001",CardWd02001},
            };
        }

        private void CardWd01001(Card card)
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
            //IfSigniInGround(card, "甲胄 皇家铠", 2000);
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

        private void CardWd01009(Card card)
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
        }

        private void CardWd02001(Card card)
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
        }

    }
}

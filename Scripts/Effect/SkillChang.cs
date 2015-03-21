using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SkillChang : MonoBehaviour
    {
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
            for (int i = 0; i < SigniSetActions.Count; i++)
            {
                if (SigniSetActions[i].CardChangAction != null)
                {
                    SigniSetActions[i].CardChangAction(card);
                }
            }
        }

        public void SigniOut()
        {
            for (int i = 0; i < SigniOutActions.Count; i++)
            {
                if (SigniOutActions[i].CardChangAction != null)
                {
                    SigniOutActions[i].CardChangAction(SigniOutActions[i].Card);
                }
            }
        }

        public void MyRoundStart()
        {
            for (int i = 0; i < MyRoundStartActions.Count; i++)
            {
                if (MyRoundStartActions[i].CardChangAction != null)
                {
                    MyRoundStartActions[i].CardChangAction(MyRoundStartActions[i].Card);
                }
            }
        }

        public void MyRoundOver()
        {
            for (int i = 0; i < MyRoundOverActions.Count; i++)
            {
                if (MyRoundOverActions[i].CardChangAction != null)
                {
                    MyRoundOverActions[i].CardChangAction(MyRoundOverActions[i].Card);
                }
            }
        }


        public void EnerCharge()
        {
            for (int i = 0; i < EnerChargeActions.Count; i++)
            {
                if (EnerChargeActions[i].CardChangAction != null)
                {
                    EnerChargeActions[i].CardChangAction(EnerChargeActions[i].Card);
                }
            }
        }

        public void LrigSet()
        {
            for (int i = 0; i < LrigSetActions.Count; i++)
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
                {"WD01-001",CardWx01001},
                {"WD01-009",CardWd01009},
            };
        }

        private void CardWx01001(Card card)
        {
            var chang = new EffectChang
            {
                Card = card,
                CardChangAction = card1 =>
                {
                    //如果条件发动后，每有一只怪出场，那只怪＋2000
                    if(card.BChang)
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

    }
}

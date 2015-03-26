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

        /// <summary>
        /// 攻击力改变时调用
        /// </summary>
        public List<EffectChang> AttackChangeActions = new List<EffectChang>();

        /// <summary>
        /// 精灵攻击时
        /// </summary>
        public List<EffectChang> SigniAtkAcitons = new List<EffectChang>();

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


        public void EnerChange()
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

        public void AttackChange()
        {
            for (int i = AttackChangeActions.Count - 1; i >= 0; i--)
            {
                if (AttackChangeActions[i].CardChangAction != null)
                {
                    AttackChangeActions[i].CardChangAction(AttackChangeActions[i].Card);
                }
            }
        }

        public void SigniAttack(Card card)
        {
            for (int i = SigniAtkAcitons.Count - 1; i >= 0; i--)
            {
                if (SigniAtkAcitons[i].CardChangAction != null)
                {
                    SigniAtkAcitons[i].CardChangAction(card);
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
                {"WD04-009",幻兽青龙},
                {"WD04-010",幻兽朱雀小姐},
                {"WD04-013",幻兽小玄武},
                {"WD04-015",幻兽白虎},
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

        private void 甲胄皇家铠(Card card)
        {
            //            var chang = new EffectChang
            //            {
            //                Card = card,
            //                CardChangAction = card1 =>
            //                {
            //                    if (!card.BChang)
            //                    {
            //                        SkillManager.AddAtkAll(1000);
            //                        card.BChang = true;
            //                    }
            //                }
            //            };
            //
            //            var chang2 = new EffectChang
            //            {
            //                Card = card,
            //                CardChangAction = card1 =>
            //                {
            //                    if (card.BChang)
            //                    {
            //                        SkillManager.AddAtkAll(-1000);
            //                        card.BChang = false;
            //                    }
            //                }
            //            };
            var chang = WhenCondictionAtkUp(card, 1000, () => {return !GameManager.BLocalRound;}, SkillManager.AddAtkAll);

            MyRoundStartActions.Add(chang);
            card.MyEffectChangMyRoundStart = chang;

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
            //            var chang = new EffectChang
            //            {
            //                Card = card,
            //                CardChangAction = card1 =>
            //                {
            //                    //如果条件发动后，每有一只怪出场，那只怪＋2000
            //                    if (card.BChang)
            //                    {
            //                        SkillManager.AddAtk(card1, 2000);
            //                    }
            //
            //                    //场上没有 皇家铠 且效果没发动
            //                    if (SkillManager.BSigniInGround("罗石 火山石") && !card.BChang)
            //                    {
            //                        SkillManager.AddAtkAll(2000);
            //                        card.BChang = true;
            //                    }
            //                }
            //            };
            //
            //            var chang2 = new EffectChang
            //            {
            //                Card = card,
            //                CardChangAction = card1 =>
            //                {
            //                    if (card.BChang)
            //                    {
            //                        SkillManager.AddAtkAll(-2000);
            //                        card.BChang = false;
            //                    }
            //                }
            //            };

            var chang = WhenSigniInGround(card, 2000, "罗石 火山石", () => GameManager.BLocalRound);

            LrigSetActions.Add(chang);
            card.MyEffectChangLrigSet = chang;
            SigniSetActions.Add(chang);
            card.MyEffectChangSigniSet = chang;

            MyRoundStartActions.Add(chang);
            card.MyEffectChangMyRoundStart = chang;

            MyRoundOverActions.Add(chang);
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
            //            var chang = new EffectChang
            //            {
            //                Card = card,
            //                CardChangAction = card1 =>
            //                {
            //                    if (card.BChang)
            //                    {
            //                        SkillManager.AddAtk(card1, 2000);
            //                    }
            //
            //                    if (SkillManager.BSigniInGround("技艺代号 R•M•N") && !card.BChang && SkillManager.GameManager.CreateHands.OtherHands.Count <= 1)
            //                    {
            //                        SkillManager.AddAtkAll(2000);
            //                        card.BChang = true;
            //                    }
            //
            //                    if (card.BChang && SkillManager.GameManager.CreateHands.OtherHands.Count > 1)
            //                    {
            //                        SkillManager.AddAtkAll(-2000);
            //                        card.BChang = false;
            //                    }
            //                }
            //            };

            var chang = WhenSigniInGround(card, 2000, "技艺代号 R•M•N", () => SkillManager.GameManager.CreateHands.OtherHands.Count <= 1);

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
            var chang = WhenSigniInGround(card, 2000, "幻兽 青龙", () => SkillManager.GameManager.EnerManager.EnerCards.Count >= 7);

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

        private void 幻兽青龙(Card card)
        {
            var chang = new EffectChang
            {
                Card = card,
                CardChangAction = card1 =>
                {
                    if (card1 != card)
                        return;

                    bool bok = true;
                    var target = SkillManager.GameManager.SetSigni.Signi;

                    if (target.Length < 3)
                    {
                        return;
                    }

                    for (int j = target.Length - 1; j >= 0; j--)
                    {
                        if (target[j] != null && target[j].Atk < 15000)
                        {
                            bok = false;
                        }
                    }

                    if (bok && !card.BChang)
                    {
                        card.BChang = true;
                        SkillManager.AddBuff(card, i =>
                        {
                            SkillManager.GameManager.SetSigni.Signi[i].Blancer = true;
                            GameManager.RpcMyBuff(1, i, true);
                        });
                    }

                    if (!bok && card.BChang)
                    {
                        card.BChang = false;
                        SkillManager.AddBuff(card, i =>
                        {
                            SkillManager.GameManager.SetSigni.Signi[i].Blancer = false;
                            GameManager.RpcMyBuff(1, i, false);
                        });
                    }
                },
            };

            var attack = new EffectChang
            {
                Card = card,
                CardChangAction = card1 =>
                {
                    if (card1 != card)
                        return;
                    if (card.BChang)
                    {
                        SkillManager.Baninish(card);
                    }
                }
            };

            AttackChangeActions.Add(chang);
            card.MyAttackChange = chang;

            SigniAtkAcitons.Add(attack);
            card.MyAtking = attack;
        }

        private void 幻兽朱雀小姐(Card card)
        {
            var chang = new EffectChang
            {
                Card = card,
                CardChangAction = card1 =>
                {
                    if (card1 != card)
                        return;

                    if (card.Atk >= 10000 && !card.BChang)
                    {
                        card.BChang = true;
                        SkillManager.AddBuff(card, i =>
                        {
                            SkillManager.GameManager.SetSigni.Signi[i].Blancer = true;
                            GameManager.RpcMyBuff(1, i, true);
                        });
                    }
                    else
                    {
                        if (card.BChang && card.Atk < 10000)
                        {
                            card.BChang = false;
                            SkillManager.AddBuff(card, i =>
                            {
                                SkillManager.GameManager.SetSigni.Signi[i].Blancer = false;
                                GameManager.RpcMyBuff(1, i, false);
                            });
                        }
                    }
                },
            };

            AttackChangeActions.Add(chang);
            card.MyAttackChange = chang;
        }

        private void 幻兽小玄武(Card card)
        {
            var chang = new EffectChang
            {
                Card = card,
                CardChangAction = card1 =>
                {
                    if (card.Atk >= 5000 && !card.BChang)
                    {
                        card.BChang = true;
                    }
                    
                    if (card.Atk < 5000 && card.BChang)
                    {
                        card.BChang = false;
                    }
                }
            };

            var attack = new EffectChang
            {
                Card = card,
                CardChangAction = card1 =>
                {
                    if (card1 != card)
                        return;
                    if (card.BChang)
                    {
                        SkillManager.EnerCharge();
                    }
                }
            };

            AttackChangeActions.Add(chang);
            card.MyAttackChange = chang;

            SigniAtkAcitons.Add(attack);
            card.MyAtking = attack;
        }

        private void 幻兽白虎(Card card)
        {
            var chang = new EffectChang
            {
                Card = card,
                CardChangAction = card1 =>
                {
                    if (card.Atk >= 3000 && !card.BChang)
                    {
                        card.BChang = true;
                    }
                    if (card.Atk < 3000 && card.BChang)
                    {
                        card.BChang = false;
                    }
                }
            };

            var attack = new EffectChang
            {
                Card = card,
                CardChangAction = card1 =>
                {
                    if (card1 != card)
                        return;
                    if (card.BChang)
                    {
                        SkillManager.EnerCharge();
                    }
                },
            };

            AttackChangeActions.Add(chang);
            card.MyAttackChange = chang;

            SigniAtkAcitons.Add(attack);
            card.MyAtking = attack;
        }



        #region 抽离出来的函数
        /// <summary>
        /// 有＊＊在场时全体加攻击力
        /// </summary>
        /// <returns>The signi in ground.</returns>
        /// <param name="target">发动这效果的卡</param>
        /// <param name="signiname">增加的攻击力</param>
        /// <param name="atkvalue">＊＊（精灵名字）</param>
        /// <param name="condiction">条件</param>
        private EffectChang WhenSigniInGround(Card target, int atkvalue, string signiname, Func<bool> condiction)
        {
            var chang = new EffectChang
            {
                Card = target,
                CardChangAction = card1 =>
                {
                    if (target.BChang)
                    {
                        SkillManager.AddAtk(card1, atkvalue);
                    }

                    if (SkillManager.BSigniInGround(signiname) && !target.BChang && condiction())
                    {
                        SkillManager.AddAtkAll(atkvalue);
                        target.BChang = true;
                    }
                    if (target.BChang && !condiction())
                    {
                        SkillManager.AddAtkAll(-atkvalue);
                        target.BChang = false;
                    }
                }
            };
            return chang;
        }

        /// <summary>
        /// 当达成某些条件时攻击力增加
        /// </summary>
        /// <returns>The condiction.</returns>
        /// <param name="target">发动这效果的卡</param>
        /// <param name="atkvalue">增加的攻击力</param>
        /// <param name="condiction">条件</param>
        /// <param name="atkadd">群体加还是单体加</param>
        private EffectChang WhenCondictionAtkUp(Card target, int atkvalue, Func<bool> condiction, Action<int> atkadd)
        {
            var chang = new EffectChang
            {
                Card = target,
                CardChangAction = card1 =>
                {
                    if (!target.BChang && condiction())
                    {
                        atkadd(atkvalue);
                        target.BChang = true;
                    }
                    if (target.BChang && !condiction())
                    {
                        atkadd(-atkvalue);
                        target.BChang = false;
                    }
                }
            };
            return chang;
        }

        #endregion
    }
}

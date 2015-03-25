using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SkillChu : MonoBehaviour
    {
        public GameManager GameManager;
        public SkillManager SkillManager;
        /// <summary>
        /// 出效果字典
        /// </summary>
        public Dictionary<String, Action<Card>> CardEffectChuDictionary;

        public void Setup()
        {
            CardEffectChuDictionary = new Dictionary<string, Action<Card>>()
            {
                {"WD01-011",笼手铁拳},
                {"WD01-014",小弓箭矢},
                {"WD02-009",罗石火山石},
                {"WD02-011",罗石石榴石},
                {"WD02-014",罗石紫水晶},
                {"WD03-009",技艺代号rmn},
                {"WD03-011",技艺代号smp},
                {"WD03-014",技艺代号rfr},
                {"WD04-011",幻兽雪怪},
                {"WD04-012",幻兽树袋熊},
                {"WD04-014",幻兽大熊猫},
            };
        }

        private void 笼手铁拳(Card card)
        {
            var target = SkillManager.FindCardByCondition(x => x.CardId == "WD01-009");

            var cardinfo = GameManager.CardInfo;

            cardinfo.ShowCardInfo(true);
            cardinfo.SetUp("探寻1张《甲胄 皇家铠", target, 1, () =>
            {
                if (cardinfo.SelectHands.Count > 0 && cardinfo.SelectHands[0] != null)
                {
                    var mycard = cardinfo.SelectHands[0].MyCard;
                    GameManager.CreateHands.CreateHandFromDeck(mycard);
                    GameManager.RpcOtherShowCards(new List<Card> { mycard }, "对方获得");
                }
                cardinfo.ShowCardInfo(false);
                SkillManager.WashDeck();
            });
        }

        private void 小弓箭矢(Card card)
        {
            SkillManager.CheckDeckNumAndSort(3, false);
        }

        private void 罗石火山石(Card card)
        {
            SkillManager.Baninish(card, null, i => i.Atk <= 15000);
        }

        private void 罗石石榴石(Card card)
        {
            SkillManager.AddAtk(card, 8000);

            var over = new SkillChang.EffectChang
            {
                Card = card,
                CardChangAction = card1 =>
                {
                    SkillManager.AddAtk(card1, -8000);
                    SkillManager.SkillChang.MyRoundOverActions.Remove(card1.MyEffectChangMyRoundOver);
                }
            };

            card.MyEffectChangMyRoundOver = over;
            SkillManager.SkillChang.MyRoundOverActions.Add(over);
        }

        private void 罗石紫水晶(Card card)
        {
            SkillManager.Baninish(card, null, i => i.Atk <= 1000);
        }

        private void 技艺代号rmn(Card card)
        {
            SkillManager.DesOtherCardRamdom();
        }

        private void 技艺代号smp(Card card)
        {
            GameManager.RpcShowHandAndDesLevelOne();
        }

        private void 技艺代号rfr(Card card)
        {
            SkillManager.DropCard(1,()=>SkillManager.DesCard(1));
        }

        private void 幻兽雪怪(Card card)
        {
            SkillManager.AddAtkAll(2000);

            var over = new SkillChang.EffectChang
            {
                Card = card,
                CardChangAction = card1 =>
                {
                    SkillManager.AddAtkAll(-2000);
                    SkillManager.SkillChang.MyRoundOverActions.Remove(card1.MyEffectChangMyRoundOver);
                }
            };

            card.MyEffectChangMyRoundOver = over;
            SkillManager.SkillChang.MyRoundOverActions.Add(over);
        }

        private void 幻兽树袋熊(Card card)
        {
            SkillManager.AttackUpSigni(card, 3000);
        }

        private void 幻兽大熊猫(Card card)
        {
            SkillManager.AttackUpSigni(card, 2000);
        }
    }
}

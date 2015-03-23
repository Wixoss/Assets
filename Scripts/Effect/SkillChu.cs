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
                {"WD01-011",CardWd01011},
                {"WD01-014",CardWd01014},
                {"WD02-009",CardWd02009},
                {"WD02-011",CardWd02011},
                {"WD02-014",CardWd02014},
            };
        }

        private void CardWd01011(Card card)
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
                    GameManager.RpcOtherShowCards(new List<Card> { mycard });
                }
                cardinfo.ShowCardInfo(false);
                SkillManager.WashDeck();
            });
        }

        private void CardWd01014(Card card)
        {
            SkillManager.CheckDeckNumAndSort(3, false);
        }

        private void CardWd02009(Card card)
        {
            SkillManager.Baninish(card, null, i => i.Atk <= 15000);
        }

        private void CardWd02011(Card card)
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

        private void CardWd02014(Card card)
        {
            SkillManager.Baninish(card, null, i => i.Atk <= 1000);
        }
    }
}

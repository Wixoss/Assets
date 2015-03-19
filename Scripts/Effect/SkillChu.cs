using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SkillChu : MonoBehaviour
    {
        public GameManager GameManager;
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
            };
        }

        private void CardWd01011(Card card)
        {
            var target = SkillManager.FindCardByCondition(x => x.CardId=="WD01-009");

            var cardinfo = GameManager.CardInfo;

            cardinfo.SetUp("探寻1张《甲胄 皇家铠", target, 1, ()=>
            {
                if(cardinfo.SelectHands[0]!=null)
                {
                    var mycard = cardinfo.SelectHands [0].MyCard;
                    GameManager.CreateHands.CreateHandFromDeck(mycard);
                    GameManager.RpcOtherShowCards(new List<Card>{mycard});
                    cardinfo.ShowCardInfo(false);
                }
            });
            cardinfo.ShowCardInfo(true);
        }

        private void CardWd01014(Card card)
        {
            SkillManager.CheckDeckNumAndSort(3, false);
        }
    }
}

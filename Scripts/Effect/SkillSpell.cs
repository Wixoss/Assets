using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SkillSpell : MonoBehaviour
    {
        public Dictionary<String, Action<Card>> CardEffectSpellDictionary;
        public GameManager GameManager;

        public void Setup()
        {
            CardEffectSpellDictionary = new Dictionary<string, Action<Card>>()
            {
                {"WD01-006",CardWd01006},
                {"WD01-007",CardWd01007},
                {"WD01-008",CardWd01008},
                {"WD01-015",CardWd01015},
                {"WX01-103",CardWx01103},
            };
        }


        private void CardWd01006(Card card)
        {
            SkillManager.BackHand(card,() => SkillManager.BackHand(card));
        }


        private void CardWd01007(Card card)
        {
            var targets = SkillManager.FindCardByCondition(x => x.MyCardColor == Card.CardColor.白);
            var cardinfo = GameManager.CardInfo;

            cardinfo.ShowCardInfo(true);
            cardinfo.SetUp("探寻至多2只白色SIGNI", targets, 2, () =>
            {
                var mycards = new List<Card>();
                for (int i = 0; i < cardinfo.SelectHands.Count; i++)
                {
                    var mycard = cardinfo.SelectHands[i].MyCard;
                    GameManager.CreateHands.CreateHandFromDeck(mycard);
                    mycards.Add(mycard);
                }
                GameManager.RpcOtherShowCards(mycards);
                cardinfo.ShowCardInfo(false);
                SkillManager.WashDeck();
            });
        }

        private void CardWd01008(Card card)
        {
            GameManager.SetSigni.ShowOtherSelections(true, true);
            GameManager.SetSigni.SetSelections(false, null, true, i =>
            {
                GameManager.SetSigni.OtherSigni[i].BCantAttack = true;
                GameManager.RpcOtherDebuff(2, i, true);
            },true);

           // GameManager.Lrig.SetOtherLrigSelection(() => GameManager.RpcOtherDebuff(2, 3, true));
        }

        private void CardWd01015(Card card)
        {
            var targets = SkillManager.FindCardByCondition(x => x.MyCardType ==  Card.CardType.精灵卡);
            var cardinfo = GameManager.CardInfo;
            cardinfo.ShowCardInfo(true);
            cardinfo.SetUp("探寻至多1只SIGNI", targets, 1, () =>
            {
                if(cardinfo.SelectHands.Count>0 && cardinfo.SelectHands[0]!=null)
                {
                    var mycard = cardinfo.SelectHands[0].MyCard;
                    GameManager.CreateHands.CreateHandFromDeck(mycard);
                    GameManager.RpcOtherShowCards(new List<Card>{mycard});
                } 
                cardinfo.ShowCardInfo(false);
                SkillManager.WashDeck();
            });

        }

        private void CardWx01103(Card card)
        {
            GameManager.SkillManager.DropCard(1);
        }


//        private void DisSelectionBtn()
//        {
//            SkillManager.DisSelect();
//        }

    }
}

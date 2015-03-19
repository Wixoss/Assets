using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SkillSpell : MonoBehaviour
    {
        public Dictionary<String, Action<Card>> CardEffectSpellDictionary;
        public GameManager GameManager;

        private void Awake()
        {
            CardEffectSpellDictionary = new Dictionary<string, Action<Card>>()
            {
                {"WD01-006",CardWd01006},
                {"WD01-007",CardWd01007},
                {"WD01-008",CardWd01008},
            };
        }


        private void CardWd01006(Card card)
        {
            SkillManager.BackHand(() =>
            {
                CancelInvoke("DisSelectionBtn");
                Invoke("DisSelectionBtn", 10f);
                GameManager.ShowCard.ShowMyCardEffect(card);
                SkillManager.BackHand(() =>
                {
                    CancelInvoke("DisSelectionBtn");
                    Invoke("DisSelectionBtn", 10f);
                    GameManager.ShowCard.ShowMyCardEffect(card);
                });
            });
        }


        private void CardWd01007(Card card)
        {
            var cards = GameManager.ShowDeck.MainDeck;
            List<Card> shows = new List<Card>();
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].MyCardColor == Card.CardColor.白)
                {
                    shows.Add(cards[i]);
                }
            }

            GameManager.CardInfo.SetUp("探寻至多2只白色SIGNI", shows, 2, () =>
            {
                for (int i = 0; i < GameManager.CardInfo.SelectHands.Count; i++)
                {
                    GameManager.CreateHands.CreateHandByCard(GameManager.CardInfo.SelectHands[i].MyCard);
                }
            });

            GameManager.RpcOtherShowCards(shows);
            GameManager.CardInfo.ShowCardInfo(true);
        }

        private void CardWd01008(Card card)
        {
            GameManager.SetSigni.ShowOtherSelections(true, true);
            GameManager.SetSigni.SetSelections(false, null, true, i =>
            {
                GameManager.SetSigni.OtherSigni[i].BCantAttack = true;
                GameManager.RpcOtherDebuff(2, i, true);
            });

            GameManager.Lrig.SetOtherLrigSelection(() => GameManager.RpcOtherDebuff(2, 3, true));
        }


        private void DisSelectionBtn()
        {
            SkillManager.DisSelect();
        }

    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SkillSpell : MonoBehaviour
    {
        public Dictionary<String, Action<Card>> CardEffectSpellDictionary;
        public GameManager GameManager;
        public SkillManager SkillManager;

        public void Setup()
        {
            CardEffectSpellDictionary = new Dictionary<string, Action<Card>>()
            {
                {"WD01-006",洛可可界线},
                {"WD01-007",艾本之书},
                {"WD01-008",巴洛克防御},
                {"WD01-015",获得圣经},
                {"WX01-103",喷流的知识},
                {"WD02-006",飞火夏虫},
                {"WD02-007",背炎之阵},
                {"WD02-008",烧石炎},
                {"WD02-015",轰音火柱},
                {"WD03-006",窥视分析},
                {"WD03-007",不可行动},
                {"WD03-008",双重抽卡},
                {"WD03-015",真可惜},
                {"WD04-006",意气扬扬},
                {"WD04-007",再三再四},
                {"WD04-008",付和雷同},
                {"WD04-018",堕络},
            };
        }

        private void 洛可可界线(Card card)
        {
            SkillManager.BackHand(card, () => SkillManager.BackHand(card));
        }

        private void 艾本之书(Card card)
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
                StartCoroutine(GameManager.RpcOtherShowCards(mycards, "对方获得"));
                cardinfo.ShowCardInfo(false);
                SkillManager.WashDeck();
            });
        }

        private void 巴洛克防御(Card card)
        {
            GameManager.SetSigni.ShowOtherSelections(true);
            GameManager.SetSigni.SetSelections(false, null, true, i =>
            {
                if (i < 3)
                {
                    GameManager.SetSigni.OtherSigni[i].BCantAttack = true;
                }
                else
                {
                    GameManager.Lrig.OtherLrig.BCantAttack = true;
                }
                GameManager.RpcOtherDebuff(2, i, true);
            }, true);

            // GameManager.Lrig.SetOtherLrigSelection(() => GameManager.RpcOtherDebuff(2, 3, true));
        }

        private void 获得圣经(Card card)
        {
            var targets = SkillManager.FindCardByCondition(x => x.MyCardType == Card.CardType.精灵卡);
            var cardinfo = GameManager.CardInfo;
            cardinfo.ShowCardInfo(true);
            cardinfo.SetUp("探寻至多1只SIGNI", targets, 1, () =>
            {
                if (cardinfo.SelectHands.Count > 0 && cardinfo.SelectHands[0] != null)
                {
                    var mycard = cardinfo.SelectHands[0].MyCard;
                    GameManager.CreateHands.CreateHandFromDeck(mycard);
                    StartCoroutine(GameManager.RpcOtherShowCards(new List<Card> { mycard }, "对方获得"));
                }
                cardinfo.ShowCardInfo(false);
                SkillManager.WashDeck();
            });
        }

        private void 喷流的知识(Card card)
        {
            GameManager.SkillManager.DropCard(1);
        }

        private void 飞火夏虫(Card card)
        {
            SkillManager.Baninish(card, null, i => i.Atk <= 15000);
        }

        private void 背炎之阵(Card card)
        {
            SkillManager.DesCard(3, () =>
            {
                for (int i = 0; i < GameManager.SetSigni.Signi.Length; i++)
                {
                    GameManager.SetSigni.BanishMySigni(i);
                    GameManager.SetSigni.BanishOtherSigni(i);
                }
                SkillManager.ShowMyCard(card);
            });
        }

        private void 烧石炎(Card card)
        {
            SkillManager.Baninish(card, null, i => i.Atk <= 7000);
        }

        private void 轰音火柱(Card card)
        {
            SkillManager.Baninish(card, null, i => i.Atk <= 5000);
        }

        private void 窥视分析(Card card)
        {
            var showselects = new List<Card>()
            {
                new Card("WD03-004") {Level = 1},
                new Card("WD03-003") {Level = 2},
                new Card("WD03-002") {Level = 3},
                new Card("WD03-001") {Level = 4},
            };

            var cardinfo = GameManager.CardInfo;
            cardinfo.ShowCardInfo(true);

            cardinfo.SetUp("选择一个等级与你宣言数字相同的分身", showselects, 1, () =>
            {
                if (cardinfo.SelectHands.Count > 0)
                {
                    SkillManager.DesHandByLevel(card, cardinfo.SelectHands[0].Level, false);
                    StartCoroutine(GameManager.RpcOtherShowCards(new List<Card> { cardinfo.SelectHands[0].MyCard }, "对方宣言的等级"));
                    //cardinfo.ShowCardInfo(false);
                    GameManager.RpcGetOtherHand();
                }
            });
        }

        private void 不可行动(Card card)
        {
            SkillManager.HorizionOtherSigni(card, () => SkillManager.HorizionOtherSigni(card));
        }

        private void 双重抽卡(Card card)
        {
            SkillManager.DropCard(2);
        }

        private void 真可惜(Card card)
        {
            GameManager.RpcDesHandRandom();
        }

        private void 意气扬扬(Card card)
        {
            SkillManager.AddAtkAll(5000);

            var over = new SkillChang.EffectChang
            {
                Card = card,
                CardChangAction = card1 =>
                {
                    SkillManager.AddAtkAll(-5000);
                    SkillManager.SkillChang.MyRoundOverActions.Remove(card1.MyEffectChangMyRoundOver);
                }
            };

            card.MyEffectChangMyRoundOver = over;
            SkillManager.SkillChang.MyRoundOverActions.Add(over);
        }

        private void 再三再四(Card card)
        {
            var targets = GameManager.EnerManager.EnerCards;
            var cardinfo = GameManager.CardInfo;
            cardinfo.ShowCardInfo(true);
            cardinfo.SetUp("从你的能量区将至多2张卡加入手牌", targets, 2, () =>
            {
                var showcards = new List<Card>();
                for (int i = cardinfo.SelectHands.Count - 1; i >= 0; i--)
                {
                    GameManager.EnerManager.GetCardFromEner(cardinfo.SelectHands[i].MyCard);
                    showcards.Add(cardinfo.SelectHands[i].MyCard);
                }

                StartCoroutine(GameManager.RpcOtherShowCards(showcards, "对方从能量区获得"));

                cardinfo.ShowCardInfo(false);
            });
        }

        private void 付和雷同(Card card)
        {
            SkillManager.Baninish(card, null, i => i.Atk >= 12000);
        }

        private void 堕络(Card card)
        {
            SkillManager.HorizionMySigni(card, () => SkillManager.Baninish(card, null, i => i.Atk <= SkillManager.TargetSigni.Atk), x => x.Bset);
        }
    }
}

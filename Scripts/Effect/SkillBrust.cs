using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SkillBrust : MonoBehaviour
    {
        public Dictionary<string, Action<Card>> CardEffectBrustDictionary;
        public GameManager GameManager;
        public SkillManager SkillManager;

        public void Setup()
        {
            CardEffectBrustDictionary = new Dictionary<string, Action<Card>>()
            {
                {"WD01-009",甲胄皇家铠},
                {"WD01-011",DropCard},
                {"WX01-101",EnerChange},
                {"WX01-102",EnerChange},
                {"WX01-103",EnerChange},
                {"WD02-009",罗石火山石},
                {"WD02-011",DropCard},
                {"WD03-009",技艺代号rmn},
                {"WD03-011",DropCard},
                {"WD04-009",幻兽青龙},
                {"WD04-010",幻兽朱雀小姐},
                {"WD04-011",EnerChange},
                {"WD04-012",DropCard},
                {"WD04-014",DropCard},
            };
        }

        // 迸发最好都加上一个成功了的回调

        private void 甲胄皇家铠(Card card)
        {
            SkillManager.BackHand(card);
        }

        private void DropCard(Card card)
        {
            GameManager.SkillManager.DropCard(1);
            SkillManager.BSelected = true;
        }

        private void EnerChange(Card card)
        {
            GameManager.EnerManager.EnerCharge();
            SkillManager.BSelected = true;
        }

        private void 罗石火山石(Card card)
        {
            SkillManager.Baninish(card, null, i => i.Atk <= 7000);
        }

        private void 技艺代号rmn(Card card)
        {
            SkillManager.DesOtherCard(1);
        }

        private void 幻兽青龙(Card card)
        {
            SkillManager.Baninish(card, null, i => i.Atk >= 10000);
        }

        private void 幻兽朱雀小姐(Card card)
        {
            SkillManager.Baninish(card, null, i => i.Atk >= 12000);
        }

    }
}

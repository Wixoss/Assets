using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SkillBrust : MonoBehaviour
    {
        public Dictionary<string, Action<Card>> CardEffectBrustDictionary;
        public GameManager GameManager;

        public void Setup()
        {
            CardEffectBrustDictionary = new Dictionary<string, Action<Card>>()
            {
                {"WD01-009",CardWd01009},
                {"WD01-011",DropCard},
                {"WX01-101",EnerChange},
                {"WX01-102",EnerChange},
                {"WX01-103",EnerChange},
            };
        }

        // 迸发最好都加上一个成功了的回调

        private void CardWd01009(Card card)
        {
            SkillManager.BackHand();
        }

        private void DropCard(Card card)
        {
            GameManager.SkillManager.DropCard(1);
        }

        private void EnerChange(Card card)
        {
            GameManager.EnerManager.EnerCharge();
        }
    }
}

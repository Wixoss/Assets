using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SkillBrust : MonoBehaviour
    {
        public Dictionary<string, Action<Card>> CardEffectBrustDictionary;
        public GameManager GameManager;

        private void Awake()
        {
            CardEffectBrustDictionary = new Dictionary<string, Action<Card>>();
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SkillQi : MonoBehaviour
    {
        /// <summary>
        /// 起效果字典
        /// </summary>
        public Dictionary<String, Action<Card>> CardEffectQiDictionary;
    }
}

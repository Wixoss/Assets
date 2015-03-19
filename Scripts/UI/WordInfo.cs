﻿using UnityEngine;

namespace Assets.Scripts
{
    public class WordInfo : MonoBehaviour
    {
        public GameObject EndPhaseBtn;

        private void Awake()
        {
            EndPhaseBtn.SetActive(false);
        }

        public void SetTheEndPhase(UIEventListener.MyVoidDelegate action)
        {
            UIEventListener.Get(EndPhaseBtn).MyOnClick = action;
        }

        public void ShowTheEndPhaseBtn(bool bshow)
        {
            EndPhaseBtn.SetActive(bshow);
        }

//        public void SetSlider(int num, int max)
//        {
//            UiSlider.value = (float)num / max;
//            UiLabel.text = num.ToString();
//        }
    }
}

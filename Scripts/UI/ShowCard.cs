using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class ShowCard : MonoBehaviour
    {

        public TweenRotation TweenRotation;
        public TweenColor TweenColor;

        public UITexture MyCardTexture;
        public GameObject MyCard;
        //public UITexture  MyCardEffectTexture;
        public GameObject MyCardEffect;

        //private int num;
        public void ShowMyCard(Card card)
        {
            DisMyCardEffect();
            CancelInvoke("DisMyCard");
            ResetMyCard();
            MyCardTexture.mainTexture = card.CardTexture;
            MyCard.SetActive(true);
            ShowCardDetail.ShowCardDetailByCard(card);
            Invoke("DisMyCard", 1.2f);           
        }

        private void DisMyCard()
        {
            MyCard.SetActive(false);
            ResetMyCard();
        }

		//private int _usingNum = 0;

        public void ShowMyCardEffect(Card card)
        {
            DisMyCard();
            CancelInvoke("DisMyCardEffect");
            MyCardEffect.renderer.material.mainTexture = card.CardTexture;
            MyCardEffect.SetActive(true);
            ShowCardDetail.ShowCardDetailByCard(card);
            Invoke("DisMyCardEffect", 1.2f);
        }
		
        private void DisMyCardEffect()
        {
            MyCardEffect.SetActive(false);
        }

        private void ResetMyCard()
        {
            TweenRotation.ResetToBeginning();
            TweenColor.ResetToBeginning();
            TweenRotation.enabled = true;
            TweenColor.enabled = true;
        }
    }
}

using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
	public class ShowCard : MonoBehaviour {

		public TweenRotation TweenRotation;
		public TweenColor TweenColor;

        public TweenAlpha TweenAlpha;
        public UITexture  MyCardTexture;
        public GameObject MyCard;
        public UITexture  MyCardEffectTexture;
        public GameObject MyCardEffect;

        //private int num;
		public void ShowMyCard(Card card)
		{
            CancelInvoke("DisMyCard");
            ResetMyCard();
            MyCardTexture.mainTexture = card.CardTexture;    
            MyCard.SetActive(true);
            Invoke("DisMyCard", 1.6f);
		}

		private void DisMyCard()
		{
            MyCard.SetActive (false);
            ResetMyCard ();
		}

        public void ShowMyCardEffect(Card card)
        {
            CancelInvoke("ResetMyCardEffect");
            MyCardEffectTexture.mainTexture = card.CardTexture;
            MyCardEffect.SetActive(true);
            Invoke("DisMyCardEffect", 1.2f);
        }

        private void DisMyCardEffect()
        {
            MyCardEffect.SetActive(false);
            ResetMyCardEffect();
        }

		private void ResetMyCard()
		{
			TweenRotation.ResetToBeginning ();
			TweenColor.ResetToBeginning ();
			TweenRotation.enabled = true;
			TweenColor.enabled = true;
		}

        private void ResetMyCardEffect()
        {
            TweenAlpha.ResetToBeginning();
            TweenAlpha.enabled = true;
        }
	}
}

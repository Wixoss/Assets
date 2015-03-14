using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
	public class ShowCard : MonoBehaviour {

		public TweenRotation TweenRotation;
		public TweenColor TweenColor;
		public UITexture UiTexture;

        private int num;
		public void ShowMyCard(Card card)
		{
            num = 0;
            gameObject.SetActive(true);
            StartCoroutine(ShowMyCard2(card));
		}

	    public IEnumerator ShowMyCard2(Card card)
	    {
            Reset();
            UiTexture.mainTexture = card.CardTexture;        
	        
            while (true)
            {
                yield return new WaitForSeconds(0.5f);
                num++;
                if(num >= 3)
                {
                    DisMyCard();
                    num = 0;
                    yield break;
                }
            }	      
	    }

		public void DisMyCard()
		{
			gameObject.SetActive (false);
			Reset ();
		}

		public void Reset()
		{
			TweenRotation.ResetToBeginning ();
			TweenColor.ResetToBeginning ();
			TweenRotation.enabled = true;
			TweenColor.enabled = true;
		}
	}
}

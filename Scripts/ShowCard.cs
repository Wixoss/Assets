using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
	public class ShowCard : MonoBehaviour {

		public TweenRotation TweenRotation;
		public TweenColor TweenColor;
		public UITexture UiTexture;

		public void ShowMyCard(Card card)
		{
            gameObject.SetActive(true);
            StartCoroutine(ShowMyCard2(card));
		}

	    public IEnumerator ShowMyCard2(Card card)
	    {
            Reset();
            UiTexture.mainTexture = card.CardTexture;        
	        yield return new WaitForSeconds(1.5f);
	        DisMyCard();
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

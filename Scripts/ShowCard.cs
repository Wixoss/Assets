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
			Reset ();
			UiTexture.mainTexture = card.CardTexture;
			gameObject.SetActive (true);
			Invoke ("DisMyCard", 1.5f);
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

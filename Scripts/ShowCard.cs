using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
	public class ShowCard : MonoBehaviour {

		public TweenRotation TweenRotation;
		public TweenColor TweenColor;
		public UITexture UITexture;

		private void Awake()
		{
			gameObject.SetActive (false);
		}

		public void ShowMyCard(Card card)
		{
			Reset ();
			UITexture.mainTexture = card.CardTexture;
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

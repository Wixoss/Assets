using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class Check : MonoBehaviour
    {
        public UITexture UiTexture;
        public Trash Trash;
        public UITexture OtherUiTexture;
        public GameManager GameManager;
        private Card _myCard;
        private Card _otherCard;

        public IEnumerator SetCheck(Card card,bool bcloth = false)
        {
            UiTexture.mainTexture = card.CardTexture;
            UiTexture.gameObject.SetActive(true);
            _myCard = card;
            yield return new WaitForSeconds(3);
            UiTexture.gameObject.SetActive(false);
            if (!bcloth)
            {
                Trash.AddTrash(_myCard);
            }
        }

        public void SetOtherCheck(Card card)
        {
            _otherCard = card;
            OtherUiTexture.mainTexture = _otherCard.CardTexture;
            OtherUiTexture.gameObject.SetActive(true);
            Invoke("DisOtherCheck", 3);
        }

        public Card GetOtherCard()
        {
            return _otherCard;
        }

        private void DisOtherCheck()
        {
            OtherUiTexture.gameObject.SetActive(false);
            _otherCard = null;
        }
    }
}

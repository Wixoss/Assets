using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Trash : MonoBehaviour
    {
        public List<Card> TrashCards = new List<Card>();
        public List<Card> OtherTrashCards = new List<Card>();

        public UITexture UiTexture;
        public UITexture OtherUiTexture;

        public GameObject ShowBtn;
        public GameObject OtherShowBtn;

        public CardInfo CardInfo;

        private void Awake()
        {
            UIEventListener.Get(OtherShowBtn).MyOnClick = ShowOtherTrash;
            UIEventListener.Get(ShowBtn).MyOnClick = ShowTrash;
        }

        public void AddTrash(Card card)
        {
            TrashCards.Add(card);
            UiTexture.gameObject.SetActive(TrashCards.Count > 0);
            UiTexture.mainTexture = card.CardTexture;
        }

        public void AddOtherTrash(string cardid)
        {
            var card = new Card(cardid);
            OtherTrashCards.Add(card);
            OtherUiTexture.gameObject.SetActive(OtherTrashCards.Count > 0);
            OtherUiTexture.mainTexture = card.CardTexture;
        }

        public void ShowOtherTrash()
        {
            CardInfo.SetUp("显示废弃", OtherTrashCards, 0, () => CardInfo.ShowCardInfo(false));
            CardInfo.ShowCardInfo(true);
        }

        public void ShowTrash()
        {
            CardInfo.SetUp("显示废弃", TrashCards, 0, () => CardInfo.ShowCardInfo(false));
            CardInfo.ShowCardInfo(true);
        }

        public void DeleteTrash(Card card)
        {

        }
    }
}

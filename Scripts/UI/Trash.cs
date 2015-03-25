using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Trash : MonoBehaviour
    {
        public List<Card> TrashCards = new List<Card>();
        public List<Card> OtherTrashCards = new List<Card>();

        public List<Card> LrigTrash = new List<Card>();
        public List<Card> OtherLrigTrash = new List<Card>();

        public UITexture UiTexture;
        public UITexture OtherUiTexture;

        public GameObject ShowBtn;
        public GameObject OtherShowBtn;
        public GameManager GameManager;

        public CardInfo CardInfo;

        private void Awake()
        {
            UIEventListener.Get(OtherShowBtn).MyOnClick = ShowOtherTrash;
            UIEventListener.Get(ShowBtn).MyOnClick = ShowTrash;
        }

        public void ShowShowBtn(bool bshow)
        {
            ShowBtn.SetActive(bshow);
            OtherShowBtn.SetActive(bshow);
        }

        public void AddTrash(Card card)
        {
            if (card.MyCardType != Card.CardType.技艺卡)
            {
                TrashCards.Add(card);
            }
            else
            {
                LrigTrash.Add(card);
            }

            GameManager.RpcOtherTrash(card.CardId);

            UiTexture.gameObject.SetActive(TrashCards.Count > 0);
            UiTexture.mainTexture = card.CardTexture;
        }

        public void AddOtherTrash(Card card)
        {
            if (card.MyCardType != Card.CardType.技艺卡)
            {
                OtherTrashCards.Add(card);
            }
            else
            {
                OtherLrigTrash.Add(card);
            }
            OtherUiTexture.gameObject.SetActive(OtherTrashCards.Count > 0);
            OtherUiTexture.mainTexture = card.CardTexture;
        }

        public void ShowOtherTrash()
        {
            var othertrash = new List<Card>(OtherTrashCards);
            othertrash.AddRange(OtherLrigTrash);

            CardInfo.ShowCardInfo(true);
            CardInfo.SetUp("显示废弃(包括了分身废弃)", othertrash, 0, () => CardInfo.ShowCardInfo(false));
        }

        public void ShowTrash()
        {
            var trash = new List<Card>(TrashCards);
            trash.AddRange(LrigTrash);

            CardInfo.ShowCardInfo(true);
            CardInfo.SetUp("显示废弃(包括了分身废弃)", trash, 0, () => CardInfo.ShowCardInfo(false));
        }

        /// <summary>
        /// 重构卡组
        /// </summary>
        /// <returns>The deck.</returns>
        /// <param name="from">From.</param>
        public List<Card> RewriteDeck(List<Card> from)
        {
            var to = new List<Card>();
            int count = from.Count;
            for (int i = 0; i < count; i++)
            {
                var card = from[Random.Range(0, from.Count)];
                to.Add(card);
                from.Remove(card);
            }
            UiTexture.gameObject.SetActive(false);
            return to;
        }

        public void GetCardFromTrash(Card card)
        {
            GameManager.CreateHands.CreateHandByCard(card);
            if (card.MyCardType == Card.CardType.技艺卡)
            {
                LrigTrash.Remove(card);
            }
            else
            {
                TrashCards.Remove(card);
            }
        }

        public void OtherGetCardFromTrash(bool bRewrite, Card card)
        {
            if (bRewrite)
            {
                OtherTrashCards.Clear();
                OtherUiTexture.gameObject.SetActive(OtherTrashCards.Count > 0);
                return;
            }
            //var card = new Card(cardid);
            if (card.MyCardType == Card.CardType.技艺卡)
            {
                OtherLrigTrash.Remove(card);
            }
            else
            {
                OtherTrashCards.Remove(card);
            }
        }
    }
}

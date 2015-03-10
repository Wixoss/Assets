using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Deck
    {
        /// <summary>
        /// 卡组名
        /// </summary>
        public string DeckName;

        /// <summary>
        /// 卡组类型
        /// </summary>
        public enum DeckType
        {
            Red,
            Green,
            Blue,
            White,
            Black,
        }

        public DeckType MyDeckType;
        public List<string> DeckCardId;
        //List<Card> DeckCards;
    }

    public class DeckSelect : MonoBehaviour
    {
        public Deck MyDeck;

        public string Deckname;
        private Color _defultColor;

        private void OnEnable()
        {
            CreateTheFakeDeck();
        }

        public void CreateTheFakeDeck()
        {
            Deckname = "Deck" + "  " + Random.Range(5000, 20000);
            gameObject.GetComponentInChildren<UILabel>().text = Deckname;
            MyDeck = new Deck
            {
                DeckName = Deckname,
                MyDeckType = CreateRandomDeckType(Random.Range(1, 6)),
            };
        }

        public Deck.DeckType CreateRandomDeckType(int num)
        {
            var deck = Deck.DeckType.Red;
            switch (num)
            {
                case 1:
                    deck = Deck.DeckType.Red;
                    break;
                case 2:
                    deck = Deck.DeckType.Green;
                    break;
                case 3:
                    deck = Deck.DeckType.Blue;
                    break;
                case 4:
                    deck = Deck.DeckType.White;
                    break;
                case 5:
                    deck = Deck.DeckType.Black;
                    break;
            }
            return deck;
        }
    }
}
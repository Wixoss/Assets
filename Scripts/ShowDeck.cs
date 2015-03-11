using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class ShowDeck : MonoBehaviour
    {
        public List<Card> MainDeck = new List<Card>();
        public List<Card> LrigDeck = new List<Card>();
        public Trash Trash;

        public void WashMainDeck()
        {
            var newcards = new List<Card>();
            int count = MainDeck.Count;
            for (int i =0; i<count; i++)
            {
                var card = MainDeck[Random.Range(0,MainDeck.Count)];
                newcards.Add(card);
                MainDeck.Remove(card);
            }
            MainDeck = newcards;
        }

        public void TrashToMainDeck()
        {
            var newcards = new List<Card>();
            for (int i =0; i<Trash.TrashCards.Count; i++)
            {
                var card = Trash.TrashCards[Random.Range(0,Trash.TrashCards.Count)];
                newcards.Add(card);
                Trash.TrashCards.Remove(card);
            }
            MainDeck = newcards;
        }
    }
}

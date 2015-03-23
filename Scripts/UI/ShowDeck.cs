using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class ShowDeck : MonoBehaviour
    {
        public List<Card> MainDeck = new List<Card>();
        public List<Card> LrigDeck = new List<Card>();
        public Trash Trash;
        public GameObject MainDeckObj;
        public GameObject OtherMainDeckObj;
        public GameManager GameManager;
        public SkillManager SkillManager;

        public void WashMainDeck()
        {
            var newcards = new List<Card>();
            int count = MainDeck.Count;
            for (int i = 0; i < count; i++)
            {
                var card = MainDeck[Random.Range(0, MainDeck.Count)];
                newcards.Add(card);
                MainDeck.Remove(card);
            }
            MainDeck = newcards;
        }

        /// <summary>
        /// 把卡组顶一张卡放置到废弃
        /// </summary>
        public void MainDeckToTrash()
        {
            var card = Lastcard();
            if (card == null)
                return;
            Trash.AddTrash(card);
        }

        public void TrashToMainDeck()
        {
            MainDeck = Trash.RewriteDeck(Trash.TrashCards);
            GameManager.RpcGetCardFromTrash(true);
        }

        public Card Lastcard()
        {
            Card lastcard;
            if (MainDeck.Count > 0)
            {
                //lastcard = MainDeck[MainDeck.Count - 1];
                lastcard = MainDeck[0];
            }
            else
            {
                if (GameManager.LifeCloth.LifeCloths.Count > 0)
                {
                    //重构卡组掉一护甲
                    GameManager.LifeCloth.CrashClothToTrash();
                    GameManager.RpcCrashMyCloth(false);
                }
                TrashToMainDeck();
                //20张卡在手上和能量区中!!NULL
                if (MainDeck.Count > 0)
                {
                    lastcard = MainDeck[0];
                }
                else
                {
                    lastcard = null;
                }
            }
           
            MainDeck.Remove(lastcard);
            MainDeckObj.SetActive(MainDeck.Count > 0);
            MainDeckObj.transform.localScale = new Vector3(1, 0.06f * MainDeck.Count, 1);
            GameManager.RpcOtherDeck(MainDeck.Count);
            return lastcard;
        }

        public void OtherMainDeck(int num)
        {
            OtherMainDeckObj.transform.localScale = new Vector3(1, 0.06f * num, 1);
            OtherMainDeckObj.SetActive(num > 0);
        }
    }
}

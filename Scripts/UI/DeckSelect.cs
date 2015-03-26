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
        public UISprite UISprite;
        public Color MyColor;

        public List<string> DeckCards;

        private bool _bSelect;
        public bool BSelect
        {
            get{return _bSelect;}
            set
            {
                _bSelect = value;
                UISprite.color = _bSelect ? Color.gray : MyColor;
            }
        }

//        private void OnEnable()
//        {
//            CreateTheFakeDeck();
//        }

        public void CreateTheFakeDeck()
        {
            //Deckname = "Deck" + "  " + Random.Range(5000, 20000);
            //gameObject.GetComponentInChildren<UILabel>().text = Deckname;
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

        public void GetDeckByNum(int num)
        {
            switch (num)
            {
                case 0:
                    GetWhite();
                    break;
                case 1:
                    GetRed();
                    break;
                case 2:
                    GetBlue();
                    break;
                case 3:
                    GetGreen();
                    break;
            }
        }

        public void GetGreen()
        {
            DataSource.LrigDeckCardId = new List<string>
            {
                "WD04-001",
                "WD04-002",
                "WD04-003",
                "WD04-004",
                "WD04-005",
                "WD04-006",
                "WD04-007",
                "WD04-008",
            };
            
            DataSource.MainDeckCardId = new List<string>
            {
                "WD04-009",
                "WD04-009",
                "WD04-009",
                "WD04-009",
                
                "WD04-010",
                "WD04-010",
                "WD04-010",
                "WD04-010",
                
                "WD04-011",
                "WD04-011",
                "WD04-011",
                "WD04-011",
                
                "WD04-012",
                "WD04-012",
                "WD04-012",
                "WD04-012",
                
                "WD04-013",
                "WD04-013",
                "WD04-013",
                "WD04-013",
                
                "WD04-014",
                "WD04-014",
                "WD04-014",
                "WD04-014",
                
                "WD04-015",
                "WD04-015",
                "WD04-015",
                "WD04-015",

                "WD04-016",
                "WD04-016",
                "WD04-016",
                "WD04-016",
                
                "WD04-017",
                "WD04-017",
                "WD04-017",
                "WD04-017",
                
                "WD04-018",
                "WD04-018",
                "WD04-018",
                "WD04-018",
            };
        }

        public void GetWhite()
        {
            DataSource.LrigDeckCardId = new List<string>
            {
                "WD01-001",
                "WD01-002",
                "WD01-003",
                "WD01-004",
                "WD01-005",
                "WD01-006",
                "WD01-007",
                "WD01-008",
            };
            
            DataSource.MainDeckCardId = new List<string>
            {
                "WD01-009",
                "WD01-009",
                "WD01-009",
                "WD01-009",
                
                "WD01-010",
                "WD01-010",
                "WD01-010",
                "WD01-010",
                
                "WD01-011",
                "WD01-011",
                "WD01-011",
                "WD01-011",
                
                "WD01-012",
                "WD01-012",
                "WD01-012",
                "WD01-012",
                
                "WD01-013",
                "WD01-013",
                "WD01-013",
                "WD01-013",
                
                "WD01-014",
                "WD01-014",
                "WD01-014",
                "WD01-014",
                
                "WD01-015",
                "WD01-015",
                "WD01-015",
                "WD01-015",
                
                "WX01-101",
                "WX01-101",
                "WX01-101",
                "WX01-101",
                
                "WX01-102",
                "WX01-102",
                "WX01-102",
                "WX01-102",
                
                "WX01-103",
                "WX01-103",
                "WX01-103",
                "WX01-103",
            };
        }

        public void GetRed()
        {
            DataSource.LrigDeckCardId = new List<string>
            {
                "WD02-001",
                "WD02-002",
                "WD02-003",
                "WD02-004",
                "WD02-005",
                "WD02-006",
                "WD02-007",
                "WD02-008",
            };
            
            DataSource.MainDeckCardId = new List<string>
            {
                "WD02-009",
                "WD02-009",
                "WD02-009",
                "WD02-009",
                
                "WD02-010",
                "WD02-010",
                "WD02-010",
                "WD02-010",
                
                "WD02-011",
                "WD02-011",
                "WD02-011",
                "WD02-011",
                
                "WD02-012",
                "WD02-012",
                "WD02-012",
                "WD02-012",
                
                "WD02-013",
                "WD02-013",
                "WD02-013",
                "WD02-013",
                
                "WD02-014",
                "WD02-014",
                "WD02-014",
                "WD02-014",
                
                "WD02-015",
                "WD02-015",
                "WD02-015",
                "WD02-015",
                
                "WX01-101",
                "WX01-101",
                "WX01-101",
                "WX01-101",
                
                "WX01-102",
                "WX01-102",
                "WX01-102",
                "WX01-102",
                
                "WX01-103",
                "WX01-103",
                "WX01-103",
                "WX01-103",
            };
        }

        public void GetBlue()
        {
            DataSource.LrigDeckCardId = new List<string>
            {
                "WD03-001",
                "WD03-002",
                "WD03-003",
                "WD03-004",
                "WD03-005",
                "WD03-006",
                "WD03-007",
                "WD03-008",
            };
            
            DataSource.MainDeckCardId = new List<string>
            {
                "WD03-009",
                "WD03-009",
                "WD03-009",
                "WD03-009",
                
                "WD03-010",
                "WD03-010",
                "WD03-010",
                "WD03-010",
                
                "WD03-011",
                "WD03-011",
                "WD03-011",
                "WD03-011",
                
                "WD03-012",
                "WD03-012",
                "WD03-012",
                "WD03-012",
                
                "WD03-013",
                "WD03-013",
                "WD03-013",
                "WD03-013",
                
                "WD03-014",
                "WD03-014",
                "WD03-014",
                "WD03-014",
                
                "WD03-015",
                "WD03-015",
                "WD03-015",
                "WD03-015",
                
                "WX01-101",
                "WX01-101",
                "WX01-101",
                "WX01-101",
                
                "WX01-102",
                "WX01-102",
                "WX01-102",
                "WX01-102",
                
                "WX01-103",
                "WX01-103",
                "WX01-103",
                "WX01-103",
            };
        }
    }
}
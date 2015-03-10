using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnerManager : MonoBehaviour
    {
        /// <summary>
        /// 表现上
        /// </summary>
        public List<Hands> Eners = new List<Hands>();
        /// <summary>
        /// 具体的卡信息
        /// </summary>
        public List<Card> EnerCards = new List<Card>();

        /// <summary>
        /// 暂时保存的卡
        /// </summary>
        public List<Card> SavingEnerCards = new List<Card>();

        /// <summary>
        /// 对方的能量区
        /// </summary>
        public List<Hands> OtherEner = new List<Hands>();

        public UIGrid Grid;
        public UIGrid OtherGrid;
        public GameObject Enerobj;
        public GameObject ShowBtn;
        public GameObject OtherShowBtn;

        public CardInfo CardInfo;
        public Trash Trash;
        public GameManager GameManager;

        /// <summary>
        /// 黑
        /// </summary>
        public int BlackEner;
        /// <summary>
        /// 白
        /// </summary>
        public int WhiteEner;
        /// <summary>
        /// 红
        /// </summary>
        public int RedEner;
        /// <summary>
        /// 蓝
        /// </summary>
        public int BlueEner;
        /// <summary>
        /// 绿
        /// </summary>
        public int GreenEner;
        /// <summary>
        /// 万花
        /// </summary>
        public int AllEner;
        /// <summary>
        /// 无色
        /// </summary>
        public int NoEner;


        private void Awake()
        {
            UIEventListener.Get(ShowBtn).MyOnClick = ShowEner;
            UIEventListener.Get(OtherShowBtn).MyOnClick = ShowOtherEner;
        }


        private void ShowEner()
        {
            CardInfo.SetUp("显示能量", EnerCards, 0, () => CardInfo.ShowCardInfo(false));
            CardInfo.ShowCardInfo(true);
        }

        private void ShowOtherEner()
        {
            List<Card> cards = new List<Card>();
            for (int i = 0; i < OtherEner.Count; i++)
            {
                cards.Add(OtherEner[i].MyCard);
            }

            CardInfo.SetUp("对方能量", cards, 0, () => CardInfo.ShowCardInfo(false));
            CardInfo.ShowCardInfo(true);
        }

        /// <summary>
        /// 排列卡
        /// </summary>
        public void SetTheEner()
        {
            Grid.Reposition();
        }

        public void CreateEner(Card card)//, UseEnerType enerType
        {
            var obj = Instantiate(Enerobj) as GameObject;
            Transform trans = obj.transform;
            trans.parent = Grid.transform;
            trans.localPosition = Vector3.zero;
            trans.localEulerAngles = Vector3.zero;
            trans.localScale = new Vector3(0.7f, 0.7f, 1);
            //            if (enerType == UseEnerType.Hands)
            //            {
            var hand = obj.GetComponent<Hands>();
            hand.MyCard = card;
            Eners.Add(hand);
            EnerCards.Add(card);
            CountEner(EnerCards);
            SetTheEner();
            //            }
        }

        /// <summary>
        /// 在对方场上能量区显示
        /// </summary>
        /// <param name="cardid"></param>
        public void CreateOtherEner(string cardid)
        {
            var obj = Instantiate(Enerobj) as GameObject;
            Transform trans = obj.transform;
            trans.parent = OtherGrid.transform;
            trans.localPosition = Vector3.zero;
            trans.localEulerAngles = Vector3.zero;
            trans.localScale = new Vector3(0.7f, 0.7f, 1);
            var hand = obj.GetComponent<Hands>();
            hand.MyCard = new Card(cardid);
            OtherEner.Add(hand);
            OtherGrid.Reposition();
        }

        public void DestoryOtherEner(string cardsid)
        {
            for (int i = OtherEner.Count - 1; i >= 0; i--)
            {
                if (OtherEner[i].MyCard.CardId == cardsid)
                {
                    OtherEner[i].DestoryHands();
                    OtherEner.Remove(OtherEner[i]);                
                }
            }
            OtherGrid.Reposition();
        }


        public void CountEner(List<Card> cards)
        {
            ReflashNum();
            for (int i = 0; i < cards.Count; i++)
            {
                switch (cards[i].MyEner.MyEnerType)
                {
                    case Card.Ener.EnerType.万花:
                        AllEner += cards[i].MyEner.Num;
                        break;
                    case Card.Ener.EnerType.无:
                        NoEner += cards[i].MyEner.Num;
                        break;
                    case Card.Ener.EnerType.白:
                        WhiteEner += cards[i].MyEner.Num;
                        break;
                    case Card.Ener.EnerType.红:
                        RedEner += cards[i].MyEner.Num;
                        break;
                    case Card.Ener.EnerType.绿:
                        GreenEner += cards[i].MyEner.Num;
                        break;
                    case Card.Ener.EnerType.蓝:
                        BlueEner += cards[i].MyEner.Num;
                        break;
                    case Card.Ener.EnerType.黑:
                        BlackEner += cards[i].MyEner.Num;
                        break;
                }
            }
        }

        private void ReflashNum()
        {
            AllEner = 0;
            NoEner = 0;
            WhiteEner = 0;
            RedEner = 0;
            GreenEner = 0;
            BlueEner = 0;
            BlackEner = 0;
        }

        /// <summary>
        /// 删除能量物体 （包括移除能量数组里的能量） 
        /// </summary>
        /// <param name="hands"></param>
        public void DestoryEner(List<Card> cards)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                EnerCards.Remove(cards[i]);
                Trash.AddTrash(cards[i]);
                GameManager.RpcOtherTrash(cards[i].CardId);
                DestoryHands(cards[i]);
                CountEner(EnerCards);
                //                card = hands[i].MyCard;
                //                Eners.Remove(hands[i]);
                //                EnerCards.Remove(card);
                //                Trash.AddTrash(card);
                //                hands[i].DestoryHands();
                //                CountEner(EnerCards);
            }
        }

        private void DestoryHands(Card card)
        {
            for (int i = Eners.Count - 1; i >= 0; i--)
            {
                if (Eners[i].MyCard == card)
                {
                    Eners[i].DestoryHands();
                    Eners.Remove(Eners[i]);
                    Grid.Reposition();
                }
            }
        }

        public void SavingEner(Card card)
        {
            SavingEnerCards.Add(card);
            EnerCards.Remove(card);
        }

        public void SavingBackToEner()
        {
            EnerCards.AddRange(SavingEnerCards);
            SavingEnerCards.Clear();
        }

        /// <summary>
        /// 是何种卡被用于充能?
        /// </summary>
        public enum UseEnerType
        {
            /// <summary>
            /// 从场上选择一张去充能
            /// </summary>
            Signi,
            /// <summary>
            /// 从手卡选择一张去充能
            /// </summary>
            Hands,
            /// <summary>
            /// 充能(技艺卡)
            /// </summary>
            Deck,
        }
    }
}

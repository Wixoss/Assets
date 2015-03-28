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
        /// 任何颜色等于无色
        /// </summary>
        public int NoEner;


        private void Awake()
        {
            UIEventListener.Get(ShowBtn).MyOnClick = ShowEner;
            UIEventListener.Get(OtherShowBtn).MyOnClick = ShowOtherEner;
        }


        private void ShowEner()
        {
            CardInfo.ShowCardInfo(true);
            CardInfo.SetUp("显示能量", EnerCards, 0, () => CardInfo.ShowCardInfo(false));
        }

        private void ShowOtherEner()
        {
            List<Card> cards = new List<Card>();
            for (int i = 0; i < OtherEner.Count; i++)
            {
                cards.Add(OtherEner[i].MyCard);
            }

            CardInfo.ShowCardInfo(true);
            CardInfo.SetUp("对方能量", cards, 0, () => CardInfo.ShowCardInfo(false));
        }


        public void ShowShowBtn(bool bshow)
        {
            ShowBtn.SetActive(bshow);
            OtherShowBtn.SetActive(bshow);
        }
        /// <summary>
        /// 排列卡
        /// </summary>
        public void ResetTheEner()
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
            CancelInvoke("ResetTheEner");
            Invoke("ResetTheEner", 0.5f);
            //            }
        }

        /// <summary>
        /// 从能量区中获取卡牌到手卡
        /// </summary>
        /// <param name="card"></param>
        public void GetCardFromEner(Card card)
        {
            GameManager.CreateHands.CreateHandByCard(card);
            EnerCards.Remove(card);
            DestoryEner(new List<Card>{card});  
            GameManager.CreateHands.ShowTheUseBtn();
            GameManager.RpcDeleteOtherEner(card.CardId);
        }

        /// <summary>
        /// 把卡组顶一张卡放置到能量区
        /// </summary>
        public void EnerCharge()
        {
            var card = GameManager.ShowDeck.Lastcard();
            if (card == null)
                return;
            CreateEner(card);
            GameManager.RpcEnerCharge(card.CardId);
            //常效果:我方回合冲能时
            GameManager.SkillManager.EnerChange();
        }

        /// <summary>
        /// 在对方场上能量区显示
        /// </summary>
        /// <param name="card"></param>
        public void CreateOtherEner(Card card)
        {
            var obj = Instantiate(Enerobj) as GameObject;
            Transform trans = obj.transform;
            trans.parent = OtherGrid.transform;
            trans.localPosition = Vector3.zero;
            trans.localEulerAngles = Vector3.zero;
            trans.localScale = new Vector3(0.7f, 0.7f, 1);
            var hand = obj.GetComponent<Hands>();
            hand.MyCard = card;
            OtherEner.Add(hand);
            CancelInvoke("ResetOtherEner");
            Invoke("ResetOtherEner", 0.5f);
            //常效果:我方回合冲能时
            GameManager.SkillManager.EnerChange();
        }

        private void ResetOtherEner()
        {
            OtherGrid.Reposition();
        }

        /// <summary>
        /// 每次删除一个
        /// </summary>
        /// <param name="cardsid">Cardsid.</param>
        public void DestoryOtherEner(string cardsid)
        {
            for (int i = OtherEner.Count - 1; i >= 0; i--)
            {
                if (OtherEner[i].MyCard.CardId == cardsid)
                {
                    OtherEner[i].DestoryHands();
                    OtherEner.Remove(OtherEner[i]);
                    CancelInvoke("ResetOtherEner");
                    Invoke("ResetOtherEner", 0.5f);
                    return;
                }
            }
            //常效果:我方回合冲能时
            GameManager.SkillManager.EnerChange();
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
        /// <param name="cards"></param>
        public void DestoryEner(List<Card> cards)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                EnerCards.Remove(cards[i]);
                Trash.AddTrash(cards[i]);
                DestoryHands(cards[i]);
                CountEner(EnerCards);
            }
            CancelInvoke("ResetTheEner");
            Invoke("ResetTheEner", 0.5f);
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

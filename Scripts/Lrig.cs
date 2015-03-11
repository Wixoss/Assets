using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Assets.Scripts
{
    public class Lrig : MonoBehaviour
    {
        private List<Card> _myLrigs = new List<Card>();

        public string LrigId;
        public UITexture UiTexture;
        public GameObject UpBtn;
        public GameObject EffectBtn;
        public GameObject AttackBtn;
        public GameObject SkipBtn;

        public CardInfo CardInfo;
        public EnerManager EnerManager;
        public GameManager GameManager;

        public Hands OtherLrigObj;

        public Card MyLrig;
        public Card OtherLrig;

        public bool Bset;
        public Card UpgradingLrig;
        public GameObject LrigDeck;

        public LifeCloth LifeCloth;

        /// <summary>
        ///Other Guard. 0 is nothing,1 guard,-1 not guard
        /// </summary>
        public int Bguard = 0;

        private void Awake()
        {
            UIEventListener.Get(UpBtn).MyOnClick = Upgrade;
            UIEventListener.Get(SkipBtn).MyOnClick = GameManager.RpcGrow;
        }

        public void SetShowLrigDeck()
        {
            UIEventListener.Get(LrigDeck).MyOnClick = ShowLrigDeck;
        }

        private void ShowLrigDeck()
        {
            CardInfo.ShowCardInfo(true);
            CardInfo.SetUp("分身卡组", DataSource.LrigDeck, 0, () => CardInfo.ShowCardInfo(false));
        }

        public void SetUp(Card card)
        {
            _myLrigs.Add(card);
            MyLrig = _myLrigs[_myLrigs.Count - 1];
            Bset = true;
            LrigId = MyLrig.CardId;
            UiTexture.mainTexture = MyLrig.CardTexture;
        }

        public void Upgrade()
        {
            var lrig = new List<Card>();
            for (int i = 0; i < DataSource.LrigDeck.Count; i++)
            {
                if (DataSource.LrigDeck[i].MyCardType == Card.CardType.分身卡)
                {
                    //是同一个角色的高一级分身
                    if (DataSource.LrigDeck[i].Level == MyLrig.Level + 1 && DataSource.LrigDeck[i].Type == MyLrig.Type)
                    {
                        lrig.Add(DataSource.LrigDeck[i]);
                    }
                }
            }

            CardInfo.ShowCardInfo(true);
            CardInfo.SetUp("分身升级", lrig, 1, UpgradeCost);
        }


        private void UpgradeCost()
        {
            if (CardInfo.SelectHands.Count < 1)
            {
                CardInfo.ShowCardInfo(false);
                return;
            }
            UpgradingLrig = CardInfo.SelectHands[0].MyCard;
            if (UpgradingLrig.GrowCost.Count == 0)
            {
                SetUp(UpgradingLrig);
                GameManager.ShowCard.ShowMyCard(UpgradingLrig);
                DataSource.LrigDeck.Remove(UpgradingLrig);
            }
            else
            {
                int enerTypeCount = UpgradingLrig.GrowCost.Count;
                SetTheCost(0, enerTypeCount - 1, UpgradingLrig, () =>
                {
                    SetUp(UpgradingLrig);
                    DataSource.LrigDeck.Remove(UpgradingLrig);
                    //Last Call
                    GameManager.RpcGrow();
                    GameManager.RpcOtherLrig(UpgradingLrig.CardId);
                    GameManager.ShowCard.ShowMyCard(UpgradingLrig);
                }, true);
            }
        }

        //叼爆了的消耗费用算法        
        public List<Card> _savingHands = new List<Card>();
        /// <summary>
        /// 叼爆了的消耗费用算法  
        /// </summary>
        /// <param name="num">0</param>
        /// <param name="max">费用的种类数(cost.count-1)</param>
        /// <param name="targetCard">需要费用的那张卡</param>
        /// <param name="successd">成功的回调</param>
        /// <param name="bgrowcost">是否是成长费用</param>
        public void SetTheCost(int num, int max, Card targetCard, Action successd, bool bgrowcost = false)
        {
            var cards = new List<Card>();

            if (bgrowcost)
            {
                for (int i = 0; i < EnerManager.EnerCards.Count; i++)
                {
                    //万花等于任何颜色
                    if (EnerManager.EnerCards[i].MyEner.MyEnerType == targetCard.GrowCost[num].MyEnerType || EnerManager.EnerCards[i].MyEner.MyEnerType == Card.Ener.EnerType.万花)
                    {
                        cards.Add(EnerManager.EnerCards[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < EnerManager.EnerCards.Count; i++)
                {
                    //万花等于任何颜色
                    if (EnerManager.EnerCards[i].MyEner.MyEnerType == targetCard.Cost[num].MyEnerType || EnerManager.EnerCards[i].MyEner.MyEnerType == Card.Ener.EnerType.万花)
                    {
                        cards.Add(EnerManager.EnerCards[i]);
                    }
                }
            }

            string info;
            if (bgrowcost)
            {
                info = "所需 " + targetCard.GrowCost[num].MyEnerType + "色 费用 " + targetCard.GrowCost[num].Num + " 个";
            }
            else
            {
                info = "所需 " + targetCard.Cost[num].MyEnerType + "色 费用 " + targetCard.Cost[num].Num + " 个";
            }

            CardInfo.SetUp(info, cards, bgrowcost ? targetCard.Cost[num].Num : targetCard.GrowCost[num].Num, () =>
            {
                bool enough = true;

                int count = bgrowcost ? targetCard.GrowCost.Count : targetCard.Cost.Count;

                for (int i = 0; i < count; i++)
                {
                    enough = BEnerEnough(targetCard, i);
                }

                if (enough)
                {
                    Card card;
                    for (int i = 0; i < CardInfo.SelectHands.Count; i++)
                    {
                        card = CardInfo.SelectHands[i].MyCard;
                        _savingHands.Add(card);
                        EnerManager.SavingEner(card);
                    }

                    if (num < max)
                    {
                        num = num + 1;
                        //重复调用以达到目标
                        SetTheCost(num, max, targetCard, successd, bgrowcost);
                    }
                    else
                    {
                        EnerManager.SavingBackToEner();
                        CardInfo.ShowCardInfo(false);

                        EnerManager.DestoryEner(_savingHands);

                        for (int i = 0; i < _savingHands.Count; i++)
                        {
                            GameManager.RpcDeleteOtherEner(_savingHands[i].CardId);
                        }

                        _savingHands.Clear();

                        if (successd != null)
                        {
                            successd();
                        }
                    }
                }
                else
                {
                    CardInfo.ShowCardInfo(false);
                    _savingHands.Clear();
                    EnerManager.SavingBackToEner();
                }
            });
        }

        private bool BEnerEnough(Card target, int num, bool bgrowcost = false)
        {
            int all = 0;
            for (int i = 0; i < CardInfo.SelectHands.Count; i++)
            {
                all += CardInfo.SelectHands[i].MyEnerNum;
            }

            if (bgrowcost)
            {
                return all >= target.GrowCost[num].Num;
            }

            return all >= target.Cost[num].Num;
        }


        public void ShowLrig(bool bshow)
        {
            UiTexture.gameObject.SetActive(bshow);
        }

        public void ShowUpBtn(bool bshow)
        {
            UpBtn.SetActive(bshow);
            SkipBtn.SetActive(bshow);
        }

        public void ShowEffectBtn(bool bshow)
        {
            EffectBtn.SetActive(bshow);
        }

        public void ShowAttackBtn(bool bshow)
        {
            if (Bset)
            {
                AttackBtn.SetActive(bshow);
                UIEventListener.Get(AttackBtn).MyOnClick = () =>
                {
                    Bset = false;
                    UiTexture.transform.localEulerAngles = new Vector3(90, 90, 0);
                    AttackBtn.SetActive(false);
                    GameManager.RpcLrigSet(false);
                    GameManager.RpcLrigAttack();
                    StartCoroutine(WaitToOtherGuard());

                };
            }
        }

        private IEnumerator WaitToOtherGuard()
        {
            int i = 0;
            while (true)
            {
                yield return new WaitForSeconds(1);
                i++;

                if (i >= 10)
                {
                    LifeCloth.CrashOtherCloth();
                    GameManager.RpcCrashOtherLifeCloth();
                    yield break;
                }
                else
                {
                    if (Bguard != 0)
                    {
                        if (Bguard == 1)
                        {
                            Bguard = 0;
                            yield break;
                        }
                        else if (Bguard == -1)
                        {
                            LifeCloth.CrashOtherCloth();
                            GameManager.RpcCrashOtherLifeCloth();
                            Bguard = 0;
                            yield break;
                        }
                    }
                }
            }
        }


        public void ResetLrig()
        {
            Bset = true;
            UiTexture.transform.localEulerAngles = new Vector3(90, 0, 0);
            GameManager.RpcLrigSet(true);
        }

        public void SetOtherLrig(string card)
        {
            OtherLrigObj.gameObject.SetActive(true);
            OtherLrigObj.MyCard = new Card(card);
            OtherLrig = OtherLrigObj.MyCard;
        }

        public void SetOtherLrigSet(bool bset)
        {
            OtherLrigObj.transform.localEulerAngles = bset ? new Vector3(90, 0, 0) : new Vector3(90, 90, 0);
        }
    }
}

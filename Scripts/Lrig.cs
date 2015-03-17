﻿using System;
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
        public Hands CardHands;
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
        public GameObject OtherLrigSelection;

        /// <summary>
        ///Other Guard. 0 is nothing,1 guard,-1 not guard
        /// </summary>
        public int Bguard = 0;

        private void Awake()
        {
            UIEventListener.Get(UpBtn).MyOnClick = Upgrade;
            UIEventListener.Get(SkipBtn).MyOnClick = GameManager.RpcGrow;
            ShowLrigDeck(false);
        }

        public void SetShowLrigDeck()
        {
            UIEventListener.Get(LrigDeck).MyOnClick = () =>ShowLrigDeck(GameManager.ShowDeck.LrigDeck);
        }

        public void ShowLrigDeck(bool bshow)
        {
            LrigDeck.SetActive(bshow);
        }

        public void ShowLrigDeck(List<Card> targets)
        {
            CardInfo.ShowCardInfo(true);
            CardInfo.SetUp("使用记艺", targets.Count <= 0 ? GameManager.ShowDeck.LrigDeck : targets, 1, UseArt);
        }

        private void UseArt()
        {
            Card mycard;
            if (CardInfo.SelectHands.Count > 0)
            {
                mycard = CardInfo.SelectHands[0].MyCard;

                if (mycard.MyCardType != Card.CardType.技艺卡)
                {
                    CardInfo.ShowCardInfo(false);
                    GameManager.Reporting.text = "该卡不是技艺卡";
                    return;
                }

                //是否在时点
                bool btimg = false;

                for (int i = 0; i < mycard.MyTiming.Count; i++)
                {
                    if (mycard.MyTiming[i] == GameManager.MyTiming)
                    {
                        btimg = true;
                    }
                }

                if (!btimg)
                {
                    CardInfo.ShowCardInfo(false);
                    GameManager.Reporting.text = "使用时点错误!";
                    return;
                }

                if (mycard.Cost.Count < 1)
                {
                    mycard.Effect_Spell(mycard);

                    StartCoroutine(GameManager.Check.SetCheck(mycard));
                    GameManager.ShowCard.ShowMyCard(mycard);
                    GameManager.RpcCheck(mycard.CardId);
                    GameManager.ShowDeck.LrigDeck.Remove(mycard);

                    CardInfo.ShowCardInfo(false);

                }
                else
                {
                    SetTheCost(0, mycard.Cost.Count - 1, mycard, () =>
                    {
                        mycard.Effect_Spell(mycard);

                        StartCoroutine(GameManager.Check.SetCheck(mycard));
                        GameManager.ShowCard.ShowMyCard(mycard);
                        GameManager.RpcCheck(mycard.CardId);
                        GameManager.ShowDeck.LrigDeck.Remove(mycard);

                        CardInfo.ShowCardInfo(false);
                    },1);
                }
            }
            else
            {
                CardInfo.ShowCardInfo(false);
            }
        }

        public void SetUp(Card card)
        {
            _myLrigs.Add(card);
            MyLrig = _myLrigs[_myLrigs.Count - 1];
            Bset = true;
            LrigId = MyLrig.CardId;
            //UiTexture.mainTexture = MyLrig.CardTexture;
            CardHands.MyCard = card;
        }

        public void Upgrade()
        {
            var lrig = new List<Card>();
            for (int i = 0; i < GameManager.ShowDeck.LrigDeck.Count; i++)
            {
                if (GameManager.ShowDeck.LrigDeck[i].MyCardType == Card.CardType.分身卡)
                {
                    //是同一个角色的高一级分身
                    if (GameManager.ShowDeck.LrigDeck[i].Level == MyLrig.Level + 1 && GameManager.ShowDeck.LrigDeck[i].Type == MyLrig.Type)
                    {
                        lrig.Add(GameManager.ShowDeck.LrigDeck[i]);
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
                GameManager.ShowDeck.LrigDeck.Remove(UpgradingLrig);

                GameManager.RpcGrow();
                GameManager.RpcOtherLrig(UpgradingLrig.CardId);
                GameManager.ShowCard.ShowMyCard(UpgradingLrig);
                CardInfo.ShowCardInfo(false);
            }
            else
            {
                int enerTypeCount = UpgradingLrig.GrowCost.Count;
                SetTheCost(0, enerTypeCount - 1, UpgradingLrig, () =>
                {
                    SetUp(UpgradingLrig);
                    GameManager.ShowDeck.LrigDeck.Remove(UpgradingLrig);
                    //Last Call
                    GameManager.RpcGrow();
                    GameManager.RpcOtherLrig(UpgradingLrig.CardId);
                    GameManager.ShowCard.ShowMyCard(UpgradingLrig);
                }, 2);
            }
        }

        //叼爆了的消耗费用算法        
        private List<Card> _savingHands = new List<Card>();
        /// <summary>
        /// 叼爆了的消耗费用算法  
        /// </summary>
        /// <param name="num">0</param>
        /// <param name="max">费用的种类数(cost.count-1)</param>
        /// <param name="targetCard">需要费用的那张卡</param>
        /// <param name="successd">成功的回调</param>
        /// <param name="bgrowcost">是否是成长费用</param>
        /// <param name="Costtype">1:cost,2:GrowCost,3:ChuCost,4:QiCost</param>
        public void SetTheCost(int num, int max, Card targetCard, Action successd, int Costtype)
        {
            var cards = new List<Card>();

            List<Card.Ener> Mycosttype = new List<Card.Ener>();

            switch (Costtype)
            {
                case 1:
                    Mycosttype = targetCard.Cost;
                    break;
                case 2:
                    Mycosttype = targetCard.GrowCost;
                    break;
                case 3:
                    Mycosttype = targetCard.EffectCost_Chu;
                    break;
                case 4:
                    Mycosttype = targetCard.EffectCost_Qi;
                    break;
            }

//                for (int i = 0; i < EnerManager.EnerCards.Count; i++)
//                {
//                    //万花等于任何颜色
//                    if (EnerManager.EnerCards[i].MyEner.MyEnerType == Mycosttype[num].MyEnerType || EnerManager.EnerCards[i].MyEner.MyEnerType == Card.Ener.EnerType.万花)
//                    {
//                        cards.Add(EnerManager.EnerCards[i]);
//                    }
//                }

                for (int i = 0; i < EnerManager.EnerCards.Count; i++)
                {
                    //任何颜色都等于无色
                    if (Mycosttype[num].MyEnerType == Card.Ener.EnerType.无)
                    {
                        cards.Add(EnerManager.EnerCards[i]);
                    }
                    //万花等于任何颜色
                    if (EnerManager.EnerCards[i].MyEner.MyEnerType == Mycosttype[num].MyEnerType || EnerManager.EnerCards[i].MyEner.MyEnerType == Card.Ener.EnerType.万花)
                    {
                        cards.Add(EnerManager.EnerCards[i]);
                    }
                }

            string info;
            info = "所需 " + Mycosttype[num].MyEnerType + "色 费用 " + Mycosttype[num].Num + " 个";

            CardInfo.SetUp(info, cards, Mycosttype[num].Num, () =>
            {
                bool enough = true;

                //int count = bgrowcost ? targetCard.GrowCost.Count : targetCard.Cost.Count;

                //for (int i = 0; i < count; i++)
                //{
                enough = BEnerEnough(targetCard, num, Costtype);
                //}

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
                        SetTheCost(num, max, targetCard, successd, Costtype);
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
                    GameManager.Reporting.text = "费用不足!";
                    _savingHands.Clear();
                    EnerManager.SavingBackToEner();
                }
            });
        }

        public bool BEnerEnough(Card target, int num, int type)
        {
            int all = 0;
            for (int i = 0; i < CardInfo.SelectHands.Count; i++)
            {
                all += CardInfo.SelectHands[i].MyEnerNum;
            }
            bool benough = false;
            switch(type)
            {
                case 1:
                    benough = all >= target.Cost[num].Num ? true : false;
                    break;
                case 2:
                    benough = all >= target.GrowCost[num].Num ? true : false;
                    break;
                case 3:
                    benough = all >= target.EffectCost_Chu[num].Num ? true : false;
                    break;
                case 4:
                    benough = all >= target.EffectCost_Qi[num].Num ? true : false;
                    break;
            }
            return benough;
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

//                if (i >= 10 && Bguard == 0)
//                {
//                    LifeCloth.CrashOtherCloth(true);                    
//                    GameManager.RpcCrashOtherLifeCloth(true);
//                    yield break;
//                }
//                else
//                {
                    if (Bguard != 0)
                    {
                        if (Bguard == 1)
                        {
                            Bguard = 0;
                            yield break;
                        }
                        else if (Bguard == -1)
                        {
                            LifeCloth.CrashOtherCloth(true);
                            GameManager.RpcCrashOtherLifeCloth(true);
                            Bguard = 0;
                            yield break;
                        }
                    }
//                }
            }
        }


        public void ResetLrig()
        {
            Bset = true;
            UiTexture.transform.localEulerAngles = new Vector3(90, 0, 0);
            GameManager.RpcLrigSet(true);
        }

        /// <summary>
        /// 设置对方分身选择按钮的事件且显示
        /// </summary>
        /// <param name="myaction">Myaction.</param>
        public void SetOtherLrigSelection(Action myaction)
        {
            OtherLrigSelection.SetActive(true);
            UIEventListener.Get(OtherLrigSelection).MyOnClick = () => 
            {
                if(myaction!=null)
                    myaction();
                GameManager.SetSigni.ShowOtherSelections(false,false);
                OtherLrigSelection.SetActive(false);
            };           
        }

        /// <summary>
        /// 横置分身
        /// </summary>
        public void HorizontalLrig()
        {
            Bset = false;
            UiTexture.transform.localEulerAngles = new Vector3(90, 90, 0);
            GameManager.RpcLrigSet(false);
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

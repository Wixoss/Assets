﻿using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class SetSigni : MonoBehaviour
    {
        public Trash Trash;
        public Lrig Lrig;
        public EnerManager EnerManager;

        public Card[] Signi = new Card[3];
        public Hands[] MyHands = new Hands[3];
        public Card[] OtherSigni = new Card[3];
        public Hands[] OtherHands = new Hands[3];
        public UITexture[] OtherCardTexture;

        public GameObject[] MySelections = new GameObject[3];
        public GameObject[] OtherSelections = new GameObject[3];

        public CreateHands CreateHands;

        /// <summary>
        /// 选择我方,0,1,2
        /// </summary>
        public int MySelection = -1;
        /// <summary>
        /// 选择对方,0,1,2
        /// </summary>
        public int OtherSelection = -1;

        private Card _sendingCard;

        public GameManager GameManager;

        /// <summary>
        /// 卡图
        /// </summary>
        public UITexture[] CardTexture;
        /// <summary>
        /// 放置
        /// </summary>
        public GameObject[] SetBtn;
        /// <summary>
        /// 效果
        /// </summary> 
        //public GameObject[] EffectBtn;
        /// <summary>
        /// 丢弃
        /// </summary>
        public GameObject[] TrashBtn;
        /// <summary>
        /// 充能
        /// </summary>
        public GameObject[] ChargeBtn;
        /// <summary>
        /// 攻击
        /// </summary>
        public GameObject[] AttackBtn;

        /// <summary>
        /// 是否竖置
        /// </summary>
        //public bool[] BSet;
        /// <summary>
        /// 场上精灵等级总和
        /// </summary>
        public int SigniLevelCount;


        private void Awake()
        {
            UIEventListener.Get(SetBtn[0]).MyOnClick = () => SetMySigni1(0);
            UIEventListener.Get(SetBtn[1]).MyOnClick = () => SetMySigni1(1);
            UIEventListener.Get(SetBtn[2]).MyOnClick = () => SetMySigni1(2);

            UIEventListener.Get(CardTexture[0].gameObject).MyOnClick = () => ShowTrashBtn(0);
            UIEventListener.Get(CardTexture[1].gameObject).MyOnClick = () => ShowTrashBtn(1);
            UIEventListener.Get(CardTexture[2].gameObject).MyOnClick = () => ShowTrashBtn(2);

            UIEventListener.Get(TrashBtn[0]).MyOnClick = () => TrashSigni(0);
            UIEventListener.Get(TrashBtn[1]).MyOnClick = () => TrashSigni(1);
            UIEventListener.Get(TrashBtn[2]).MyOnClick = () => TrashSigni(2);

            UIEventListener.Get(ChargeBtn[0]).MyOnClick = () => Charge(0);
            UIEventListener.Get(ChargeBtn[1]).MyOnClick = () => Charge(1);
            UIEventListener.Get(ChargeBtn[2]).MyOnClick = () => Charge(2);
        }

        private void SetMySigni1(int num)
        {
            Signi[num] = _sendingCard;
            if (Signi[num] != null)
            {
                StartCoroutine(EffectChuDalay(Signi[num]));
                if (Signi[num].EffectChang != null)
                {
                    Signi[num].EffectChang(Signi[num]);
                }
                Signi[num].Bset = true;
                //BSet[num] = true;
                CardTexture[num].gameObject.SetActive(true);
                CardTexture[num].transform.localEulerAngles = new Vector3(90, 0, 0);
                //CardTexture[num].mainTexture = _sendingCard.CardTexture;
                MyHands[num].MyCard = _sendingCard;
                //SetEffectDelegate(num);
                DisAllSetBtn();
                GameManager.RpcSetSigni(num, _sendingCard.CardId);
                GameManager.RpcSet(num, true);
                GameManager.ShowCard.ShowMyCard(_sendingCard);

                //常效果:精灵出场时调用
                GameManager.SkillManager.SigniSet(Signi[num]);
            }
            CountSigniLevel();
            GameManager.CreateHands.ShowTheUseBtn();
        }

        private System.Collections.IEnumerator EffectChuDalay(Card card)
        {
            yield return new WaitForSeconds(2f);
            if (card.EffectChu != null)
            {
                if (card.EffectCostChu.Count < 1)
                {
                    card.EffectChu(card);
                    GameManager.ShowCard.ShowMyCardEffect(card);
                    GameManager.RpcOtherCardBuff(card.CardId);
                }
                else
                {
                    GameManager.CardInfo.ShowCardInfo(true);
                    GameManager.Lrig.SetTheCost(0, card.EffectCostChu.Count - 1, card, () =>
                    {
                        card.EffectChu(card);
                        GameManager.ShowCard.ShowMyCardEffect(card);
                        GameManager.RpcOtherCardBuff(card.CardId);
                    }, 3);
                }
            }
        }



        public bool BEnety()
        {
            bool bEnety = false;
            for (int i = 0; i < Signi.Length; i++)
            {
                if (Signi[i] == null)
                {
                    bEnety = true;
                }
            }
            return bEnety;
        }

        /// <summary>
        /// 重新计算场上精灵的总等级
        /// </summary>
        public void CountSigniLevel()
        {
            SigniLevelCount = 0;

            for (int i = 0; i < Signi.Length; i++)
            {
                if (Signi[i] != null)
                {
                    SigniLevelCount += Signi[i].Level;
                }
            }
        }

        //        private void SetEffectDelegate(int num)
        //        {
        //            UIEventListener.Get(EffectBtn[num]).MyOnClick = () =>
        //            {
        //横置
        //                    CardTexture[num].transform.localEulerAngles = new Vector3(90, 90, 0);
        //                    BSet[num] = false;
        //                    GameManager.RpcSet(num, false);
        //            };
        //        }

        /// <summary>
        /// 横置我方精灵
        /// </summary>
        /// <param name="num"></param>
        public void HorizontalSigni(int num)
        {
            CardTexture[num].transform.localEulerAngles = new Vector3(90, 90, 0);
            //BSet[num] = false;
            Signi[num].Bset = false;
            AttackBtn[num].SetActive(false);
            GameManager.RpcSet(num, false);
        }

        /// <summary>
        /// 显示废弃按钮,且显示发动效果按钮
        /// </summary>
        /// <param name="num"></param>
        private void ShowTrashBtn(int num)
        {
            if (Signi[num] != null && GameManager.MyGameState == GameManager.GameState.主要阶段)
            {
                CreateHands.ShowEffectButton(Signi[num], () =>
                {
                    if (Signi[num].EffectCostQi.Count < 1)
                    {
                        Signi[num].EffectQi(Signi[num]);
                    }
                    else
                    {
                        GameManager.CardInfo.ShowCardInfo(true);
                        Lrig.SetTheCost(0, Signi[num].EffectCostQi.Count - 1, Signi[num], () =>
                        {
                            GameManager.CardInfo.ShowCardInfo(true);
                            Signi[num].EffectQi(Signi[num]);
                        }, 4);
                    }
                },()=> 
                {
                    if(Signi[num].EffectQi == null)
                        return false;
                    else
                        return true;
                });
                ShowTrashBtn(num, !TrashBtn[num].activeSelf);
                if (!TrashBtn[num].activeSelf)
                    CreateHands.DisEffectBtn();
            }
        }

        public void SetSendingSigni(Card card)
        {
            _sendingCard = card;
            ShowTheSetBtn();
        }

        public void ShowTheSetBtn()
        {
            for (int i = 0; i < Signi.Length; i++)
            {
                if (Signi[i] == null)
                {
                    SetBtn[i].SetActive(true);
                }
            }
        }

        private void ShowTrashBtn(int num, bool bshow)
        {
            TrashBtn[num].SetActive(bshow);
			if (bshow) 
			{
				StartCoroutine (DisTrashBtn (num));
			}
        }

		private System.Collections.IEnumerator DisTrashBtn(int num)
		{
			yield return new WaitForSeconds (2);
			TrashBtn [num].SetActive (false);
		}

        public void ShowChargeBtn()
        {
            for (int i = 0; i < Signi.Length; i++)
            {
                if (Signi[i] != null)
                {
                    ShowChargeBtn(i, true);
                }
            }
        }

        private void ShowChargeBtn(int num, bool bshow)
        {
            ChargeBtn[num].SetActive(bshow);
        }

        public void DisAllSetBtn()
        {
            for (int i = 0; i < SetBtn.Length; i++)
            {
                SetBtn[i].SetActive(false);
            }
        }

        public void DisAllChargeBtn()
        {
            for (int i = 0; i < ChargeBtn.Length; i++)
            {
                ChargeBtn[i].SetActive(false);
            }
        }

        public void DisAllAttackBtn()
        {
            for (int i = 0; i < AttackBtn.Length; i++)
            {
                AttackBtn[i].SetActive(false);
            }
        }

        public void DisAllTrashBtnAndEffectBtn()
        {
            for (int i = 0; i < TrashBtn.Length; i++)
            {
                TrashBtn[i].SetActive(false);
            }
            CreateHands.DisEffectBtn();
        }


        public void TrashSigni(int num)
        {      
            if (Signi[num] != null)
            {
                if (Signi[num].SigniOutAction != null)
                {
                    Signi[num].SigniOutAction(Signi[num]);
                }
                RemoveSigniChangAction(Signi[num]);

                Signi[num].ResetCardConfig();
                Trash.AddTrash(Signi[num]);
                GameManager.RpcOtherTrash(Signi[num].CardId,num);
                CreateHands.DisEffectBtn();
                Signi[num] = null;
                CardTexture[num].gameObject.SetActive(false);
                ShowTrashBtn(num, false);
            }
            CountSigniLevel();
            GameManager.CreateHands.ShowTheUseBtn();
			//常效果:精灵离场时调用
			GameManager.SkillManager.SigniOut();
        }

        /// <summary>
        /// 清除常效果的回调
        /// </summary>
        /// <param name="card"></param>
        public void RemoveSigniChangAction(Card card)
        {
            var skillchang = GameManager.SkillManager.SkillChang;
            skillchang.EnerChangeActions.Remove(card.MyEffectChangEnerCharge);
            skillchang.MyRoundOverActions.Remove(card.MyEffectChangMyRoundOver);
            skillchang.MyRoundStartActions.Remove(card.MyEffectChangMyRoundStart);
            skillchang.SigniOutActions.Remove(card.MyEffectChangSigniOut);
            skillchang.SigniSetActions.Remove(card.MyEffectChangSigniSet);
            skillchang.LrigSetActions.Remove(card.MyEffectChangLrigSet);
            skillchang.HandsChangeActions.Remove(card.MyEffectChangHandChange);
            skillchang.AttackChangeActions.Remove(card.MyAttackChange);
            skillchang.SigniAtkAcitons.Remove(card.MyAtking);
        }


        public void TrashOtherSigni(int num)
        {
            if (OtherSigni[num] != null)
            {
                OtherCardTexture[num].gameObject.SetActive(false);
                OtherSigni[num] = null;
            }
        }

        /// <summary>
        /// 从场上充能
        /// </summary>
        /// <param name="num"></param>
        public void Charge(int num)
        {
            if (Signi[num] != null)
            {
                BanishMySigni(num);
                //GameManager.RpcBanish(num);
                GameManager.RpcEnerCharge();
            }

            //常效果:我方充能时
            GameManager.SkillManager.EnerChange();
        }

        public void ShowAttackBtn()
        {
            if (GameManager.MyGameState != GameManager.GameState.精灵攻击阶段)
                return;
            ShowTrashBtn(0, false);
            ShowTrashBtn(1, false);
            ShowTrashBtn(2, false);

            for (int i = 0; i < Signi.Length; i++)
            {
                if (Signi[i] != null && Signi[i].Bset && !Signi[i].BCantAttack)
                {
                    AttackBtn[i].SetActive(true);
                    int i1 = i;
                    UIEventListener.Get(AttackBtn[i]).MyOnClick = () => AttackOther(i1);
                }
            }
        }

        public bool BWaiting = false;
        public bool BWaitFinish = false;

        private System.Collections.IEnumerator WaitToBrust()
        {
            while (true)
            {
                if (BWaitFinish)
                {
                    BWaitFinish = false;
                    BWaiting = false;
                    Debug.Log("BWaiting = false");
                    GameManager.LifeCloth.CrashOtherCloth(true);
                    GameManager.RpcCrashOtherLifeCloth(true);
                    yield break;
                }
                yield return new WaitForSeconds(1);
            }
        }

        private void AttackOther(int num)
        {
            if (OtherSigni[num] == null)
            {
                GameManager.SkillManager.SigniAttack(Signi[num]);

                GameManager.LifeCloth.CrashOtherCloth(true);
                GameManager.RpcCrashOtherLifeCloth(true);

                if (Signi[num].Bdouble && GameManager.LifeCloth.OtherLifeObjs.Count > 0)
                {
                    BWaitFinish = false;
                    BWaiting = true;
                    StartCoroutine(WaitToBrust());
                    Debug.Log("BWaiting = false");
                }
            }
            else
            {				
                if (OtherSigni[num].Atk <= Signi[num].Atk)
                {
                    BanishOtherSigni(num);

                    if (Signi[num].Blancer && GameManager.LifeCloth.OtherLifeObjs.Count > 0)
                    {
                        GameManager.LifeCloth.CrashOtherCloth(true);
                        GameManager.RpcCrashOtherLifeCloth(true);
                    }
                }

                GameManager.SkillManager.SigniAttack(Signi[num]);
            }

            AttackBtn[num].SetActive(false);
            CardTexture[num].transform.localEulerAngles = new Vector3(90, 90, 0);
            Signi[num].Bset = false;
            GameManager.RpcSet(num, false);
        }

        public void ResetSigni()
        {
            for (int i = 0; i < Signi.Length; i++)
            {
                if (Signi[i] != null && !Signi[i].Bfreeze)
                {
                    Signi[i].Bset = true;
                    CardTexture[i].transform.localEulerAngles = new Vector3(90, 0, 0);
                    GameManager.RpcSet(i, true);
                }

                //因为buff和debuff都基本上是维持一个回合而已
                if (Signi[i] != null)
                {
                    if (Signi[i].Bfreeze)
                    {
                        Signi[i].Bfreeze = false;
                        GameManager.RpcOtherDebuff(1, i, false);
                    }
                    if (Signi[i].BCantAttack)
                    {
                        Signi[i].BCantAttack = false;
                        GameManager.RpcOtherDebuff(2, i, false);
                    }
                    if (Signi[i].Bdouble)
                    {
                        Signi[i].Bdouble = false;
                        GameManager.RpcMyBuff(2, i, false);
                    }
                    if (Signi[i].Blancer)
                    {
                        Signi[i].Blancer = false;
                        GameManager.RpcMyBuff(1, i, false);
                    }
                }
            }
        }

        /// <summary>
        /// 返回手卡
        /// </summary>
        /// <param name="num">位置</param>
        public void BackToHand(int num)
        {           
            Card card = Signi[num];
            RemoveSigniChangAction(Signi[num]);
            Signi[num] = null;
            CardTexture[num].gameObject.SetActive(false);
            card.ResetCardConfig();
            GameManager.CreateHands.CreateHandByCard(card);
            CountSigniLevel();
			//常效果:精灵离场时调用
			GameManager.SkillManager.SigniOut();
        }

        /// <summary>
        /// 显示我方选择按钮
        /// </summary>
        /// <param name="bshow">是否显示</param>
        /// <param name="condiction">条件Func</param>
        public void ShowMySelections(bool bshow, Func<Card, bool> condiction = null)
        {
            if (bshow)
            {
                for (int i = 0; i < MySelections.Length; i++)
                {
                    if (condiction != null && Signi[i] != null)
                    {
                        if (condiction(Signi[i]))
                        {
                            MySelections[i].SetActive(true);
                        }
                    }
                    else if(condiction == null && Signi[i]!=null)
                    {
                        MySelections[i].SetActive(true);
                    }
                }
            }
            else
            {
                for (int i = 0; i < MySelections.Length; i++)
                {
                    MySelections[i].SetActive(false);
                }
            }
        }

        /// <summary>
        /// 显示对方选择按钮
        /// </summary>
        /// <param name="bshow">是否显示</param>
        /// <param name="condiction">条件</param>
        /// <param name="bLrig">是否包含对方分身</param>
        public void ShowOtherSelections(bool bshow, Func<Card, bool> condiction = null, bool bLrig = false)
        {
            if (bshow)
            {
                for (int i = 0; i < OtherSelections.Length; i++)
                {
                    if (condiction != null && OtherSigni[i] != null)
                    {
                        if (condiction(OtherSigni[i]))
                        {
                            OtherSelections[i].SetActive(true);
                        }
                    }
                    else if(condiction == null && OtherSigni[i]!=null)
                    {
                        OtherSelections[i].SetActive(true);
                    }
                }
            }
            else
            {
                for (int i = 0; i < OtherSelections.Length; i++)
                {
                    OtherSelections[i].SetActive(false);
                }
            }
        }

        /// <summary>
        /// 在结束主要阶段的时候隐藏所有按钮
        /// </summary>
        public void DisAllSelection()
        {
            for (int i = 0; i < OtherSelections.Length; i++)
            {
                OtherSelections[i].SetActive(false);
            }
            for (int i = 0; i < MySelections.Length; i++)
            {
                MySelections[i].SetActive(false);
            }
        }

        /// <summary>
        /// 显示怪物的选择按钮(这只是设置按钮委托,不显示按钮!!)
        /// </summary>
        /// <param name="showMy">是否显示我方的按钮</param>
        /// <param name="myAction">选择己方怪物添加状态</param>
        /// <param name="showOther">是否显示对方的按钮</param>
        /// <param name="otherAction">选择对方怪物添加状态</param>
        /// <param name="bincludeLrig">是否包括分身</param>
        public void SetSelections(bool showMy, Action<int> myAction, bool showOther, Action<int> otherAction, bool bincludeLrig)
        {
            if (showMy)
            {
                for (int i = 0; i < MySelections.Length; i++)
                {
                    var i1 = i;
                    UIEventListener.Get(MySelections[i]).MyOnClick = () =>
                    {
                        MySelection = i1;
                        //为了后续的成功回调
                        for (int k = MySelections.Length - 1; k >= 0; k--)
                        {
                            MySelections[k].SetActive(false);
                        }

                        if (bincludeLrig)
                        {
                            Lrig.MyLrigSelection.SetActive(false);
                        }

                        if (myAction != null)
                        {
                            myAction(MySelection);
                            MySelection = -1;
                        }
                    };
                }

                if (bincludeLrig)
                {
                    Lrig.SetMyLrigSelection(() =>
                    {
                        if (myAction != null)
                        {
                            MySelection = 3;
                            myAction(MySelection);
                            MySelection = -1;
                        }
                    });
                }

            }

            if (showOther)
            {
                for (int j = OtherSelections.Length - 1; j >= 0; j--)
                {
                    var j1 = j;
                    UIEventListener.Get(OtherSelections[j]).MyOnClick = () =>
                    {
                        OtherSelection = j1;
                        //为了后续的成功回调
                        for (int k = MySelections.Length - 1; k >= 0; k--)
                        {
                            OtherSelections[k].SetActive(false);
                        }

                        if (bincludeLrig)
                        {
                            Lrig.OtherLrigSelection.SetActive(false);
                        }

                        if (otherAction != null)
                        {
                            otherAction(OtherSelection);
                            OtherSelection = -1;
                        }
                    };
                }

                if (bincludeLrig)
                {
                    Lrig.SetOtherLrigSelection(() =>
                    {
                        if (otherAction != null)
                        {
                            OtherSelection = 3;
                            otherAction(OtherSelection);
                            OtherSelection = -1;
                        }
                    });
                }
            }
        }


        #region 对方的精灵操作

        public void SetOtherSigni(int num, Card card)
        {
            OtherSigni[num] = card;
            //OtherCardTexture [num].mainTexture = card.CardTexture;
            OtherHands[num].MyCard = card;
            OtherCardTexture[num].gameObject.SetActive(true);
        }

        /// <summary>
        /// 被驱逐精灵
        /// </summary>
        /// <param name="num">第几只</param>
        public void BanishMySigni(int num)
        {           
            if (Signi[num] != null)
            {
                if (Signi[num].SigniOutAction != null)
                {
                    Signi[num].SigniOutAction(Signi[num]);
                }

                RemoveSigniChangAction(Signi[num]);

                Signi[num].ResetCardConfig();
                EnerManager.CreateEner(Signi[num]);
                GameManager.RpcBanishOther(num);
                GameManager.RpcEnerCharge(Signi[num].CardId);
                Signi[num] = null;
                CardTexture[num].gameObject.SetActive(false);
            }
            CountSigniLevel();
			//常效果:精灵离场时调用,在清除卡的常效果前调用
			GameManager.SkillManager.SigniOut();
        }


        public void BanishOtherSigni(int num)
        {
            if (OtherSigni[num] != null)
            {
                //EnerManager.CreateOtherEner(OtherSigni[num].MyCard.CardId);
                GameManager.RpcBanish(num);
                OtherSigni[num] = null;
                OtherCardTexture[num].gameObject.SetActive(false);
            }
        }

        public void BackOtherSigniHand(int num)
        {
            if (OtherSigni [num] != null)
            {
                GameManager.RpcBackHand(num);
                OtherSigni[num] = null;
                OtherCardTexture[num].gameObject.SetActive(false);
            }
        }

        public void ShowBanishOtherSigni(int num)
        {
            if (OtherSigni[num] != null)
            {
                //EnerManager.CreateOtherEner(OtherSigni[num].MyCard.CardId);
                OtherSigni[num] = null;
                OtherCardTexture[num].gameObject.SetActive(false);
            }
        }

        public void SetOtherSigniSet(int num, bool bset)
        {
            OtherCardTexture[num].transform.localEulerAngles = bset ? new Vector3(90, 0, 0) : new Vector3(90, 90, 0);
        }

        #endregion
    }
}

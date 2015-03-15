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

        /// <summary>
        /// 选择我方
        /// </summary>
        public int MySelection;
        /// <summary>
        /// 选择对方
        /// </summary>
        public int OtherSelection;

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
        public GameObject[] EffectBtn;
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
        public bool[] BSet;
        /// <summary>
        /// 场上精灵等级总和
        /// </summary>
        public int SigniLevelCount;

        private void Awake()
        {
            UIEventListener.Get(SetBtn[0]).MyOnClick = () => SetMySigni1(0);
            UIEventListener.Get(SetBtn[1]).MyOnClick = () => SetMySigni1(1);
            UIEventListener.Get(SetBtn[2]).MyOnClick = () => SetMySigni1(2);

            UIEventListener.Get(CardTexture[0].gameObject).MyOnClick = () => ShowEffectBtn(0);
            UIEventListener.Get(CardTexture[1].gameObject).MyOnClick = () => ShowEffectBtn(1);
            UIEventListener.Get(CardTexture[2].gameObject).MyOnClick = () => ShowEffectBtn(2);

            UIEventListener.Get(TrashBtn[0]).MyOnClick = () => TrashSigni(0);
            UIEventListener.Get(TrashBtn[1]).MyOnClick = () => TrashSigni(1);
            UIEventListener.Get(TrashBtn[2]).MyOnClick = () => TrashSigni(2);

            UIEventListener.Get(ChargeBtn[0]).MyOnClick = () => Charge(0);
            UIEventListener.Get(ChargeBtn[1]).MyOnClick = () => Charge(1);
            UIEventListener.Get(ChargeBtn[2]).MyOnClick = () => Charge(2);

            for (int i =0; i<MySelections.Length; i++)
            {
                var i1 = i;
                UIEventListener.Get(MySelections[i]).MyOnClick = ()=> 
                {
                    MySelection = i1;
                    for (int k = MySelections.Length-1; k>=0; k--)
                    {
                        MySelections[k].SetActive(false);
                    }
                };
            }

            for (int j = MySelections.Length-1; j>=0; j--)
            {
                var j1 = j;
                UIEventListener.Get(OtherSelections[j]).MyOnClick = ()=> 
                {
                    OtherSelection = j1;
                    for (int k = MySelections.Length-1; k>=0; k--)
                    {
                        OtherSelections[k].SetActive(false);
                    }
                };
            }
        }

        private void SetMySigni1(int num)
        {
            Signi[num] = _sendingCard;
            if (Signi[num] != null)
            {
                BSet[num] = true;
                CardTexture[num].gameObject.SetActive(true);
                CardTexture[num].transform.localEulerAngles = new Vector3(90, 0, 0);
                //CardTexture[num].mainTexture = _sendingCard.CardTexture;
                MyHands[num].MyCard = _sendingCard;
                SetEffectDelegate(num);
                DisAllSetBtn();
                GameManager.RpcSetSigni(num, _sendingCard.CardId);
				GameManager.RpcSet(num, true);
				GameManager.ShowCard.ShowMyCard(_sendingCard);
            }
            CountSigniLevel();
            GameManager.CreateHands.ShowTheUseBtn();
        }

		public bool BEnety()
		{
			bool bEnety = false;
			for (int i = 0; i<Signi.Length; i++) 
			{
				if(Signi[i]==null)
				{
					bEnety = true;
				}
			}
			return bEnety;
		}

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

        private void SetEffectDelegate(int num)
        {
            UIEventListener.Get(EffectBtn[num]).MyOnClick = () =>
            {
                if (Signi[num].Cost.Count < 1)
                {
                    Signi[num].Effect_Qi(Signi[num]);
                    //横置
                    CardTexture[num].transform.localEulerAngles = new Vector3(90, 90, 0);
                    BSet[num] = false;
                    GameManager.RpcSet(num, false);
                }
                else
                {
                    int num1 = num;
//                    if(Signi[num1].Cost.Count <= 0)
//                    {
//                        return;
//                    }
                    Lrig.SetTheCost(0, Signi[num1].Cost.Count - 1, Signi[num1], () =>
                    {
                        Signi[num].Effect_Qi(Signi[num]);
                        //横置
                        CardTexture[num].transform.localEulerAngles = new Vector3(90, 90, 0);
                        BSet[num] = false;
                        GameManager.RpcSet(num, false);
                    });
                }
            };
        }

        private void ShowEffectBtn(int num)
        {
            if (Signi[num] != null && GameManager.MyGameState == GameManager.GameState.主要阶段)
            {
                ShowEffectBtn(num, !EffectBtn[num].activeSelf);
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

        private void ShowEffectBtn(int num, bool bshow)
        {
            EffectBtn[num].SetActive(bshow);
            TrashBtn[num].SetActive(bshow);
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


        public void TrashSigni(int num)
        {
            if (Signi[num] != null)
            {
                Trash.AddTrash(Signi[num]);
                Signi[num] = null;
                CardTexture[num].gameObject.SetActive(false);
                ShowEffectBtn(num, false);
                CountSigniLevel();
                GameManager.CreateHands.ShowTheUseBtn();
            }
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
                GameManager.RpcBanishOther(num);
                GameManager.RpcEnerCharge();
            }
        }

        public void ShowAttackBtn()
        {
            ShowEffectBtn(0, false);
            ShowEffectBtn(1, false);
            ShowEffectBtn(2, false);

            for (int i = 0; i < Signi.Length; i++)
            {
                if (Signi[i] != null && BSet[i])
                {
                    AttackBtn[i].SetActive(true);
                    int i1 = i;
                    UIEventListener.Get(AttackBtn[i]).MyOnClick = () => AttackOther(i1);
                }
            }
        }

        private void AttackOther(int num)
        {
            if (OtherSigni[num] == null)
            {
                GameManager.LifeCloth.CrashOtherCloth(true);
                GameManager.RpcCrashOtherLifeCloth(true);

                if(Signi[num].Bdouble)
                {
                    GameManager.LifeCloth.CrashOtherCloth(true);
                    GameManager.RpcCrashOtherLifeCloth(true);
                }
            }
            else
            {
                if (OtherSigni[num].Atk <= Signi[num].Atk)
                {
                    BanishOtherSigni(num);

                    if(Signi[num].Blancer)
                    {
                        GameManager.LifeCloth.CrashOtherCloth(true);
                        GameManager.RpcCrashOtherLifeCloth(true);
                    }
                }
            }

            AttackBtn[num].SetActive(false);
            CardTexture[num].transform.localEulerAngles = new Vector3(90, 90, 0);
            BSet[num] = false;
            GameManager.RpcSet(num, false);
        }

        public void ResetSigni()
        {
            for (int i = 0; i < Signi.Length; i++)
            {
                if (Signi[i] != null && !Signi[i].Bfreeze)
                {
                    BSet[i] = true;
                    CardTexture[i].transform.localEulerAngles = new Vector3(90, 0, 0);
                    GameManager.RpcSet(i, true);
                }

                if(Signi[i]!=null&&Signi[i].Bfreeze)
                {
                    Signi[i].Bfreeze = false;
                }
            }
        }

        /// <summary>
        /// 返回手卡
        /// </summary>
        /// <param name="num">位置</param>
        public void BackToHand(int num)
        {
            Card card = Signi [num];
            Signi [num] = null;
            CardTexture[num].gameObject.SetActive(false);
            GameManager.CreateHands.CreateHandByCard(card);
            GameManager.RpcBanishOther(num);
        }

        /// <summary>
        /// 显示我方选择按钮
        /// </summary>
        /// <param name="bshow">If set to <c>true</c> bshow.</param>
        /// <param name="condiction">If set to <c>true</c> condiction.</param>
        public void ShowMySelections(bool bshow,bool condiction)
        {
            if (bshow)
            {
                for (int i =0; i<MySelections.Length; i++)
                {
                    if (condiction && Signi[i]!=null)
                    {
                        MySelections [i].SetActive(true);
                    }
                }
            } 
            else
            {
                for (int i =0; i<MySelections.Length; i++)
                {
                    MySelections [i].SetActive(false);
                }
            }
        }

        /// <summary>
        /// 显示对方选择按钮
        /// </summary>
        /// <param name="bshow">If set to <c>true</c> bshow.</param>
        /// <param name="condiction">If set to <c>true</c> condiction.</param>
        public void ShowOtherSelections(bool bshow,bool condiction)
        {
            if (bshow)
            {
                for (int i =0; i<OtherSelections.Length; i++)
                {
                    if (condiction && OtherSigni[i]!=null)
                    {
                        OtherSelections [i].SetActive(true);
                    }
                }
            } 
            else
            {
                for (int i =0; i<OtherSelections.Length; i++)
                {
                    OtherSelections [i].SetActive(false);
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
                EnerManager.CreateEner(Signi[num]);
                GameManager.RpcEnerCharge(Signi[num].CardId);
                Signi[num] = null;
                CardTexture[num].gameObject.SetActive(false);
            }
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

using System;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class Hands : MonoBehaviour
    {
        /// <summary>
        /// 换牌阶段且选择了
        /// </summary>
        public bool Bselect;
        /// <summary>
        /// 是否显示
        /// </summary>
        //public bool Bshow;
        /// <summary>
        /// 效果
        /// </summary>
        public Action Effect;
        /// <summary>
        /// 在哪里?
        /// </summary>
        [HideInInspector]
        public Transform Parent;
        /// <summary>
        /// 点击事件
        /// </summary>
        public Action<GameObject> OnClickAction;

        public Card.CardType MyCardType;
        public Card.CardColor MyColor;
        //public Card.State MyState;
        public Card.Ener.EnerType MyEnerType;
        public int MyEnerNum;

        //        public List<string> MyCostType = new List<string>();
        //        public List<int> MyCostNum = new List<int>();
        //
        //        public List<string> MyGrowCostType = new List<string>();
        //        public List<int> MyGrowCostNum = new List<int>();

        public int Level;
        public int Limit;
        public int Atk;

        //        [HideInInspector]
        //        public GameObject UseBtn;
        //        [HideInInspector]
        //        public GameObject GuardBtn;
        //        [HideInInspector]
        //        public UISprite UseBtnTexture;
        //        [HideInInspector]
        //        public UILabel UseBtnLabel;
        //        [HideInInspector]
        //        public UISprite GuardBtnTexture;
        //        [HideInInspector]
        //        public UILabel GuardBtnLabel;
        [HideInInspector]
        public BoxCollider BoxCollider;

        private Card _myCard;
        public Card MyCard
        {
            get { return _myCard; }
            set
            {
                if (value == null)
                    return;
                Reflash();
                _myCard = value;
                Cardid = _myCard.CardId;
                MyCardType = _myCard.MyCardType;
                MyColor = _myCard.MyCardColor;
                MyEnerNum = _myCard.MyEner.Num;
                MyEnerType = _myCard.MyEner.MyEnerType;

                //                if (_myCard.Cost.Count > 0)
                //                {
                //                    for (int i = 0; i < _myCard.Cost.Count; i++)
                //                    {
                //                        MyCostType.Add(_myCard.Cost[i].MyEnerType.ToString());
                //                        MyCostNum.Add(_myCard.Cost[i].Num);
                //                    }
                //                }
                //
                //                if (_myCard.GrowCost.Count > 0)
                //                {
                //                    for (int i = 0; i < _myCard.GrowCost.Count; i++)
                //                    {
                //                        MyGrowCostType.Add(_myCard.GrowCost[i].MyEnerType.ToString());
                //                        MyGrowCostNum.Add(_myCard.GrowCost[i].Num);
                //                    }
                //                }

                Level = _myCard.Level;
                Limit = _myCard.Limit;
                Atk = _myCard.Atk;
                CardDetail = _myCard.CardDetail;
                if (UiTexture == null)
                {
                    gameObject.GetComponent<UITexture>().mainTexture = MyCard.CardTexture;
                }
                else
                {
                    UiTexture.mainTexture = MyCard.CardTexture;
                }
            }
        }

        [HideInInspector]
        public UITexture UiTexture;
        [HideInInspector]
        public TweenColor TweenColor;

        public string Cardid;
        public string CardDetail;

        /// <summary>
        /// 重置
        /// </summary>
        public void Reflash()
        {
            Bselect = false;
            Effect = null;
            OnClickAction = null;
            if (TweenColor != null && UiTexture != null)
            {
                TweenColor.ResetToBeginning();
                TweenColor.enabled = false;
                UiTexture.color = Color.white;
            }            
            //            MyCostType.Clear();
            //            MyCostNum.Clear();
            //            MyGrowCostType.Clear();
            //            MyGrowCostNum.Clear();
        }

        /// <summary>
        /// 创建手牌
        /// </summary>
        /// <returns></returns>
        public void CreateHands(Card myCard, Action<GameObject> onClick = null)
        {
            MyCard = myCard;
            OnClickAction = onClick;
        }

        public void SetOnClickAction(Action<GameObject> action)
        {
            OnClickAction = action;
        }

        //        public void SetUseBtnDelegate(UIEventListener.VoidDelegate useAction)
        //        {
        //            if (UseBtn != null)
        //            {
        //                UIEventListener.Get(UseBtn).onClick = useAction;
        //            }
        //        }
        //
        //        public void SetGuardBtnDelegate(UIEventListener.MyVoidDelegate guardAction)
        //        {
        //            if (GuardBtn != null)
        //            {
        //                UIEventListener.Get(GuardBtn).MyOnClick = guardAction;
        //            }
        //        }

        public void DestoryHands()
        {
            Reflash();
            Destroy(gameObject);
        }

        public void ShowCanUseTips(bool bshow)
        {
            TweenColor.ResetToBeginning();
            TweenColor.PlayForward();
            TweenColor.enabled = bshow;           
            //UiTexture.color = bshow ? Color.Lerp(Color.white, Color.green, 0.1f) : Color.white;
        }

        private void OnClick()
        {
            if (OnClickAction != null)
                OnClickAction(gameObject);
        }

        //        public void ShowUseBtn(bool bshow)
        //        {
        //            UseBtn.SetActive(bshow);
        //        }

    }
}

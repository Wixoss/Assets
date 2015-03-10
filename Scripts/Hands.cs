using System;
using UnityEngine;

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
        public bool Bshow;
        /// <summary>
        /// 效果
        /// </summary>
        public Action Effect;
        /// <summary>
        /// 在哪里?
        /// </summary>
        public Transform Parent;
        /// <summary>
        /// 点击事件
        /// </summary>
        public Action<GameObject> OnClickAction;

        public Card.CardType MyCardType;
        public Card.CardColor MyColor;
        public Card.State MyState;
        public Card.Ener.EnerType MyEnerType;
        public int MyEnerNum;
        public Card.Ener.EnerType MyCostType;
        public int MyCostNum;
        public int Level;
        public int Limit;

        public GameObject UseBtn;
        public GameObject GuardBtn;

        private Card _myCard;
        public Card MyCard
        {
            get { return _myCard; }
            set
            {
                if(value==null)
                    return;
                _myCard = value;
                Cardid = _myCard.CardId;
                MyCardType = _myCard.MyCardType;
                MyColor = _myCard.MyCardColor;
                MyState = _myCard.MyState;
                MyEnerNum = _myCard.MyEner.Num;
                MyEnerType = _myCard.MyEner.MyEnerType;

                if (_myCard.Cost.Count > 0)
                {
                    MyCostNum = _myCard.Cost[0].Num;
                    MyCostType = _myCard.Cost[0].MyEnerType;
                }              
                Level = _myCard.Level;
                Limit = _myCard.Limit;
                //UiTexture.mainTexture = MyCard.CardTexture;
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

        public UITexture UiTexture;

        public string Cardid;

        /// <summary>
        /// 重置
        /// </summary>
        public void Reflash()
        {
            Bselect = false;
            Effect = null;
            OnClickAction = null;
        }

        /// <summary>
        /// 创建手牌
        /// </summary>
        /// <returns></returns>
        public void CreateHands(Card myCard, Action<GameObject> onClick = null)
        {
            Reflash();
            OnClickAction = onClick;
            MyCard = myCard;
            Cardid = myCard.CardId;
            gameObject.GetComponent<UITexture>().mainTexture = MyCard.CardTexture;
        }

        public void SetOnClickAction(Action<GameObject> action)
        {
            OnClickAction = action;
        }

        public void SetUseBtnDelegate(UIEventListener.VoidDelegate useAction)
        {
            if (UseBtn != null)
            {
                UIEventListener.Get(UseBtn).onClick = useAction;
            }
        }

        public void SetGuardBtnDelegate(UIEventListener.MyVoidDelegate guardAction)
        {
            if (GuardBtn != null)
            {
                UIEventListener.Get(GuardBtn).MyOnClick = guardAction;
            }
        }

        public void DestoryHands()
        {
            Reflash();
            Destroy(gameObject);
            //Breed.Instance().Get("Hands").Unspawn(gameObject);
        }

        private void OnEnable()
        {
            //            MyState = State.手牌;
            Bshow = true;
        }

        private void OnDisable()
        {
            Bshow = false;
        }

        private void OnClick()
        {
            if (OnClickAction != null)
                OnClickAction(gameObject);
        }
    }
}

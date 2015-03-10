using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class CardInfo : MonoBehaviour
    {
        private List<Card> _showList = new List<Card>();

        public List<UITexture> UiTextures = new List<UITexture>();

        public Transform Parent;

        /// <summary>
        /// 点击物体的事件
        /// </summary>
        public UIEventListener.VoidDelegate ClickObjAction = null;

        public UILabel InfoLabel;

        public GameObject OkButton;

        public void ShowTheShowList(string word, UIEventListener.VoidDelegate action = null)
        {
            InfoLabel.text = word;
            ClickObjAction = action;
            for (int i = 0; i < _showList.Count; i++)
            {
                SetTheTexture(i, _showList[i]);
            }
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="cards">卡牌</param>
        /// <param name="limit">选择的个数限制</param>
        /// <param name="action">点击确定按钮代理</param>
        public void SetUp(string title, List<Card> cards, int limit, UIEventListener.MyVoidDelegate action)
        {
            Reset();

            _showList = cards;
            _limitSelectNum = limit;

            ShowTheShowList(title, SelectAction);
            UIEventListener.Get(OkButton).MyOnClick = action;
        }

        //        private void OnEnable()
        //        {
        //            _showList = DataSource.LrigDeck;
        //            _limitSelectNum = 1;
        //            ShowTheShowList("测试", SelectAction);
        //
        //            OkButton.onClick.Add(new EventDelegate(() =>
        //            {
        //                for (int i = 0; i < SelectHands.Count; i++)
        //                {
        //                    Debug.Log(SelectHands[i].Cardid);
        //                    DataSource.LrigDeck.Remove(SelectHands[i].MyCard);
        //                    //gameObject.SetActive(false);
        //                }
        //            }));
        //        }

        private void SetTheTexture(int num, Card id)
        {
            UiTextures[num].GetComponent<Hands>().MyCard = id;
            UiTextures[num].gameObject.SetActive(true);
            UIEventListener.Get(UiTextures[num].gameObject).onClick = ClickObjAction;
        }

        private void Reset()
        {
            for (int i = 0; i < UiTextures.Count; i++)
            {
                UiTextures[i].gameObject.SetActive(false);
                UiTextures[i].color = Color.white;
            }

            ClickObjAction = null;
            UIEventListener.Get(OkButton).onClick = null;
            InfoLabel.text = "";

            for (int i = 0; i < SelectHands.Count; i++)
            {
                SelectHands[i].Bselect = false;
            }
            
            SelectHands.Clear();
            _selectnum = 0;
        }

        public void ShowCardInfo(bool bshow)
        {
            gameObject.SetActive(bshow);
        }

        private int _selectnum = 0;
        private int _limitSelectNum;
        public List<Hands> SelectHands = new List<Hands>();
        private void SelectAction(GameObject go)
        {
            var card = go.GetComponent<Hands>();
            if (_selectnum < _limitSelectNum)
            {
                card.Bselect = !card.Bselect;
                card.GetComponent<UITexture>().color = card.Bselect ? Color.gray : Color.white;
                if (card.Bselect)
                {
                    SelectHands.Add(card);                   
                }
                else
                {
                    SelectHands.Remove(card);
                }
                _selectnum = SelectHands.Count;
            }
            else
            {
                if (card.Bselect)
                {
                    card.Bselect = !card.Bselect;
                    card.GetComponent<UITexture>().color = card.Bselect ? Color.gray : Color.white;
                    if (card.Bselect)
                    {
                        SelectHands.Add(card);
                    }
                    else
                    {
                        SelectHands.Remove(card);
                    }
                    _selectnum = SelectHands.Count;
                }
            }
        }
    }
}

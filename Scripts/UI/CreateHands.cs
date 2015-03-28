using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class CreateHands : MonoBehaviour
    {
        /// <summary>
        /// 手牌(复制)
        /// </summary>
        public GameObject Hands;
        public GameObject OtherHand;
        public UIGrid Grid;
        public UIGrid OtherGrid;
        public Transform Parent;
        public Transform OtherParent;

        //public UIGrid Grid;
        public GameObject NotGuard;
        public GameObject Guard;
        public GameObject MyChangeBtn;
        public GameObject MyEnerChagerBtn;
        public GameObject MyDesBtn;
        public GameObject UseBtn;
        public GameObject NotUseBtn;
        public GameObject UseEffectBtn;
        public GameObject NotUseEffectBtn;
        public GameObject UseCardBtn;
        public GameObject NotUseCardBtn;
        public List<Hands> MyHands = new List<Hands>();
        public List<GameObject> OtherHands = new List<GameObject>();
        public List<Card> MyHandCards = new List<Card>();
        public Hands Ener;
        public EnerManager EnerManager;
        public Lrig Lrig;
        public SetSigni SetSigni;
        public Trash Trash;
        public Check Check;
        public GameManager GameManager;
        public CardInfo CardInfo;

        /// <summary>
        /// 对面展示的卡
        /// </summary>
        public List<Card> OtherShowCards = new List<Card>();


        private void Awake()
        {
            MyChangeBtn.SetActive(false);
            MyEnerChagerBtn.SetActive(false);
            UIEventListener.Get(MyChangeBtn).MyOnClick = ChangeMyHands;
            UIEventListener.Get(MyEnerChagerBtn).MyOnClick = EnerChange;
            UIEventListener.Get(MyDesBtn).MyOnClick = DesHands;
            UIEventListener.Get(UseBtn).MyOnClick = () =>
            {
                ShowUseArtBtn(false);
                Lrig.ShowLrigDeck(_targetCards);
                GameManager.RpcOtherUseArt(1);
                _targetCards.Clear();
            };
            UIEventListener.Get(NotUseBtn).MyOnClick = () =>
            {
                ShowUseArtBtn(false);
                GameManager.RpcOtherUseArt(-1);
            };

            UIEventListener.Get(NotUseCardBtn).MyOnClick = () =>
            {
                _usingHands = null;
                NotUseCardBtn.SetActive(false);
                UseCardBtn.SetActive(false);
            };
        }

        private Card _effectionCard;

        public void ShowEffectButton(Card card, System.Action myaction, System.Func<bool> condiction = null,System.Action failed = null)
        {
            UIEventListener.Get(UseEffectBtn).MyOnClick = () =>
            {
                if (myaction != null)
                    myaction();
            };

            UIEventListener.Get(NotUseEffectBtn).MyOnClick = () =>
            {
                if (failed != null)
                    failed();
                UseEffectBtn.SetActive(false);
                NotUseEffectBtn.SetActive(false);
            };

            if (condiction == null)
            {
                UseEffectBtn.SetActive(true);
            } 
            else
            {
                UseEffectBtn.SetActive(condiction());
            }

            NotUseEffectBtn.SetActive(true);
        }

        /// <summary>
        /// 隐藏效果按钮
        /// </summary>
        public void DisEffectBtn()
        {
            UseEffectBtn.SetActive(false);
            NotUseEffectBtn.SetActive(false);
        }

        /// <summary>
        /// 抽5张卡
        /// </summary>
        /// <returns></returns>
        public IEnumerator Setup()
        {
            for (int i = 0; i < 5; i++)
            {
                var card = GameManager.ShowDeck.Lastcard();
                CreateMyFirstHands(card);
                yield return new WaitForSeconds(0.2f);
            }
            Reposition();
            GameManager.RpcCreateOtherHands(5);
            //            yield return new WaitForSeconds(5);
            //            SetEnerChange();
            //            MyEnerChagerBtn.gameObject.SetActive(true);
        }

        /// <summary>
        /// 未换卡之前的第一次抽卡
        /// </summary>
        /// <param name="myCard"></param>
        private void CreateMyFirstHands(Card myCard)
        {
            //var obj = Breed.Instance().Get("Hands").Spawn(Vector3.zero, Vector3.zero, new Vector3(0.5f, 0.5f, 1), Parent, true);
            var obj = InsObj(Hands, Vector3.zero, Vector3.zero, new Vector3(0.5f, 0.5f, 1), Parent);
            var hand = obj.GetComponent<Hands>();
            hand.CreateHands(myCard, o =>
            {
                hand.Bselect = !hand.Bselect;
                hand.UiTexture.color = hand.Bselect ? Color.gray : Color.white;
            });
            MyHands.Add(hand);
            MyHandCards.Add(myCard);
        }

        /// <summary>
        /// 选择一张卡作为能量,这里主要是设置按钮事件
        /// </summary>
        public void SetEnerChange()
        {
            for (int i = MyHands.Count - 1; i >= 0; i--)
            {
                int i1 = i;
                MyHands[i].OnClickAction = o =>
                {
                    if (Ener == null)
                    {
                        Ener = MyHands[i1];
                        MyHands[i1].Bselect = !MyHands[i1].Bselect;
                        MyHands[i1].UiTexture.color = MyHands[i1].Bselect ? Color.gray : Color.white;
                    }
                    else
                    {
                        if (MyHands[i1].Bselect)
                        {
                            MyHands[i1].Bselect = !MyHands[i1].Bselect;
                            MyHands[i1].UiTexture.color = MyHands[i1].Bselect ? Color.gray : Color.white;
                            Ener = MyHands[i1].Bselect ? MyHands[i1] : null;
                        }
                    }
                };
            }
        }

        /// <summary>
        /// 把选择的卡作为能量
        /// </summary>
        private void EnerChange()
        {
            
            if (Ener != null)
            {
                EnerManager.CreateEner(Ener.MyCard);
                GameManager.RpcEnerCharge(Ener.MyCard.CardId);
                Ener.DestoryHands();

                GameManager.RpcDestoryOtherHands(MyHands.Count - 1);
                MyHands.Remove(Ener);
                MyHandCards.Remove(Ener.MyCard);

            }

            //常效果:我方回合冲能时
            GameManager.SkillManager.EnerChange();
            //            EnerManager.SetTheEner();
            CancelInvoke("SetZero");
            CancelInvoke("Reposition");
            Invoke("SetZero", 0.5f);
            Invoke("Reposition", 1);
            GameManager.RpcEnerCharge();
        }

        private Hands _desCard;
        private System.Action _Succeed;
        private int _disNum;

        public void DesMyHandsOverSix()
        {
            MyDesBtn.SetActive(true);
            if (MyHands.Count <= _disNum)
            {
                if (_Succeed != null)
                {
                    _Succeed();
                }
                MyDesBtn.SetActive(false);
                return;
            }

            for (int i = MyHands.Count - 1; i >= 0; i--)
            {
                int i1 = i;
                MyHands[i].SetOnClickAction(o =>
                {
                    if (_desCard == null)
                    {
                        _desCard = MyHands[i1];
                        MyHands[i1].Bselect = !MyHands[i1].Bselect;
                        MyHands[i1].UiTexture.color = MyHands[i1].Bselect ? Color.gray : Color.white;
                    }
                    else
                    {
                        if (MyHands[i1].Bselect)
                        {
                            MyHands[i1].Bselect = !MyHands[i1].Bselect;
                            MyHands[i1].UiTexture.color = MyHands[i1].Bselect ? Color.gray : Color.white;
                            _desCard = MyHands[i1].Bselect ? MyHands[i1] : null;
                        }
                    }
                });
            }

            GameManager.SkillManager.HandChange();
        }

        /// <summary>
        /// 丢弃手卡,把>num的手卡全丢弃
        /// </summary>
        /// <param name="num">剩下手卡数</param>
        /// <param name="succeed"></param>
        public void SetDesBtnOverSix(int num, System.Action succeed)
        {
            _disNum = num;
            _Succeed = succeed;
        }

        private void DesHands()
        {
            if (_desCard != null)
            {
                DestoryHands(_desCard);
            }
            DesMyHandsOverSix();
        }

        public void Reflash()
        {
            for (int i = 0; i < MyHands.Count; i++)
            {
                MyHands[i].Reflash();
            }
        }

        private GameObject InsObj(GameObject obj, Vector3 pos, Vector3 rot, Vector3 scale, Transform parent)
        {
            var obj2 = Instantiate(obj) as GameObject;
            Transform objtran = obj2.transform;
            objtran.parent = parent;
            objtran.localPosition = pos;
            objtran.localEulerAngles = rot;
            objtran.localScale = scale;
            return obj2;
        }

        /// <summary>
        /// 换卡
        /// </summary>
        private void ChangeMyHands()
        {
            int change = 0;
            Card card;
            for (int i = MyHands.Count - 1; i >= 0; i--)
            {
                if (MyHands[i].Bselect)
                {
                    change++;
                    MyHands[i].UiTexture.color = Color.white;
                    card = MyHands[i].MyCard;
                    MyHands[i].DestoryHands();
                    MyHands.Remove(MyHands[i]);
                    MyHandCards.Remove(card);
                    var obj = InsObj(Hands, Vector3.zero, Vector3.zero, new Vector3(0.5f, 0.5f, 1), Parent);
                    //var random = Random.Range(0, DataSource.MainDeck.Count);
                    var random = Random.Range(0, GameManager.ShowDeck.MainDeck.Count);
                    var hands = obj.GetComponent<Hands>();
                    //hands.CreateHands(DataSource.MainDeck[random]);
                    hands.CreateHands(GameManager.ShowDeck.MainDeck[random]);
                    //DataSource.MainDeck[random] = card;
                    GameManager.ShowDeck.MainDeck[random] = card;
                    MyHands.Add(hands);
                    MyHandCards.Add(card);
                }
                else
                {
                    MyHands[i].Reflash();
                }
            }

            MyChangeBtn.gameObject.SetActive(false);
            Invoke("SetZero", 0.5f);
            Invoke("Reposition", 1f);

            GameManager.RpcChangeCards(change);
        }

        /// <summary>
        /// 抽卡
        /// </summary>
        public void DropCard()
        {
            var lastcard = GameManager.ShowDeck.Lastcard();
            if (lastcard == null)
                return;
            var obj = InsObj(Hands, Vector3.zero, Vector3.zero, new Vector3(0.5f, 0.5f, 1), Parent);
            var hands = obj.GetComponent<Hands>();
            hands.MyCard = lastcard;
            MyHands.Add(hands);
            MyHandCards.Add(lastcard);
            Reposition();
            GameManager.RpcCreateOtherHands(1);
        }

        public void CreateHandByCard(Card card)
        {
            var obj = InsObj(Hands, Vector3.zero, Vector3.zero, new Vector3(0.5f, 0.5f, 1), Parent);
            var hands = obj.GetComponent<Hands>();
            hands.MyCard = card;
            MyHands.Add(hands);
            MyHandCards.Add(card);
            Reposition();
            if (GameManager.MyGameState == GameManager.GameState.主要阶段 || GameManager.MyGameState == GameManager.GameState.分身攻击阶段)
            {
                ShowTheUseBtn();
            }
            GameManager.RpcCreateOtherHands(1);
        }

        public void CreateHandFromDeck(Card card)
        {
            CreateHandByCard(card); 
            GameManager.ShowDeck.MainDeck.Remove(card);
        }

        public IEnumerator DropCard(int num)
        {
            for (int i = 0; i < num; i++)
            {
                DropCard();
                yield return new WaitForSeconds(0.3f);
            }
            GameManager.SkillManager.HandChange();
        }

        //        public void DestoryHands(Hands hands)
        //        {
        //            hands.DestoryHands();
        //            MyHands.Remove(hands);
        //        }

        private bool _bsetbox;

        /// <summary>
        /// 从新排列手卡
        /// </summary>
        private void Reposition()
        {
            if (MyHands.Count > 6)
            {
                Grid.cellWidth = 140 * ((float)6 / MyHands.Count);
                Grid.Reposition();

                _bsetbox = true;
                BoxCollider boxCollider;

                for (int i = 0; i < MyHands.Count; i++)
                {
                    MyHands[i].UiTexture.depth = i;
                    boxCollider = MyHands[i].BoxCollider;

                    if (i != MyHands.Count - 1)
                    {
                        boxCollider.center = new Vector3(-Mathf.RoundToInt((250 - 250 * (float)7 / MyHands.Count) / 2), 0, 0);
                        boxCollider.size = new Vector3(Mathf.RoundToInt(250 * (float)7 / MyHands.Count), 350, 0);
                    }
                    else
                    {
                        boxCollider.center = Vector3.zero;
                        boxCollider.size = new Vector3(250, 350, 0);
                    }
                }
            }
            else
            {
                Grid.cellWidth = 140;
                Grid.Reposition();

                for (int i = 0; i < MyHands.Count; i++)
                {
                    MyHands[i].UiTexture.depth = i;
                    if (_bsetbox)
                    {
                        MyHands[i].BoxCollider.center = Vector3.zero;
                        MyHands[i].BoxCollider.size = new Vector3(250, 350, 0);
                    }
                    if (i == MyHands.Count - 1)
                    {
                        _bsetbox = false;
                    }
                }
            }
        }

        private void SetOtherZero()
        {
            var childs = OtherGrid.GetChildList();
            for (int i = 0; i < childs.Count; i++)
            {
                childs[i].transform.localPosition = Vector3.zero;
            }
        }

        private void OtherReposition()
        {
            if (OtherGrid.GetChildList().Count > 6)
            {
                OtherGrid.cellWidth = 140 * ((float)6 / OtherGrid.GetChildList().Count);
                OtherGrid.Reposition();

                var child = OtherGrid.GetChildList();

                for (int i = 0; i < child.Count; i++)
                {
                    child[i].GetComponent<UITexture>().depth = i;
                }
            }
            else
            {
                OtherGrid.cellWidth = 140;
                OtherGrid.Reposition();

                var child = Grid.GetChildList();

                for (int i = 0; i < child.Count; i++)
                {
                    child[i].GetComponent<UITexture>().depth = i;
                }
            }
        }

        private Hands _usingHands;

        private void UseCard()
        {
            if (_usingHands.MyCard.MyCardType == Card.CardType.精灵卡)
            {
                UIEventListener.Get(UseCardBtn).MyOnClick = () =>
                {
                    Reflash();
                    UseCardBtn.SetActive(false);
                    NotUseCardBtn.SetActive(false);

                    SetSigni.SetSendingSigni(_usingHands.MyCard);
                    _usingHands.DestoryHands();
                    GameManager.RpcDestoryOtherHands(MyHands.Count - 1);
                    MyHandCards.Remove(_usingHands.MyCard);
                    MyHands.Remove(_usingHands);
                    Invoke("Reposition", 0.5f);
                };
            }
            if (_usingHands.MyCard.MyCardType == Card.CardType.法术卡)
            {
                UIEventListener.Get(UseCardBtn).MyOnClick = () =>
                {
                    int count = _usingHands.MyCard.Cost.Count;

                    if (_usingHands.MyCard.Cost.Count < 1)
                    {
                        GameManager.RpcOtherTiming(2);

                        UseCardBtn.SetActive(false);
                        NotUseCardBtn.SetActive(false);
                        Reflash();

                        GameManager.Reporting.text = "等待对方操作中...";
                        StartCoroutine(WaitToOtherUseArt(_usingHands.MyCard));
                        GameManager.WordInfo.ShowTheEndPhaseBtn(false);
                        GameManager.RpcOtherUseArt(true);

                        StartCoroutine(Check.SetCheck(_usingHands.MyCard));
                        GameManager.RpcCheck(_usingHands.MyCard.CardId);
                        GameManager.ShowCard.ShowMyCard(_usingHands.MyCard);

                        _usingHands.DestoryHands();
                        GameManager.RpcDestoryOtherHands(MyHands.Count - 1);
                        MyHandCards.Remove(_usingHands.MyCard);
                        MyHands.Remove(_usingHands);

                        Invoke("Reposition", 0.5f);
                        return;
                    }

                    CardInfo.ShowCardInfo(true);

                    //int i3 = i2;
                    Lrig.SetTheCost(0, count - 1, _usingHands.MyCard, () =>
                    {
                        GameManager.RpcOtherTiming(2);

                        UseCardBtn.SetActive(false);
                        NotUseCardBtn.SetActive(false);
                        Reflash();

                        GameManager.Reporting.text = "等待对方操作中...";
                        StartCoroutine(WaitToOtherUseArt(_usingHands.MyCard));
                        GameManager.WordInfo.ShowTheEndPhaseBtn(false);
                        GameManager.RpcOtherUseArt(true);

                        StartCoroutine(Check.SetCheck(_usingHands.MyCard));
                        GameManager.ShowCard.ShowMyCard(_usingHands.MyCard);
                        GameManager.RpcCheck(_usingHands.MyCard.CardId);

                        _usingHands.DestoryHands();
                        GameManager.RpcDestoryOtherHands(MyHands.Count - 1);
                        MyHandCards.Remove(_usingHands.MyCard);
                        MyHands.Remove(_usingHands);

                        Invoke("Reposition", 0.5f);
                    }, 1);
                };
            }
        }

        /// <summary>
        /// 显示手卡中的出卡按钮
        /// </summary>
        public void ShowTheUseBtn()
        {
            if (GameManager.MyGameState != GameManager.GameState.主要阶段)
                return;

            Card card;
            for (int i = 0; i < MyHands.Count; i++)
            {
                card = MyHands[i].MyCard;
                if (card.TypeOnly.Length > 0)
                {
                    if (card.TypeOnly != Lrig.MyLrig.Type)
                        return;
                }
                if (card.MyCardType == Card.CardType.精灵卡)
                {
                    //等级少于等于分身等级,且场上等级总和少于等于分身限制数
                    if (card.Level <= Lrig.MyLrig.Level && (SetSigni.SigniLevelCount + card.Level) <= Lrig.MyLrig.Limit && SetSigni.BEnety())
                    {
                        int i1 = i;
                        MyHands[i].ShowCanUseTips(true);
                        MyHands[i].SetOnClickAction(g =>
                        {
                            _usingHands = MyHands[i1];
                            UseCard();
                            UseCardBtn.SetActive(true);
                            NotUseCardBtn.SetActive(true);
                            DisEffectBtn();
                        });
                    }
                    else
                    {
                        MyHands[i].ShowCanUseTips(false);
                        MyHands[i].SetOnClickAction(g =>
                        {
                            UseCardBtn.SetActive(false);
                            NotUseCardBtn.SetActive(false);
                        });
                    }
                }
                //法术牌
                if (card.MyCardType == Card.CardType.法术卡)
                {
                    if (CountCost(card, 1))
                    {
                        int i1 = i;
                        MyHands[i].ShowCanUseTips(true);
                        MyHands[i].SetOnClickAction(g =>
                        {
                            _usingHands = MyHands[i1];
                            UseCard();
                            UseCardBtn.SetActive(true);
                            NotUseCardBtn.SetActive(true);
                            DisEffectBtn();
                        });
                    }
                    else
                    {
                        MyHands[i].ShowCanUseTips(false);
                        MyHands[i].SetOnClickAction(g =>
                        {
                            UseCardBtn.SetActive(false);
                            NotUseCardBtn.SetActive(false);
                        });
                    }
                }
            }
        }

        public string Cardid;

        private IEnumerator WaitToOtherUseArt(Card card)
        {
            int i = 0;
            while (true)
            {
                yield return new WaitForSeconds(1);
                i++;
                if (i > 5 && GameManager.BUseArt == 0)
                {
                    card.EffectSpell(card);

                    GameManager.ShowCard.ShowMyCardEffect(card);
                    GameManager.RpcOtherCardBuff(card.CardId);

                    GameManager.Reporting.text = "主要阶段";
                    GameManager.WordInfo.ShowTheEndPhaseBtn(true);
                    GameManager.RpcOtherUseArt(false);
                    GameManager.RpcOtherTiming(0);
                    ShowTheUseBtn();
                    yield break;
                }
                if (GameManager.BUseArt != 0)
                {
                    if (GameManager.BUseArt == 1)
                    {
                        GameManager.BUseArt = 0;
                        StartCoroutine(WaitToOtherUseArt2(card));
                        yield break;
                    }
                    if (GameManager.BUseArt == -1)
                    {
                        GameManager.Reporting.text = "主要阶段";
                        GameManager.WordInfo.ShowTheEndPhaseBtn(true);
                        GameManager.BUseArt = 0;
                        card.EffectSpell(card);

                        GameManager.ShowCard.ShowMyCardEffect(card);
                        GameManager.RpcOtherCardBuff(card.CardId);

                        GameManager.RpcOtherTiming(0);
                        ShowTheUseBtn();
                        yield break;
                    }
                    GameManager.RpcOtherUseArt(false);
                }
            }
        }

        private IEnumerator WaitToOtherUseArt2(Card card)
        {
            int i = 0;
            while (true)
            {
                yield return new WaitForSeconds(1);
                i++;
                if (i >= 10)
                {
                    card.EffectSpell(card);

                    GameManager.ShowCard.ShowMyCardEffect(card);
                    GameManager.RpcOtherCardBuff(card.CardId);

                    GameManager.Reporting.text = "主要阶段";
                    GameManager.WordInfo.ShowTheEndPhaseBtn(true);
                    GameManager.RpcOtherUseArt(false);
                    GameManager.RpcOtherTiming(0);
                    ShowTheUseBtn();
                    yield break;
                }
                else
                {
                    if (GameManager.Check.GetOtherCard() != null)
                    {
                        //                        GameManager.Check.GetOtherCard().EffectSpell();                                            
                        yield return new WaitForSeconds(1f);
                        GameManager.Reporting.text = "主要阶段";
                        GameManager.WordInfo.ShowTheEndPhaseBtn(true);
                        card.EffectSpell(card);

                        GameManager.ShowCard.ShowMyCardEffect(card);
                        GameManager.RpcOtherCardBuff(card.CardId);

                        ShowTheUseBtn();
                        yield break;
                    }
                }
            }
        }

        private int _waitguard = 0;
        private int _waitguard2 = 0;
        private bool _bguard;

        public void ShowGuardBtn()
        {
            for (int i = 0; i < MyHands.Count; i++)
            {
                if (MyHands[i].MyCard.BCanGuard)
                {
                    MyHands[i].ShowCanUseTips(true);
                    Guard.SetActive(true);
                    int i1 = i;
                    MyHands[i].SetOnClickAction(g =>
                    {
                        if (!_bguard)
                            return;
                        StartCoroutine(Check.SetCheck(MyHands[i1].MyCard));
                        GameManager.ShowCard.ShowMyCard(MyHands[i1].MyCard);
                        GameManager.RpcCheck(MyHands[i1].MyCard.CardId);
                        GameManager.RpcGuard(1);
                        DisTheGuardBtn();
                        MyHands[i1].DestoryHands();
                        GameManager.RpcDestoryOtherHands(MyHands.Count - 1);
                        MyHandCards.Remove(MyHands[i1].MyCard);
                        MyHands.Remove(MyHands[i1]);
                        Invoke("Reposition", 0.5f);
                        _waitguard2 = 1;
                        _bguard = false;
                    });
                }
            }

            UIEventListener.Get(Guard).MyOnClick = () =>
            {
                Guard.SetActive(false);
                NotGuard.SetActive(false);
                StartCoroutine(WaitGuard());
                _waitguard = 1;
                _bguard = true;
            };

            UIEventListener.Get(NotGuard).MyOnClick = () =>
            {
                _waitguard = -1;
                DisTheGuardBtn();
                GameManager.RpcGuard(-1);
            };

            NotGuard.SetActive(true);
            StartCoroutine(WaitClickGuard());
        }

        private IEnumerator WaitClickGuard()
        {
            int j = 0;
            while (true)
            {
                if (_waitguard == 0)
                {
                    j++;
                    if (j >= 5)
                    {
                        GameManager.RpcGuard(-1);
                        yield break;
                    }
                }
                else
                {
                    _waitguard = 0;
                    yield break;
                }
                yield return new WaitForSeconds(1);
            }
        }


        private IEnumerator WaitGuard()
        {
            int j = 0;
            while (true)
            {
                if (_waitguard2 == 0)
                {
                    yield return new WaitForSeconds(1f);
                    j++;
                    if (j >= 10)
                    {
                        Reflash();
                        GameManager.RpcGuard(-1);
                        yield break;
                    }
                }
                else
                {
                    _waitguard2 = 0;
                    yield break;
                }
            }
        }

        public void DisTheUseBtn()
        {
            for (int i = 0; i < MyHands.Count; i++)
            {
                MyHands[i].ShowCanUseTips(false);
            }
            UseCardBtn.SetActive(false);
            NotUseCardBtn.SetActive(false);
        }

        /// <summary>
        /// 计算能量区是否有足够的能量来使用该卡
        /// </summary>
        /// <returns><c>true</c>,足够, <c>false</c> 不足够.</returns>
        /// <param name="card">目标卡</param>
        /// <param name="costtype">cost类型,1.cost,2.Growcost,3.Effect_Chu,4.Effect_Qi</param>
        public bool CountCost(Card card, int costtype)
        {
            List<Card.Ener> myEners = new List<Card.Ener>();

            switch (costtype)
            {
                case 1:
                    myEners = card.Cost;
                    break;
                case 2:
                    myEners = card.GrowCost;
                    break;
                case 3:
                    myEners = card.EffectCostChu;
                    break;
                case 4:
                    myEners = card.EffectCostQi;
                    break;
            }

            bool benough = true;
            int all = 0;
            //只要有一种不符合都不行
            for (int i = 0; i < myEners.Count; i++)
            {
                all += myEners[i].Num;

                if ((EnerManager.AllEner + EnerManager.WhiteEner + EnerManager.RedEner + EnerManager.GreenEner +
                    EnerManager.BlueEner + EnerManager.BlackEner + EnerManager.NoEner) < all)
                    benough = false;

                switch (myEners[i].MyEnerType)
                {
                    case Card.Ener.EnerType.无:
                        if (EnerManager.AllEner + EnerManager.WhiteEner + EnerManager.RedEner + EnerManager.GreenEner + EnerManager.BlueEner + EnerManager.BlackEner >= myEners[i].Num) //任何颜色都等于无色
                            break;
                        if (EnerManager.NoEner < myEners[i].Num)
                            benough = false;
                        break;
                    case Card.Ener.EnerType.白:
                        if (EnerManager.AllEner >= myEners[i].Num)
                            break;
                        if (EnerManager.WhiteEner + EnerManager.AllEner < myEners[i].Num) //万花等于任何颜色
                            benough = false;
                        break;
                    case Card.Ener.EnerType.红:
                        if (EnerManager.AllEner >= myEners[i].Num)
                            break;
                        if (EnerManager.RedEner + EnerManager.AllEner < myEners[i].Num) //万花等于任何颜色
                            benough = false;
                        break;
                    case Card.Ener.EnerType.绿:
                        if (EnerManager.AllEner >= myEners[i].Num)
                            break;
                        if (EnerManager.GreenEner + EnerManager.AllEner < myEners[i].Num) //万花等于任何颜色
                            benough = false;
                        break;
                    case Card.Ener.EnerType.蓝:
                        if (EnerManager.AllEner >= myEners[i].Num)
                            break;
                        if (EnerManager.BlueEner + EnerManager.AllEner < myEners[i].Num) //万花等于任何颜色
                            benough = false;
                        break;
                    case Card.Ener.EnerType.黑:
                        if (EnerManager.AllEner >= myEners[i].Num)
                            break;
                        if (EnerManager.BlackEner + EnerManager.AllEner < myEners[i].Num) //万花等于任何颜色
                            benough = false;
                        break;
                    case Card.Ener.EnerType.万花:
                        if (EnerManager.AllEner < myEners[i].Num)
                            benough = false;
                        break;
                }
            }

            return benough;
        }

        //        private void UseBtn(GameObject go)
        //        {
        //            var card = go.GetComponent<Hands>().MyCard;
        //            if (card.MyCardType == Card.CardType.精灵卡)
        //            {
        //                if (card.Level <= Lrig.MyLrig.Level)
        //                {
        //
        //                }
        //            }
        //        }

        /// <summary>
        /// 隐藏手卡中的防御按钮
        /// </summary>
        public void DisTheGuardBtn()
        {
            Reflash();
            Guard.SetActive(false);
            NotGuard.SetActive(false);
        }

        public void ShowUseArtBtn(bool bshow)
        {
            if (bshow)
            {
                UseBtn.SetActive(BHasArt());
                NotUseBtn.SetActive(true);
                return;
            }
            UseBtn.SetActive(false);
            NotUseBtn.SetActive(false);
        }

        private List<Card> _targetCards = new List<Card>();

        private bool BHasArt()
        {
            bool bcan = false;
            _targetCards.Clear();
            for (int i = 0; i < GameManager.ShowDeck.LrigDeck.Count; i++)
            {
                if (GameManager.ShowDeck.LrigDeck[i].MyTiming.Count > 0)
                {
                    for (int j = 0; j < GameManager.ShowDeck.LrigDeck[i].MyTiming.Count; j++)
                    {
                        if (GameManager.ShowDeck.LrigDeck[i].MyTiming[j] == GameManager.MyTiming)
                        {
                            if (CountCost(GameManager.ShowDeck.LrigDeck[i], 1))
                            {
                                bcan = true;
                                _targetCards.Add(GameManager.ShowDeck.LrigDeck[i]);
                            }
                        }
                    }
                }
            }
            return bcan;
        }

        //        public bool BEnerEnough(Card target, int num, int type)
        //        {   
        //            List<Card.Ener> targetCost = new List<Card.Ener>();
        //            switch (type)
        //            {
        //                case 1:
        //                    targetCost = target.Cost;
        //                    break;
        //                case 2:
        //                    targetCost = target.GrowCost;
        //                    break;
        //                case 3:
        //                    targetCost = target.EffectCost_Chu;
        //                    break;
        //                case 4:
        //                    targetCost = target.EffectCost_Qi;
        //                    break;
        //            }
        //
        //            bool benough = true;
        //            for (int i =0; i<targetCost.Count; i++)
        //            {
        //                switch(targetCost[i].MyEnerType)
        //                {
        //                    case Card.Ener.EnerType.白:
        //                        benough = EnerManager.WhiteEner >= targetCost[i].Num;
        //                        break;
        //                    case Card.Ener.EnerType.黑:
        //                        benough = EnerManager.BlackEner >= targetCost[i].Num;
        //                        break;
        //                    case Card.Ener.EnerType.红:
        //                        benough = EnerManager.RedEner >= targetCost[i].Num;
        //                        break;
        //                    case Card.Ener.EnerType.蓝:
        //                        benough = EnerManager.BlueEner >= targetCost[i].Num;
        //                        break;
        //                    case Card.Ener.EnerType.绿:
        //                        benough = EnerManager.GreenEner >= targetCost[i].Num;
        //                        break;
        //                    case Card.Ener.EnerType.万花:
        //                        benough = EnerManager.AllEner >= targetCost[i].Num;
        //                        break;
        //                    case Card.Ener.EnerType.无:
        //                        benough = EnerManager.NoEner>= targetCost[i].Num;
        //                        break;
        //                }
        //            }
        //            return benough;
        //        }

        /// <summary>
        /// 手卡排列为0
        /// </summary>
        private void SetZero()
        {
            var child = Grid.GetChildList();
            for (int i = 0; i < child.Count; i++)
            {
                child[i].transform.localPosition = Vector3.zero;
            }
        }

        /// <summary>
        /// 能量填充
        /// </summary>
        /// <param name="bshow"></param>
        public void ShowChargeBtn(bool bshow)
        {
            MyEnerChagerBtn.gameObject.SetActive(bshow);
        }

        /// <summary>
        /// 换卡
        /// </summary>
        /// <param name="bshow"></param>
        public void ShowChangeBtn(bool bshow)
        {
            MyChangeBtn.gameObject.SetActive(bshow);
        }

        public void CreateOtherHands(int num)
        {
            for (int i = 0; i < num; i++)
            {
                var obj = InsObj(OtherHand, Vector3.zero, Vector3.zero, Vector3.one, OtherParent);
                OtherHands.Add(obj);
            }
            Invoke("OtherReposition", 0.5f);
            GameManager.SkillManager.HandChange();
        }

        /// <summary>
        /// 随机丢弃手卡
        /// </summary>
        public void DestoryHandRamdom()
        {
            int rand = Random.Range(0, MyHands.Count);
            DestoryHands(rand);
            ShowTheUseBtn();
            GameManager.SkillManager.HandChange();
        }

        /// <summary>
        /// 丢弃全部与条件相符合的手卡
        /// </summary>
        /// <param name="condiction">条件</param>
        /// <param name="bOne">单张还是全部</param>
        public void DestoryHandCondiction(System.Func<Card, bool> condiction,bool bOne)
        {
            for (int i = MyHands.Count-1; i >=0 ; i--)
            {
                if (condiction(MyHands[i].MyCard))
                {
                    DestoryHands(MyHands[i]);
                    if (bOne)
                    {
                        return;
                    }
                }
            }
        }

        private void DestoryHands(int num)
        { 
            if (MyHands.Count - 1 < num)
                return;
            Trash.AddTrash(MyHands[num].MyCard);
            MyHands[num].DestoryHands();
            GameManager.RpcDestoryOtherHands(MyHands.Count - 1);
            MyHandCards.Remove(MyHands[num].MyCard);
            MyHands.Remove(MyHands[num]);
            CancelInvoke("SetZero");
            CancelInvoke("Reposition");
            Invoke("SetZero", 0.5f);
            Invoke("Reposition", 1);
        }

        public void DestoryHands(Hands hands)
        {
            Trash.AddTrash(hands.MyCard);
            GameManager.RpcDestoryOtherHands(MyHands.Count - 1);
            hands.DestoryHands();
            MyHandCards.Remove(hands.MyCard);
            MyHands.Remove(hands);
            CancelInvoke("SetZero");
            CancelInvoke("Reposition");
            Invoke("SetZero", 0.5f);
            Invoke("Reposition", 1);
        }

        public void DestoryOtherHands(int num)
        {  
            var obj = OtherHands[num];
            OtherHands.Remove(obj);
            Destroy(obj);
            CancelInvoke("OtherReposition");
            Invoke("OtherReposition", 0.5f);
            GameManager.SkillManager.HandChange();
        }
    }
}

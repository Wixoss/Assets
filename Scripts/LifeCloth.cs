using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class LifeCloth : MonoBehaviour
    {
        public List<Card> LifeCloths = new List<Card>();
        public GameObject LifeObj;
        public List<GameObject> LifeObjs = new List<GameObject>();
        //public List<string> LifeClothId = new List<string>();

        public GameObject OtherLifeObj;
        public List<GameObject> OtherLifeObjs = new List<GameObject>();
        public Transform OtherLifeParent;
        public GameManager GameManager;
        public UIGrid OtherGrid;
        public UIGrid Grid;

        public EnerManager EnerManager;
        public Check Check;
        public GameOver GameOver;

        /// <summary>
        /// 生命护甲!!
        /// </summary>
        //        public List<Card> LifeCloths
        //        {
        //            get { return LifeForShow; }
        //            set
        //            {
        //                LifeForShow = value;
        //                if (LifeForShow.Count >= 0)
        //                {
        //                    for (int i = 0; i < LifeObj.Count; i++)
        //                    {
        //                        LifeObj[i].SetActive(i < LifeForShow.Count - 1);
        //                    }
        //                }
        //            }
        //        }

        public void CreateLifeCloth()
        {
            var card = GameManager.ShowDeck.Lastcard();
            if (card == null)
                return;
            LifeCloths.Add(card);
            CreateObj(LifeCloths.Count - 1);
            GameManager.RpcCreateLifeCloth(LifeCloths.Count - 1);
        }

        public void CreateObj(int num)
        {
            GameObject life = Instantiate(LifeObj) as GameObject;
            LifeObjs.Add(life);
            Transform tran = life.transform;
            tran.parent = transform;
            tran.localScale = new Vector3(620, 620, 1);
            tran.localEulerAngles = Vector3.zero;
            tran.localPosition = new Vector3(0, 0, num);
            Invoke("ResetObj", 0.5f);
        }

        private void ResetObj()
        {
            Grid.Reposition();
        }

        public void CreateOtherCloth(int num)
        {
            GameObject otherLife = Instantiate(OtherLifeObj) as GameObject;
            OtherLifeObjs.Add(otherLife);
            Transform tran = otherLife.transform;
            tran.parent = OtherLifeParent;
            tran.localScale = new Vector3(620, 620, 1);
            tran.localEulerAngles = Vector3.zero;
            tran.localPosition = new Vector3(0, 0, num);
            Invoke("OtherResetObj", 0.5f);
        }

        private void OtherResetObj()
        {
            OtherGrid.Reposition();
        }

        /// <summary>
        /// 击溃护甲
        /// </summary>
        public void CrashCloth(bool bHurt)
        {
            if (bHurt && LifeCloths.Count == 0)
            {
                GameManager.GameOver.ShowGameResoult("You Lost!!");
                GameManager.RpcGameResult("You Win!!");
                return;
            }
            var card = LifeCloths[LifeCloths.Count - 1];
            var obj = LifeObjs[LifeCloths.Count - 1];
            LifeObjs.Remove(obj);
            Destroy(obj);
            if (card.HasBrust)
            {
                if (card.Brust != null)
                {
                    GameManager.RpcStopOtherAttack();
                    //StartCoroutine(WaitToUseBrust(card));  //可选择发不发动
                    StartCoroutine(WaitToBrust(card)); //默认都发动
                }
            }

            EnerManager.CreateEner(card);
            GameManager.RpcEnerCharge(card.CardId);

            StartCoroutine(Check.SetCheck(card, true));
            GameManager.RpcCheck(card.CardId);
            GameManager.ShowCard.ShowMyCard(card);
            LifeCloths.Remove(LifeCloths[LifeCloths.Count - 1]);
        }

//        private int _waitbrust = 0;
//
//        private System.Collections.IEnumerator WaitToUseBrust(Card card)
//        {
//            int i = 0;
//
//            GameManager.CreateHands.ShowEffectButton(card, () => StartCoroutine(WaitToBrust(card)), () => _waitbrust = -1);
//
//            while (true)
//            {
//                yield return new WaitForSeconds(1);
//                if (_waitbrust == 0)
//                {
//                    i++;
//                    if (i > 5)
//                    {
//                        GameManager.RpcContinueOtherAttack();
//                        GameManager.CreateHands.DisEffectBtn();
//                        yield break;
//                    }
//                }
//                if (_waitbrust == 1)
//                {
//                    _waitbrust = 0;
//                    yield break;
//                }
//                if (_waitbrust == -1)
//                {
//                    _waitbrust = 0;
//                    GameManager.RpcContinueOtherAttack();
//                    GameManager.CreateHands.DisEffectBtn();
//                    yield break;
//                }
//            }
//        }

        //private int _brust;
        //private System.Collections.IEnumerator WaitToBrust(Card card)
        private System.Collections.IEnumerator WaitToBrust(Card card)
        {
            card.Brust = GameManager.SkillManager.BackHand;
            card.Brust();
            //GameManager.ShowCard.ShowMyCardEffect(card);

            int i = 0;
            while (true)
            {
                yield return new WaitForSeconds(1);
                if(GameManager.SkillManager.BSelected)
                {
                    GameManager.SkillManager.BSelected = false;
                    GameManager.RpcContinueOtherAttack();
                    yield break;
                }
                else
                {
                    i++;
                    if(i>=10)
                    {
                        GameManager.SkillManager.BSelected = false;
                        GameManager.SkillManager.DisSelect();
                        GameManager.RpcContinueOtherAttack();
                        yield break;
                    }
                }
            }
        }

        /// <summary>
        /// 把一护甲放置废气区
        /// </summary>
        public void CrashClothToTrash()
        {
            if (LifeCloths.Count == 0)
            {
                return;
            }
            var card = LifeCloths[LifeCloths.Count - 1];
            var obj = LifeObjs[LifeCloths.Count - 1];
            LifeObjs.Remove(obj);
            Destroy(obj);
            GameManager.Trash.AddTrash(card);
            LifeCloths.Remove(LifeCloths[LifeCloths.Count - 1]);
        }

        public void CrashOtherCloth(bool bHurt)
        {
            if (bHurt && OtherLifeObjs.Count == 0)
            {
                GameManager.GameOver.ShowGameResoult("You Win!!");
                GameManager.RpcGameResult("You Lost!!");
                return;
            }
            var obj = OtherLifeObjs[OtherLifeObjs.Count - 1];
            OtherLifeObjs.Remove(obj);
            Destroy(obj);
        }
    }
}

using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class JyanKen : MonoBehaviour
    {
        public UIButton[] UiButtons;
        public UIButton SelectBtn;
        public int JyanKenNum;
        public GameObject SelectGameObject;

        private void Awake()
        {
            for (int i = 0; i < 3; i++)
            {
                int i1 = i;
                UiButtons[i].onClick.Add(new EventDelegate(() =>
                {
                    JyanKenNum = i1;
                    if (Network.isClient)
                    {
                        DataSource.ClientPlayer.BJyanKen = true;
                        DataSource.ClientPlayer.JyanKenNum = i1;
                        GameObject.Find("RPC").GetComponent<MyRpc>().Rpc("ReportClientJyanKen", RPCMode.Others, true, i1);
                    }
                    if (Network.isServer)
                    {
                        DataSource.ServerPlayer.BJyanKen = true;
                        DataSource.ServerPlayer.JyanKenNum = i1;
                        GameObject.Find("RPC").GetComponent<MyRpc>().Rpc("ReportServerJyanKen", RPCMode.Others, true, i1);
                    }

                    SelectBtn = UiButtons[i1];

                    for (int j = 0; j < UiButtons.Length; j++)
                    {
                        if (UiButtons[j] != UiButtons[i1])
                        {
                            UiButtons[j].gameObject.SetActive(false);
                        }
                    }
                    //开启判断
                    StartCoroutine(WaitTheresult());
                }));
            }
        }

        private IEnumerator WaitTheresult()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.2f);
                //双方是否已经猜拳?
                if (DataSource.ClientPlayer.BJyanKen && DataSource.ServerPlayer.BJyanKen)
                {
                    if (DataSource.ClientPlayer.JyanKenNum == DataSource.ServerPlayer.JyanKenNum)
                    {
                        DataSource.ClientPlayer.BJyanKen = false;
                        DataSource.ServerPlayer.BJyanKen = false;
                        for (int i = 0; i < UiButtons.Length; i++)
                        {
                            UiButtons[i].gameObject.SetActive(true);
                        }
                        yield break;
                    }

                    DataSource.ClientPlayer.BJyanKenWin = GetJyanKenResult(DataSource.ClientPlayer.JyanKenNum,
                        DataSource.ServerPlayer.JyanKenNum);
                    DataSource.ServerPlayer.BJyanKenWin = !DataSource.ClientPlayer.BJyanKenWin;

                    if (SelectBtn != null)
                        SelectBtn.gameObject.SetActive(false);

                    //选择先后攻
                    if (Network.isClient && DataSource.ClientPlayer.BJyanKenWin)
                    {
                        SelectGameObject.SetActive(true);
                    }

                    if (Network.isServer && DataSource.ServerPlayer.BJyanKenWin)
                    {
                        SelectGameObject.SetActive(true);
                    }

                    yield break;
                }
            }
        }

        /// <summary>
        /// 这种两边都要设置的情况下用RPCMode.All
        /// </summary>
        public void SelectFirstAttack()
        {
            if (Network.isClient)
            {
                GameObject.Find("RPC").GetComponent<MyRpc>().Rpc("ClientSelectFirstAttack", RPCMode.All, true);
            }
            if (Network.isServer)
            {
                GameObject.Find("RPC").GetComponent<MyRpc>().Rpc("ServerSelectFirstAttack", RPCMode.All, true);
            }
            SelectGameObject.SetActive(false);
        }

        public void SelectAfterAttack()
        {
            if (Network.isClient)
            {
                GameObject.Find("RPC").GetComponent<MyRpc>().Rpc("ClientSelectFirstAttack", RPCMode.All, false);
            }
            if (Network.isServer)
            {
                GameObject.Find("RPC").GetComponent<MyRpc>().Rpc("ServerSelectFirstAttack", RPCMode.All, false);
            }
            SelectGameObject.SetActive(false);
        }

        private bool GetJyanKenResult(int num1, int num2)
        {
            if (num1 < num2)
            {
                if (num1 == 0 && num2 == 2)
                {
                    return true;
                }
                return false;
            }
            if (num2 == 0 && num1 == 2)
            {
                return false;
            }
            return true;
        }
    }
}

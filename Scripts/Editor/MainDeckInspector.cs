using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    [CustomEditor(typeof(ShowDeck))]
    public class MainDeckInspector : Editor
    {
        private ShowDeck _showdeck;
        private Vector2 _pos;
        private static bool _bCheat;

        private void OnEnable()
        {
            _showdeck = target as ShowDeck;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            //return;
            _pos = EditorGUILayout.BeginScrollView(_pos);
            EditorGUILayout.BeginVertical();

            _bCheat = EditorGUILayout.Foldout(_bCheat, "开启作弊!!");
            if (_bCheat)
            {
                if (GUILayout.Button("抽卡"))
                {
                    _showdeck.GameManager.CreateHands.DropCard();
                }

                if (GUILayout.Button("充能"))
                {
                    _showdeck.GameManager.EnerManager.EnerCharge();
                }

                if (GUILayout.Button("随机丢弃手卡"))
                {
                    _showdeck.GameManager.CreateHands.DestoryHandRamdom();
                }

                if (GUILayout.Button("从卡组顶丢弃到废弃"))
                {
                    _showdeck.MainDeckToTrash();
                }

                if (GUILayout.Button("修复"))
                {
                    _showdeck.GameManager.LifeCloth.CreateLifeCloth();
                }

                if (GUILayout.Button("洗刷卡组"))
                {
                    _showdeck.WashMainDeck();
                }

                if (GUILayout.Button("指定对方一只精灵回手"))
                {
                    _showdeck.GameManager.SetSigni.ShowOtherSelections(true, true);
                    _showdeck.GameManager.SetSigni.SetSelections(false, null, true, i =>
                        GameManager.RpcBackHand(_showdeck.GameManager.SetSigni.OtherSelection));
                }

                if (GUILayout.Button("指定对方一只精灵驱逐"))
                {
                    _showdeck.GameManager.SetSigni.ShowOtherSelections(true, true);
                    _showdeck.GameManager.SetSigni.SetSelections(false, null, true,
                        i => _showdeck.GameManager.SetSigni.BanishOtherSigni(i));
                }

                if (GUILayout.Button("指定我方一只精灵枪兵"))
                {
                    _showdeck.GameManager.SetSigni.ShowMySelections(true, true);
                    _showdeck.GameManager.SetSigni.SetSelections(true, i =>
                    {
                        _showdeck.GameManager.SetSigni.Signi[i].Blancer = true;
                        GameManager.RpcMyBuff(1, i, true);
                    }, false, null);
                }

                if (GUILayout.Button("指定我方一只精灵双重击溃"))
                {
                    _showdeck.GameManager.SetSigni.ShowMySelections(true, true);
                    _showdeck.GameManager.SetSigni.SetSelections(true, i =>
                    {
                        _showdeck.GameManager.SetSigni.Signi[i].Bdouble = true;
                        GameManager.RpcMyBuff(2, i, true);
                    }, false, null);
                }

                if (GUILayout.Button("指定我方分身再次攻击"))
                {
                    _showdeck.GameManager.Lrig.ResetLrig();
                    _showdeck.GameManager.Lrig.ShowAttackBtn(true);
                }

                if (GUILayout.Button("指定对方一只精灵冰冻"))
                {
                    _showdeck.GameManager.SetSigni.ShowOtherSelections(true, true);
                    _showdeck.GameManager.SetSigni.SetSelections(false, null, true, i =>
                    {
                        _showdeck.GameManager.SetSigni.OtherSigni[i].Bfreeze = true;
                        GameManager.RpcOtherDebuff(1, i, true);
                    });
                }

                if (GUILayout.Button("指定对方一只精灵不能攻击"))
                {
                    _showdeck.GameManager.SetSigni.ShowOtherSelections(true, true);
                    _showdeck.GameManager.SetSigni.SetSelections(false, null, true, i =>
                    {
                        _showdeck.GameManager.SetSigni.OtherSigni[i].BCantAttack = true;
                        GameManager.RpcOtherDebuff(2, i, true);
                    });
                }

                if (GUILayout.Button("横置对方精灵"))
                {
                    _showdeck.GameManager.SetSigni.ShowOtherSelections(true, true);
                    _showdeck.GameManager.SetSigni.SetSelections(false, null, true,
                        GameManager.RpcSetOtherSigniSet);
                }

                if (GUILayout.Button("横置对方分身"))
                {
                    _showdeck.GameManager.Lrig.SetOtherLrigSelection(GameManager.RpcSetOtherLrigSet);
                }

                if (GUILayout.Button("丢弃3手卡,自己选择"))
                {
                    _showdeck.SkillManager.DesCard(3);
                }

                if (GUILayout.Button("检查卡组3张,按喜欢的顺序排列"))
                {
                    SkillManager.CheckDeckNumAndSort(3, false);
                }
            }

            for (int i = _showdeck.MainDeck.Count - 1; i >= 0; i--)
            {
                EditorGUILayout.LabelField("CardId", _showdeck.MainDeck[i].CardId);
                EditorGUILayout.LabelField("CardName", _showdeck.MainDeck[i].CardName);
                EditorGUILayout.LabelField("MyCardType", _showdeck.MainDeck[i].MyCardType.ToString());
                EditorGUILayout.LabelField("MyCardColor", _showdeck.MainDeck[i].MyCardColor.ToString());
                EditorGUILayout.ObjectField("CardTexture", _showdeck.MainDeck[i].CardTexture, typeof(Texture2D), false);
                EditorGUILayout.LabelField("CardDetail", _showdeck.MainDeck[i].CardDetail);
                EditorGUILayout.Toggle("HasBrust", _showdeck.MainDeck[i].HasBrust);
                for (int l = 0; l < _showdeck.MainDeck[i].Cost.Count; l++)
                {
                    EditorGUILayout.LabelField("Cost", _showdeck.MainDeck[i].Cost[l].MyEnerType.ToString());
                    EditorGUILayout.IntField("Cost", _showdeck.MainDeck[i].Cost[l].Num);
                }

                for (int j = 0; j < _showdeck.MainDeck[i].GrowCost.Count; j++)
                {
                    EditorGUILayout.LabelField("GrowCost", _showdeck.MainDeck[i].GrowCost[j].MyEnerType.ToString());
                    EditorGUILayout.IntField("GrowCost", _showdeck.MainDeck[i].GrowCost[j].Num);
                }

                EditorGUILayout.Toggle("BCanGuard", _showdeck.MainDeck[i].BCanGuard);
                EditorGUILayout.IntField("Level", _showdeck.MainDeck[i].Level);
                EditorGUILayout.IntField("Limit", _showdeck.MainDeck[i].Limit);
                if (_showdeck.MainDeck[i].Type != null)
                {
                    EditorGUILayout.LabelField("Type", _showdeck.MainDeck[i].Type.ToString());
                }
                if (_showdeck.MainDeck[i].TypeOnly != null)
                {
                    EditorGUILayout.LabelField("TypeOnly", _showdeck.MainDeck[i].TypeOnly.ToString());
                }
                EditorGUILayout.IntField("Atk", _showdeck.MainDeck[i].Atk);
                EditorGUILayout.LabelField("--------------------" + (i + 1) + "-----------------------");
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
    }
}
